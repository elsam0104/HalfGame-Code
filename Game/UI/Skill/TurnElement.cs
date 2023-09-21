
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnElement : MonoBehaviour
{
    public Image image;
    private void Awake()
    {
        image= GetComponent<Image>();
    }

    public void Activate(bool isActive)
    {
        image.color = new Color(1,1,1,isActive?1:0.5f);
    }
}
