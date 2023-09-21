using GameEvent;
using GameEvent.UI;
using NativeSerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LobbyPanel
{
    void Active();
    void DeActive();
}

public class LobbyPanelManager : MonoSingleton<LobbyPanelManager>,UIListener
{
    [SerializeField]
    public SerializableDictionary<MainUIEventState, LobbyPanel> lobbyPanels;
        
    public LobbyPanel current;
    public void ListenerUpdate(UISubject subject)
    {   
        if(subject is MainUIEvent)
        {
            var main = (MainUIEvent)subject;
            if(lobbyPanels.ContainsKey(main.State))
            {
                current?.DeActive();

                current = lobbyPanels[main.State];
                current.Active();
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MainUIEvent.Instance.StartListening(this);
        MainUIEvent.Instance.SetPacket(MainUIEventState.DefaultPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


