using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Auth;
using GameEvent;
using TMPro;
using System;

public class LobbyUIManager : MonoSingleTon<LobbyUIManager>
{
    [SerializeField]
    private List<LobbyUI> lobbyUI = new List<LobbyUI>();
    [SerializeField]
    private TMP_Text userName;
    private void Start()
    {
        Listening();
        EventManager.Instance.TriggerEvent(DicEventType.RefreshRoomList);
        userName.text = UserSOManager.Instance.dataSO.UserName;
    }

    private void Listening()
    {
        for (int i = 0; i < lobbyUI.Count; i++)
        {
            if(lobbyUI[i].HaveRunType())
            {
                LobbyUIHaveT curUI = (LobbyUIHaveT)lobbyUI[i];
                curUI.Listening();
            }
            else
            {
                LobbyUIVoidF curUI = (LobbyUIVoidF)lobbyUI[i];
                EventManager.Instance.StartListening(curUI.eventType,new Action(curUI.Run));
            }
        }
    }
}
