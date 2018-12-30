using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{

    [Serializable]
    class MultiLevelDictionary<K, V> where K : struct
    {

        Dictionary<K, object> dicts = new Dictionary<K, object>();
        object emptyKeyValue = null;

        public MultiLevelDictionary()
        {
        }

        private void GetKeysValues(Dictionary<K, object> dict, List<K> currentKey, ref List<Tuple<List<K>, V>> kvs)
        {
            foreach (K k in dict.Keys)
            {
                List<K> newKey = new List<K>(currentKey);
                newKey.Add(k);

                if (Global.IsDictionary(dict[k]))
                {
                    GetKeysValues((Dictionary<K, object>)dict[k], newKey, ref kvs);
                }
                else
                {
                    kvs.Add(new Tuple<List<K>, V>(newKey, (V)dict[k]));
                }
            }
        }

        public Dictionary<K[],V> Flatten()
        {
            List<Tuple<List<K>, V>> kvs = new List<Tuple<List<K>, V>>();
            GetKeysValues(this.dicts, new List<K>(), ref kvs);
            if (emptyKeyValue != null)
            {
                kvs.Add(new Tuple<List<K>, V>(new List<K>() { }, (V)emptyKeyValue));
            }

            Dictionary<K[], V> flattenedDict = new Dictionary<K[], V>();

            foreach(var kv in kvs)
            {
                flattenedDict[kv.Item1.ToArray()] = kv.Item2;
            }

            return flattenedDict;
        }


        

        private Dictionary<K, object> GetDict(K[] key, bool set)
        {
            if (key.Length == 0)
            {
                return null;
            }

            Dictionary<K, object> dict = dicts;

            for (int i = 0; i < key.Length - 1; i++)
            {
                K k = key[i];

                if (!dict.ContainsKey(k))
                {
                    if (set == true)
                    {
                        dict[k] = new Dictionary<K, object>();
                    }
                    else
                    {
                        return null;
                    }
                }

                dict = (Dictionary<K, object>)dict[k];
            }

            return dict;
        }

        public bool ContainsKey(K[] key)
        {
            if (key.Length == 0)
            {
                if (emptyKeyValue == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            Dictionary<K, object> dict = GetDict(key, false);

            if (dict == null || !dict.ContainsKey(key.Last()))
            {
                return false;
            }

            return true;
        }

        public V this[K[] key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException();
                }

                if (key.Length == 0)
                {
                    return (V)emptyKeyValue;
                }

                Dictionary<K, object> dict = GetDict(key, false);

                return (V)dict[key.Last()];
            }

            set
            {
                if (key.Length == 0)
                {
                    emptyKeyValue = value;
                }
                else
                {
                    Dictionary<K, object> dict = GetDict(key, true);
                    dict[key.Last()] = value;
                }
            }
        }
    }
}
