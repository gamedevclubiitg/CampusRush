
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.Netcode;
using System.Text;
using System;
using Unity.Collections;

public class TerrainManager : NetworkBehaviour
{
    
    [SerializeField] private GameObject PlayerprefabM;
    [SerializeField] private GameObject PlayerprefabF;
    private GameObject hostPlayer;
    private GameObject clientPlayer;

    [SerializeField] private GameObject Gender, loginPanel;

    //[SerializeField] private float SpawnRange = 10f;
    private GameObject[] BoostSpawnSpots;
    bool once = true, once1 = true;
    [SerializeField]
    GameObject PlayerUI;
    //public GameObject panel;
    private GameObject NetManager, roomName;
    private NetworkManager networkManager;
    private RelayManager relayManager;

    private RelayHostData relayHostData;

    [SerializeField]
    GameObject connect;
    GameObject server;

    [SerializeField]
    InputField roomnamefield;

    //public GameObject football;
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField] private GameObject SendToGamesUI;
    private GameObject player;

    // private RGBA colour_ratio=new RGBA();
    private void Start()
    {
        Debug.Log("Yoooo");
        NetManager = GameObject.Find("NetworkManager");
        networkManager = NetManager.GetComponent<NetworkManager>();
        relayManager = NetManager.GetComponent<RelayManager>();
        roomName = GameObject.Find("Name Room");
        server = GameObject.Find("Server");
        server.SetActive(false);
        GameObject.Find("Sound").GetComponent<AudioSource>().Play();
        once1 = true;
    }

    private void Update()
    {
        if (IsServer && once)
        {
            once = false;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Q) && once1)
        {
            server.SetActive(true);
            once1 = false;
        }
    }

    public void Server()
    {
        // networkManager.GetComponent<PhotonRealtimeTransport>().RoomName = chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "";
        // roomName.GetComponent<Text>().text = networkManager.GetComponent<PhotonRealtimeTransport>().RoomName;
        networkManager.StartServer();
        PlayerUI.SetActive(true);
        connect.SetActive(false);
    }
    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Your logic here
        bool approve = true;
        bool createPlayerObject = true;
        Vector3 positionToSpawnAt = new Vector3(UnityEngine.Random.Range(410, 450), 1, UnityEngine.Random.Range(350, 370));
        Quaternion rotationToSpawnWith = Quaternion.identity;

        // The prefab hash. Use null to use the default player prefab
        // If using this hash, replace "MyPrefabHashGenerator" with the name of a prefab added to the NetworkPrefabs field of your NetworkManager object in the scene

        uint? prefabHash = null;


        int gender = connectionData[0];
        Debug.Log(gender);

        switch (gender)
        {
            case 0: 
                prefabHash = 83727080;
                break;
            case 1:
                prefabHash = 1393644718;
                break;
            case 2:
                prefabHash = 1450849897;
                break;
            default:
                prefabHash = null;
                break;
        }
        callback(createPlayerObject, prefabHash, approve, positionToSpawnAt, rotationToSpawnWith);
    }


    public async void Host()
    {
        // NetManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomnamefield.text;
        // Debug.Log(networkManager.GetComponent<PhotonRealtimeTransport>().RoomName);

        if (relayManager.IsRelayEnabled)
        {
            relayHostData = await relayManager.SetupRelay();
        }

        ///networkManager.ConnectionApprovalCallback += ApprovalCheck;
        //Camera.main.transform.gameObject.SetActive(false);

        ///byte[] gender = { (byte)Gender.GetComponent<Gender>().gender.value };
        ///networkManager.NetworkConfig.ConnectionData = gender;

        networkManager.StartHost();
        PlayerUI.SetActive(true);
        connect.SetActive(false);
    }

    public async void Client()
    {
        byte[] gender = { (byte)Gender.GetComponent<Gender>().gender.value };

        if (roomnamefield.text.Length == 0)
        {
            return;
        }
        else
        {
            //NetManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomnamefield.text;

            if (relayManager.IsRelayEnabled)
            {
                await relayManager.JoinRelay(roomnamefield.text);
            }
        }
        ///networkManager.ConnectionApprovalCallback += ApprovalCheck;
        ///networkManager.NetworkConfig.ConnectionData = gender;
        networkManager.StartClient();

        PlayerUI.SetActive(true);
        connect.SetActive(false);

        StartCoroutine(CheckConnection());
    }

    IEnumerator CheckConnection()
    {
        yield return new WaitForSeconds(20);
        if (!IsClient)
        {
            SceneManager.LoadScene("Terrain");
        }
        else
        {
            // Spawn();
        }
    }
    public void Rematch()
    {
        networkManager.Shutdown();
        SceneManager.LoadScene("Terrain");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Terrain");
        if (DBManager.isLoggedIn)
        {
            loginPanel.SetActive(false);
            StartGame();
        }

    }

    /*public void ToFootball()
    {
        SceneManager.LoadScene("Football");
    }*/

    public void ToBasketball()
    {
        SceneManager.LoadScene("Basketball");
    }

    public void ToHurdles()
    {
        SceneManager.LoadScene("Hurdle");
    }

    public void ToSwimming()
    {
        SceneManager.LoadScene("Swimming");
    }



    public void HideCanvas()
    {
        SendToGamesUI.SetActive(false);
    }

    public void StartGame()
    {
        WWWForm checkCodeForm = DBops.GetDataForm(DBManager.email, "joinCode");

        StartCoroutine(checkCode(Link.getData, checkCodeForm));

    }

    IEnumerator checkCode(string target, WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(target, form);
        yield return request.SendWebRequest();

        // Debug.Log(request.text);

        string[] checkCodeResult = DBops.GetResultFromRequest(request);

        if (checkCodeResult[0] == "0")
        {
            Debug.Log(checkCodeResult[1]);
            DBManager.isHost = false;
            DBManager.joinCode = checkCodeResult[1];
            ClientAuto(DBManager.joinCode);
        }

        if (checkCodeResult[0] == "9")
        {
            Debug.Log(checkCodeResult[0]);
            DBManager.isHost = true;
            Host();
            StartCoroutine(SetJoinCode());
        }

        yield return new WaitForSeconds(5);
        server.SetActive(true);

    }

    public void Reconnect()
    {
        WWWForm delCodeForm = DBops.DeleteCodeForm(DBManager.teamID.ToString());
        StartCoroutine(delCode(Link.deleteCode, delCodeForm));
    }

    IEnumerator delCode(string target, WWWForm delCodeForm)
    {
        Debug.Log("coroutine started");
        UnityWebRequest request = UnityWebRequest.Post(target, delCodeForm);
        request.SetRequestHeader("User-Agent", "DefaultBrowser"); 
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

        string[] delCodeResult = DBops.GetResultFromRequest(request);
        Debug.Log(delCodeResult[0]);

        if (delCodeResult[0] == "0")
        {
            yield return new WaitForSeconds(10);
            StartGame();
        }
    }

    private async void ClientAuto(string joinCode)
    {
        byte[] gender = { (byte)Gender.GetComponent<Gender>().gender.value }; 

        if (relayManager.IsRelayEnabled)
        {
            await relayManager.JoinRelay(joinCode);
        }

        //networkManager.ConnectionApprovalCallback += ApprovalCheck;
        //networkManager.NetworkConfig.ConnectionData = gender;
        
        networkManager.StartClient();

        PlayerUI.SetActive(true);
        connect.SetActive(false);

        StartCoroutine(CheckConnection());
    }

    IEnumerator SetJoinCode()
    {
        yield return new WaitForSeconds(10);
        // Debug.Log(relayManager.joinCode);
        Debug.Log(relayHostData.JoinCode);
        WWWForm setCodeForm = DBops.SetCodeForm(DBManager.teamID.ToString(), relayHostData.JoinCode);

        UnityWebRequest request = UnityWebRequest.Post(Link.setCode, setCodeForm);
        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);
    }
    // vghfgkgkjg
}
