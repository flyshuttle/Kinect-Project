﻿using UnityEngine;
using System.Collections;
using UnitySteer.Behaviors;

public class Player_WaveHandState : State<Player>
{

    #region 成员变量
    private AudioClip WaveHandsAndSay;
    private Entrance entrance;

    #endregion

    #region override
    public override void Enter(Player player)
    {
        player.PlayerAC.WaveHand();
        player.CleanWaittime();
        Player_WaveHandsAndSay(player);

    }

    public override void Execute(Player player)
    {
        player.gameObject.transform.LookAt(player.target.transform.position);
        player.PlusWaittime();
        if (/*!GUItest.playerWaveHands || */player.GetWaittime() > Const.PLAYER_WAVEHAND_TIME)
        {
            player.States.WaitingState.SetEntrance(entrance);
            player.GetFSM().ChangeState(player.States.WaitingState);
        }
    }

    public override void Exit(Player player)
    {
        player.CleanWaittime();
        player.PlayerAC.StopWaveHand();
    }

    public override bool OnMessage(Player player, MessageEnum msg)
    {
        switch (msg)
        {
            case MessageEnum.LeaderCloseToPlayer:
                {

                    player.GetFSM().ChangeState(player.States.CloseToLeaderState); break;
                    //MessageDispatcher.Instance().DisparcherMessage(Const.PLAYER_WAVEHAND_TIME, player.ID(), player.ID(), MessageEnum.LeaderCloseToPlayer);
                    break;
                }
            case MessageEnum.LeaderReturned:
                {
                    //MessageDispatcher.Instance().DisparcherMessage(Const.PLAYER_WAVEHAND_TIME, player.ID(), player.ID(), MessageEnum.LeaderReturned);
                    RevertToEntrance(player);break;
                }
        }
        return true;
    }
    #endregion

    #region 内部服务函数
    private void Player_WaveHandsAndSay(Player player)
    {
        float rand = Random.Range(0, 10);
        if (rand > 0 && rand <= 3)
        {
            WaveHandsAndSay = player.PlayerVoices[AudioClipHash.Player_WaveHandsAndSay01];
        }
        else if (rand > 3 && rand < 6)
        {
            WaveHandsAndSay = player.PlayerVoices[AudioClipHash.Player_WaveHandsAndSay01];
        }
        else
        {
            WaveHandsAndSay = player.PlayerVoices[AudioClipHash.Player_WaveHandsAndSay01];
        }
        player.voice.clip = WaveHandsAndSay;
        player.voice.Play();

        player.target.GetComponent<Leader>().States.InteractiveState.SetInteractiveType(InteractiveType.WaveHands);
        //其实最好voiceTrigger的半径和声音强度有一定关系.激活声音触发器，由触发器给Leader或者follower发消息
        //如果主席没有在触发器内的话，就告诉follower
        if (Vector3.Distance(player.transform.position, player.target.transform.position) > Const.PLAYER_VOICE_LENGTH)
        {
            Follower follower = GameObject.FindGameObjectWithTag(Tags.Follower).GetComponent<Follower>();
            MessageDispatcher.Instance().DisparcherMessage(Const.FOLLOWER_REACT_TIME, player.ID(), follower.ID(), MessageEnum.PlayerWantsInteractive);
        }
        else
        {
            MessageDispatcher.Instance().DisparcherMessage(Const.LEADER_REACT_TIME, player.ID(), player.target.gameObject.GetComponent<Leader>().ID(), MessageEnum.PlayerWantsInteractive);
        }
   

    }

    private void RevertToEntrance(Player player)
    {
        if (entrance == Entrance.FromIdling)
        {
            player.GetFSM().ChangeState(player.States.IdleState);
        }
        else if (entrance == Entrance.FromWalking)
        {
            player.GetFSM().ChangeState(player.States.WalkingState);
        }
    }

    #endregion

    #region 外部接口
    public void SetEntrance(Entrance en)
    { entrance = en; }
    #endregion

}
