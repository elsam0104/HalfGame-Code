using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameEvent.UI;
using PROTO;
using Google.Protobuf.Collections;
using System;

public class WaitMemPanelUI : MonoBehaviour/*, UIListener*/
{
    [SerializeField]
    private GameObject usersPanel;
    [SerializeField]
    private GameObject userPrefab;
    [Obsolete]
    private void InitOneUser(string name,int i)
    {
        var child = Instantiate(userPrefab, usersPanel.transform);
        child.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().text = name;
        child.GetComponent<UserTarget>().targetNo = i;
        List<int> target = new List<int>();
        target.Add(i);
    }

    public void InitAllWaitUser(RepeatedField<string> users)
    {
        int i = 0;
        foreach (string user in users)
            InitOneUser(user, ++i);
    }
    //public void ListenerUpdate(UISubject subject)
    //{
    //    MainUIEvent ui = (MainUIEvent)subject;


    //    if (ui.State == MainUIEventState.InitUserPanel)
    //    {
    //        OnJoinRoom onJoinRoom = ui.Packet as OnJoinRoom;
    //        //InitAllWaitUser(onJoinRoom.Username,);
    //    }
    //}
}