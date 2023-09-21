using GameEvent;
using GameEvent.UI;
using PROTO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobMemoElement : MonoBehaviour
{
    public Jobs jobs;
    public Image image;
    //public TMP_Text jobName;
    private void Start()
    {
        SetElement(jobs);
    }

    public void SetElement(Jobs jobs)
    {
        image.sprite = JobImage.Instance.IconImage[jobs];
    }
    public void SendMemo(int seat)
    {
        MemoManager.Instance.SendMemo(seat,jobs);
    }
}
