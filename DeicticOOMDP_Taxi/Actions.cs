using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class Actions
    {
        public const int NORTH = 0;
        public const int EAST = 1;
        public const int SOUTH = 2;
        public const int WEST = 3;
        public const int PICKUP = 4;
        public const int DROPOFF = 5;

        public static readonly int[] ALLACTIONS;

        static Actions()
        {
            List<int> allActions = new List<int>();

            allActions.Add(NORTH);
            allActions.Add(EAST);
            allActions.Add(SOUTH);
            allActions.Add(WEST);
            allActions.Add(PICKUP);
            allActions.Add(DROPOFF);

            ALLACTIONS = allActions.ToArray();
        }

        public int Random()
        {
            int index = Global.Random.Next(0, ALLACTIONS.Length);
            return ALLACTIONS[index];
        }

        public int EpsilonGreedy(State state,QFunc qFunc, double exploreProp)
        {
            double u = Global.Random.NextDouble();

            if (u < exploreProp)
            {
                return Random();
            }
            else
            {
                return Greedy(state,qFunc);
            }
        }

        public int Greedy(State state, QFunc qFunc)
        {
            double? max = null;
            int? argmax = null;

            List<int> order = new List<int>();

            for (int i = 0; i < ALLACTIONS.Length; i++)
            {
                order.Add(i);
            }

            order = order.OrderBy(x => Global.Random.Next()).ToList();

            for (int i = 0; i < order.Count; i++)
            {
                int index = order[i];
                int action = ALLACTIONS[index];
                double val = qFunc.Get(state, action);

                if (argmax == null || val > max)
                {
                    max = val;
                    argmax = action;
                }
            }

            return (int)argmax;
        }

        public static int Count
        {
            get { return ALLACTIONS.Length; }
        }
    }
}
