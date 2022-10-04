using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public GameObject ExitPanel;
    public GameObject escUI;

    void Start(){
        ExitPanel.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            ExitPanel.SetActive(true);
            Cursor.lockState=CursorLockMode.None;
            Time.timeScale=0;
            // Debug.Log("working");
        }
        
    }
    public void MainScene(){
        escUI.SetActive(false);
        SceneManager.LoadScene("TerrainSingle");
    }
    public void BackToGame(){
        ExitPanel.SetActive(false);
        Cursor.lockState=CursorLockMode.Locked;
        Time.timeScale=1;
    }
}
