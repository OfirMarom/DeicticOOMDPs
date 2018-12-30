using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    [Serializable]
    class CondDistribution
    {
        MultiLevelDictionary<int, Distribution> condProbs;
        int numOutcomes;
        double[] defaultPrior;
        bool update = true;

        public CondDistribution(int numOutcomes, double[] defaultPrior = null)
        {
            condProbs = new MultiLevelDictionary<int, Distribution>();
            this.numOutcomes = numOutcomes;
            this.defaultPrior = defaultPrior;
        }

        public bool ContainsKey(int[] cond)
        {
            return condProbs.ContainsKey(cond);
        }

        public void  Set(int[] cond)
        {
            condProbs[cond] = new Distribution(numOutcomes, defaultPrior);
        }

        public Distribution Get(int[] cond)
        {
            if(!condProbs.ContainsKey(cond))
            {
                return null;
            }

            return condProbs[cond];
        }

        public Dictionary<int[],Distribution> Flatten()
        {
            return condProbs.Flatten();
        }

        public bool Update
        {
            get { return update; }
            set { update = value; }
        }

    }
}
