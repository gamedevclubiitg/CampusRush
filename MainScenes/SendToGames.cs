using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class SendToGames : NetworkBehaviour
{
    private GameObject SendToGamesUI, terrainManager, captionTextObject;
    private string caption;
    [SerializeField] private Text captionText;

    // [SerializeField]
    private Button Yes;
    private string SceneName;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(GameObject.Find("ConfirmButton") + "also");
        //Yes = GameObject.Find("ConfirmButton").GetComponent<Button>();
        //captionText = GameObject.Find("MainText").GetComponent<Text>();
        // captionTextObject = GameObject.FindWithTag("SendtoGamesCaption");
        // captionText = GameObject.FindWithTag("SendtoGamesCaption").GetComponent<Text>();
        // captionTextObject.SetActive(false);
        //SendToGamesUI = GameObject.Find("GotoMiniGamesUI");
        //SendToGamesUI.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(gameObject.name);
        Debug.Log(GameObject.Find("ConfirmButton"));
        Yes = GameObject.Find("ConfirmButton").GetComponent<Button>();
        captionText = GameObject.Find("MainText").GetComponent<Text>();
        // captionTextObject = GameObject.FindWithTag("SendtoGamesCaption");
        // captionText = GameObject.FindWithTag("SendtoGamesCaption").GetComponent<Text>();
        // captionTextObject.SetActive(false);
        SendToGamesUI = GameObject.Find("GotoMiniGamesUI");
        SendToGamesUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("football trigger"))
        // {
        //     caption = "Wanna Play Football?";
        //     Debug.Log(caption);
        //     SceneName = "Football";
            // Yes.GetComponent<Button>().onClick.AddListener(SendToScene);
        //     Debug.Log(SceneName);
        // }

        if (other.gameObject.CompareTag("basketball trigger") && DBManager.isCollectCompleted)
        {
            
            // captionText = GameObject.Find("MainText").GetComponent<Text>();
            // SendToGamesUI = GameObject.Find("GotoMiniGamesUI");
            // Yes = GameObject.Find("ConfirmButton").GetComponent<Button>();
            caption = "Wanna Play Basketball?";
            Debug.Log(caption);
            SceneName = "Basketball";
            Yes.onClick.AddListener(SendToScene);
            Debug.Log(SceneName);
            captionText.text = caption;
            SendToGamesUI.SetActive(true);
        }

        if (other.gameObject.CompareTag("swimming trigger") && DBManager.isCollectCompleted)
        {
            
            // captionText = GameObject.Find("MainText").GetComponent<Text>();
            // SendToGamesUI = GameObject.Find("GotoMiniGamesUI");
            // Yes = GameObject.Find("ConfirmButton").GetComponent<Button>();
            caption = "Wanna Swim?";
            Debug.Log(caption);
            SceneName = "Swimming";
            Yes.onClick.AddListener(SendToScene);
            Debug.Log(SceneName);
            captionText.text = caption;
            SendToGamesUI.SetActive(true);
        }

        if (other.gameObject.CompareTag("hurdles trigger") && DBManager.isCollectCompleted)
        {
            
            // captionText = GameObject.Find("MainText").GetComponent<Text>();
            // SendToGamesUI = GameObject.Find("GotoMiniGamesUI");
            // Yes = GameObject.Find("ConfirmButton").GetComponent<Button>();
            caption = "Wanna Play Hurdles?";
            Debug.Log(caption + gameObject.name);
            SceneName = "Hurdle";
            Debug.Log(Yes);
            Yes.onClick.AddListener(SendToScene);
            Debug.Log(SceneName);
            captionText.text = caption;
            SendToGamesUI.SetActive(true);
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        SendToGamesUI.SetActive(false);
        SceneName = null;
    }

    public void SendToScene()
    {
        // if (IsHost)
        // {
        //     //NetworkManager.Singleton.GetComponent<NetworkManager>().StopHost();
        // }
        // else if (IsClient)
        // {
        //     //NetworkManager.Singleton.GetComponent<NetworkManager>().StopClient();
        // }
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(SceneName);
    }

    

}
