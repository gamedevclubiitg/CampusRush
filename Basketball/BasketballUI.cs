using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class BasketballUI : MonoBehaviour
{
    int S = 0;
    [SerializeField]  private float T=60f;
    public GameObject Score;
    public GameObject Timer;
    public GameObject panel;
    public GameObject Yscore;
    public GameObject player_cam;
    public GameObject net;

    [SerializeField] private GameObject PlayerUI, prevHighScore;

    
    private Text score;
    private Text timer;
    private Text yscore;

    public bool basket;
    //PlayerBasketball pbb;
    public GameObject playerGG;

    [SerializeField] ParticleSystem trail;
    private ParticleSystem.EmissionModule em;

    [SerializeField] private float basketEffectDuration = 0.5f;
    [SerializeField] private float basketEffectIntensity = 20f;
    public int time = 80;
    bool once = false;
    private bool isScoreUpdated = false;
    private string prevHScore;
    
    void Start()
    {
        Score=GameObject.FindWithTag("Score");
        Timer=GameObject.FindWithTag("Timer");
        panel.SetActive(false);

        score=Score.GetComponent<Text>();
        timer=Timer.GetComponent<Text>();
        yscore=Yscore.GetComponent<Text>();
        timer.text="60";
        score.text="0";

        basket = false;
        prevHScore = DBManager.basket.ToString();

        
        
        once = true;
        em = trail.emission;
        Debug.Log(gameObject.name);
    }

    
    void Update()
    {

        
        

        if (T<=0){
            // Debug.Log("Time's up");
            timer.text="Time's up";
            yscore.text=score.text;
            //Debug.Log(hscore.text);
            if(!isScoreUpdated)
            {
                UpdateScoreinDB(yscore.text);
                isScoreUpdated = true;
            }
            Debug.Log(yscore.text);
            prevHighScore.GetComponent<Text>().text=prevHScore;
            panel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            PlayerUI.SetActive(false);
        }
        else{
            T -= Time.deltaTime;
            //Debug.Log(T);
            timer.text=((int)T).ToString();
            if(T == 10 && once)
            {
                timer.gameObject.GetComponent<AudioSource>().Play();
                once = false;
            }
            
        }

        

    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.CompareTag("Goal") && gameObject.GetComponent<Rigidbody>().velocity.y <= 0 && playerGG.GetComponent<PlayerBasketball>().holdingball == false)
        {
            float distance = Vector3.Distance(new Vector3(player_cam.transform.position.x, 0, player_cam.transform.position.z), new Vector3(collider.gameObject.transform.position.x, 0, collider.gameObject.transform.position.z));
            Debug.Log("Basket");
            basket = true;
            StartBasketEffect();
            if (basket && distance < 10f)
            {
                Debug.Log(distance);
                S++;
                score.text = S.ToString();
                basket = false;
            }
            else if (basket && (10f < distance && distance < 20f))
            {
                S = S + 2;
                Debug.Log(distance);
                score.text = S.ToString();
                basket = false;
            }
            else if (basket && (distance >= 20f))
            {
                S = S + 3;
                Debug.Log(distance);
                score.text = S.ToString();
                basket = false;
            }
        }

        if(collider.gameObject.CompareTag("Goal") && gameObject.GetComponent<Rigidbody>().velocity.y >= 0 && playerGG.GetComponent<PlayerBasketball>().holdingball == false)
        {
            Debug.Log("foul");
            playerGG.GetComponent<PlayerBasketball>().stopball();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<AudioSource>().Play();
    }


    void StartBasketEffect()
    {
        em.rateOverDistance = basketEffectIntensity;
        StartCoroutine(StopBasketEffect(basketEffectDuration));
    }

    IEnumerator StopBasketEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        em.rateOverDistance = 0f;
    }

    private void UpdateScoreinDB(string score)
    {
        WWWForm setScoreForm = DBops.SetScoreForm(DBManager.email, "basket", score, "1");
        StartCoroutine(SetScore(Link.setScore, setScoreForm, score));
    }

    IEnumerator SetScore(string target, WWWForm setScoreForm, string score)
    {
        Debug.Log("coroutine started");
        UnityWebRequest request = UnityWebRequest.Post(target, setScoreForm);
        request.SetRequestHeader("User-Agent", "DefaultBrowser"); 
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("returned request");
            Debug.Log(request.downloadHandler.text);
        }

        string[] leaderboardCheckResult = DBops.GetResultFromRequest(request);
        Debug.Log(leaderboardCheckResult[1]);

        if (leaderboardCheckResult[1] == "0"){
            DBManager.basket = int.Parse(score);
        }
    }
}
