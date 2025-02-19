using UnityEngine;
using PurpacaGames.StateManagement;

public class IdleState : IState
{
    float timer;

    public void OnStateEnter()
    {
        timer = 0.0f;
        Debug.Log("现在是静止状态~");
    }

    public void OnStateExit()
    {
        Debug.Log("已经结束静止状态");
    }

    public void OnStateUpdate()
    {
        Debug.Log($"静止中，已经静止了 {timer} 秒...");
        timer += Time.deltaTime;
    }
}
