using System.Collections;
using UnityEngine;

namespace PurpacaGames.StateManagement
{
    /// <summary>
    /// 状态机执行器
    /// </summary>
    public class StateMachineHandler : MonoBehaviour 
    {
        //[SerializeField]
        //public StateMachineAsset m_stateMachine;

        private Coroutine _coroutine;
        private StateMachine _currentFSM;

        #region Public 方法
        /// <summary>
        /// 启动给定的状态机
        /// </summary>
        public void Launch(StateMachine machine) 
        {
            _currentFSM = machine;

            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(RunStateMachine());
        }
        #endregion

        #region Private 方法
        private IEnumerator RunStateMachine() 
        {
            bool isStateMachineRunning = true;
            _currentFSM.AddOnStateMachineExitListener(() => 
            {
                isStateMachineRunning = false;
            });

            _currentFSM.OnStateEnter();
            yield return null;

            while (isStateMachineRunning) 
            {
                _currentFSM.OnStateUpdate();
                yield return null;
            }

            _currentFSM.OnStateExit();
        }
        #endregion

        #region Unity 消息
        public void Start()
        {
            //if(m_stateMachine != null)
            //{
            //    if(!m_stateMachine.ToRunTimeFSM(out _currentFSM))
            //    {
            //        Debug.LogError("FSM Asset -> Runtime failed!");
            //    }
            //    _coroutine = StartCoroutine(RunStateMachine());
            //}
        }
        #endregion
    }
}