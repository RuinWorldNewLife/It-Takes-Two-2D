using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerJumpPlatState : PlayerInExternalState
{
    public PlayerJumpPlatState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityScaleFun(3f, RpcTarget.Others);//调整其他玩家的重力缩放
        player.RB.gravityScale = 3f;
        player.SetVelocityY(player.jumpPlatForce);
    }

    public override void Exit()
    {
        base.Exit();
        player.isJumpPlat = false;
        player.SetGravityScaleFun(playerData.gravityValue, RpcTarget.Others);//还原重力缩放
        player.RB.gravityScale = playerData.gravityValue;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.airMoveSpeed * xInput * 0.5f);
            player.Anim.SetFloat("yVerlocity", player.CurrentVelocity.normalized.y);
            if (Time.time>startTime + player.jumpPlatTime)
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
