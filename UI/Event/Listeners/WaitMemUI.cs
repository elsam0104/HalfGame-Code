using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameEvent.UI;
using UnityEngine.UI;
using PROTO;
using Google.Protobuf.WellKnownTypes;

public class WaitMemUI : MonoBehaviour, UIListener
{
    [SerializeField]
    private GameObject pointPrefab;
    [SerializeField]
    private RectTransform uiParent;
    [SerializeField]
    private Slider memSlider;
    private void Awake()
    {
        MainUIEvent.Instance.StartListening(this);
    }

    private void OnDestroy()
    {
        MainUIEvent.Instance.StopListening(this);
    }
    private void ResetSlider()
    {
        foreach(Transform child in uiParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void InitSlider(int maxMember, int curMember)
    {
        memSlider.minValue = 1;
        memSlider.maxValue = maxMember;
        float value = uiParent.GetComponent<RectTransform>().rect.width;
        ResetSlider();
        if (maxMember == 1)
        {
            GameObject point = Instantiate(pointPrefab, uiParent);
            RectTransform rect = point.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(value, 0);
            ChangeValue(maxMember, curMember);
            return;
        }

        //lastPoint = SetPivot(lastPoint, new Vector2(0, 0.5f));
        /* - lastPoint.anchoredPosition.x + */
        //float value = Mathf.Abs(lastPoint.anchoredPosition.x) - firstPoint.anchoredPosition.x;
        Debug.Log($"first:value : {value} last val : {value / maxMember}");
        for (int i = 0; i < maxMember; i++)
        {
            GameObject point = Instantiate(pointPrefab, uiParent);
            RectTransform rect = point.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2((value) / (maxMember - 1) * i, 0);
            Debug.Log((value / maxMember) * i);
        }
        ChangeValue(maxMember, curMember);
        //SetPivot(lastPoint, new Vector2(1, 0.5f));
        //lastPoint.pivot = new Vector2(0.5f,0.5f);
        //lastPoint.anchorMin = new Vector2(1, 0.5f);
        //lastPoint.anchorMax = new Vector2(1, 0.5f);
    }

    private void ChangeValue(int maxMember, int curMember)
    {
        memSlider.value = curMember;
        for (int i = 0; i < maxMember; i++)
        {
            GameObject child = uiParent.GetChild(i).gameObject;
            if (i + 1 <= curMember)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public RectTransform SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        //새 피벗 위치에서 이전 피벗 위치를 빼고 여기에 개체 크기 및 배율을 곱합니다.이것은 개체가 제자리에 유지되도록 이동하려는 거리입니다. 현재 개체의 위치에 추가합니다.
        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
        return rectTransform;
    }

    public void ListenerUpdate(UISubject subject)
    {
        if (subject is MainUIEvent)
        {
            MainUIEvent ui = (MainUIEvent)subject;

            if (ui.State == MainUIEventState.InitWaitMemUI)
            {
                OnJoinRoom onJoinRoom = ui.Packet as OnJoinRoom;
                InitSlider(onJoinRoom.Room.MaxPlayer, onJoinRoom.Room.CurrentPlayer);
            }
            if (ui.State == MainUIEventState.OnJoinWaitMemUI)
            {
                OnJoinRoom onJoinRoom = ui.Packet as OnJoinRoom;
                ChangeValue(onJoinRoom.Room.MaxPlayer, onJoinRoom.Room.CurrentPlayer);
            }
            if (ui.State == MainUIEventState.OnLeaveWaitMemUI)
            {
                OnLeaveRoom onLeaveRoom = ui.Packet as OnLeaveRoom;
                ChangeValue(onLeaveRoom.Room.MaxPlayer, onLeaveRoom.Room.CurrentPlayer);
            }
            //if(ui.State == MainUIEventState.ShowChat)
            //{
            //    OnJoinRoom onJoinRoom = ui.Packet as OnJoinRoom;
            //    ResetValue();
            //}
        }
    }
}
