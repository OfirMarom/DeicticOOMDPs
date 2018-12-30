using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class CondFuncs
    {


        public static Dictionary<string, bool> GetCondFuncCache(State state)
        {
            Dictionary<string, bool> condFuncCache = new Dictionary<string, bool>();

            condFuncCache = new Dictionary<string, bool>();

            Func<BaseObject, State, bool> condFunc;

            condFunc = TaxiOnAnyDestination;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = AnyPassengerInTaxi;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = TaxiOnAnyPassenger;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = AllPassenagersAtDestination;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = TaxiNorthOfAnyWall;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = TaxiEastOfAnyWall;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = TaxiSouthOfAnyWall;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            condFunc = TaxiWestOfAnyWall;
            condFuncCache[condFunc.Method.Name] = condFunc(null, state);

            return condFuncCache;

        }
        
        public static bool WallNorthOfMe(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(me.X, me.Y) && w.Pos == Wall.NORTH) || state.Walls[true].Any(w => w.EqualXY(me.X, me.Y + 1) && w.Pos == Wall.SOUTH);
        }

        public static bool WallEastOfMe(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(me.X, me.Y) && w.Pos == Wall.EAST) || state.Walls[true].Any(w => w.EqualXY(me.X+1, me.Y) && w.Pos == Wall.WEST);
        }

        public static bool WallSouthOfMe(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(me.X, me.Y) && w.Pos == Wall.SOUTH) || state.Walls[true].Any(w => w.EqualXY(me.X, me.Y - 1) && w.Pos == Wall.NORTH);
        }

        public static bool WallWestOfMe(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(me.X, me.Y) && w.Pos == Wall.WEST) || state.Walls[true].Any(w => w.EqualXY(me.X -1, me.Y) && w.Pos == Wall.EAST);
        }

        public static bool TaxiOnMe(BaseObject me, State state)
        {
            return state.Taxis[0].EqualXY(me);
        }

        public static bool AtDestinationMe(Passenger me, State state)
        {
            return me.AtDestination == true;
        }

        public static bool InTaxiMe(Passenger me, State state)
        {
            return me.InTaxi == true;
        }

        public static bool TaxiOnAnyDestination(BaseObject me, State state)
        {
            return state.Destinations[true].Any(d => d.EqualXY(state.Taxis[0]));
        }

        public static bool AnyPassengerInTaxi(BaseObject me, State state)
        {
            return state.Passengers[true].Any(x => x.InTaxi == true);
        }

        public static bool TaxiOnAnyPassenger(BaseObject me, State state)
        {
            return state.Passengers[true].Any(x => x.EqualXY(state.Taxis[0]));
        }

        public static bool AllPassenagersAtDestination(BaseObject me,State state)
        {
            return state.Passengers[true].All(x => x.AtDestination == true);
        }

        public static bool TaxiNorthOfAnyWall(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X, state.Taxis[0].Y) && w.Pos == Wall.NORTH) || state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X, state.Taxis[0].Y + 1) && w.Pos == Wall.SOUTH);
        }

        public static bool TaxiEastOfAnyWall(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X, state.Taxis[0].Y) && w.Pos == Wall.EAST) || state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X + 1, state.Taxis[0].Y) && w.Pos == Wall.WEST);
        }

        public static bool TaxiSouthOfAnyWall(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X, state.Taxis[0].Y) && w.Pos == Wall.SOUTH) || state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X, state.Taxis[0].Y - 1) && w.Pos == Wall.NORTH);
        }

        public static bool TaxiWestOfAnyWall(BaseObject me, State state)
        {
            return state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X, state.Taxis[0].Y) && w.Pos == Wall.WEST) || state.Walls[true].Any(w => w.EqualXY(state.Taxis[0].X - 1, state.Taxis[0].Y) && w.Pos == Wall.EAST);
        }


    }
}
