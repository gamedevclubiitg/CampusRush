using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class TestNetworkManager : NetworkBehaviour
{
    private GameObject NetManager, roomName;
    private NetworkManager networkManager;
    private RelayManager relayManager;

    [SerializeField]
    InputField roomnamefield;
    // private bool CanStartServer = false;

    [SerializeField]
    Button HostButton;

    [SerializeField]
    Button ClientButton;

    private RelayHostData relayHostData;

    // Start is called before the first frame update
    void Start()
    {
        NetManager = GameObject.Find("NetworkManager");
        networkManager = NetManager.GetComponent<NetworkManager>();
        relayManager = NetManager.GetComponent<RelayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(CanStartServer)
        // {
        //     Host();
        //     CanStartServer = false;
        // }


    }

    public async void Host()
    {
        // NetManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomnamefield.text;
        // Debug.Log(networkManager.GetComponent<PhotonRealtimeTransport>().RoomName);

        if (relayManager.IsRelayEnabled)
        {
            relayHostData = await relayManager.SetupRelay();
        }

        // networkManager.ConnectionApprovalCallback += ApprovalCheck;
        //Camera.main.transform.gameObject.SetActive(false);

        // byte[] gender = { (byte)8 };
        // networkManager.NetworkConfig.ConnectionData = gender;

        networkManager.StartHost();
        // PlayerUI.SetActive(true);
        // connect.SetActive(false);
    }


    public async void Client()
    {
        // byte[] gender = { (byte)Gender.GetComponent<Gender>().gender.value };

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
        //networkManager.ConnectionApprovalCallback += ApprovalCheck;
        // networkManager.NetworkConfig.ConnectionData = gender;
        networkManager.StartClient();

        // PlayerUI.SetActive(true);
        // connect.SetActive(false);

        // StartCoroutine(CheckConnection());
    }

    public async void ClientAuto(string joinCode)
    {

        if (relayManager.IsRelayEnabled)
        {
            await relayManager.JoinRelay(joinCode);
        }


        networkManager.StartClient();

        // PlayerUI.SetActive(true);
        // connect.SetActive(false);

        // StartCoroutine(CheckConnection());
    }

    public override void OnNetworkDespawn(){
        if(DBManager.isHost)
        {
            Debug.Log("xyz");
        }
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
}
