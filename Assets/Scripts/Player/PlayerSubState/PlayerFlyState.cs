using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFlyState : PlayerInExternalState
{
    public PlayerFlyState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityScaleFun(0, RpcTarget.Others);//��������������Ϊ0
        player.RB.gravityScale = 0f;
        player.Anim.SetBool("idle", false);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScaleFun(playerData.gravityValue, RpcTarget.Others);//��ԭ��������
        player.RB.gravityScale = playerData.gravityValue;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            //�ڷ��еĹ����У�����ת��״̬
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
            //�����ҷɳ��˹�����Χ
            if (!player.isFly)
            {
                if (player.IsMinePlayer)
                {
                    player.Anim.SetBool("In Air", true);//����������Ϊ�ڿ��С�
                }
                if (player.windToFlyDirection.y > 0f)//������з��������Ϸ�
                {
                    player.CheckIfShouldFlip(xInput);//���Է�ת����
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetFloat("yVerlocity", player.CurrentVelocity.y);
                    }
                    player.SetVelocityX(playerData.airMoveSpeed * xInput);//���������ƶ�
                }
                else//����������Ϸɣ����������ҷɡ�
                {
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetBool("Fly", false);
                        player.Anim.SetFloat("yVerlocity", player.CurrentVelocity.y);
                    }
                    player.RB.gravityScale = playerData.gravityValue;//��ԭ����
                }
                //����һ��ʱ�������л�������״̬
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
