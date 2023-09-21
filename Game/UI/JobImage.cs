using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeSerializableDictionary;
using PROTO;

public class JobImage : MonoSingleton<JobImage>
{
    [SerializeField]
    private SerializableDictionary<Jobs,Sprite> skillImage = new SerializableDictionary<Jobs,Sprite>();
    [SerializeField]
    private SerializableDictionary<Jobs,Sprite> iconImage = new SerializableDictionary<Jobs,Sprite>();

    public SerializableDictionary<Jobs,Sprite> SkillImage => skillImage;
    public SerializableDictionary<Jobs, Sprite> IconImage => iconImage;
}
