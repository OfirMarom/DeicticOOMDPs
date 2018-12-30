using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    class Global
    {
        private static Random random = new Random();

        public static Random Random
        {
            get { return random; }
        }

        public static void SerializeObject<T>(T serializableObject, string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, serializableObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T DeserializeObject<T>(string path)
        {
            try
            {

                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    T obj = (T)formatter.Deserialize(stream);
                    stream.Close();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static void BoolPermitations(List<bool> container, ref List<bool[]> vals, int depth, int maxDepth)
        {
            if (depth == maxDepth)
            {
                vals.Add(container.ToArray());
                return;
            }

            foreach (bool b in new bool[] { true, false })
            {
                List<bool> containerNew = container.ToList();
                containerNew.Add(b);
                BoolPermitations(containerNew, ref vals, depth + 1, maxDepth);
            }
        }

        public static T[] FilterByBoolArr<T>(T[] arr, bool[] bools)
        {
            List<T> l = new List<T>();

            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i] == true)
                {
                    l.Add(arr[i]);
                }
            }

            return l.ToArray();
        }

        //https://stackoverflow.com/questions/5015593/how-to-replace-part-of-string-by-position

        public static string ReplaceStingAtIndex(string originalString, string replaceString, int replaceIndex)
        {
            return originalString.Remove(replaceIndex, replaceString.Length).Insert(replaceIndex, replaceString);
        }

        public static T[] DeepCopyArray<T>(T[] arr)
        {
            if (arr == null)
            {
                return null;
            }

            T[] arrCopy = new T[arr.Length];

            Array.Copy(arr, arrCopy, arr.Length);

            return arrCopy;

        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static T[][] ListOfListsToJaggedArray<T>(List<List<T>> list)
        {
            if (list == null)
            {
                return null;
            }

            T[][] arr = new T[list.Count][];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = list[i].ToArray();
            }

            return arr;
        }

        public static bool IsDictionary(object o)
        {
            Type t = o.GetType();
            bool isDict = t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
            return isDict;
        }

        public static T[] Sample<T>(T[] arr, int numSample, bool replace)
        {
            if (replace == false && numSample > arr.Length)
            {
                return arr;
            }

            T[] arrNew = new T[numSample];

            int count = 0;

            while (count < numSample)
            {
                T t = arr[random.Next(0, arr.Length)];

                if (replace == false && arrNew.Contains(t))
                {
                    continue;
                }

                arrNew[count] = t;

                count++;
            }

            return arrNew;
        }
    }
}
