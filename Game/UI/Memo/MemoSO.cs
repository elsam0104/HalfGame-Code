using PROTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoSO", menuName = "SO/User/Memo")]
public class MemoSO : ScriptableObject
{
    /// <summary>
    /// ���۽� �ʱ�ȭ �ϴ°� ������ ��
    /// </summary>
    public List<Jobs> memoJobs = new List<Jobs>();
}
