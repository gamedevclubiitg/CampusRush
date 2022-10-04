using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;



public class LobbyMenuController : NetworkBehaviour
{
    [SerializeField] private string VersionName = "0.1";
    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;

    [SerializeField] private GameObject NetManager;

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;

    [SerializeField] private GameObject StartButton;

    [SerializeField] private byte maxPlayers = 10;

    //public static NetworkList<string> Roomnames = new NetworkList<string>();



    // Start is called before the first frame update
    private void Start()
    {
        UsernameMenu.SetActive(true);
        StartButton.SetActive(false);
    }

    public void ChangeUsernameInput()
    {
        if (UsernameInput.text.Length >= 3)
        {
            StartButton.SetActive(true);
        }

        else
        {
            StartButton.SetActive(false);
        }
    }

    public void SetUserName()
    {
        UsernameMenu.SetActive(false);
        ConnectPanel.SetActive(true);
    }

    public void HostGame()
    {
        NetManager.GetComponent<NetworkManager>().StartHost();
        Debug.Log("Room Creating");
        //This can only be called on the server
        SceneManager.LoadScene("Football");
    }

    public void JoinGame()
    {
        NetManager.GetComponent<NetworkManager>().StartClient();
        Debug.Log("Room Joined");
        SceneManager.LoadScene("Football");
    }

}
