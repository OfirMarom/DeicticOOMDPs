using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class EffectFuncs
    {
        public static int Absolute(int index, int val)
        {
            for (int i = 0; i <=10; i++)
            {
                if (i == index)
                {
                    return i+1;
                }
            }

            throw new Exception();
        }

        public static int Relative(int index, int val)
        {
            if (index == 0)
            {
                return val;
            }
            else if (index == 1)
            {
                return val = val + 1;
            }
            else if (index == 2)
            {
                return val = val - 1;
            }

            throw new Exception();
        }

        public static bool Flip(int index, bool val)
        {
            if (index == 0)
            {
                return val;
            }
            else if (index == 1)
            {
                return !val;
            }

            throw new Exception();
        }

        public static bool SetBool(int index, bool val)
        {
            if (index == 0)
            {
                return false;
            }
            else if (index == 1)
            {
                return true;
            }

            throw new Exception();
        }
    }
}
