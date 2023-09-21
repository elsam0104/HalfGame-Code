using DG.Tweening;
using DTT.Utils.Extensions;
using GameEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillBarToggle : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
    [SerializeField]
    private RectTransform moveRect;
    private float changeY = 0;
    private bool toggle = false;
    private void Start()
    {
        changeY = moveRect.rect.height;
        Debug.LogError(moveRect.rect.height);
        EventManager.Instance.StartListening(DicEventType.SkillBarToggle, new Action(SkillBarAnim));
        SkillBarAnim();
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(DicEventType.SkillBarToggle,new  Action(SkillBarAnim));
    }
    private void SkillBarAnim()
    {
        Debug.Log(changeY);
        moveRect.DOKill();
        toggle = !toggle;
        float y = moveRect.anchoredPosition.y + (toggle ? 1 : -1) * changeY;
        moveRect.DOAnchorPosY(y, 0.4f);
    }
}
