using GameEvent;
using PROTO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoManager : MonoSingleTon<MemoManager>
{
    JobMemoManager jobMemoManager;
    private void Start()
    {
        EventManager.Instance.StartListening(DicEventType.MemoJob, new Action<int,Jobs>(SendMemo));
        jobMemoManager = FindObjectOfType<JobMemoManager>();
    }

    public void SendMemo(int seat, Jobs job)
    {
        Debug.Log("SendMemo");
        memoJob memoJob = new memoJob();
        memoJob.Job = job;
        memoJob.Seat = seat;
        NetworkManager.Instance.Send(memoJob, PacketID.MemoJob);
    }

    public void Memo(Jobs jobType,int seat)
    {
        jobMemoManager.Memo(jobType, seat);
    }
}
