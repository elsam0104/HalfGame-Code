using System.Collections;
using System.Text;
using UnityEngine;
using TMPro;
using PROTO;
using GameEvent.UI;
using GameEvent;

public class DayUI : MonoBehaviour, UIListener
{
    [SerializeField]
    private TMP_Text dayText;
    private DayType curDayType = DayType.Agreement;

    private void Start()
    {
        GameUIEvent.Instance.StartListening(this);
    }

    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    public void ChangeDay(int day, DayType dayType)
    {
        StringBuilder sb = new StringBuilder();
        switch (dayType)
        {
            case DayType.Morning:
                sb.Append("³·");
                break;
            case DayType.Votetime:
                sb.Append("ÅõÇ¥");
                break;
            case DayType.Lastword:
                sb.Append("¹Ý·Ð");
                break;
            case DayType.Agreement:
                sb.Append("Âù¹Ý");
                break;
            case DayType.Night:
                sb.Append("¹ã");
                break;
            default:
                break;
        }
        dayText.text = $"{day}ÀÏÂ÷ {sb}";
        curDayType = dayType;
        UserSOManager.Instance.dataSO.curDayType = dayType;
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;

        
        if (ui.State == GameUIEventState.ChangeDay)
        {
            DayPass dayPass = ui.Packet as DayPass;
            {
                ChangeDay(dayPass.Day, dayPass.DayType);
            }
        }
    }
}
