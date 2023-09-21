using GameEvent.UI;
using GameEvent;
using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AtkUI : MonoBehaviour,UIListener
{
    [SerializeField]
    private TextMeshProUGUI amountText;
    private void Start()
    {
        GameUIEvent.Instance.StartListening(this);
    }
    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    public void Init(int value)
    {
        amountText.text = value.ToString();
    }

    public void OnChangeAtk(float atk)
    {
        amountText.text = atk.ToString();
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;

        if (ui.State == GameUIEventState.InitAtk)
        {
            ShowMyJob showMyJob = ui.Packet as ShowMyJob;
            {
                Init(showMyJob.JobData.Atk);
            }

        }
        if (ui.State == GameUIEventState.ChangeAtk)
        {
            OnAtkChanged hpChange = ui.Packet as OnAtkChanged;
            {
                OnChangeAtk(hpChange.Atk);
            }
        }
    }
}
