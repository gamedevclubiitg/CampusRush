using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timerswim : MonoBehaviour
{
    public int coutdown;
    public Text countTime;
    public GameObject Timerpanel;
    public bool Timetodive = false;
    public bool racebegin = false;
    public bool botdive = false;
    public bool botdive1 = false;

    public void starttimer()
    {
        StartCoroutine(countdowntimer());
    }


    public IEnumerator countdowntimer()
    {
        
        while(coutdown>0)
        {
            countTime.text = coutdown.ToString();
            yield return new WaitForSeconds(1f);
            coutdown--;
        }

        countTime.text = "GO!";
        yield return new WaitForSeconds(1f);
        Timerpanel.SetActive(false);
        GameObject[] AIplayers = GameObject.FindGameObjectsWithTag("SwimAI");
        foreach (GameObject AI in AIplayers)
        {
            // if (Random.Range(0, 3) == 2)
            // {
            //     yield return new WaitForSeconds(0.5f);
            // }
            AI.GetComponent<PlayerSwimAI>().Dive();
            
        }
        Timetodive = true;
        racebegin = true;
        botdive = true;
        botdive1=true;

    }
    
}
