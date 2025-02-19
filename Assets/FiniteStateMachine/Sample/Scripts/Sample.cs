using UnityEngine;
using UnityEngine.UI;
using PurpacaGames.StateManagement;

public class Sample : MonoBehaviour
{
    [SerializeField]
    Button bIdle, bMove, bJump;

    [SerializeField]
    Transform cube;

    StateMachine machine;

    bool isIdle, isMove, isJump;

    private void Start()
    {
        bIdle.onClick.AddListener(() => 
        {
            isIdle = true;
            isMove = false;
            isJump = false;
        });

        bMove.onClick.AddListener(() =>
        {
            isIdle = false;
            isMove = true;
            isJump = false;
        });

        bJump.onClick.AddListener(() =>
        {
            isIdle = false;
            isMove = false;
            isJump = true;
        });

        machine = new StateMachine();
        IdleState idleState = new IdleState();
        MoveState moveState = new MoveState { transform = cube };
        JumpState jumpState = new JumpState { transform = cube };

        machine.AddState(idleState);
        machine.AddState(moveState);
        machine.AddState(jumpState);

        machine.SetEntry(idleState);

        machine.MakeAnyStateTransition(idleState, () => 
        {
            if(isIdle)
            {
                isIdle = false;
                return true;
            }

            return isIdle;
        });

        machine.MakeAnyStateTransition(moveState, () =>
        {
            if (isMove)
            {
                isMove = false;
                return true;
            }

            return isMove;
        });

        machine.MakeAnyStateTransition(jumpState, () =>
        {
            if (isJump)
            {
                isJump = false;
                return true;
            }

            return isJump;
        });

        gameObject.AddComponent<StateMachineHandler>().Launch(machine);
    }
}
