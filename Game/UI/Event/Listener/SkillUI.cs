using System.Collections;
using System.Collections.Generic;
using UniSoftwareKeyboardArea;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PROTO;
using GameEvent.UI;
using GameEvent;
using Newtonsoft.Json;
using NativeSerializableDictionary;

public class SkillUI : MonoBehaviour, UIListener
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private RectTransform upperScreenRectTransform;
    [SerializeField]
    private RectTransform BottomPanel;
    [SerializeField]
    private RectTransform skillPanel;
//    [SerializeField]
//    private Button skillBtn;
    private bool toggle = false;
    private float changeY = 0f,screenChangeY = 0f;
    [SerializeField] 
    private SerializableDictionaryBoxed<SkillType, SkillNum> skills = new SerializableDictionaryBoxed<SkillType, SkillNum>();
    private void Start()
    {
        //changeY = rectTransform.anchoredPosition.y * -2;
        changeY = BottomPanel.rect.height;
        screenChangeY = BottomPanel.rect.height;
        foreach (Transform child in skillPanel)
        {
            SkillNum skill;
            if (child.TryGetComponent<SkillNum>(out skill))
                skills.Add(skill.skill, skill);
        }

        //skillBtn.onClick.AddListener(OnPressSkillBtn);
        GameUIEvent.Instance.StartListening(this);
    }

    private void OnDestroy()
    {
        GameUIEvent.Instance.StopListening(this);
    }
    public void OnPressSkillBtn()
    {
        Debug.Log(changeY);
        rectTransform.DOKill();
        toggle = !toggle;
        float y = toggle ? changeY : 0;
        rectTransform.DOAnchorPosY(y, 0.1f);
        float screenY = toggle ? screenChangeY : 0;
        upperScreenRectTransform.DOAnchorPosY(screenY, 0.1f);
    }
    public void CoolTimeSet(SkillType skillType, float time)
    {
        ShowCoolTime(skillType, time);
        StartCoroutine(CoolTime(skillType, time));
    }
    private void ShowCoolTime(SkillType skillType, float time)
    {
        Image fill = skills[skillType].gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        fill.DOFillAmount(0, time);
    }
    private IEnumerator CoolTime(SkillType skillType, float time)
    {
        skills[skillType].isUseable = false;
        yield return new WaitForSeconds(time);
        skills[skillType].isUseable = true;
    }

    public void ListenerUpdate(UISubject subject)
    {
        GameUIEvent ui = (GameUIEvent)subject;
        if (ui.State == GameUIEventState.InitCoolTime)
        {
            CoolDownStart coolDownStart = ui.Packet as CoolDownStart;
            {
                //CoolTimeSet(coolDownStart.SkillType, coolDownStart.CoolDownTime);
            }
        }
    }
}
