using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class Domain
    {

        int width;
        int length;
        State state;
        Taxi taxi;
        Walls walls;
        Passengers passengers;
        Destinations destinations;
        State initState;
       

        public Domain(State initState)
        {
            this.initState = initState;
            Reset();
        }


        public void Reset()
        {
            this.state = initState.Copy();
            this.taxi = state.Taxis[0];
            this.walls = state.Walls;
            this.passengers = state.Passengers;
            this.destinations = state.Destinations;
            this.width = walls[true].Max(w => w.X);
            this.length = walls[true].Max(w => w.Y);
        }

        private bool AnyWallAtCoordAndPos(int x, int y, int wallPos)
        {
            return walls[true].Any(w => w.EqualXY(x, y) && w.Pos == wallPos);
        }

        private bool AnyPassengerAtCoordAndNotInTaxiAndNotAtDestination(int x, int y)
        {
            return passengers[true].Any(p => p.EqualXY(x, y) && p.InTaxi == false && p.AtDestination == false);
        }

        private bool AnyDestinationAtCoord(int x, int y)
        {
            return destinations[true].Any(d => d.EqualXY(x, y));
        }

        private bool IsWallBlockingTaxi(int action)
        {
            switch (action)
            {
                case Actions.NORTH:
                    {
                        return CondFuncs.WallNorthOfMe(taxi, state);
                    }
                case Actions.EAST:
                    {
                        return CondFuncs.WallEastOfMe(taxi, state);
                    }

                case Actions.SOUTH:
                    {
                        return CondFuncs.WallSouthOfMe(taxi, state);
                    }
                case Actions.WEST:
                    {
                        return CondFuncs.WallWestOfMe(taxi, state);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        public void TakeAction(int action)
        {
            if (IsWallBlockingTaxi(action))
            {
                return;
            }

            switch (action)
            {
                case Actions.NORTH:
                    {
                        taxi.Set(taxi.X, taxi.Y + 1);
                        break;
                    }
                case Actions.EAST:
                    {
                        taxi.Set(taxi.X + 1, taxi.Y);
                        break;
                    }
                case Actions.SOUTH:
                    {
                        taxi.Set(taxi.X, taxi.Y - 1);
                        break;
                    }
                case Actions.WEST:
                    {
                        taxi.Set(taxi.X - 1, taxi.Y);
                        break;
                    }
                case Actions.PICKUP:
                    {
                        foreach (Passenger p in passengers[true])
                        {
                            if (CondFuncs.TaxiOnMe(p, state) && !CondFuncs.AnyPassengerInTaxi(null, state) && !CondFuncs.AtDestinationMe(p, state))
                            {
                                p.Set(true, p.AtDestination);
                            }
                        }
                        break;
                    }
                case Actions.DROPOFF:
                    {
                        foreach (Passenger p in passengers[true])
                        {
                            if (CondFuncs.TaxiOnAnyDestination(null, state) && CondFuncs.InTaxiMe(p, state))
                            {
                                p.Set(false, true);
                            }
                        }
                        break;
                    }
            }
        }

        public void Print()
        {
            for (int y = length; y >= 1; y--)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int x = 1; x <= width; x++)
                    {
                        string s = "---  ";

                        if (i == 1)
                        {
                            s = Global.ReplaceStingAtIndex(s, "O", 1);
                        }

                        if (i == 0 && AnyWallAtCoordAndPos(x, y, Wall.NORTH))
                        {
                            s = Global.ReplaceStingAtIndex(s, "W", 1);
                        }

                        if (i == 2 && AnyWallAtCoordAndPos(x, y, Wall.SOUTH))
                        {
                            s = Global.ReplaceStingAtIndex(s, "W", 1);
                        }

                        if (i == 1 && AnyWallAtCoordAndPos(x, y, Wall.WEST))
                        {
                            s = Global.ReplaceStingAtIndex(s, "W", 0);
                        }

                        if (i == 1 && AnyWallAtCoordAndPos(x, y, Wall.EAST))
                        {
                            s = Global.ReplaceStingAtIndex(s, "W", 2);
                        }

                        if (i == 1 && AnyDestinationAtCoord(x, y))
                        {
                            s = Global.ReplaceStingAtIndex(s, "D", 1);
                        }

                        if (i == 1 && AnyPassengerAtCoordAndNotInTaxiAndNotAtDestination(x, y))
                        {
                            s = Global.ReplaceStingAtIndex(s, "P", 1);
                        }

                        if (i == 1 && taxi.EqualXY(x, y))
                        {
                            s = Global.ReplaceStingAtIndex(s, "T", 1);
                        }

                        Console.Write(s);

                    }

                    Console.WriteLine();
                }
                Console.WriteLine();

            }

            Console.WriteLine("-------------------------------------------------------");
        }

        public State State
        {
            get { return state; }
        }

        public State InitState
        {
            get { return initState; }
        }
    }
}
