using GameEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    None = -1,
    Lobby,
    CreateRoom,
    Game,
}
public class PanelChangeManager : MonoSingleTon<PanelChangeManager>
{
    [SerializeField]
    private NativeSerializableDictionary.SerializableDictionary<PanelType, GameObject> panelDic = new NativeSerializableDictionary.SerializableDictionary<PanelType, GameObject>();


    private void Start()
    {
        EventManager.Instance.StartListening(DicEventType.ChangePanel, new Action<PanelType>(ChangePanel));
    }

    /// <param name="type"></param>
    /// <param name="isTurnOn">�ǳ��� �������� �������� ����</param>
    /// <param name="isAlloff">�ش� �ǳ��� ������ �ǳ��� �������� ����</param>
    public void ChangePanel(PanelType type, bool isTurnOn,bool isAlloff)
    {
        if(isAlloff)
        {
            foreach(GameObject tempPanel in panelDic.Values)
            {
                if (tempPanel == panelDic[type]) continue;
                panelDic[type].SetActive(false);
            }
        }
        GameObject panel;
        if(panelDic.TryGetValue(type,out panel))
        {
            panel.SetActive(isTurnOn);
        }
    }
    public void ChangePanel(PanelType type)
    {
        ChangePanel(type, true, true);
    }

}
