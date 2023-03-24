using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallHandleState : PlayerWallTouchState
{
    private Vector3 holdPostion;
    public WallHandleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //holdPostion = player.transform.position;//将角色位置设置为触碰到墙面时的位置
        HoldPostion();
        player.SetRBisKinematicFun(true, RpcTarget.Others);
        player.RB.isKinematic = true;
        MonoHelper.Instance.WaitSomeTimeInvoke(() =>
        {
            stateMachine.ChangeState(player.wallSlideState);
        }, playerData.WallSlideWaittingTime, () => { return false; });
    }

    public override void Exit()
    {
        base.Exit();

        player.SetRBisKinematicFun(false, RpcTarget.Others);
        player.RB.isKinematic = false;
        MonoHelper.Instance.WaitSomeTimeInvoke(() => { }, 0, () => { return true; });
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if (!isExitingState)
        //{
        //    HoldPostion();
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void HoldPostion()
    {
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
    }
}
