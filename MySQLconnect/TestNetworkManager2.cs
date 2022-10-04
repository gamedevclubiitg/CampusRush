using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TestNetworkManager2 : NetworkBehaviour
{
    private GameObject NetManager, roomName;
    private NetworkManager networkManager;
    private RelayManager relayManager;

    [SerializeField]
    InputField roomnamefield;

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
        
    }

    public async void Host()
    {
        Debug.Log("host clicked");
        // NetManager.GetComponent<PhotonRealtimeTransport>().RoomName = roomnamefield.text;
        // Debug.Log(networkManager.GetComponent<PhotonRealtimeTransport>().RoomName);

        if (relayManager.IsRelayEnabled)
        {
            await relayManager.SetupRelay();
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
        Debug.Log("client clicked");
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

    
}
