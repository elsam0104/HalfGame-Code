using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButtion : BaseButton
{
    [SerializeField]
    ScrollManager scrollManager;
    public int targetIndex = 0;
    public override void Run()
    {
        int target = scrollManager.TargetIndex == targetIndex ? 1 : targetIndex;
        scrollManager.TabClick(target);
        print(scrollManager.TargetIndex);
    }
}
