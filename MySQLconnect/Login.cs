using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    [SerializeField]
    private InputField emailField;
    [SerializeField]
    private GameObject loginPanel;
    [SerializeField]
    private GameObject charselectui;

    private void Update()
    {
        if (DBManager.isLoggedIn)
        {
            loginPanel.SetActive(false);
            charselectui.SetActive(true);
            DBManager.isLoggingIn = false;
        }
    }

    public void Loginfunc()
    {
        WWWForm loginForm = DBops.LoginForm(emailField.text);
        Debug.Log("Login started");
        StartCoroutine(emailCheck(Link.logIn, loginForm));
        //charselectui.SetActive(true);


    }

    IEnumerator emailCheck(string target, WWWForm loginForm)
    {
        Debug.Log("coroutine started");
        UnityWebRequest request = UnityWebRequest.Post(target, loginForm);
        // request.SetRequestHeader("Content-Type", "application/php");
        request.SetRequestHeader("User-Agent", "DefaultBrowser"); 
        // request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("returned request");
            Debug.Log(request.downloadHandler.text);
        }



        string[] emailCheckResult = DBops.GetResultFromRequest(request);
        if (emailCheckResult[0] == "0")
        {
            Debug.Log(emailCheckResult[0]);
            DBManager.email = emailCheckResult[1];
            DBManager.name = emailCheckResult[2];
            DBManager.rolln = emailCheckResult[3];
            // if(emailCheckResult[4] != null){
            //     DBManager.teamID = int.Parse(emailCheckResult[4]);
            // }
            DBManager.swim = float.Parse(emailCheckResult[5]);
            DBManager.hurdle = float.Parse(emailCheckResult[6]);
            DBManager.basket = int.Parse(emailCheckResult[7]);
            DBManager.collectStart = int.Parse(emailCheckResult[8]);
            DBManager.collect1 = int.Parse(emailCheckResult[9]);
            DBManager.collect2 = int.Parse(emailCheckResult[10]);
            DBManager.collect3 = int.Parse(emailCheckResult[11]);
            DBManager.collect4 = int.Parse(emailCheckResult[12]);
            DBManager.isCollectStarted = (DBManager.collectStart != 0);
            DBManager.isCollected1 = (DBManager.collect1 != 0);
            DBManager.isCollected2 = (DBManager.collect2 != 0);
            DBManager.isCollectCompleted = (DBManager.collect3 != 0);

            Debug.Log(DBManager.email + " " + DBManager.name + " " + DBManager.rolln + " " + DBManager.teamID + " " + DBManager.swim
                 + " " + DBManager.hurdle + " " + DBManager.basket + " collect start -" + DBManager.collectStart + " " + DBManager.collect1
                  + " " + DBManager.collect2 + " " + DBManager.collect3 + " " + DBManager.collect4 + " " + DBManager.isCollectStarted
                   + " " + DBManager.isCollected1 + " " + DBManager.isCollected2 + " " + DBManager.isCollectCompleted);
            // SceneManager.LoadScene("TerrainS");
            DBManager.isLoggingIn = true;
            
        }

        if (emailCheckResult[0] == "9")
        {
            Debug.Log(emailCheckResult[0]);
        }
    }
}