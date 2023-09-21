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
            // ��ư ���� ����
            SkillImage.color = Color.red;
            isCheck = true;

            // ������ ��ư�� ��Ȱ��ȭ
            foreach (Button button in disabledSkill)
            {
                button.interactable = false;
            }
        }
        else
        {
            // ��ư ���� ����
            SkillImage.color = new Color(176, 176, 176, 255);
            isCheck = false;

            // ������ ��ư�� Ȱ��ȭ
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