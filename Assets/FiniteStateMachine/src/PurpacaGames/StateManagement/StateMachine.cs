using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PurpacaGames.StateManagement
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    [Serializable]
    public class StateMachine : IState
    {
        private IState m_initialState;
        private Dictionary<IState, List<Transition>> m_states;
        private List<Transition> m_anystateTransitions;
        private UnityAction m_onStateExitCallback;

        private IState _current;

        #region 构造器
        public StateMachine() 
        {
            m_anystateTransitions = new List<Transition>();
            m_states = new Dictionary<IState, List<Transition>>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 当前状态机所持有的所有状态
        /// </summary>
        public IState[] ManagedStates 
        {
            get 
            {
                return m_states.Keys.ToArray();
            }
        }
        #endregion

        #region Public 方法
        /// <summary>
        /// 添加状态到当前状态机
        /// </summary>
        public void AddState(IState state) 
        {
            if (m_states.ContainsKey(state)) 
            {
                return;
            }

            if(state == this) 
            {
                Debug.LogWarning($"当前的状态机({this})将自己添加为了子状态！");
            }

            m_states.Add(state, new List<Transition>());
        }

        /// <summary>
        /// 从当前的状态机中移除状态
        /// </summary>
        public void RemoveState(IState state)
        {
            if (!m_states.ContainsKey(state))
            {
                return;
            }

            // 检查并移除所有目标状态为要移除的状态的任意状态过渡
            for (int i = m_anystateTransitions.Count - 1; i >= 0; i--)
            {
                if (m_anystateTransitions[i].target == state)
                {
                    m_anystateTransitions.RemoveAt(i);
                }
            }

            m_states.Remove(state);
        }

        /// <summary>
        /// 为此状态机设置入口状态
        /// </summary>
        public void SetEntry(IState state) 
        {
            if (!m_states.ContainsKey(state))
            {
                return;
            }

            m_initialState = state;
        }

        /// <summary>
        /// 为给定的起始状态添加到给定的目标状态的过渡
        /// </summary>
        /// <param name="origin">起始状态</param>
        /// <param name="target">目标状态</param>
        /// <param name="condition">判断过渡条件的回调方法</param>
        public void MakeTransition(IState origin, IState target, Func<bool> condition = null)
        {
            if (!m_states.ContainsKey(origin) || (target != Exit.instance && !m_states.ContainsKey(target)))
            {
                Debug.LogError($"无法为起始状态({origin})添加到目标状态({target})的过渡，给定的初始状态或目标状态未添加到当前状态机{this}中！");
                return;
            }

            Transition transition = new Transition { target = target, condition = condition };
            for (int i = 0; i < m_states[origin].Count; i++)
            {
                if (m_states[origin][i].Equals(transition))     // 避免重复加入相同的过渡
                {
                    return;
                }
            }

            m_states[origin].Add(transition);
        }

        /// <summary>
        /// 设置一个从任何状态到给定的目标状态的过渡
        /// </summary>
        /// <param name="target">目标状态</param>
        /// <param name="condition">判断过渡条件的回调方法</param>
        public void MakeAnyStateTransition(IState target, Func<bool> condition) 
        {
            if (!m_states.ContainsKey(target)) 
            {
                Debug.LogError($"无法设置从任意状态到目标状态({target})的过渡，给定的目标状态未添加到当前状态机({this})中！");
                return;
            }

            if(condition == null)
            {
                Debug.LogError($"无法设置从任意状态到目标状态({target})的过渡，从任意状态到目标状态的条件判断方法不能为 null！");
                return;
            }

            Transition transition = new Transition { target = target, condition = condition };
            
            for (int i = 0; i < m_anystateTransitions.Count; i++)
            {
                if (m_anystateTransitions[i].Equals(transition))     // 避免重复加入相同的过渡
                {
                    return;
                }
            }

            m_anystateTransitions.Add(transition);
        }

        /// <summary>
        /// 检查状态是否存在于当前的状态机中
        /// </summary>
        /// <returns>给定的状态是否存在于当前的状态机中？</returns>
        public bool DoesStateExist(IState state) 
        {
            return m_states.ContainsKey(state);
        }

        /*
        /// <summary>
        /// 检查状态是否在当前状态机中可以最终被访问到
        /// </summary>
        /// <returns>给定的状态是否在当前状态机中可以最终被访问到？</returns>
        public bool IsStateNotAcessable(IState state)
        {
            if (!m_states.ContainsKey(state))
            {
                return false;
            }
        }
        */

        /// <summary>
        /// 添加当状态机退出时的回调方法
        /// </summary>
        public void AddOnStateMachineExitListener(UnityAction callback)
        {
            m_onStateExitCallback += callback;
        }

        /// <summary>
        /// 移除当状态机退出时的回调方法
        /// </summary>
        public void RemoveOnStateMachineExitListener(UnityAction callback)
        {
            m_onStateExitCallback -= callback;
        }

        /// <summary>
        /// 清除当状态机退出时的回调方法
        /// </summary>
        public void ClearOnStateMachineExitListener()
        {
            m_onStateExitCallback = null;
        }

        public void OnStateEnter()
        {
            if (m_initialState == null) 
            {
                Debug.LogError($"没有为当前的状态机({this})设置默认的入口状态，此状态机已退出！");
                OnStateExit();
                return;
            }

            _current = m_initialState;
            _current.OnStateEnter();
        }

        public void OnStateExit()
        {
            m_onStateExitCallback?.Invoke();
        }

        public void OnStateUpdate()
        {
            if (_current == null)
            {
                Debug.LogError($"当前状态机({this})处于的状态为 null，已经自动退出此状态机！");
                OnStateExit();
                return;
            }

            //检查 任意状态 的过渡
            for(int i = 0; i < m_anystateTransitions.Count; i++) 
            {
                if (m_anystateTransitions[i].condition())
                {
                    _current.OnStateExit();

                    if (m_anystateTransitions[i].target == Exit.instance)
                    {
                        OnStateExit();
                    }
                    else
                    {
                        _current = m_anystateTransitions[i].target;
                        _current.OnStateEnter();
                    }

                    return;
                }
            }

            //检查当前状态的所有过渡的条件，如果满足条件则过渡到目标状态
            List<Transition> transitions = m_states[_current];
            for (int i = 0; i < transitions.Count; i++)
            {
                //如果当前过渡没有条件或条件满足，过渡到目标状态
                if (transitions[i].condition == null || transitions[i].condition())
                {
                    _current.OnStateExit();

                    if (transitions[i].target == Exit.instance)
                    {
                        OnStateExit();
                    }
                    else
                    {
                        _current = transitions[i].target;
                        _current.OnStateEnter();
                    }

                    return;
                }
            }

            _current.OnStateUpdate();
        }
        #endregion

        #region 内部类型
        /// <summary>
        /// 状态的过渡
        /// </summary>
        /// <returns></returns>
        public class Transition
        {
            /// <summary>
            /// 目标状态
            /// </summary>
            public IState target;

            /// <summary>
            /// 判断过渡条件的回调方法
            /// </summary>
            public Func<bool> condition;

            #region Public 方法
            public override bool Equals(object obj)
            {
                if(obj is not Transition)
                {
                    return false;
                }

                Transition t = (Transition)obj;

                if(t.target == target && t.condition == condition)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode() 
            {
                return base.GetHashCode();
            }
            #endregion
        }

        /// <summary>
        /// 状态机的退出节点
        /// </summary>
        public sealed class Exit : IState
        {
            public static Exit instance;
            static Exit() 
            {
                instance = new Exit();
            }

            private Exit() { }

            public void OnStateEnter()
            {
                
            }

            public void OnStateExit()
            {
                
            }

            public void OnStateUpdate()
            {
                
            }
        }
        #endregion
    }
}