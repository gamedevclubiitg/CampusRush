
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
// using Photon.Realtime;
// using MLAPI.Transports.PhotonRealtime;
//using Photon;

public class FootballManager : NetworkBehaviour
{
    [SerializeField] private GameObject Football;
    [SerializeField] private GameObject Boost;

    [SerializeField] private GameObject PlayerprefabM;
    [SerializeField] private GameObject PlayerprefabF;
    private GameObject hostPlayer;
    private GameObject clientPlayer;

    [SerializeField] private GameObject Gender;

    [SerializeField] private GameObject one, three, four, five;
    //[SerializeField] private float SpawnRange = 10f;
    private GameObject[] BoostSpawnSpots;
    bool once = true;
    [SerializeField]
    GameObject PlayerUI;
    public GameObject panel;

    public static int players = 0;
    [SerializeField]
    private GameObject NetManager;
    private NetworkManager networkManager;
    private RelayManager relayManager;

    [SerializeField]
    GameObject connect;

    [SerializeField]
    InputField roomnamefield, nameField;

    public GameObject football;
    bool once2 = false;
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public Renderer GoalR;
    public Renderer GoalL;

    [SerializeField] private GameObject ConnectPanel1;
    [SerializeField] private GameObject ConnectPanel2, server, nameRoom, Instructions;
    bool once1 = true;

    public GameObject escUI;

    // private RGBA colour_ratio=new RGBA();
    private void Start()
    {
        // colour_ratio.a=1f;

        NetManager = GameObject.Find("NetworkManager");
        networkManager = NetManager.GetComponent<NetworkManager>();
        BoostSpawnSpots = GameObject.FindGameObjectsWithTag("boostSpawner");
        relayManager = NetManager.GetComponent<RelayManager>();
        server = GameObject.Find("Server");
        nameRoom = GameObject.Find("Name Room");
        ConnectPanel2.SetActive(false);
        Instructions.SetActive(false);
        server.SetActive(false);

        //Gender = GameObject.Find("SelectCharacter");
        //once2 = true;
        //Debug.Log("Well");
        once1 = true;
    }

    private void Update()
    {
        if (IsServer && once)
        {
            //Debug.Log("Here Spawns");
            GameObject ball = Instantiate(Football, Vector3.zero + new Vector3(0, 2, 0), Quaternion.identity);
            ball.GetComponent<NetworkObject>().Spawn();

            foreach (GameObject spot in BoostSpawnSpots)
            {
                //Debug.Log("Spawning boost");
                GameObject boost = Instantiate(Boost, spot.transform.position, spot.transform.rotation);
                boost.GetComponent<NetworkObject>().Spawn();
            }


            once = false;
            football = ball;
        }

        if (IsServer && football.GetComponent<Football>().respawningBall)
        {
            football.transform.position = Vector3.zero + new Vector3(0, 2, 0);

            football.GetComponent<Football>().respawningBall = false;
        }
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Q) && once1){
            server.SetActive(true);
            once1 = false;
        }

        /*if (IsHost && once2)
        {
            //LobbyMenuController.Roomnames.Add(NetManager.GetComponent<PhotonRealtimeTransport>().RoomName);
            //Debug.Log(LobbyMenuController.Roomnames[0]);
            once2 = false;
        }*/
            //Debug.Log(LobbyMenuController.Roomnames.Count);
    }

    
    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Your logic here
        bool approve = true;
        bool createPlayerObject = true;
        Vector3 positionToSpawnAt = new Vector3(0, -10, 0);
        Quaternion rotationToSpawnWith = Quaternion.identity;

        // The prefab hash. Use null to use the default player prefab
        // If using this hash, replace "MyPrefabHashGenerator" with the name of a prefab added to the NetworkPrefabs field of your NetworkManager object in the scene
        uint? prefabHash = null;

        /*int gender = connectionData[0];
        Debug.Log(gender);

        
        if (gender != 0)
        {
            //prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Female Character");
            Debug.Log("Well Hashhh: " + prefabHash + " " + gender);
        }*/

        //If approve is true, the connection gets added. If it's false. The client gets disconnected
        callback(createPlayerObject, prefabHash, approve, positionToSpawnAt, rotationToSpawnWith);
    }
    

    public async void Host()
    {

        // networkManager.GetComponent<PhotonRealtimeTransport>().RoomName = chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "";
        // Debug.Log(networkManager.GetComponent<PhotonRealtimeTransport>().RoomName);
        //NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        //Camera.main.transform.gameObject.SetActive(false);

        /*
        bool createPlayerObject = true;
        Vector3 positionToSpawnAt = new Vector3(0, -10, 0);
        Quaternion rotationToSpawnWith = Quaternion.identity;
        Gender.GetComponent<Gender>().gender.RefreshShownValue();

        
        if (Gender.GetComponent<Gender>().gender.value != 0)
        {
            ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Female Character");
            Debug.Log("Well Hashhh: " + prefabHash + " " + Gender.GetComponent<Gender>().gender.value);
            networkManager.StartHost(positionToSpawnAt, rotationToSpawnWith, createPlayerObject, prefabHash);
        }
        else
        {
            networkManager.StartHost();
        }
        */
        if (relayManager.IsRelayEnabled)
        {
            await relayManager.SetupRelay();
        }



        NetworkManager.Singleton.StartHost();


        //panel = GameObject.Find("endPanel");
        //panel.SetActive(false);
        PlayerUI.SetActive(true);
        connect.SetActive(false);

        //Spawn();


    }

    public void Server()
    {
        // networkManager.GetComponent<PhotonRealtimeTransport>().RoomName = chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "" + chars[Random.Range(0, 26)] + "";
        // nameRoom.GetComponent<Text>().text = networkManager.GetComponent<PhotonRealtimeTransport>().RoomName;
        networkManager.StartServer();
        

    }

    public async void Client()
    {
        byte[] gender = { (byte)Gender.GetComponent<Gender>().gender.value };
        if (roomnamefield.text.Length == 0)
        {
            roomnamefield.text = "Core";
        }
        else
        {
            //NetManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomnamefield.text;
            if (relayManager.IsRelayEnabled)
            {
                await relayManager.JoinRelay(roomnamefield.text);
            }
        }
        //networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.NetworkConfig.ConnectionData = gender;
        networkManager.StartClient();
        switch (FootballManager.players)
        {
            case 2: One();
                break;
            case 6: Three();
                break;
            case 8: Four();
                break;
            case 10: Five();
                break;
            default: Debug.Log("Invalid! U a hacker");
                break;
        }
        PlayerUI.SetActive(true);
        //panel = GameObject.Find("endPanel");
        //panel.SetActive(false);
        connect.SetActive(false);

        StartCoroutine(CheckConnection());
        
    }

    IEnumerator CheckConnection()
    {
        yield return new WaitForSeconds(10);
        if (!IsClient)
        {
            SceneManager.LoadScene("Football");
        }
        else
        {
            //Spawn();
        }
    }
    public void One()
    {
        one.SetActive(true);
        three.SetActive(false);
        four.SetActive(false);
        five.SetActive(false);
        players = 2;
        ShowConnectButtons();
    }

    public void Three()
    {
        one.SetActive(false);
        three.SetActive(true);
        four.SetActive(false);
        five.SetActive(false);
        players = 6;
        ShowConnectButtons();
    }
    public void Four()
    {
        one.SetActive(false);
        three.SetActive(false);
        four.SetActive(true);
        five.SetActive(false);
        players = 8;
        ShowConnectButtons();
    }
    public void Five()
    {
        one.SetActive(false);
        three.SetActive(false);
        four.SetActive(false);
        five.SetActive(true);
        players = 10;
        ShowConnectButtons();
    }

    public void Rematch()
    {
        // if (IsHost)
        // {
        //     networkManager.GetComponent<NetworkManager>().StopHost();
        // }
        // else if (IsClient)
        // {
        //     networkManager.GetComponent<NetworkManager>().StopClient();
        // }
        SceneManager.LoadScene("Football");
    }


    public void Exit()
    {
        SceneManager.LoadScene("Terrain");
    }

    private void ShowConnectButtons()
    {
        Instructions.SetActive(true);
        ConnectPanel1.SetActive(false);
    }

    public void ShowPreviousPanel()
    {
        ConnectPanel1.SetActive(true);
        ConnectPanel2.SetActive(false);
    }

    public void GotInstructions()
    {
        escUI.SetActive(true);
        ConnectPanel2.SetActive(true);
        Instructions.SetActive(false);
    }

    /*private void Spawn()
    {
        Debug.Log("here spawns player");
        if (IsHost)
        {
            if (Gender.value == 0)
            {
                hostPlayer = Instantiate(PlayerprefabM, new Vector3(0, -10, 0), Quaternion.identity);
            }
            else
            {
                hostPlayer = Instantiate(PlayerprefabF, new Vector3(0, -10, 0), Quaternion.identity);
            }

            hostPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId, null, true);
        }

        else
        {
            Debug.Log("Is Client Spawner runs" + networkManager.LocalClientId);
            SpawnServerRpc(networkManager.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnServerRpc(ulong clientId)
    {
        Debug.Log("Is Client Spawner runs server rpc");
        if (Gender.value == 0)
        {
            clientPlayer = Instantiate(PlayerprefabM, new Vector3(0, -10, 0), Quaternion.identity);
        }
        else
        {
            clientPlayer = Instantiate(PlayerprefabF, new Vector3(0, -10, 0), Quaternion.identity);
        }
        clientPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, null, true);
        Debug.Log("Is Client Spawner runs again");
    }*/
}