namespace PurpacaGames.StateManagement
{
    /// <summary>
    /// 可被状态机管理的状态
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 当进入此状态
        /// </summary>
        public void OnStateEnter();

        /// <summary>
        /// 当离开此状态
        /// </summary>
        public void OnStateExit();

        /// <summary>
        /// 当在此状态时进行帧更新
        /// </summary>
        public void OnStateUpdate();
    }
}