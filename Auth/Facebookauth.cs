using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Firebase.Auth;
using TMPro;
using Auth;

public class FacebookAuth : MonoBehaviour
{
    [SerializeField]
    private TMP_Text log;
    private void Awake()
    {
        if (!FB.IsInitialized) //초기화 확인
        {
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }
    private void InitCallBack()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp(); //개발자 페이지의 앱 활성화
            AddLog("Start initialize");
        }
        else
        {
            AddLog("Failed to initialize");
        }
    }
    private void OnHideUnity(bool isgameshown)
    {
        if (!isgameshown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Facebook_Login()
    {
        var permission = new List<string>() { "public_profile", "email", "user_friends" };//권한
        FB.LogInWithReadPermissions(permission, AuthCallBack);
    }
    public void Facebook_LogOut()
    {
        FB.LogOut();
        AddLog("User log out.");
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (result.Error != null)
        {
            AddLog($"Auth error : {result.Error}");
            return;
        }
        if (FB.IsLoggedIn)
        {
            // Print current access token's User IDDebug.Log(aToken);
            var aToken = AccessToken.CurrentAccessToken;

            AddLog(aToken.UserId);
            foreach (string perm in aToken.Permissions)
            {
                AddLog(perm);
            }
            StartCoroutine(coLogin(aToken));
        }
        else
        {
            AddLog("User Cancelled login");
        }

    }

    /// <summary>
    /// 실제 로그인
    /// </summary>
    IEnumerator coLogin(AccessToken aToken)
    {
        AddLog($"\n Try to get token...{aToken}");
        while (System.String.IsNullOrEmpty(aToken.TokenString))
            yield return null;

        AddLog($"\n Try auth for facebook ...{aToken}");
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        //credential은 인증서 같은 의미
        //FacebookAuthProvider : 페이스북에서 제공하는 액세스토큰을 가져오는것.
        Credential credential = FacebookAuthProvider.GetCredential(aToken.TokenString);

        AddLog($"\n credential is {credential}");

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                AddLog("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                AddLog(string.Format("SignInWithCredentialAsync encounted an error : {0}", task.Exception));
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            AuthManager.Instance.SaveUserId(aToken.TokenString,LoginType.Facebook);
        });
    }

    private void AddLog(string str)
    {
        log.text = str;
    }
    //public void authwithfirebase(string accesstoken)
    //  {
    //      auth = FirebaseAuth.DefaultInstance;
    //      Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accesstoken);
    //      auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
    //      {
    //          if (task.IsFaulted)
    //          {
    //             Debug.Log("singin encountered error" + task.Exception);
    //          }
    //          Firebase.Auth.FirebaseUser newuser = task.Result;
    //         Debug.Log(newuser.DisplayName);
    //      });
    //  }
}
