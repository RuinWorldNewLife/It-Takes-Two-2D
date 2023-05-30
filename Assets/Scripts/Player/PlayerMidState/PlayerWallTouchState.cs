using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爬墙状态实体类
/// </summary>
public class PlayerWallTouchState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool jumpInput;
    protected float xInput;
    protected float yInput;
    protected bool isTouchingLedge;
    public PlayerWallTouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        isGrounded = player.CheckIfIsGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
        player.resetLastAmountOfJump();
        player.DashState.CanDash();
    }
    public override void Exit()
    {
        base.Exit();
        
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        if (!isExitingState)
        {
            if (jumpInput)
            {
                player.wallJumpState.DeterminWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.wallJumpState);
            }
            else if (isGrounded)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (!isTouchingWall || xInput != player.FacingDirection)
            {
                player.JumpState.DecreaseAmountOfJumpsLeft();
                stateMachine.ChangeState(player.InAirState);
            }
            else if (player.isRotate)
            {
                stateMachine.ChangeState(player.RotateState);
            }
            else if(player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
            {
                player.Flip();
                //在墙上进行闪避，算作一次跳跃，跳跃次数减少一次
                player.JumpState.lastAmountOfJump--;
                stateMachine.ChangeState(player.DashState);
            }
        }
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
