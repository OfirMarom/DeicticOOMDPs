using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    
    class State
    {
        Taxis taxis;
        Walls walls;
        Passengers passengers;
        Destinations destinations;

        public State(Taxis taxis, Walls walls, Passengers passengers, Destinations destinations)
        {
            this.taxis = taxis;
            this.walls = walls;
            this.passengers = passengers;
            this.destinations = destinations;
        }

        public int[] ToArray()
        {         
            return taxis.ToArray().Concat(passengers.ToArray()).Concat(destinations.ToArray()).Concat(walls.ToArray()).ToArray();
        }


        public object GetObjectByIndex(int index)
        {
            int count = 0;

            for (int i = 0; i < taxis[true].Length; i++)
            {
                if(index==count)
                {
                    return taxis[i];
                }
                count++;
            }

            for (int i = 0; i < walls[true].Length; i++)
            {
                if (index == count)
                {
                    return walls[i];
                }
                count++;
            }

            for (int i = 0; i < passengers[true].Length; i++)
            {
                if (index == count)
                {
                    return passengers[i];
                }
                count++;
            }

            for (int i = 0; i < destinations[true].Length; i++)
            {
                if (index == count)
                {
                    return destinations[i];
                }
                count++;
            }

            return null;
        }

        public State Copy()
        {
            return new State(taxis.Copy(), walls.Copy(), passengers.Copy(), destinations.Copy());
        }

        public Taxis Taxis
        {
            get { return taxis; }
        }

        public Walls Walls
        {
            get { return walls; }
        }

        public Passengers Passengers
        {
            get { return passengers; }
        }

        public Destinations Destinations
        {
            get { return destinations; }
        }

        public int CountObjects
        {
            get { return taxis.Length + walls.Length + passengers.Length + destinations.Length; }
        }

    }
}
