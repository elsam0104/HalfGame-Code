using DG.Tweening;
using GameEvent;
using GameEvent.UI;
using PROTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour, UIListener
{
    [SerializeField]
    private GameObject userPanel;
    [SerializeField]
    private GameObject skillPanel;
    [SerializeField]
    private UserTarget userPrefab;
    [SerializeField]
    private ParticleSystem chooseParticle;
    private SkillType skillType;

    [SerializeField]
    private Dictionary<int, UserTarget> userTargets = new Dictionary<int, UserTarget>();

    public SkillType curSkillType
    {
        get => skillType;
        set
        {
            skillType = value;
            moveEffect((int)value);
        }
    }

    private void Start()
    {
        skillType = SkillType.NoneSkill;
        chooseParticle.gameObject.SetActive(false);
        GameUIEvent.Instance.StartListening(this);
        EventManager.Instance.StartListening(DicEventType.ClickSkillBtn, new Action<SkillType>(SetSkillType));
        EventManager.Instance.StartListening(DicEventType.ResetSkillType, new Action(ResetSkillType));
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(DicEventType.ClickSkillBtn, new Action<SkillType>(SetSkillType));
        EventManager.Instance.StopListening(DicEventType.ResetSkillType, new Action(ResetSkillType));
        GameUIEvent.Instance.StopListening(this);
    }
    public void moveEffect(int index)
    {
        if (index == 0)
        {
            chooseParticle.Stop();
            chooseParticle.gameObject.SetActive(false);
            return;
        }
        chooseParticle.gameObject.SetActive(true);
        chooseParticle.transform.position = skillPanel.transform.GetChild(index).position;
        chooseParticle.Play();
    }
    public void ShakeUsers()
    {
        for (int i = 0; i < userPanel.transform.childCount; i++)
        {
            Transform obj = userPanel.transform.GetChild(i).GetChild(0);
            obj.DOKill();
            obj.DOShakePosition(1).SetLoops(-1);
        }
    }
    public void StopShakeUsers()
    {
        for (int i = 0; i < userPanel.transform.childCount; i++)
        {
            Transform obj = userPanel.transform.GetChild(i).GetChild(0);
            obj.DOKill();
        }
    }
    public void ResetSkillType()
    {
        curSkillType = 0;
        FindObjectOfType<LowerPanelButton>().Toggle(true);
    }
    public void InitUser(string user, int seat, Jobs jobs, bool isAlive)
    {
        UserTarget userTarget = null;
        if (userTargets.ContainsKey(seat) == false)
        {
            userTarget = Instantiate(userPrefab, userPanel.transform.GetChild(seat - 1));
            List<int> target = new List<int>();
            target.Add(seat);
            userTarget.GetComponent<Button>().onClick.AddListener(() => JugeAttack(target, (int)curSkillType));
            userTargets.Add(seat, userTarget);
        }
        else
        {
            userTarget = userTargets[seat];
        }

        //UserSOManager.Instance.memoSO.memoJobs[seat - 1] = jobs;


        userTarget.userName.text = user;
        userTarget.targetNo = seat;
        userTarget.userProfile.sprite = JobImage.Instance.IconImage[jobs];
        Color color = userTarget.GetComponent<Image>().color;
        userTarget.setAlive(isAlive);

    }
    public void ClickSkillBtn(SkillType skillType)
    {
        Debug.Log("send");
        curSkillType = skillType;
    }

    public void JugeAttack(List<int> target, int skillType)
    {
        //StopShakeUsers();
        if (skillType != 0)
        {
            PlayerManager.Instance.SkillUse(target, (SkillType)skillType);
            SetSkillType(SkillType.NoneSkill);
        }
        else if (UserSOManager.Instance.dataSO.curDayType == DayType.Votetime)
            PlayerManager.Instance.Vote(target[0]);

        else if (UserSOManager.Instance.dataSO.curDayType == DayType.Night)
        {
            PlayerManager.Instance.Attack(target[0]);
        }
        print("Skill : " + curSkillType);
    }
    public void SetSkillType(SkillType type)
    {
        if (curSkillType == type)
        {
            curSkillType = SkillType.NoneSkill;
        }
        else
        {
            curSkillType = type;
        }
        userTargets.Values.ToList().ForEach(target =>
        {
            target.Shake(curSkillType != SkillType.NoneSkill);
        });
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if (ui.State == GameUIEventState.InitUserPanel)
        {
            PlayerBox playerBox = ui.Packet as PlayerBox;
            InitUser(playerBox.Username, playerBox.Seat, playerBox.JobType, playerBox.IsAlive);
        }
    }
}
