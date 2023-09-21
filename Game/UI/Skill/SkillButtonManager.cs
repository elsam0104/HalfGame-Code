using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonManager : MonoBehaviour
{
    List<SkillButton> skillButtons;

    public void SetUsing(SkillType skillType)
    {
        SkillButton current = skillButtons.Find(btn=>btn.skill == skillType);
        if (current == null) return;

        skillButtons.ForEach(btn => btn.OnSelected(btn.skill == skillType));
    }

    public void ResetSelection()
    {
        skillButtons.ForEach(btn => btn.ResetSelection());
    }
}
