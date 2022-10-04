using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasketballUImananger : MonoBehaviour
{

    [SerializeField] private GameObject Instructions;
    [SerializeField] private GameObject ball;

    public GameObject escUI;

    public void HideInstructions()
    {
        escUI.SetActive(true);
        Instructions.SetActive(false);
        ball.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitToTerrain()
    {
        SceneManager.LoadScene("TerrainSingle");
    }

    public void PlayagainBasketball()
    {
        SceneManager.LoadScene("Basketball");
    }
}
