using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    [Serializable]
    class ConjunctionTM
    {
        int[] uniqueEffectIndexes;
        Dictionary<int, int[]> uniqueEffectConds;
        bool isValidTreeModel = true;
        List<int> allObservedEffectIndexes = new List<int>();
      
        public ConjunctionTM(int[] uniqueEffectIndexes)
        {
            this.uniqueEffectIndexes = uniqueEffectIndexes;
            uniqueEffectConds = new Dictionary<int, int[]>();
        }

        public bool IsEffectIndexInModel(int index)
        {
            return uniqueEffectIndexes.Any(i => i == index);
        }

        public void Update(int index, int[] cond)
        {      
            if (!uniqueEffectConds.ContainsKey(index))
            {
                uniqueEffectConds[index] = cond;
            }
            else
            {
                uniqueEffectConds[index] = UpdateCond(index, cond);
            }
        }

        public int? GetEffectIndex(int[] condToMatch)
        {
            foreach(int index in uniqueEffectConds.Keys)
            {
               bool isMatched = IsSubset(uniqueEffectConds[index], condToMatch);    

                if(isMatched==true)
                {
                    return index;
                }
            }

            return null;
        }


        private bool IsSubset(int[] condLeft, int[] condRight)
        {
            for (int i = 0; i < condLeft.Length; i++)
            {
                if (!(condLeft[i] == -1 || condLeft[i] == condRight[i]))
                {
                    return false;
                }
            }

            return true;
        }


      
        private int[] UpdateCond(int index, int[] cond)
        {         
            int[] currentCond = uniqueEffectConds[index];

            int[] newCond = Global.DeepCopyArray(currentCond);

            for (int i=0;i<cond.Length;i++)
            {
                if(cond[i]!=currentCond[i])
                {
                    newCond[i] = -1;
                }
            }

            return newCond;
        }

        private int[] DisjunctionIndexesOfEffectIndex(int index, Dictionary<int[],Distribution> disjunctions)
        {
            List<int> indexes = new List<int>();

            int count = 0;

            foreach(int[] cond in disjunctions.Keys)
            {
                Distribution dist = disjunctions[cond];

                if (dist.AllMassAtIndex(index))
                {
                    indexes.Add(count);
                }

                count++;
            }

            return indexes.ToArray();
        }

        public void SetIsValidTreeModel(int index, int[] cond, Dictionary<int[], Distribution> disjunctions)
        {
            bool isFirstTimeEffectObserved = false;

            if (!allObservedEffectIndexes.Contains(index))
            {
                isFirstTimeEffectObserved = true;
                allObservedEffectIndexes.Add(index);
            }

            if (isFirstTimeEffectObserved == false && IsEffectIndexInModel(index))
            {
                cond = UpdateCond(index, cond);
            }

            foreach (int i in uniqueEffectConds.Keys)
            {
                if (index == i)
                {
                    continue;
                }

                bool isMatched;

                if (isFirstTimeEffectObserved == true)
                {
                    int[] cond1 = uniqueEffectConds[i];
                    int[] cond2 = cond;
                    isMatched = IsSubset(cond1, cond2);

                }
                else
                {
                    int[] cond1 = cond;
                    int[] cond2 = uniqueEffectConds[i];
                    isMatched = IsSubset(cond1, cond2) || IsSubset(cond2, cond1);
                }

                if (isMatched == true)
                {
                    isValidTreeModel = false;
                    return;
                }
            }

            int[] indexes = DisjunctionIndexesOfEffectIndex(index, disjunctions);

            int count = 0;

            foreach (int[] disjunctionCond in disjunctions.Keys)
            {
                if (indexes.Contains(count))
                {
                    count++;
                    continue;
                }

                bool isMatched;

                if (isFirstTimeEffectObserved == true)
                {
                    int[] cond1 = disjunctionCond;
                    int[] cond2 = cond;
                    isMatched = IsSubset(cond1, cond2);

                }
                else
                {
                    int[] cond1 = cond;
                    int[] cond2 = disjunctionCond;
                    isMatched = IsSubset(cond1, cond2) || IsSubset(cond2, cond1);
                }

                if (isMatched == true)
                {
                    isValidTreeModel = false;
                    return;
                }

                count++;
            }
        }

       
        public bool IsValidTreeModel
        {
            get { return isValidTreeModel; }
        }

        public List<int> AllObservedEffectIndexes
        {
            get { return allObservedEffectIndexes; }
            set { allObservedEffectIndexes = value; }
        }
    }
}
