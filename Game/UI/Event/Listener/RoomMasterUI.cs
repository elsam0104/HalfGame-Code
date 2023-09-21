using GameEvent;
using GameEvent.UI;
using Google.Protobuf;
using PROTO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomMasterUI : MonoBehaviour, UIListener
{
    [SerializeField]
    private GameObject editButton;
    [SerializeField]
    private GameObject LeaveButton;

    [SerializeField]
    private Button sendButton;
    [SerializeField]
    private Button gameStartButton;

    [SerializeField]
    private GameObject gameRoomPrefab;

    private GameObject gameRoomObject;

    private void Start()
    {
        GameUIEvent.Instance.StartListening(this);
        MainUIEvent.Instance.StartListening(this);
        gameStartButton.onClick.AddListener(OnPressStartGame);
        sendButton.onClick.AddListener(ChatPanelManager.Instance.current.waitingRoomChatManager.OnPrevChat);
    }

    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
        MainUIEvent.Instance.StopListening(this);
    }
    public void OnPressStartGame()
    {
        //Packet
        LobbyPacketManager.Instance.GameStart(UserSOManager.Instance.dataSO.curRoomID);
        //Manage Button
        //sendButton.onClick.RemoveListener(WCMAcess.Instance.ChatManager.OnPrevChat);
        //sendButton.onClick.AddListener(GCMAcess.Instance.ChatManager.OnClickSubmit);
    }

    public void InitGame(OnGameStart onGameStart)
    {
        //Manage UI
        GameObject game = Instantiate(gameRoomPrefab);
        UserSOManager.Instance.dataSO.isRunningGame = true;
        WaitFunc.Instance.TriggerEvent(WaitFuncType.StartGame);
        gameRoomObject = game;
        //ChangeToInGameUI();
    }
    public void EndGame(bool isWin)
    {
        if (gameRoomObject == null) return;
        Destroy(gameRoomObject);
        gameRoomObject = null;
        if (isWin)
            ChatPanelManager.Instance.current.waitingRoomChatManager.WaitingRoomNotice("|| 게임 승리 ||");
        else
            ChatPanelManager.Instance.current.waitingRoomChatManager.WaitingRoomNotice("|| 게임 패배 ||");
    }
    private void ChangeRoomMasterUI(bool isRoomMaster)
    {
        //editButton.gameObject.SetActive(isRoomMaster);
        gameStartButton.gameObject.SetActive(isRoomMaster);
    }

    public void ListenerUpdate(UISubject subject)
    {
        if (subject is MainUIEvent)
        {
            MainUIEvent ui = (MainUIEvent)subject;
            if (ui.State == MainUIEventState.ChangeRoomMasterUI)
            {
                ChangeRoomMasterUI(UserSOManager.Instance.dataSO.isRoomMaster);
            }
        }
        if (subject is GameUIEvent)
        {
            GameUIEvent ui = (GameUIEvent)subject;
            if (ui.State == GameUIEventState.RoomMasterChanged)
            {
                ChangeRoomMasterUI(UserSOManager.Instance.dataSO.isRoomMaster);
            }

            if (ui.State == GameUIEventState.GameStarted)
            {
                OnGameStart onGameStart = ui.Packet as OnGameStart;
                InitGame(onGameStart);
            }

            if (ui.State == GameUIEventState.GameEnded)
            {
                CloseGame closeGame = ui.Packet as CloseGame;
                EndGame(closeGame.IsWin);
            }
        }
    }
}
