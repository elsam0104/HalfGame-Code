using GameEvent;
using GameEvent.UI;
using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobMemoManager : MonoBehaviour, UIListener
{
    [SerializeField]
    private Transform jobMemoParent;
    [SerializeField]
    private Transform userButtonParent;
    [SerializeField]
    private GameObject userButtonPrefab;

    private List<JobMemoElement> jobMemoButtons = new List<JobMemoElement>();
    private List<UserBtn> userButtons = new List<UserBtn>();
    private void Start()
    {
        Init();
    }
    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    private void Init()
    {
        GameUIEvent.Instance.StartListening(this);
        for (int i = 0; i < jobMemoParent.childCount; i++)
        {
            var child = jobMemoParent.GetChild(i);
            JobMemoElement element = child.GetComponent<JobMemoElement>();
            //element.jobs = (PROTO.Jobs)i;
            jobMemoButtons.Add(element);
        }
    }
    public void SetUpSeat(int seat, string name, Jobs job = Jobs.None)
    {
        Button button = null;
        if (userButtonParent.GetChild(seat - 1).childCount <= 0)
        {
            button = Instantiate(userButtonPrefab, userButtonParent.GetChild(seat - 1)).GetComponent<Button>();
            //Button button = userButtonParent.GetChild(seat).gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => SetjobMemoPanel(seat));
            Debug.Log($"{seat} 번 초기화실시");
        }
        else
        {
            button = userButtonParent.GetChild(seat - 1).GetChild(0).GetComponent<Button>();
        }

        UserBtn btn = button.GetComponent<UserBtn>();
        btn.userName.text = name;
        btn.image.sprite = JobImage.Instance.IconImage[job];
        userButtons.Add(btn);
    }
    private void SetjobMemoPanel(int seat)
    {
        foreach (var element in jobMemoButtons)
        {
            Button button = element.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => element.SendMemo(seat));
            button.onClick.AddListener(() => EventManager.Instance.TriggerEvent(DicEventType.PopGamePopUp));
        }
    }

    public void Memo(Jobs jobType, int seat)
    {
        userButtons[seat-1].image.sprite = JobImage.Instance.IconImage[jobType];
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if (ui.State == GameUIEventState.InitUserPanel)
        {
            PlayerBox playerBox = ui.Packet as PlayerBox;
            SetUpSeat(playerBox.Seat, playerBox.Username, playerBox.JobType);
        }
    }
}
