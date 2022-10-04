using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class Exit1 : NetworkBehaviour 
{
    public GameObject ExitPanel;
    //  public GameObject escUI;
    void Start(){
        ExitPanel.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && DBManager.isLoggedIn){
            ExitPanel.SetActive(true);
            Cursor.lockState=CursorLockMode.None;
            Time.timeScale=0;
            // Debug.Log("working");
        }
    }
    public void MainScene(){
        //NetworkManager.Singleton.Shutdown();
        Time.timeScale = 1;
        SceneManager.LoadScene("TerrainSingle");
        
    }
    public void BackToGame(){
        ExitPanel.SetActive(false);
        // Cursor.lockState=CursorLockMode.Locked;
        Time.timeScale=1;
    }
}
