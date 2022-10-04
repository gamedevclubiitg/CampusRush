using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class Score : NetworkBehaviour
{
    public NetworkVariable<float> R = new NetworkVariable<float>(/*new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly
    },*/ 0f);

    public NetworkVariable<float> L = new NetworkVariable<float>(/*new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly
    },*/ 0f);

    public NetworkVariable<float> sec = new NetworkVariable<float>(/*new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly
    },*/ 0f);

    public NetworkVariable<int> min = new NetworkVariable<int>(/*new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly
    },*/ 1);

    //float R=0f, L=0f;
    public GameObject panel;
    

    public GameObject scoreR;
    public GameObject scoreL;
    public GameObject Timer;
    private int noOfPlayer;
    private Text ScoreRight;
    private Text ScoreLeft;
    private Text timer;
    // private int min = 5;
    // private float sec = 0f;
    float temp = 1f;
    private bool ttt = false;
    private bool once = false, goaled = false;
    bool oncce, twice, thrice;
    // Start is called before the first frame update
    void Start()
    {
        scoreR = GameObject.FindWithTag("scoreR");
        scoreL = GameObject.FindWithTag("scoreL");
        Timer = GameObject.Find("Timer");

        //Debug.Log(GameObject.FindGameObjectsWithTag("GoalL").Length + GameObject.FindGameObjectsWithTag("GoalR").Length);
        
        ScoreRight = scoreR.GetComponent<Text>();
        ScoreLeft = scoreL.GetComponent<Text>();
        timer = Timer.GetComponent<Text>();
        
        timer.text = (min.Value).ToString() + ":" + sec.Value.ToString();
        
        if (IsServer)
        {
            min.Value = 5;
            sec.Value = 0;
        }
        once = true;
        goaled = false;
        oncce = true;
        twice = true;
        thrice = true;
    }

    // Update is called once per frame
    void Update()
    {
        noOfPlayer = GameObject.FindGameObjectsWithTag("Player").Length;

        if (noOfPlayer == FootballManager.players || ttt)
        {
            ttt = true;
            if (min.Value == 0 && sec.Value <= 0)
            {
                foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    player.GetComponent<PlayerMovement>().GameDone(R.Value, L.Value);
                }
                gameObject.GetComponent<Score>().enabled = false;
            }
            else
            {
                if (sec.Value <= 0 && IsServer)
                {
                    min.Value = min.Value - 1;
                    sec.Value = 59f;
                }
                if (IsServer)
                {
                    sec.Value = sec.Value - Time.deltaTime;
                }
            }
            if (min.Value == 0 && (int)sec.Value == 30 && oncce)
            {
                timer.gameObject.GetComponent<AudioSource>().Play();
                timer.color = new Color32(0, 0, 0, 255);
                oncce = false;
            }
            if (min.Value == 0 && (int)sec.Value == 5 && thrice) 
            {
                timer.gameObject.GetComponent<AudioSource>().Play();
                timer.color = new Color32(255, 255, 255, 255);
                thrice = false;
            }
        }
        ScoreRight.text = R.Value.ToString();
        ScoreLeft.text = L.Value.ToString();
        timer.text = min.Value.ToString() + ":" + ((int)sec.Value).ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Goal Done
        StartCoroutine(Scores(other));
    }

    IEnumerator Scores(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("GoalR") && !goaled)
            {
                R.Value += 1;
                goaled = true;
                Debug.Log(R);

            }
            if (other.gameObject.CompareTag("GoalL") && !goaled)
            {
                L.Value += 1;
                Debug.Log(L);
                goaled = true;
            }
        }

        yield return new WaitForSeconds(5);
        goaled = false;
    }
    /*[ServerRpc(RequireOwnership = false)]
    float RGServerRpc(float r){
        r++;
        return r;
    }
    [ServerRpc(RequireOwnership = false)]
    void LGServerRpc(float x){
        L=L+temp;
    }*/

    

    public void Rematch()
    {
        //NetworkManager.StopHost(); 
        GameObject.Find("ConnectUI").SetActive(true);
    }

    /*public void Exit()
    {

    }*/
}
