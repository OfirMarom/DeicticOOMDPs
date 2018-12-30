using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class EpisodeData
    {
        int numSteps;
        double sumReward;
        bool success;

        public EpisodeData(int numSteps, double sumReward, bool success)
        {
            this.numSteps = numSteps;
            this.sumReward = sumReward;
            this.success = success;
        }

        public int NumSteps
        {
            get { return numSteps; }
        }

        public double SumReward
        {
            get { return sumReward; }
        }

        public bool Success
        {
            get { return success; }
        }

        public static void Write(CSV csv, List<EpisodeData> episodeData)
        {

            foreach (EpisodeData e in episodeData)
            {
                csv.Add(e.NumSteps.ToString());
                csv.Add(e.SumReward.ToString());
                csv.Add(e.Success.ToString());
                csv.EndLine();

            }

            csv.Write();
            csv.Clear();
        }

    }
}
