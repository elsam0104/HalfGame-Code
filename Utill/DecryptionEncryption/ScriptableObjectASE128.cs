using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectASE128 : ScriptableObject
{
    protected Dictionary<string, object> soDictionary = new Dictionary<string, object>();
    private void OnEnable()
    {
        Init();
    }
    protected abstract void Init();
    public T GetObj<T>(string name)
    {
        if (!soDictionary.ContainsKey(name)) return default(T);
        if (typeof(T) == typeof(string))
        {
            Debug.LogError("string type have to crypto. try GetStrObj method.");
            return default(T);
        }
        try
        {
            return (T)soDictionary[name];
        }
        catch (InvalidCastException ex)
        {
            Debug.LogError("Error in GetObj: " + ex.Message);
            return default(T);
        }
    }
    public void SetObj(string name, object value)
    {
        try
        {
            soDictionary[name] = value;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in SetObj: " + ex.Message);
        }
    }
    public string GetStrObj(string name)
    {
        string decrypt = soDictionary[name].ToString();
        decrypt = Crypto.AESDecrypt128(decrypt);
        return decrypt;
    }
    public void SetStrObj(string name, string value)
    {
        try
        {
            value = Crypto.AESEncrypt128(value);
            soDictionary[name] = value;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in SetStrObj: " + ex.Message);
        }
    }
}
