using GameEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NativeSerializableDictionary;
using System;

public enum LobbyPopUpType
{
    None,
    Setting,
    GlobalTalk,
    MakeRoom,
    SearchRoom,
}
public class LobbyPopUpManager : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<LobbyPopUpType, GameObject> popUpDic = new SerializableDictionary<LobbyPopUpType, GameObject>();
    [SerializeField]
    private GameObject popUpParantPanel;

    private GameObject undoPanel;
    private Stack<LobbyPopUpType> popUpStack = new Stack<LobbyPopUpType>();

    private void Start()
    {
        undoPanel = popUpParantPanel.transform.GetChild(0).gameObject;
        EventManager.Instance.StartListening(DicEventType.PushMainPopUp, new Action<LobbyPopUpType>(Push));
        EventManager.Instance.StartListening(DicEventType.PopMainPopUp, new Action(Pop));
    }

    public void Pop()
    {
        LobbyPopUpType res;
        if (popUpStack.TryPop(out res))
        {
            popUpDic[res].SetActive(false);
            if (popUpStack.Count == 0)
                ChangeUndoPanel(false);
        }
    }
    public void Push(LobbyPopUpType type)
    {
        if (isFirstPush())
            ChangeUndoPanel(true);
        popUpStack.Push(type);
        popUpDic[type].SetActive(true);
    }

    private void ChangeUndoPanel(bool turnOn)
    {
        undoPanel.SetActive(turnOn);
    }
    private bool isFirstPush()
    {
        foreach (Transform child in popUpParantPanel.transform)
        {
            if (child.childCount != 0 && child.gameObject.activeSelf == true)
                return false;
        }
        return true;
    }
}