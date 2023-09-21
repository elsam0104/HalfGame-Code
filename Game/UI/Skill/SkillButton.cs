using GameEvent;
using GameEvent.UI;
using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using WebSocketSharp;

public class SkillButton : IntParamButton,UIListener
{
    public SkillType skill = 0;
    public bool isUseable = false;

    [SerializeField]
    Image skillIcon;
    [SerializeField]
    CanvasGroup fadeGroup;

    [SerializeField]
    SkillTurn skillTurn;

    //Image 

    [SerializeField]
    float coolTime;

    Coroutine coolTimeCor = null;



    public float CoolTime
    {
        get 
        {
            return coolTime; 
        }
        set
        { 
            coolTime = value;
            StopCoroutine(coolTimeCor);
            StartCoroutine(CoolTimeProcess(value));
        }
    }

    private void Awake()
    {
        type.Add(GameEvent.DicEventType.ClickSkillBtn);
        value.Add((int)skill);
    }

    protected override void Start()
    {
        base.Start();
        GameUIEvent.Instance.StartListening(this);
    }
    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if (ui.State == GameUIEventState.SkillUsableNotice)
        {
            SkillUsableNotice packet = ui.Packet as SkillUsableNotice;
            if(packet.SkillType.Equals(skill))
            {
                fadeGroup.alpha = packet.IsUsable?1:0.3f;
            }
        }
        if (ui.State == GameUIEventState.ShowJob)
        {
            ShowMyJob showMyJob = ui.Packet as ShowMyJob;
            var jobdata = showMyJob.JobData;
            string skillName = skillNameConverter(jobdata);
            if(skillName==null)
            {
                Destroy(gameObject);
            }
            else
            {
                Addressables.LoadAssetAsync<Sprite>($"skillIcon_{skillName}").Completed += (handle) =>
                {
                    skillIcon.sprite = handle.Result;
                };
            }

        }
    }

    public string skillNameConverter(JobData jobData) =>
        skill switch
        {
            SkillType.First => jobData.FirstSkillData?.Name,
            SkillType.Second => jobData.SecondSkillData?.Name,
            SkillType.Ultimate => jobData.UltimateSkillData?.Name,
            _ => ""
        };


    public void FocusEffect(bool isActive)
    {

    }


    public IEnumerator CoolTimeProcess(float coolTime)
    {
        float delta = 0.1f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delta);

        while(coolTime > 0)
        {
            yield return waitForSeconds;
            coolTime -= delta;
        }
    }

    public void OnSelected(bool isMe)
    {

    }

    public void ResetSelection()
    {

    }


}
