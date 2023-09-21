using DG.Tweening.Core.Easing;
using PROTO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonClickLight : MonoBehaviour
{
    [SerializeField]
    private Image SkillImage;
    [SerializeField]
    private Button[] disabledSkill;

    private bool isCheck = false;
    void Start()
    {
        SkillImage = GetComponent<Image>();
    }

    public void OnButtonClick()
    {
        if (!isCheck)
        {
            // 버튼 색상 변경
            SkillImage.color = Color.red;
            isCheck = true;

            // 지정된 버튼들 비활성화
            foreach (Button button in disabledSkill)
            {
                button.interactable = false;
            }
        }
        else
        {
            // 버튼 색상 변경
            SkillImage.color = new Color(176, 176, 176, 255);
            isCheck = false;

            // 지정된 버튼들 활성화
            foreach (Button button in disabledSkill)
            {
                button.interactable = true;
            }
        }
    }

    public void PlayerClick()
    {
        SkillImage.color = Color.red;
        StartCoroutine(PlayerCool());
    }

    IEnumerator PlayerCool()
    {
        yield return new WaitForSeconds(0.3f);
        SkillImage.color = Color.white;
    }
}