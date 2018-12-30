using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    [Serializable]
    class Distribution
    {
        double[] probs;
        double[] prior;

        public Distribution(int numOutcomes, double[] prior = null)
        {
            probs = new double[numOutcomes];

            if(prior==null)
            {
                prior = new double[numOutcomes];
            }

            this.prior = Global.DeepCopyArray(prior);
        }

        public Distribution(Distribution dist)
        {
            this.probs = Global.DeepCopyArray(dist.probs);
            this.prior = Global.DeepCopyArray(dist.prior);        
        }

        public void Set(int index, double val)
        {
            probs[index] = val;
        }

        public Distribution Normalize()
        {
            Distribution dist = new Distribution(this);
            dist.probs = dist.NormalizeProbs();
            return dist;
        }


        public  bool AllMassAtIndex(int index)
        {
            if (probs[index] > 0 && probs.Sum() == probs[index])
            {
                return true;
            }

            return false;
        }

        private double[] NormalizeProbs()
        {
            double[] probs = new double[this.probs.Length];

            double sum = this.probs.Sum() + prior.Sum();

            for (int i = 0; i < probs.Length; i++)
            {
                if (sum == 0)
                {
                    probs[i] = 1 / (double)this.probs.Length;
                }
                else
                {
                    probs[i] = (this.probs[i] + prior[i]) / sum;
                }
            }

            return probs;
        }


        public int Sample()
        {
            double[] probs = NormalizeProbs();

            double u = Global.Random.NextDouble();

            double cum = 0;

            for (int i = 0; i < probs.Length; i++)
            {
                if (u >= cum && u < cum + probs[i])
                {
                    return i;
                }

                cum += probs[i];
            }


            return probs.Length - 1;
        }

        public double[] Probs
        {
            get { return probs; }
        }

        public double[] Prior
        {
            get { return prior; }
        }

        public int Count
        {
            get { return probs.Length; }
        }

    }
}
