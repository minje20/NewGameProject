using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace IndieLINY.AI.Core
{
    public interface IStateBehaviour
    {
        
    }
    public abstract class StateBehaviour : MonoBehaviour, IStateBehaviour
    {
    }

    public class BehaviourBinder : MonoBehaviour
    {
        private Dictionary<Type, IStateBehaviour> _table;
        private GameObject _gameObject;

        private const string DECLARATION = "__BehaviourBinder__";
        private const string DECLARATION_GAMEOBJECT = "__BehaviourBinder_GameObject__";
        public Dictionary<Type, IStateBehaviour> Table
        {
            get
            {
                if (_table == null)
                {
                    _table = new Dictionary<Type, IStateBehaviour>();
                }

                return _table;
            }
        }

        public void Clear()
        {
            Table.Clear();
            _table = null;
        }

        public static BehaviourBinder GetBinder(Flow flow)
        {
            VariableDeclarations declarations = flow.stack.gameObject.GetComponent<Variables>()?.declarations;
            
            Debug.Assert(declarations != null);
            
            if (declarations.IsDefined(DECLARATION))
            {
                return declarations.Get<BehaviourBinder>(DECLARATION);
            }

            
            var parentObject = declarations.Get<GameObject>(DECLARATION_GAMEOBJECT);

            var obj = new GameObject(DECLARATION);
            obj.transform.SetParent(parentObject.transform);
            obj.transform.position = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.rotation = Quaternion.identity;

            var com = obj.AddComponent<BehaviourBinder>();
                
            declarations.Set(DECLARATION, com);
            com._gameObject = obj;

            return com;
        }
        
        public T GetBehaviour<T>()
            where T : StateBehaviour
        {
            Type t = typeof(T);
            if (Table.TryGetValue(t, out var v))
            {
                Debug.Assert(v is T, "타입과 매칭되지 않는 StateBehaviour 입니다.");
                return v as T;
            }
            
            var com = gameObject.AddComponent<T>();

            Table.Add(t, com);

            return com;
        }
        
        public T GetIBehaviour<T>()
            where T : class, IStateBehaviour
        {
            Type t = typeof(T);
            if (Table.TryGetValue(t, out var v))
            {
                Debug.Assert(v is T, "타입과 매칭되지 않는 StateBehaviour 입니다.");
                return v as T;
            }
            
            if (_gameObject.TryGetComponent<T>(out var com))
            {
                _table.Add(t, com);
                return com;
            }
            
            Debug.Assert(false, "존재하지 않는 StateBehaviour 입니다.");
            return null;
        }
    }
}