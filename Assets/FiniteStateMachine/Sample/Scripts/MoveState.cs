using UnityEngine;
using PurpacaGames.StateManagement;

public class MoveState : IState
{
    public Transform transform;
    bool direction = true;

    public void OnStateEnter()
    {
        Debug.Log("溜达溜达~");
    }

    public void OnStateExit()
    {
        Debug.Log("不溜达了...");
    }

    public void OnStateUpdate()
    {
        if (direction)
        {
            if (transform.position.x < 1.0f)
            {
                Vector3 pos = transform.position;
                pos.x += 1.0f * Time.deltaTime;
                transform.position = pos;
            }
            else
            {
                direction = false;
            }
        }
        else
        {
            if (transform.position.x > -1.0f)
            {
                Vector3 pos = transform.position;
                pos.x -= 1.0f * Time.deltaTime;
                transform.position = pos;
            }
            else
            {
                direction = true;
            }
        }
    }
}
