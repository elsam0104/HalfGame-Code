using GameEvent;
using GameEvent.UI;
using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTurn : MonoBehaviour,UIListener
{
    public Transform parent;
    public TurnElement turnPrefab;
    public SkillType skillType;
    List<TurnElement> turns = new List<TurnElement>();

    private void Start()
    {
        GameUIEvent.Instance.StartListening(this);
    }

    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if (ui.State == GameUIEventState.CoolTurn)
        {
            CoolTurnNotice packet = ui.Packet as CoolTurnNotice;
            if (packet.SkillType.Equals(skillType))
            {
                SetTurn(packet.CurrentTurn, packet.MaxTurn);
            }
        }
        if (ui.State == GameUIEventState.UltimatePointNotice)
        {
            UltimatePointChanged packet = ui.Packet as UltimatePointChanged;
            if (skillType==SkillType.Ultimate)
            {
                SetUltimatePoint(packet.Point, packet.MaxPoint);
            }
        }
    }

    public void SetTurn(int current,int max)
    {
        int total = turns.Count;
        if (turns.Count<max)
        {
            for(int i=total; i<max;i++)
                turns.Add(Instantiate(turnPrefab,parent));
        }
        else if(turns.Count>max)
        {
            for (int i = max; total< i; i--)
            {
                int index = turns.Count - 1;
                Destroy(turns[index]);
                turns.RemoveAt(index);
            }
        }
        for(int i=max-1; i>=0; i--)
        {
            turns[i].Activate(i<max-current);
        }
    }

    public void SetUltimatePoint(int current, int max)
    {
        int total = turns.Count;
        if (turns.Count < max)
        {
            for (int i = total; i < max; i++)
                turns.Add(Instantiate(turnPrefab, parent));
        }
        else if (turns.Count > max)
        {
            for (int i = max; total < i; i--)
            {
                int index = turns.Count - 1;
                Destroy(turns[index]);
                turns.RemoveAt(index);
            }
        }
        for (int i = 0;i<max;i++)
        {
            turns[i].Activate(i<current);
        }
    }
}

