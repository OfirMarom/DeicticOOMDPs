using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    public class TypePropertyPair
    {
        Type type;
        PropertyInfo propertyInfo;

        public TypePropertyPair(Type type, PropertyInfo propertyInfo)
        {
            this.type = type;
            this.propertyInfo = propertyInfo;
        }

        public Type Type
        {
            get { return type; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
        }
    }

    class States
    {
        public static Type[] CLASSTYPES;
        State[] allStates;
        MultiLevelDictionary<int, int> stateIndexes;


        static States()
        {
            SetClassTypes();
        }

        private static void SetClassTypes()
        {
            List<Type> classTypes = new List<Type>();
            classTypes.Add(typeof(Taxi));
            classTypes.Add(typeof(Wall));
            classTypes.Add(typeof(Passenger));
            classTypes.Add(typeof(Destination));
            CLASSTYPES = classTypes.ToArray();
        }

    
        
        public static State SampleInitState()
        {
            int width = 5;

            int length = 5;

            int numPassengers = 2;

            int numDestinations = 1;

            List<BaseObject> validLocations = new List<BaseObject>();
            validLocations.Add(new BaseObject(1, 1));
            validLocations.Add(new BaseObject(5, 1));
            validLocations.Add(new BaseObject(1, 5));
            validLocations.Add(new BaseObject(5, 5));
            validLocations.Add(new BaseObject(3, 4));
            validLocations.Add(new BaseObject(4, 3));



            Wall[][] nonPerimeterWallsArr = new Wall[4][];

            nonPerimeterWallsArr[0] = new Wall[]
            {
               new  Wall(2, 3, Wall.NORTH),
               new  Wall(3, 3, Wall.NORTH),
               new  Wall(2, 2, Wall.NORTH),
               new  Wall(3, 2, Wall.NORTH),
               new  Wall(2, 4, Wall.NORTH),
               new  Wall(3, 4, Wall.NORTH),

            };


            

            nonPerimeterWallsArr[1] = new Wall[]
            {
               new  Wall(2, 5, Wall.EAST),
               new  Wall(2, 4, Wall.EAST),
               new  Wall(3, 5, Wall.EAST),
               new  Wall(3, 4, Wall.EAST),
               new  Wall(2, 2, Wall.NORTH),
               new  Wall(3, 2, Wall.NORTH),

            };
            nonPerimeterWallsArr[2] = new Wall[]
         {
               new  Wall(1, 5, Wall.SOUTH),
               new  Wall(2, 5, Wall.SOUTH),
               new  Wall(5, 3, Wall.NORTH),
               new  Wall(4, 3, Wall.NORTH),
               new  Wall(5, 3, Wall.SOUTH),
               new  Wall(4, 3, Wall.SOUTH),

         };

            nonPerimeterWallsArr[3] = new Wall[]
            {
                new  Wall(1, 1, Wall.EAST),
                new Wall(1, 2, Wall.EAST),
                new Wall(3, 1, Wall.EAST),
                new Wall(3, 2, Wall.EAST),
                new Wall(3, 5, Wall.WEST),
                new Wall(3, 4, Wall.WEST)
            };


            int taxiX = Global.Random.Next(0, width) + 1;

            int taxiY = Global.Random.Next(0, length) + 1;

            Wall[] nonPerimeterWalls = Global.Sample(nonPerimeterWallsArr, 1, false)[0];

            BaseObject[] destinationLocations = Global.Sample(validLocations.ToArray(), numDestinations, false);

            for(int i=validLocations.Count-1;i>=0;i--)
            {
                if (destinationLocations.Any(x => x.EqualXY(validLocations[i])))
                {
                    validLocations.Remove(validLocations[i]);
                }
            }

            BaseObject[] passengerLocations = Global.Sample(validLocations.ToArray(), numPassengers, false);

            return InitState(width, length, nonPerimeterWalls, passengerLocations, destinationLocations, taxiX, taxiY);

        }
        

        private static State InitState(int width, int length, Wall[] nonPerimiterWalls, BaseObject[] passegenerLocations, BaseObject[] destinationLocations, int taxiX, int taxiY)
        {

            Taxi taxi = new Taxi(taxiX, taxiY);

            List<Wall> walls = new List<Wall>();

            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y <= length; y++)
                {
                    if (x == 1)
                    {
                        walls.Add(new Wall(x, y, Wall.WEST));
                    }

                    if (x == width)
                    {
                        walls.Add(new Wall(x, y, Wall.EAST));
                    }

                    if (y == 1)
                    {
                        walls.Add(new Wall(x, y, Wall.SOUTH));
                    }

                    if (y == length)
                    {
                        walls.Add(new Wall(x, y, Wall.NORTH));
                    }
                }
            }

            foreach (Wall wall in nonPerimiterWalls)
            {
                walls.Add(wall);
            }


            List<Passenger> passengers = new List<Passenger>();

            foreach (var passengerLocation in passegenerLocations)
            {
                passengers.Add(new Passenger(passengerLocation.X, passengerLocation.Y, false, false));
            }

            List<Destination> destinations = new List<Destination>();

            foreach (var destinationLocation in destinationLocations)
            {
                destinations.Add(new Destination(destinationLocation.X, destinationLocation.Y));
            }

            State initState = new State(new Taxis(new Taxi[] { taxi }),
                new Walls(walls.ToArray()),
                new Passengers(passengers.ToArray()),
                new Destinations(destinations.ToArray()));

            return initState;
        }


        private void SetAllStates(State initState)
        {
            
            int width = initState.Walls[true].Max(w => w.X);

            int length = initState.Walls[true].Max(w => w.Y);

            List<State> allStates = new List<State>();

            List<bool[]> atDestinationVals = new List<bool[]>();

            Global.BoolPermitations(new List<bool>(), ref atDestinationVals, 0, initState.Passengers.Length);

            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y <= length; y++)
                {
                    foreach (bool[] atDestinationVal in atDestinationVals)
                    {
                        int[] indexesOfFalse = Enumerable.Range(0, atDestinationVal.Length).Where(idx => atDestinationVal[idx] == false).ToArray();

                        indexesOfFalse = indexesOfFalse.Concat(new int[] { -1 }).ToArray();

                        foreach (int indexOfFalse in indexesOfFalse)
                        {
                            Passenger[] passengers = new Passenger[initState.Passengers.Length];

                            for (int i = 0; i < passengers.Length; i++)
                            {
                                bool inTaxi = false;

                                if (indexOfFalse == i)
                                {
                                    inTaxi = true;
                                }

                                passengers[i] = new Passenger(initState.Passengers[i].X,
                                    initState.Passengers[i].Y,
                                    inTaxi,
                                    atDestinationVal[i]);
                            }

                            
                            Taxi taxi = new Taxi(x, y);


                            State state = new State(new Taxis(new Taxi[] { taxi}),
                               initState.Walls.Copy(),
                               new Passengers(passengers.ToArray()),
                               initState.Destinations.Copy());

                            if (IsTerminal(state) && !CondFuncs.TaxiOnAnyDestination(null, state))
                            {
                                continue;
                            }

                            allStates.Add(state);

                        }
                    }
                }
            }

            this.allStates = allStates.ToArray();


            stateIndexes = new MultiLevelDictionary<int, int>();

            for (int i = 0; i < this.allStates.Length; i++)
            {
                stateIndexes[allStates[i].ToArray()] = i;
            }
        }


        public State GetStateFromAllStates(State state)
        {
            return allStates[GetStateIndex(state)];
        }

        public int GetStateIndex(State state)
        {
            return stateIndexes[state.ToArray()];
        }

        public States(State initState)
        {
            SetAllStates(initState);
        }


        public static TypePropertyPair[] GetClassAttributesPairs()
        {
            List<TypePropertyPair> pairs = new List<TypePropertyPair>();

            foreach (Type classType in CLASSTYPES)
            {
                PropertyInfo[] classAttributes = classType.GetProperties();

                foreach (PropertyInfo classAttribute in classAttributes)
                {
                    TypePropertyPair pair = new TypePropertyPair(classType, classAttribute);
                    pairs.Add(pair);
                }
            }

            return pairs.ToArray();
        }

        public static bool IsEqual(State state1, State state2)
        {
            int[] state1Arr = state1.ToArray();

            int[] state2Arr = state2.ToArray();

            for (int i = 0; i < state1Arr.Length; i++)
            {
                if (state1Arr[i] != state2Arr[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsTerminal(State state)
        {
            return CondFuncs.AllPassenagersAtDestination(null,state);
        }

        public State[] AllStates
        {
            get { return allStates; }
        }

        public int Count
        {
            get { return allStates.Length; }
        }
    }
}
