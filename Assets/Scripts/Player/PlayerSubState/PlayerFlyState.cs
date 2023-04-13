using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFlyState : PlayerInExternalState
{
    public PlayerFlyState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, PlayerSelfData selfData, string animBoolName) : base(player, stateMachine, playerData, selfData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityScaleFun(0, RpcTarget.Others);//将重力缩放设置为0
        player.RB.gravityScale = 0f;
        player.Anim.SetBool("idle", false);
        if(selfData.ifHaveDashKey)//如果角色有闪避能力，恢复闪避次数；
        {
            player.DashState.CanDash();
        }
        if (selfData.ifHaveJumpKey)//如果角色有跳跃能力，将它剩余的跳跃次数置为1；
        {
            player.JumpState.lastAmountOfJump = 1;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScaleFun(playerData.gravityValue, RpcTarget.Others);//还原重力缩放
        player.RB.gravityScale = playerData.gravityValue;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            //在飞行的过程中，可以转换状态
            if (player.InputHandler.JumpInput && player.JumpState.CanJump())
            {
                player.isInJumpOrDashState = true;
                stateMachine.ChangeState(player.JumpState);
            }
            else if (player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
            {
                player.isInJumpOrDashState = true;
                stateMachine.ChangeState(player.DashState);
            }
            //如果玩家飞出了光柱范围
            if (!player.isFly && !player.isInJumpOrDashState)
            {
                if (player.IsMinePlayer)
                {
                    player.Anim.SetBool("In Air", true);//将动画设置为在空中。
                }
                if (player.windToFlyDirection.y > 0f)//如果飞行方向是向上飞
                {
                    player.CheckIfShouldFlip(xInput);//可以翻转方向。
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetFloat("yVerlocity", player.CurrentVelocity.y);
                    }
                    player.SetVelocityX(playerData.airMoveSpeed * xInput);//可以左右移动
                }
                else//如果不是向上飞，则是向左右飞。
                {
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetBool("Fly", false);
                        player.Anim.SetFloat("yVerlocity", player.CurrentVelocity.y);
                    }
                    player.RB.gravityScale = playerData.gravityValue;//还原重力
                }
                //经过一段时间后，玩家切换到空中状态
                if (Time.time > (player.exitWindToFlyTime + player.exitWindToFlyStartTime))
                {
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
