using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRotateState : PlayerInExternalState
{
    public PlayerRotateState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, PlayerSelfData selfData, string animBoolName) : base(player, stateMachine, playerData, selfData, animBoolName)
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
        //player.SetGravityScaleFun(0, RpcTarget.Others);
        player.SetGravityScaleFun(0, RpcTarget.Others);//���������������������Ϊ0
        player.RB.gravityScale = 0f;
        if (selfData.ifHaveDashKey)//�����ɫ�������������ָ����ܴ�����
        {
            player.DashState.CanDash();
        }
        if (selfData.ifHaveJumpKey)//�����ɫ����Ծ����������ʣ�����Ծ������Ϊ1��
        {
            player.JumpState.lastAmountOfJump = 1;
        }
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
            if (player.InputHandler.JumpInput && player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (!player.isRotate)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (player.InputHandler.DashInput && player.DashState.CheckIfCanDash())
            {
                player.CheckIfShouldFlip(xInput);
                stateMachine.ChangeState(player.DashState);
            }
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
