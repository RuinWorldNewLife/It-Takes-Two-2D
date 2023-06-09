using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;

    public WallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, PlayerSelfData selfData, string animBoolName) : base(player, stateMachine, playerData, selfData, animBoolName)
    {
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
        MusicMgr.Instance.PlayAtPointFun("hero_wall_jump", player.transform.position, false);
        player.RPCPlayClip("hero_wall_jump", player.transform.position);
        player.InputHandler.UseJumpInput();
        player.resetLastAmountOfJump();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
        player.playerTempJumpSpeed = playerData.jumpHoldOnSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.Anim.SetFloat("yVerlocity", player.CurrentVelocity.y);
        player.Anim.SetFloat("xVerlocity", Mathf.Abs(player.CurrentVelocity.x));

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public void DeterminWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -player.FacingDirection;
        }
        else
        {
            wallJumpDirection = player.FacingDirection;
        }
    }
}
