using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    class NodeData
    {
        State state;
        double prob;

        public NodeData(State state, int action, double prob)
        {
            this.state = state;
            this.prob = prob;
        }

        public State State
        {
            get { return state; }
        }


        public double Prob
        {
            get { return prob; }
        }

    }


    [Serializable]
    public struct ActionClassAttributeKey
    {
        int action;
        string className;
        string propertyName;

        public ActionClassAttributeKey(int action, string className, string propertyName)
        {
            this.action = action;
            this.className = className;
            this.propertyName = propertyName;
        }

        public override int GetHashCode()
        {
            return new Tuple<int, string, string>(action, className, propertyName).GetHashCode();
        }
    }

    [Serializable]
    class TransitionModel
    {
        Dictionary<ActionClassAttributeKey, object> d;

        private List<NodeData> NodeDataForLeaf(State rootState, int action, int objectIndex, int attributeIndex, NodeData n, int minObsForPrediction, Dictionary<string, bool> condFuncCache = null)
        {
            List<NodeData> leaves = new List<NodeData>();

            State state = n.State;

            double prob = n.Prob;

            object obj = rootState.GetObjectByIndex(objectIndex);

            Type objType = obj.GetType();

            PropertyInfo[] objAttributes = objType.GetProperties();

            PropertyInfo objAttribute = objAttributes[attributeIndex];

            ValueType val = (ValueType)objAttribute.GetValue(obj);

            ActionClassAttributeKey key = new ActionClassAttributeKey(action, objType.Name, objAttribute.Name);

            object actionAttributeBNs = d[key];

            MethodInfo attributeNextStateDistributionMethod = actionAttributeBNs.GetType().GetMethod("AttributeDistibution");

            Dictionary<ValueType,double> dist = (Dictionary<ValueType,double>)attributeNextStateDistributionMethod.Invoke(actionAttributeBNs, new object[] { obj, rootState, val, minObsForPrediction, true,condFuncCache });

            if (dist == null)
            {
                return null;
            }

            foreach (ValueType valNew in dist.Keys)
            {
                State newState = state.Copy();
                object objNew = newState.GetObjectByIndex(objectIndex);
                objAttributes[attributeIndex].SetValue(objNew, valNew);
                NodeData nodeData = new NodeData(newState, action, prob * dist[valNew]);
                leaves.Add(nodeData);

            }
            
            return leaves;
        }


        public void Save(string path)
        {
            Global.SerializeObject(this, path);
        }

        public static TransitionModel Load(string path)
        {
            return Global.DeserializeObject<TransitionModel>(path);
        }
        

       

        public Dictionary<State, double> NextStateDistribution(State state, int action, int minObsForPrediction=1, States states=null)
        {

            State rootState = state.Copy();

            Dictionary<string, bool> condFuncCache = CondFuncs.GetCondFuncCache(rootState);

            NodeData root = new NodeData(rootState, action, 1);

            List<NodeData> leaves = new List<NodeData>();

            leaves.Add(root);

            for(int i=0;i< rootState.CountObjects;i++)
            {
                PropertyInfo[] objAttributes = rootState.GetObjectByIndex(i).GetType().GetProperties();

                for (int j = 0; j < objAttributes.Length; j++)
                {
                    List<NodeData> newLeaves = new List<NodeData>();

                    for (int k = 0; k < leaves.Count; k++)
                    {                        
                        List<NodeData> nodeData = NodeDataForLeaf(rootState,action,i, j, leaves[k], minObsForPrediction,condFuncCache);

                        if (nodeData==null)
                        {
                            return null;
                        }

                        newLeaves.AddRange(nodeData);
                    }

                    leaves = newLeaves;
                }
            }

            Dictionary<State, double> dist = new Dictionary<State, double>();

            foreach (NodeData leaf in leaves)
            {
                if(states!=null)
                {
                    dist[states.GetStateFromAllStates(leaf.State)] = leaf.Prob;
                }
                else
                {
                    dist[leaf.State.Copy()] = leaf.Prob;
                }
            }
            
            return dist;
        }

        public void Update(int action, State state, State newState)
        {
            Dictionary<string, bool> condFuncCache = CondFuncs.GetCondFuncCache(state);

            for (int i = 0; i < state.CountObjects; i++)
            {
                object obj = state.GetObjectByIndex(i);

                object objNew = newState.GetObjectByIndex(i);

                Type classType = obj.GetType();

                PropertyInfo[] classAttributes = classType.GetProperties();

                for (int j = 0; j < classAttributes.Length; j++)
                {
                  
                    PropertyInfo classAttribute = classAttributes[j];
                    ActionClassAttributeKey key = new ActionClassAttributeKey(action, classType.Name, classAttribute.Name);
                    object actionAttributeBNs = d[key];
                    object val = classAttribute.GetValue(obj);
                    object valNew = classAttribute.GetValue(objNew);                   
                    MethodInfo updateMethod = actionAttributeBNs.GetType().GetMethod("Update");                    
                    updateMethod.Invoke(actionAttributeBNs, new object[] { obj, state, val, valNew,condFuncCache });
                }
            }
        }

        public TransitionModel(Func<int,Type,PropertyInfo,object> initActionAttributeBNs)
        {
            d = new Dictionary<ActionClassAttributeKey, object>();

            foreach (int action in Actions.ALLACTIONS)
            {
                foreach (TypePropertyPair pair in States.GetClassAttributesPairs())
                {
                    ActionClassAttributeKey key = new ActionClassAttributeKey(action, pair.Type.Name, pair.PropertyInfo.Name);

                    object actionAttributeBNs = initActionAttributeBNs(action, pair.Type, pair.PropertyInfo);

                    if(actionAttributeBNs==null)
                    {
                        throw new Exception();
                    }

                    d[key] = actionAttributeBNs;
                }
            }
        }
        
   
    }
}
