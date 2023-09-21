using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
namespace Auth
{
    public enum LoginType
    {
        None = -1,
        Google,
        Facebook,
    }
    public class AuthManager : MonoSingleTon<AuthManager>
    {
        public UserAuthSO userAuthSO;
        private StringBuilder _sb;
        private void Awake()
        {
            _sb = new StringBuilder();
        }
        private void SaveSB(string str)
        {
            _sb.Remove(0, _sb.Length);
            _sb.Append(str);
        }
        public void SaveUserId(string str, LoginType type)
        {
            SaveSB(str);
            userAuthSO.SetObj("loginType", type);
            userAuthSO.SetStrObj("UserAuthId", _sb.ToString());
        }
        public void SavePhoneId(string str)
        {
            SaveSB(str);
            userAuthSO.SetStrObj("PhoneAuthId", _sb.ToString());
        }
    }

}