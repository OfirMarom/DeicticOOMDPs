using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class Program
    {

        static void Main(string[] args)
        {
            string loadModelPath = @"PATH/model_"; // replace PATH with path of existing model. Set variable to null if you want to learn model from scratch.

            string saveModelPath = @"PATH/model_"; // replace PATH with path to save model. Set variable to null if you do not want to save model.

            CSV csv1 = new CSV(@"PATH\1.csv", ','); // replace PATH with path to save first result set. This result set show results for each test episode for each iteration.

            CSV csv2 = new CSV(@"PATH\2.csv", ',');  // replace PATH with path to save  second result set. This result sets averages the results of test episodes for each iteration.

            int numRuns = 20; // number of independant runs

            int numIterPerRun = 200; // number of iterations per run

            int maxSteps = 500; // maximum number of steps before run terminates

            int numTestInitStates = 10; // number of test states to test performance against after training

            
            for (int n = 0; n < numRuns; n++)
            {
                TransitionModel transitionModel;

                // load the transiton model if it exists, otherwise create a new model.

                if (loadModelPath == null)
                {
                    transitionModel = new TransitionModel(InitActionAttributeBNs.Init1);
                }
                else
                {
                    transitionModel = TransitionModel.Load(loadModelPath + n.ToString());
                }


                // Sample initial states randomly and add them to the test set. 

                RMax[] rMaxTests = new RMax[numTestInitStates];

                for (int i = 0; i < rMaxTests.Length; i++)
                {
                    State initState = States.SampleInitState();
                    RMaxTrainParams rMaxTestParams = new RMaxTrainParams(Rewards.ALLREWARDS.Max() + 0.01, 1, 1, 0.001, maxSteps);
                    rMaxTests[i] = new RMax(initState, transitionModel, rMaxTestParams);
                }
              

                for (int i = 0; i < numIterPerRun; i++)
                {
                    List<int> numEpisodeSteps = new List<int>();

                    State initState = States.SampleInitState(); // sample an initial state to train on.

                    // if this state already exists in the test set then continue - we don't want to train on a test state. 

                    if (rMaxTests.Any(x => States.IsEqual(x.Domain.InitState, initState)))
                    {
                        continue;
                    }


                    RMaxTrainParams rMaxTrainParams = new RMaxTrainParams(Rewards.ALLREWARDS.Max() + 0.01, 1, 1, 0.001, 50); // create the r-max paramaters
                    RMax rMax = new RMax(initState, transitionModel, rMaxTrainParams); // create the rmax object with the training initial state state

                   
                    rMax.Execute(1, null,false, true); // learn the model on this training task
                    

                    // evaluate performance on each of the test tasks.

                    for (int j = 0; j < rMaxTests.Length; j++)
                    {
                        RMax rMaxTest = rMaxTests[j];
                        rMaxTest.Execute(1, maxSteps, false, false);
                        numEpisodeSteps.Add(rMaxTest.EpisodeData[0].NumSteps);
                         EpisodeData.Write(csv1, rMaxTest.EpisodeData);
                    }

                    // write results to csv.
                    csv1.Add("END");
                    csv1.EndLine();
                    csv1.Write();
                    csv1.Clear();

                    csv2.Add(numEpisodeSteps.Average().ToString());
                    csv2.EndLine();
                    csv2.Write();
                    csv2.Clear();

                }

                csv1.Add("END END");
                csv1.EndLine();
                csv1.Write();
                csv1.Clear();


                csv2.Add("END");
                csv2.EndLine();
                csv2.Write();
                csv2.Clear();

                if (saveModelPath != null)
                {
                    transitionModel.Save(saveModelPath + n.ToString());
                }


            }
        }
    }
}
