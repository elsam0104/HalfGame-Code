using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameEvent.UI;
using GameEvent;
using PROTO;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using DG.Tweening;

public class JobPanelUI : GamePopUpPanel, UIListener
{
    public Image blackPanel;
    
    [SerializeField]
    private TMP_Text jobInfoText;
    [SerializeField]
    private TMP_Text jobText;
    [SerializeField]
    private List<TMP_Text> backGroundJobTexts;
    [SerializeField]
    private GameObject jobPanel;
    [SerializeField]
    private Image jobImage;

    [SerializeField]
    private Image backGroundJobImage;
    
    [SerializeField]
    private ShowPassiveSkillUI showPassiveSkillUIPrfab;
    [SerializeField]
    private ShowFirstSkillUI showFirstSkillUIPrefab;
    [SerializeField]
    private ShowSecondSkillUI showSecondSkillUIPrefab;
    [SerializeField]
    private ShowUltimateSkillUI showUltimateSkillUIPrfab;
    
    [SerializeField]
    private Transform characterSkillParent;

    [SerializeField]
    private Image jobIcon;
    [SerializeField]
    private TMP_Text hp,atk;
    const string defultValue = "name";

    List<AsyncOperation> handles;
    private void Start()
    {
        GameUIEvent.Instance.StartListening(this);
        Debug.Log(hp);
        //jobPanel.GetComponent<Button>().onClick.AddListener(HideJobPanel);
    }
    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    //private void Update()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        SkillInfoMove();
    //    }
    //}
    //private void SkillInfoMove()
    //{
    //}
    public void ReleaseAddressable()
    {
        handles.ForEach(x => Addressables.Release(x));
    }
    public void ShowJobPanel(ShowMyJob showMyJob)
    {
        GetComponent<Animator>().Play("Init");

        blackPanel?.DOFade(0, 1);
        var jobData = showMyJob.JobData;
        
        jobText.text = jobData.Name;
        backGroundJobTexts.ForEach(x=> x.text = jobData.Job.ToString().ToUpper());
        if(jobInfoText)
        jobInfoText.text = jobData.Information;

        hp.text = jobData.HP.ToString();
        atk.text = jobData.Atk.ToString();

        Addressables.LoadAssetAsync<Sprite>($"pngIllust_{jobData.Job}").Completed += (handle) =>
        {
            jobImage.sprite = handle.Result;
        };
        
        Addressables.LoadAssetAsync<Sprite>($"illust_{jobData.Job}").Completed += (handle) =>
        {
            backGroundJobImage.sprite = handle.Result;
        };

        if(jobData.PassiveSkillData!=null)
        {
            var passive = Instantiate(showPassiveSkillUIPrfab, characterSkillParent);
            passive.Init(jobData.PassiveSkillData);
        }


        if (jobData.FirstSkillData != null)
        {
            var first = Instantiate(showFirstSkillUIPrefab, characterSkillParent);
            first.Init(jobData.FirstSkillData);
        }

        if (jobData.SecondSkillData != null)
        {
            var second = Instantiate(showSecondSkillUIPrefab, characterSkillParent);
            second.Init(jobData.SecondSkillData);
        }

        if (jobData.UltimateSkillData != null)
        {
            var ultimate = Instantiate(showUltimateSkillUIPrfab, characterSkillParent);
            ultimate.Init(jobData.UltimateSkillData);
        }

    }
    private void InitJobIcon(Jobs jobType,string name)
    {
        jobIcon.sprite = JobImage.Instance.IconImage[jobType];
    }
    private void HideJobPanel()
    {
        GamePopUpManager.Instance.Pop();
    }


    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if (ui.State == GameUIEventState.ShowJob)
        {
            Debug.Log("ShowJob");
            ShowMyJob showMyJob = ui.Packet as ShowMyJob;
            {
                GamePopUpManager.Instance.Push(GamePopUpType.JobPanel);
                ShowJobPanel(showMyJob);
                //InitJobIcon(showMyJob.JobData.Job,showMyJob.JobData.Name);
            }
        }
    }


    public override void OnPushed()
    {
        gameObject.SetActive(true);
        blackPanel.DOFade(0, 0.5f).OnComplete(()=>blackPanel.gameObject.SetActive(false));
    }

    public override void OnPopped()
    {
        blackPanel.gameObject.SetActive(true);
        blackPanel.DOFade(1, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
