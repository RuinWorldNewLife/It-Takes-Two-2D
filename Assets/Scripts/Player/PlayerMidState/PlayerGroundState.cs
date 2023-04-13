using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    protected float xInput;
    private bool isGrounded;
    private bool jumpInput;
    private bool dashInput;
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfIsGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        player.resetLastAmountOfJump();
        player.playerTempJumpSpeed = 0f;
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
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
        if ((jumpInput && player.JumpState.CanJump()))
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (player.isRotate)
        {
            stateMachine.ChangeState(player.RotateState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (player.isJumpPlat)
        {
            stateMachine.ChangeState(player.JumpPlatState);
        }
        else if(StaticData.buttonSevenNum >= 2)
        {
            stateMachine.ChangeState(player.TurnInState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
}
