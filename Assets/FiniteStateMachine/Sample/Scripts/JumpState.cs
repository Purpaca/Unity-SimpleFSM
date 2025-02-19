using UnityEngine;
using PurpacaGames.StateManagement;

public class JumpState : IState
{
    public Transform transform;
    public bool direction;

    public void OnStateEnter()
    {
        Debug.Log("开始跳了！");
    }

    public void OnStateExit()
    {
        Debug.Log("累死了，不跳了...");
    }

    public void OnStateUpdate()
    {
        if (direction)
        {
            if (transform.position.y < 1.0f)
            {
                Vector3 pos = transform.position;
                pos.y += 1.0f * Time.deltaTime;
                transform.position = pos;
            }
            else
            {
                direction = false;
            }
        }
        else
        {
            if (transform.position.y > -1.0f)
            {
                Vector3 pos = transform.position;
                pos.y -= 1.0f * Time.deltaTime;
                transform.position = pos;
            }
            else
            {
                direction = true;
            }
        }
    }
}
