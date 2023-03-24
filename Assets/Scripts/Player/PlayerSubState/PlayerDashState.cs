using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private int canDash;
    private float yPostion;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        canDash = 1;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.RB.gravityScale = 0f;
        player.InputHandler.UseDashInput();
        yPostion = player.transform.position.y;
        if (stateMachine.PreviousState == player.wallHandleState && stateMachine.PreviousState == player.wallSlideState)
        {
            player.Flip();
        }
        player.SetVelocityX(playerData.dashSpeed * player.FacingDirection);
        CantDash();

    }

    public override void Exit()
    {
        base.Exit();
        player.RB.gravityScale = playerData.gravityValue;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            player.transform.position = new Vector3(player.transform.position.x, yPostion, player.transform.position.z);

            if (Time.time >= startTime + playerData.dashTime)
            {
                if(stateMachine.PreviousState == player.FlyState && player.isFly)
                {
                    stateMachine.ChangeState(player.FlyState);
                }
                isAbilityDone = true;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void CantDash()
    {
        canDash = 0;
    }
    public void CanDash()
    {
        canDash = 1;
    }
    public bool CheckIfCanDash()
    {
        if (canDash > 0 && playerData.ifHaveDashKey)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
