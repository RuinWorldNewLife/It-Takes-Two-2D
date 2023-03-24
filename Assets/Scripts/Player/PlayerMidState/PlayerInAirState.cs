using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input
    private float yVerlocity;
    private float xInput;
    private bool isGrounded;
    private bool jumpInputStop;
    private bool jumpInput;

    //Check
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    //Others
    private bool wallJumpCoyoteTime;
    private bool isJumping;
    private float startWallJumpCoyoteTime;
    public bool IsCoyote { get; private set; }
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfIsGrounded();
        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfIsGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        CheckJumpMultiplier();
        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        yVerlocity = player.CurrentVelocity.normalized.y;
        if (player.CurrentVelocity.y > 0.5f)
        {
            if (player.InputHandler.JumpInputStop && (Time.time - startTime) > playerData.jumpMinTime)
            {
                stopyJumpVerlocity();
            }
            else if (player.CurrentVelocity.y < 15f)
            {
                player.playerTempJumpSpeed = MonoHelper.Instance.TimeLerpDown(player.playerTempJumpSpeed, playerData.jump2StopVelocity);
                //当跳跃的速度小于15f时，进行速度递减的方法。
                player.SetVelocityY(player.playerTempJumpSpeed);
            }
        }
        

        
        xInput = player.InputHandler.NormInputX;
        if (!isExitingState)
        {
            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (player.JumpState.CanJump() && player.InputHandler.JumpInput)
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (isTouchingWall && xInput == player.FacingDirection && player.CurrentVelocity.y <= 2f && Time.time >= startTime + playerData.wallHandleCD)
            {
                stateMachine.ChangeState(player.wallHandleState);
            }
            else if(player.isRotate && player.CurrentVelocity.magnitude < playerData.forceOfRidOfWind)
            {
                stateMachine.ChangeState(player.RotateState);
            }
            else if (player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
            {
                stateMachine.ChangeState(player.DashState);
            }
            else if(player.isFly && player.CurrentVelocity.magnitude < playerData.forceOfRidOfWind)
            {
                stateMachine.ChangeState(player.FlyState);
            }
            else if (player.isJumpPlat)
            {
                stateMachine.ChangeState(player.JumpPlatState);
            }
            else
            {
                player.CheckIfShouldFlip(xInput);
                player.SetVelocityX(playerData.airMoveSpeed * xInput);
                if (player.IsMinePlayer)
                {
                    player.Anim.SetFloat("yVerlocity", yVerlocity);
                    player.Anim.SetBool("In Air", true);
                    player.Anim.SetBool("Fly", false);
                }
                player.Anim.SetFloat("xVerlocity", Mathf.Abs(player.CurrentVelocity.x));
            }
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void stopyJumpVerlocity()
    {
        player.SetVelocityY(playerData.stopyJumpVerlocity);
    }
    private void CheckCoyoteTime()
    {
        if (IsCoyote && Time.time > startTime + playerData.coyoteTime)
        {
            player.JumpState.lastAmountOfJump--;
            IsCoyote = false;
        }
    }
    public void StartCoyoteTime()
    {
        IsCoyote = true;
    }
    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }

        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }


    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public bool StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;


    public void SetIsJumping() => isJumping = true;
}
