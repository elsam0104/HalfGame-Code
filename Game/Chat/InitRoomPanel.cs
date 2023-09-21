using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PROTO;
using TMPro;
using GameEvent;
using GameEvent.UI;

public class InitRoomPanel : MonoBehaviour, UIListener
{
    [SerializeField]
    private RoomPanel prefab;
    [SerializeField]
    private GameObject parent;

    private TMP_Text roomId;
    private TMP_Text roomName;
    private TMP_Text maxPlayer;
    private GameObject privateIcon;
    private GameObject anonymousIcon;

    private void Start()
    {
        MainUIEvent.Instance.StartListening(this);
        //testCode();
    }
    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    private void testCode()
    {
        Room room = new Room();
        room.MaxPlayer = 5;
        room.IsPrivate = true;
        room.RoomID = 59;
        room.IsAnonymous = true;
        room.RoomName = "test room";
        CreatePanel(room);
    }


    public void CreatePanel(Room room)
    {
        
        //InitPanelObjects(obj);
        RoomPanel panelComponent = Instantiate(prefab, parent.transform);
        panelComponent.InitRoomData(room);
    }
    public void DelAll()
    {
        for (int i = 1; i < parent.transform.childCount; i++)
        {
            var obj = parent.transform.GetChild(i);
            Destroy(obj.gameObject);
        }
        RoomPanel.clickedPanel = null;
    }

    public void ListenerUpdate(UISubject subject)
    {
        if (subject is MainUIEvent)
        {
            MainUIEvent ui = (MainUIEvent)subject;
            ShowRoomList showRoomList = ui.Packet as ShowRoomList;

            if (ui.State == MainUIEventState.ShowRoomList)
            {
                DelAll();
                foreach (var room in showRoomList.RoomList)
                {
                    CreatePanel(room);
                }
            }
        }
    }
}
