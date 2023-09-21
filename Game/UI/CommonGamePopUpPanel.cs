using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonGamePopUpPanel : GamePopUpPanel
{
    public override void OnPopped()
    {
        gameObject.SetActive(false);
    }

    public override void OnPushed()
    {
        gameObject.SetActive(true);
    }
}
