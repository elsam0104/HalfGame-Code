using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : MonoSingleTon<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] list = FindObjectsOfType<T>(true);

                
                
                switch(list.Length)
                {
                    case 0:
                        Debug.LogError("No instance of " + typeof(T).Name + " found!");
                        break;
                    case 1:
                        instance = list[0];
                        break;
                    default:
                        for(int i= 0;i < list.Length; i++)
                        {
                            Debug.LogError("??" + list[i].gameObject.name);
                        }
                        Debug.LogError($"{list.Length} instance of " + typeof(T).Name + " found!");
                        break;
                }

                if(instance==null)
                {
                    Debug.LogError("There's no active " + typeof(T).Name + " in this scene.");
                    return null;
                }
            }
            return instance;
        }
    }
}
