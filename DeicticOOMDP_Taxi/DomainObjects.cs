using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    public class BaseObject
    {
        protected int x;
        protected int y;

        public BaseObject(int x,int y)
        {
            Set(x, y);
        }

        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool EqualXY(BaseObject obj)
        {
            return x == obj.x && y == obj.y;
        }

        public bool EqualXY(int x, int y)
        {
            return this.x == x && this.y == y;
        }
  
    }

    public class Taxi : BaseObject
    {

        public Taxi(int x, int y):base(x,y)
        {
        }

        public Taxi Copy()
        {
            return new Taxi(x, y);
        }

        public int[] ToArray()
        {
            return new int[] { x, y };
        }
    }

    public class Taxis
    {
        Taxi[] taxis;

        public Taxis(Taxi[] taxis)
        {
            this.taxis = taxis;
        }

        public Taxi this[int i]
        {
            get { return taxis[i]; }
            set { taxis[i] = value; }
        }

        public Taxi[] this[bool b]
        {
            get { return taxis; }
        }

        public void Order()
        {
            taxis = taxis.OrderBy(x => x.X).ThenBy(x => x.Y).ToArray();
        }

        public int[] ToArray()
        {
            List<int[]> arrs = new List<int[]>();

            foreach(Taxi taxi in taxis)
            {
                arrs.Add(taxi.ToArray());
            }

            return arrs.SelectMany(x => x).ToArray();
        }

        public int Length
        {
            get { return taxis.Length; }
        }

        public Taxis Copy()
        {
            Taxi[] taxis = new Taxi[this.Length];

            for (int i = 0; i < taxis.Length; i++)
            {
                taxis[i] = this[i].Copy();
            }

            return new Taxis(taxis);
        }

    }

    public class Destination : BaseObject
    {

        public Destination(int x, int y) : base(x, y)
        {
        }

        public Destination Copy()
        {
            return new Destination(x, y);
        }

        public int[] ToArray()
        {
            return new int[] { x, y };
        }
    }

    public class Destinations
    {
        Destination[] destinations;

        public Destinations(Destination[] destinations)
        {
            this.destinations = destinations;
        }

        public Destination this[int i]
        {
            get { return destinations[i]; }
            set { destinations[i] = value; }

        }

        public Destination[] this[bool b]
        {
            get { return destinations; }
        }

        public void Order()
        {
            destinations = destinations.OrderBy(x => x.X).ThenBy(x => x.Y).ToArray();
        }

        public int[] ToArray()
        {
            List<int[]> arrs = new List<int[]>();

            foreach (Destination destination in destinations)
            {
                arrs.Add(destination.ToArray());
            }

            return arrs.SelectMany(x => x).ToArray();
        }

        public int Length
        {
            get { return destinations.Length; }
        }

        public Destinations Copy()
        {
            Destination[] destinations = new Destination[this.Length];

            for (int i = 0; i < destinations.Length; i++)
            {
                destinations[i] = this[i].Copy();
            }

            return new Destinations(destinations);
        }

    }


    public class Passenger : BaseObject
    {
        bool inTaxi;
        bool atDestination;

        public Passenger(int x, int y, bool inTaxi, bool atDestination) : base(x, y)
        {
            Set(inTaxi, atDestination);
        }

        public void Set( bool inTaxi, bool atDestination)
        {
            this.inTaxi = inTaxi;
            this.atDestination = atDestination;
        }

        public bool InTaxi
        {
            get { return inTaxi; }
            set { inTaxi = value; }
        }

        public bool AtDestination
        {
            get { return atDestination; }
            set { atDestination = value; }
        }

        public Passenger Copy()
        {
            return new Passenger(x, y, inTaxi, atDestination);
        }

        public int[] ToArray()
        {
            return new int[] { x, y, Convert.ToInt32(inTaxi), Convert.ToInt32(AtDestination) };
        }
    }

    public class Passengers
    {
        Passenger[] passengers;

        public Passengers(Passenger[] passengers)
        {
            this.passengers = passengers;
        }

        public Passenger this[int i]
        {
            get { return passengers[i]; }
            set { passengers[i] = value; }

        }

        public Passenger[] this[bool b]
        {
            get { return passengers; }
        }

        public void Order()
        {
            passengers = passengers.OrderBy(x => x.X).ThenBy(x => x.Y).ThenBy(x => x.InTaxi).ThenBy(x => x.AtDestination).ToArray();
        }

        public int[] ToArray()
        {
            List<int[]> arrs = new List<int[]>();

            foreach (Passenger passenger in passengers)
            {
                arrs.Add(passenger.ToArray());
            }

            return arrs.SelectMany(x => x).ToArray();
        }

        public int Length
        {
            get { return passengers.Length; }
        }

        public Passengers Copy()
        {
            Passenger[] passengers = new Passenger[this.Length];

            for (int i = 0; i < passengers.Length; i++)
            {
                passengers[i] = this[i].Copy();
            }

            return new Passengers(passengers);
        }
    }

    public class Wall : BaseObject
    {

        public const int NORTH = 0;
        public const int EAST = 1;
        public const int SOUTH = 2;
        public const int WEST = 3;


        int pos;

        public Wall(int x, int y, int pos) : base(x, y)
        {
            Set(pos);
        }

        public void Set(int pos)
        {
            this.pos = pos;
        }

        public int Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Wall Copy()
        {
            return new Wall(x, y, pos);
        }

        public  int[] ToArray()
        {
            return new int[] { x, y, pos };
        }
    }

    public class Walls
    {
        Wall[] walls;

        public Walls(Wall[] walls)
        {
            this.walls = walls;
        }

        public Wall this[int i]
        {
            get { return walls[i]; }
            set { walls[i] = value; }

        }

        public Wall[] this[bool b]
        {
            get { return walls; }
        }

        public void Order()
        {
            walls = walls.OrderBy(x => x.X).ThenBy(x => x.Y).ThenBy(x => x.Pos).ToArray();
        }

        public int[] ToArray()
        {
            List<int[]> arrs = new List<int[]>();

            foreach (Wall wall in walls)
            {
                arrs.Add(wall.ToArray());
            }

            return arrs.SelectMany(x => x).ToArray();
        }

        public int Length
        {
            get { return walls.Length; }
        }

        public Walls Copy()
        {
            Wall[] walls = new Wall[this.Length];

            for (int i = 0; i < walls.Length; i++)
            {
                walls[i] = this[i].Copy();
            }

            return new Walls(walls);
        }
    }
}
