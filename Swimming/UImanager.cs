using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUI;
    [SerializeField] private GameObject InstructionUI;
    [SerializeField] private GameObject TimerUI;
    [SerializeField] private GameObject Timerswimm;
    [SerializeField] private GameObject TimerswimmBot;

    public GameObject escUI;

    void Start()
    {
    }
    
    void Update()
    {
        // timer+=Time.deltaTime;
        // if(timer<1f){
        //     message.SetActive(true);
        
        // }
        // else if(timer>=1f)
        // {
        //     message.SetActive(true);

        // }
    }

    public void HideInstructionPanel()
    {
        escUI.SetActive(true);
        Cursor.lockState=CursorLockMode.Locked;
        InstructionUI.SetActive(false);
        PlayerUI.SetActive(true);
        TimerUI.SetActive(true);
        Timerswimm.GetComponent<Timerswim>().starttimer();
        TimerswimmBot.GetComponent<Timerswim>().starttimer();
    }

    public void Replay()
    {
        SceneManager.LoadScene("Swimming");
    }

    public void ExitToTerrain()
    {
        SceneManager.LoadScene("TerrainSingle");
    }
}
