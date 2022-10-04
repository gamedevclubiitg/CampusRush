using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Hurdle_UI : MonoBehaviour
{
    // Start is called before the first frame update
   
    public GameObject Player;
    public GameObject Score;
    public GameObject Timer, Remark;
    private Text score;
    private Text timer, remark;
    private bool done, completed;
    public GameObject panel;
    public GameObject Hscore;
    public static int rank = 1;
    private Text hscore;
    private float timeleft;
    private float T = 60;
    private int y, hurdle;
    public Transform container;
    public Transform row;
    public Text Score_Hurdle;
    public Text Name;
    public Text position;
    
    string endScore;
    public Transform positionInfo;

    private List<ScoreEntry> scoreEntryList;
    private List<Transform> scoreEntryTransformList;

    string userName;
    public GameObject inputField;
    int t=0;
    
    public GameObject LeaderBoardPanel;
    
    [SerializeField]
    private GameObject prevHighScore;

    public GameObject input, entry;
    private bool isScoreUpdated = false;

private void Awake()
    {
    scoreEntryList= new List<ScoreEntry>(){

    };
    
}
    void Start()
    {
        remark = Remark.GetComponent<Text>();
        Remark.SetActive(false);
        score=Score.GetComponent<Text>();
        timer=Timer.GetComponent<Text>();
        hscore=Hscore.GetComponent<Text>();
        timer.text="0";
        score.text="0";
        hurdle=0;
        done = false;
        completed = false;
        panel.SetActive(false);
        LeaderBoardPanel.SetActive(false);

        PlayerPrefs.SetInt("initial", t);

        //Score_Hurdle.text=PlayerPrefs.GetString("1st");
        //Name.text=PlayerPrefs.GetString("1stName");

    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            T -= Time.deltaTime;
            if (T <= 0)
            {
                StartCoroutine(Wait(0f));
                Cursor.lockState = CursorLockMode.None;
            }
            timer.text = (Math.Round((decimal)T,2 )).ToString();
            endScore = timer.text;
        }

        score.text = rank.ToString();
        
    }

    public void Display(float dist)
    {
        //Debug.Log(dist);
        if(dist < 1.5f || dist > 2.5f)
        {
            remark.text = "Real Poor";
        }
        else if (dist < 1.8f || dist > 2.2f)
        {
            remark.text = "Poor Jump";
        }
        else if((dist > 1.8f && dist < 1.9f) || (dist < 2.2f && dist > 2.1f))
        {
            remark.text = "Good";
        }
        else if (dist > 1.9f || dist < 2.1f)
        {
            remark.text = "Perfect";
        }
        else if(dist > 1.98f && dist < 2.02f)
        {
            remark.text = "You are GOD of JUMP";
        }
    }

    public IEnumerator GiveRemark(float dist)
    {
        Remark.SetActive(true);
        Display(dist);
        yield return new WaitForSeconds(1.5f);
        Remark.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if(collider.gameObject.CompareTag("Hurdle"))
        {
            
        }

    }

    void OnCollisionEnter(Collision collisionInfo){

        if(collisionInfo.collider.tag=="Hurdle"){
            hurdle++;
            Debug.Log("hurdle " + hurdle);
            
            if (hurdle >= 3)
            {
                done = true;
                StartCoroutine(Wait(2f));
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                collisionInfo.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(900,900,900));
                collisionInfo.gameObject.layer = 8;
            }
        }
        if (collisionInfo.collider.CompareTag("End"))
        {
            Debug.Log("happened");
            
            done = true;
            completed = true;
            StartCoroutine(Wait(0f));
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Restart()
    {
        rank = 1;
        T = 60 + Time.time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void View_LeaderBoard(){
        panel.SetActive(false);
        LeaderBoardPanel.SetActive(true);
    }

    public void back_button(){
        LeaderBoardPanel.SetActive(false);
        panel.SetActive(true);
        entry.SetActive(false);
        input.SetActive(false);
    }

    private void sort(int[] a, string[] b){
        for(int i=0;i<a.Length;i++){
            for(int j=i+1;j<a.Length;j++){
                if(a[j]>=a[i]){
                    int temp;
                    
                    temp=a[j];
                    a[j]=a[i];
                    a[i]=temp;

                    string Temp;
                    Temp=b[j];
                    b[j]=b[i];
                    b[i]=Temp;


                }
            }
        }
    }

    private void updateLeaderBoard(ScoreEntry scoreEntry, Transform container, List<Transform> transformList)
    {
        

        // position.text= (i+1).ToString();
            Score_Hurdle.text=endScore;
            Name.text=userName;
            Transform entry = Instantiate(row, container);
            RectTransform entryRect=entry.GetComponent<RectTransform>();
            entryRect.anchoredPosition=new Vector2(0,-20*transformList.Count);


            int rank = transformList.Count +1;
            string rankString;
            
            switch (rank){
                default:
                rankString=rank+"TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }

            entry.Find("rank").GetComponent<Text>().text=rankString;

            float newScore = scoreEntry.score;

            entry.Find("Score").GetComponent<Text>().text=newScore.ToString();

            string newName = scoreEntry.name;

            entry.Find("name").GetComponent<Text>().text=newName;
    }

    private class ScoreEntry{
        public float score;

        public string name;
    }

    public void readStringInput(string s){
        
        userName=inputField.GetComponent<Text>().text;

        
        // updateLeaderBoard();
        Debug.Log(userName);
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(true);
        if (completed)
        {
            hscore.text = T.ToString();
            if(!isScoreUpdated)
            {
                UpdateScoreinDB(T.ToString());
                isScoreUpdated = true;
            }
            prevHighScore.GetComponent<Text>().text=DBManager.hurdle.ToString();
        }
        else
        {
            hscore.text = "race over";
        }
        Player.SetActive(false);
    }

    private void UpdateScoreinDB(string score)
    {
        WWWForm setScoreForm = DBops.SetScoreForm(DBManager.email, "hurdle", score, "1");
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
            DBManager.hurdle = float.Parse(score);
        }
    }


}
