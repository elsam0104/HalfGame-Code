using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameEvent.UI;
using GameEvent;
using PROTO;

public class TimerUI : MonoBehaviour , UIListener
{
    [SerializeField]
    private TMP_Text timerText;

    private void Start()
    {
        GameUIEvent.Instance.StartListening(this);
    }
    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    public void ShowTimer(bool isTurnOn)
    {
        timerText.transform.parent.gameObject.SetActive(isTurnOn);
    }
    public void SetTimer(int curTime)
    {
        int min = curTime / 60;
        if (min < 0)
            min = 0;
        timerText.text = $"{min}:{Mathf.Max(curTime-(min*60),0).ToString("d2")}";
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if(ui.State == GameUIEventState.ChangeTimer)
        {
            Timer timer = ui.Packet as Timer;
            {
                SetTimer(timer.Time);
            }
        }
    }
}
