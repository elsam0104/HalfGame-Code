using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameEvent.UI;

[RequireComponent(typeof(RectTransform),typeof(CanvasGroup))]
public class LobbyPanelElement : MonoBehaviour,LobbyPanel
{
    public MainUIEventState eventType;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    Sequence sequence;
    public void Active()
    {
        print("active");
        sequence.Kill();
        sequence = DOTween.Sequence();

        sequence.Append(canvasGroup.DOFade(1, 1));
        sequence.Join(rectTransform.DOAnchorPos(new Vector3(0, -rectTransform.localScale.x, 0), 0));
        sequence.Join(rectTransform.DOLocalMoveX(0, 1));
        canvasGroup.blocksRaycasts= true;
    }

    public void DeActive()
    {
        print("deactive");
        sequence.Kill();
        sequence = DOTween.Sequence();

        sequence.Append(canvasGroup.DOFade(0, 1));
        sequence.Join(rectTransform.DOAnchorPos(new Vector3(0, 0, 0), 0));
        sequence.Join(rectTransform.DOLocalMoveX(-rectTransform.localScale.x, 1));
        canvasGroup.blocksRaycasts= false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        LobbyPanelManager.Instance.lobbyPanels.Add(this.eventType, this);
        canvasGroup.alpha = 0;
        DeActive();
        
    }

}
