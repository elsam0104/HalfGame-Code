using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PROTO;
public class SkillNum : IntParamButton
{
    public SkillType skill = 0;
    public bool isUseable = false;
    private void Awake()
    {
        type.Add(GameEvent.DicEventType.ClickSkillBtn);
        value.Add((int)skill);
    }
}
