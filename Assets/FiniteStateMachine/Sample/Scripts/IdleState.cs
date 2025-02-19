using UnityEngine;
using PurpacaGames.StateManagement;

public class IdleState : IState
{
    float timer;

    public void OnStateEnter()
    {
        timer = 0.0f;
        Debug.Log("�����Ǿ�ֹ״̬~");
    }

    public void OnStateExit()
    {
        Debug.Log("�Ѿ�������ֹ״̬");
    }

    public void OnStateUpdate()
    {
        Debug.Log($"��ֹ�У��Ѿ���ֹ�� {timer} ��...");
        timer += Time.deltaTime;
    }
}
