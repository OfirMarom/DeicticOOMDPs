using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    [Serializable]
    class ActionAttributeBNs<O,A> where A : struct
    {
        private List<ActionAttributeBN<O, A>> collection;

        public ActionAttributeBNs(params ActionAttributeBN<O,A>[] collection)
        {
            this.collection = collection.ToList();
        }

        public void Update(O o, State state, A a, A aNew, Dictionary<string, bool> condFuncCache = null)
        {
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                ActionAttributeBN<O, A> actionAttributeBN = collection[i];

                actionAttributeBN.Update(o, state, a, aNew,condFuncCache);

                if (actionAttributeBN.ConjunctionTM != null && actionAttributeBN.ConjunctionTM.IsValidTreeModel == false)
                {
                    collection.Remove(actionAttributeBN);
                }
            }

            if (collection.Count == 0)
            {
                throw new Exception();
            }
        }

        public Dictionary<ValueType, double> AttributeDistibution(O o, State state,A a, int minObsForPrediction, bool excludeZeroProbEvents = true, Dictionary<string, bool> condFuncCache = null)
        {
            if (collection.Count == 1)
            {
                Dictionary<A, double> attributeDistribution = collection[0].AttributeDistibution(o, state, a, minObsForPrediction, excludeZeroProbEvents,condFuncCache);

                if(attributeDistribution==null)
                {
                    return null;
                }

                return attributeDistribution.ToDictionary(x => (ValueType) x.Key, x => x.Value);
            }
            else
            {
                List<A> attributeVals = new List<A>();

                foreach (ActionAttributeBN<O, A> actionAttributeBN in collection)
                {
                    Dictionary<A, double> attributeDistribution = actionAttributeBN.AttributeDistibution(o, state, a, minObsForPrediction, excludeZeroProbEvents,condFuncCache);

                    if(attributeDistribution == null)
                    {
                        return null;
                    }

                    if(attributeDistribution.Keys.Count>1)
                    {
                        throw new Exception();
                    }

                    foreach(A aNew in attributeDistribution.Keys)
                    {
                        attributeVals.Add(aNew);
                    }
                }

                if(attributeVals.Distinct().Count()>1)
                {
                    return null;
                }
                else
                {
                    return new Dictionary<A, double>() { { attributeVals[0], 1 } }.ToDictionary(x=> (ValueType)x.Key,x=>x.Value);
                }
            }
        }
        

    }
}
