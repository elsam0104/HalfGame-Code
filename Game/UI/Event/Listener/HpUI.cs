using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvent;
using GameEvent.UI;
using TMPro;

public class HpUI : MonoBehaviour, UIListener
{
    [SerializeField]
    private Slider hpBar;

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
        hpBar.maxValue = value;
        hpBar.value = value;
        amountText.text = value.ToString();
    }
    public void OnChangeHp(float hp)
    {
        hpBar.value = hp;
        amountText.text = hp.ToString();
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;

        if (ui.State == GameUIEventState.InitHp)
        {
            ShowMyJob showMyJob = ui.Packet as ShowMyJob;
            {
                Init(showMyJob.JobData.HP);
            }

        }
        if (ui.State == GameUIEventState.ChangeHp)
        {
            HpChange hpChange = ui.Packet as HpChange;
            {
                OnChangeHp(hpChange.Hp);
            }
        }
    }
}
