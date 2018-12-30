using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class Rewards
    {
        public const double TERMINALREWARD = 0;
        public const double STEPREWARD = -1;
        public const double INCORRECTPICKUPDROPOFFREWARD = -10;

        public static double[] ALLREWARDS;
      
        static Rewards()
        {
            List<double> allRewards = new List<double>();
            allRewards.Add(TERMINALREWARD);
            allRewards.Add(STEPREWARD);
            allRewards.Add(INCORRECTPICKUPDROPOFFREWARD);
        
            ALLREWARDS = allRewards.ToArray();
        }

        public Rewards()
        {
        }

        public double Get(State state,int action,State newState)
        {
            if(States.IsTerminal(newState))
            {
                return TERMINALREWARD;
            }

            if (action == Actions.PICKUP && !CondFuncs.TaxiOnAnyPassenger(null,state))
            {
                return INCORRECTPICKUPDROPOFFREWARD;
            }

            if (action == Actions.DROPOFF && !CondFuncs.TaxiOnAnyDestination(null, state) && ! CondFuncs.AnyPassengerInTaxi(null,state))
            {
                return INCORRECTPICKUPDROPOFFREWARD;
            }


            return STEPREWARD;


        }
    }
}
