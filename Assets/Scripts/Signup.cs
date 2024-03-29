using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class Signup : MonoBehaviour
{
    public static Signup instance;

    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject signupCanvas;

    public bool firstLogin;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }

    public void Submit()
    {
        StartCoroutine(CreateAccount());
    }

    public IEnumerator CreateAccount()
    {
        string saveIdBuffer = "";

        bool error = false;
        //Check if email is valid
        try
        {
            string address = new MailAddress(emailInput.text).Address;
        }
        catch (FormatException)
        {
            Error.instance.DisplayError("Error : Email format invalid");
            yield break;
        }

        using (var request = new WebRequestBuilder()
                   .SetURL("users/?where={\"username\":\"" + usernameInput.text + "\"}",
                       "GET")
                   .SetDownloadHandler(new DownloadHandlerBuffer())
                   .Build())
        {
            yield return request.SendWebRequest();
            var matches = Regex.Matches(request.downloadHandler.text,
                "\"username\":\"(.[^,]+)",
                RegexOptions.Multiline);

            if (matches.Count > 0)
            {
                Error.instance.DisplayError("Error : the username is already in use");
                error = true;
                yield break;
            }
        }

        //Check if username is already used
        /*string uri = "https://parseapi.back4app.com/users/?where={\"username\":\"" + usernameInput.text + "\"}";
        using (var request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-Parse-Application-Id",
                Secrets.appId);
            request.SetRequestHeader("X-Parse-REST-API-Key",
                Secrets.restApi);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }*/


        //Check if email is already in use

        using (var request = new WebRequestBuilder()
                   .SetURL("users/?where={\"email\":\"" + emailInput.text + "\"}", "GET")
                   .SetDownloadHandler(new DownloadHandlerBuffer())
                   .Build())
        {
            Debug.Log(request.url);
            yield return request.SendWebRequest();
            
            
            /*if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                Debug.Log(request.url);
                yield break;
            }*/

            if (request.downloadHandler.text != "{\"results\":[]}")
            {
                Error.instance.DisplayError("Error : the mail is already in use");
                error = true;
                //yield break;
            }
        }

        /*uri = "https://parseapi.back4app.com/users/?where={\"email\":\"" + emailInput.text + "\"}";
        using (var request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("X-Parse-Application-Id",
                Secrets.appId);
            request.SetRequestHeader("X-Parse-REST-API-Key",
                Secrets.restApi);
            
        }*/

        //Create PlayerProfile
        if (!error)
        {
            /*using (var request = new UnityWebRequest("https://parseapi.back4app.com/classes/PlayerProfile",
                       "POST"))
            {
                request.SetRequestHeader("X-Parse-Application-Id",
                    Secrets.appId);
                request.SetRequestHeader("X-Parse-REST-API-Key",
                    Secrets.restApi);
                request.SetRequestHeader("Content-Type",
                    "application/json");

                var data = new
                    { username = usernameInput.text, email = emailInput.text};
                string json = JsonConvert.SerializeObject(data);

                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.downloadHandler = new DownloadHandlerBuffer();*/

            using (var request = new WebRequestBuilder()
                       .SetURL("classes/PlayerProfile", "POST")
                       .SetJSON(new { username = usernameInput.text, email = emailInput.text })
                       .SetDownloadHandler(new DownloadHandlerBuffer())
                       .Build())
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(request.error);
                    yield break;
                }
                else if (request.result == UnityWebRequest.Result.Success)
                {
                    Error.instance.DisplayError("Success !");


                    var matches = Regex.Match(request.downloadHandler.text,
                        "\\\"objectId\\\":\\\"(.[^,]+)\"",
                        RegexOptions.Multiline);

                    saveIdBuffer = matches.Groups[1].ToString();
                    PlayerData.instance.saveId = saveIdBuffer;
                }
            }
        }

        /* using (var request = new UnityWebRequest("https://parseapi.back4app.com/users",
                    "POST"))
         {
             request.SetRequestHeader("X-Parse-Application-Id",
                 Secrets.appId);
             request.SetRequestHeader("X-Parse-REST-API-Key",
                 Secrets.restApi);
             request.SetRequestHeader("X-Parse-Revocable-Session", "1");
             request.SetRequestHeader("Content-Type",
                 "application/json");

             var data = new
                 { username = usernameInput.text, email = emailInput.text, password = passwordInput.text, saveId = saveIdBuffer };
             string json = JsonConvert.SerializeObject(data);

             request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
             request.downloadHandler = new DownloadHandlerBuffer();*/

        using (var request = new WebRequestBuilder()
                   .SetURL("users", "POST")
                   .Revocable()
                   .SetJSON(new { username = usernameInput.text, email = emailInput.text, password = passwordInput.text, saveId = saveIdBuffer })
                   .SetDownloadHandler(new DownloadHandlerBuffer())
                   .Build())
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                Error.instance.DisplayError("Success !");
            }
        }
        loginCanvas.SetActive(true);
        signupCanvas.SetActive(false);
        error = false;
        firstLogin = true;
        BinarySaveFormatter.CreateEmptySaveData();
    }
}