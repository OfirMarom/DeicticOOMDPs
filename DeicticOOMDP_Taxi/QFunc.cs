using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class QFunc
    {
        MultiLevelDictionary<int, double> q = new MultiLevelDictionary<int, double>();
        Func<State, int, double> defaultVal;
        States states;

        public int[] StateActionKey(State state, int action)
        {
            int stateIndex = states.GetStateIndex(state);
            return new int[] { action, stateIndex };
        }

        public QFunc(States states, Func<State, int, double> defaultVal)
        {
            this.states = states;
            this.defaultVal = defaultVal;
        }

        public double Max(State state)
        {
            double? max = null;

            foreach (int action in Actions.ALLACTIONS)
            {
                double val = Get(state, action);

                if (max == null || val > max)
                {
                    max = val;
                }
            }

            return (double)max;
        }

        public double Get(State state, int action)
        {
            int[] key = StateActionKey(state, action);

            if (States.IsTerminal(state))
            {
                q[key] = 0;
            }

            if (!q.ContainsKey(key))
            {
                q[key] = defaultVal(state, action);
            }

            return q[key];
        }

        public void Set(State state, int action, double val)
        {
            int[] key = StateActionKey(state, action);
            q[key] = val;
        }

    }
}
