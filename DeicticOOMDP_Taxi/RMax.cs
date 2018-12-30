using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    public class RMaxTrainParams
    {
        double discountFactor;
        int m;
        double thresh;
        double rmax;
        int? valueIterationFreq;


        public RMaxTrainParams(double rmax, double discountFactor, int m, double thresh, int? valueIterationFreq)
        {
            this.discountFactor = discountFactor;
            this.m = m;
            this.thresh = thresh;
            this.rmax = rmax;
            this.valueIterationFreq = valueIterationFreq;

        }

        public double DiscountFactor
        {
            get { return discountFactor; }
        }

        public int M
        {
            get { return m; }
        }

        public double Thresh
        {
            get { return thresh; }
        }

        public double Rmax
        {
            get { return rmax; }
        }

        public int? ValueIterationFreq
        {
            get { return valueIterationFreq; }
        }
    }


    class RMax
    {
        QFunc qFunc;
        RMaxTrainParams trainParams;
        Rewards rewards;
        TransitionModel transitionModel;
        Actions actions;
        List<EpisodeData> episodeData;
        int totalSteps = 0;
        MultiLevelDictionary<int, Dictionary<State, double>> cache;
        bool allStateActionPairsKnown;
        States states;
        Domain domain;

        public RMax(State initState,TransitionModel transitionModel, RMaxTrainParams trainParams)
        {
            rewards = new Rewards();
            this.trainParams = trainParams;
            this.transitionModel = transitionModel;
            actions = new Actions();
            cache = new MultiLevelDictionary<int, Dictionary<State, double>>();
            states = new States(initState);
            domain = new Domain(initState);
            qFunc = new QFunc(states, delegate (State state, int action) { return 0; });

        }

        private int ValueIteration()
        {
            MultiLevelDictionary<int, Dictionary<State, double>> localCache = new MultiLevelDictionary<int, Dictionary<State, double>>();

            int count = 0;

            bool allStateActionPairsKnown = true;

            while (true)
            {
                count++;
           
                double maxDiff = 0;

                int countStates = 0;

                foreach (State state in states.AllStates)
                {

                    countStates++;
                  
                    if (States.IsTerminal(state))
                    {
                        continue;
                    }

                    foreach (int action in Actions.ALLACTIONS)
                    {

                        Dictionary<State, double> nextStateDistribution = null;

                        int[] cacheKey = qFunc.StateActionKey(state, action);

                      
                        if (cache.ContainsKey(cacheKey))
                        {
                            nextStateDistribution = cache[cacheKey];
                        }
                        else if (localCache.ContainsKey(cacheKey))
                        {
                            nextStateDistribution = localCache[cacheKey];
                        }
                        else
                        {
                            nextStateDistribution = transitionModel.NextStateDistribution(state, action, trainParams.M,states);
                            
                            if (nextStateDistribution != null)
                            {
                                cache[cacheKey] = nextStateDistribution;
                            }else
                            {
                                localCache[cacheKey] = nextStateDistribution;                              
                                allStateActionPairsKnown = false;
                            }
                        }

                        double currentVal = qFunc.Get(state, action);

                        double newVal = 0;

                        if (nextStateDistribution == null)
                        {
                             newVal = trainParams.Rmax;
                        }
                        else
                        {
                            foreach (KeyValuePair<State, double> kv in nextStateDistribution)
                            {
                                newVal += kv.Value * (rewards.Get(state,action,kv.Key) + trainParams.DiscountFactor * qFunc.Max(kv.Key));
                            }
                        }

                        qFunc.Set(state, action, newVal);

                        maxDiff = Math.Max(maxDiff, Math.Abs(newVal - currentVal));

                    }
                }

                if (maxDiff < trainParams.Thresh)
                {
                    break;
                }
            }

            this.allStateActionPairsKnown = allStateActionPairsKnown;

            return count;
        }

        public QFunc QFunc
        {
            get { return qFunc; }
        }

        public void Execute(int numEpisodes, int? maxStepsPerEpisode = null, bool draw = false, bool updateTransitonModel = false)
        {
            episodeData = new List<EpisodeData>();

            for (int i = 0; i < numEpisodes; i++)
            {
                bool success = true;

                int numSteps = 0;

                double sumReward = 0;

                domain.Reset();

                if (draw) { Console.WriteLine("---------------------"); }

                if (draw) { domain.Print(); }

                State state = domain.State.Copy();

                while (!States.IsTerminal(state))
                {
                    numSteps++;

                    if (maxStepsPerEpisode != null && numSteps > maxStepsPerEpisode)
                    {
                        success = false;
                        numSteps--;
                        break;
                    }

                    totalSteps++;

                    int action = actions.Greedy(state, qFunc);

                    domain.TakeAction(action);

                    if (draw) { domain.Print(); }

                    State newState = domain.State.Copy();

                    double reward = rewards.Get(state, action, newState);

                    sumReward += reward;

                    if (updateTransitonModel == true)
                    {
                        transitionModel.Update(action, state, newState);
                    }

                    if (allStateActionPairsKnown == false)
                    {
                        if (trainParams.ValueIterationFreq == null || (numSteps - 1) % trainParams.ValueIterationFreq == 0)
                        {
                            ValueIteration();
                        }
                    }

                    state = newState;
                }

                episodeData.Add(new EpisodeData(numSteps, sumReward, success));
            }
        }

        public List<EpisodeData> EpisodeData
        {
            get { return episodeData; }
        }
        
        public bool AllStateActionPairsKnown
        {
            get { return allStateActionPairsKnown; }
        }

        public Domain Domain
        {
            get { return domain; }
        }
    }
}
