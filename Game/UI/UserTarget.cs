using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserTarget : MonoBehaviour
{
    public int targetNo;
    public Image userProfile;
    public TMP_Text userName;

    public Animator animator;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Attacked()
    {
        animator.SetTrigger("Attacked");
    }
    public void Shake(bool isShake)
    {
        animator.SetBool("Shake",isShake);

    }
    public void setAlive(bool isAlive)
    {
        canvasGroup.alpha = isAlive ? 1 : 0.6f;
    }
}
