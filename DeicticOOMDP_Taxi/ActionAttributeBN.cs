using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    [Serializable]
    class ActionAttributeBN<O, A> where A : struct
    {
        CondDistribution condDistribution;
        Func<int, A, A> effectFunction;
        Func<O, State, bool>[] condFuncs;
        int numEffects;
        ConjunctionTM conjunctionTM;

        
        public A ApplyEffectFunction(int index,A a)
        {
            return effectFunction(index, a);
        }

        public ActionAttributeBN(int numEffects, Func<int, A, A> effectFunction, Func<O, State, bool>[] condFuncs)
        {
            this.numEffects = numEffects;
            this.condDistribution = new CondDistribution(numEffects);
            this.effectFunction = effectFunction;
            this.condFuncs = condFuncs;
        }


        public Dictionary<A, double> AttributeDistibution(O o, State state, A a, int minObsForPrediction, bool excludeZeroProbEvents = true, Dictionary<string,bool> condFuncCache=null)
        {
            Dictionary<A, double> attributeDistribution = new Dictionary<A, double>();

            int[] cond = GetCond(o, state, condFuncCache);

            if (conjunctionTM != null)
            {
                int? effectIndexTM = conjunctionTM.GetEffectIndex(cond);

                if (effectIndexTM != null)
                {
                    A aNew = effectFunction((int)effectIndexTM, a);

                    //attributeDistribution[default(A)] = 1;

                    //return attributeDistribution;

                    attributeDistribution[aNew] = 1;                  
                    return attributeDistribution;
                }
            }

            Distribution dist = condDistribution.Get(cond);

            if (dist == null || dist.Probs.Sum() < minObsForPrediction)
            {
                return null;
            }

            dist = dist.Normalize();

            for (int i = 0; i < dist.Count; i++)
            {
                if (excludeZeroProbEvents == true && dist.Probs[i] == 0)
                {
                    continue;
                }

                A aNew = effectFunction(i, a);
                attributeDistribution[aNew] = dist.Probs[i];
            }

            if (attributeDistribution.Values.Sum() != 1)
            {
                throw new Exception();
            }

            return attributeDistribution;
        }

        public int[] GetCond(O o, State state, Dictionary<string, bool> condFuncCache = null)
        {
           
            int[] cond = new int[condFuncs.Length];

            for (int i = 0; i < condFuncs.Length; i++)
            {
                var condFunc = condFuncs[i];

                string condFuncName = condFunc.Method.Name;

                bool b;

                if (condFuncCache != null && condFuncCache.ContainsKey(condFuncName))
                {
                    b = condFuncCache[condFuncName];
                }
                else
                {
                    b = condFuncs[i](o, state);
                }

                cond[i] = Convert.ToInt32(b);
            }

            return cond;
        }

        public void Update(O o, State state, A a, A aNew, Dictionary<string, bool> condFuncCache = null)
        {
            int? effectIndex = null;
           
            for (int i = 0; i < numEffects; i++)
            {
                if (effectFunction(i, a).Equals(aNew))
                {
                    effectIndex = i;
                    break;
                }
            }

            int[] cond = GetCond(o, state,condFuncCache);

            if (conjunctionTM != null)
            {
                Dictionary<int[], Distribution> flattenedCondDistribution = condDistribution.Flatten();

                conjunctionTM.SetIsValidTreeModel((int)effectIndex, cond, flattenedCondDistribution);

                if (conjunctionTM.IsValidTreeModel == false)
                {                
                    return;
                }
            }
            

            if (conjunctionTM != null && conjunctionTM.IsEffectIndexInModel((int)effectIndex))
            {
                conjunctionTM.Update((int)effectIndex, cond);
            }
            else
            {
                if(!condDistribution.ContainsKey(cond))
                {
                    condDistribution.Set(cond);
                }

                Distribution dist = condDistribution.Get(cond);

                dist.Set((int)effectIndex, dist.Probs[(int)effectIndex] + 1);
            }

        }


        public ConjunctionTM ConjunctionTM
        {
            get { return conjunctionTM; }
            set { conjunctionTM = value; }
        }

        public CondDistribution CondDistribution
        {
            get { return condDistribution; }
        }

      
    }
}
