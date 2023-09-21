using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoSO", menuName = "SO/User/Memo")]
public class MemoSO : ScriptableObject
{
    /// <summary>
    /// 시작시 초기화 하는거 만들어야 함
    /// </summary>
    public List<Jobs> memoJobs = new List<Jobs>();
}
