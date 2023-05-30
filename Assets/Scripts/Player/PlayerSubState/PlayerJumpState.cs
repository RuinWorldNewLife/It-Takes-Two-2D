using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerJumpState : PlayerAbilityState
{
    public int lastAmountOfJump;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, PlayerSelfData selfData, string animBoolName) : base(player, stateMachine, playerData, selfData, animBoolName)
    {
        lastAmountOfJump = selfData.amountOfJump;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        MusicMgr.Instance.PlayAtPointFun("hero_jump", player.transform.position, false);
        player.RPCPlayClip("hero_jump", player.transform.position);
        player.InputHandler.UseJumpInput();
        player.SetVelocityX(player.CurrentVelocity.x);
        player.SetVelocityY(playerData.jumpSpeed);
        player.playerTempJumpSpeed = playerData.jumpHoldOnSpeed;

        DecreaseAmountOfJumpsLeft();
        isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public bool CanJump()
    {
        if (lastAmountOfJump > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 跳跃次数减少方法
    /// </summary>
    public void DecreaseAmountOfJumpsLeft() => lastAmountOfJump--;
}
