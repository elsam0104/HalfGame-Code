using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PROTO;
using Google.Protobuf.Collections;
using System;
using GameEvent;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private ChatUIManager chatManager;

    private void Start()
    {
        EventManager.Instance.StartListening(DicEventType.TimerOperator, new Action<bool>(TimerOperator));
        EventManager.Instance.StartListening(DicEventType.ClickSkillBtn, new Action<SkillType>(ClickSkillBtn));
    }
    //public void InitAllUser(RepeatedField<string> users)
    //{
    //    chatManager = FindObjectOfType<ChatUIManager>();
    //    for (int i = 0; i < users.Count; i++)
    //    {
    //        chatManager.InitUser(users[i], i + 1);
    //    }
    //}
    public void SendChat(string str)
    {
        gameChatMessage gameChatMessage = new gameChatMessage();
        gameChatMessage.Text = str;
        NetworkManager.Instance.Send(gameChatMessage, PacketID.GameChatMessage);
    }
    public void SkillUse(List<int> target, SkillType skillType)
    {
        skillUse skillUse = new skillUse();
        skillUse.SkillType = skillType;
        skillUse.TargetUsers.Set(target);
        NetworkManager.Instance.Send(skillUse, PacketID.SkillUse);
    }
    public void Attack(int target)
    {
        attack attack = new attack();
        attack.Target = target;
        NetworkManager.Instance.Send(attack, PacketID.Attack);
    }
    public void Vote(int target)
    {
        vote vote = new vote();
        vote.Target = target;
        NetworkManager.Instance.Send(vote, PacketID.Vote);
    }
    public void TimerOperator(bool isAdd)
    {
        timerOperator timerOperator = new timerOperator();
        timerOperator.IsAdd = isAdd;
        NetworkManager.Instance.Send(timerOperator, PacketID.TimerOperator);
    }

    public void ClickSkillBtn(SkillType skillType)
    {
        Debug.Log("send3");
        clickSkillBtn clickSkillBtn = new clickSkillBtn();
        clickSkillBtn.SkillType = skillType;
        NetworkManager.Instance.Send(clickSkillBtn, PacketID.ClickSkillBtn);
    }
}
/// <summary>
/// List setter
/// </summary>
public static class RepeatedFieldExtension
{
    public static void Set<T>(this Google.Protobuf.Collections.RepeatedField<T> rp,List<T> list)
    {
        list.ForEach((i) => rp.Add(i));
    }

    public static void Set<T>(this Google.Protobuf.Collections.RepeatedField<T> rp, T[] list)
    {
        for(int i=0;i<list.Length;i++)
        {
            rp.Add(list[i]);
        }
    }
}
