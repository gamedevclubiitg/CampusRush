using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MLAPI;
//using MLAPI.NetworkVariable;
//using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class PlayerSwim : MonoBehaviour
{
    public float MovespeedM;
    public float Runspeed = 2f;
    public float Walkspeed = 1f;

    [SerializeField] private float smoothingcam, jumpSpeed;
    [SerializeField] private Transform cam;

    public bool IsGrounded = false;
    [SerializeField] private float Gravity;

    public Vector3 Movedir;
    public Vector3 moveincamdir;
    public Vector3 Velocity;

    private float refsmoothvelocity;
    private float InpuX;
    public float InpuZ;
    private CharacterController Controller;
    private Animator Anim;

    public GameObject Barrier;
    public bool IsFallAnim;
    public bool fordive;
    public RuntimeAnimatorController swim;

    public float forward, downward;
    private Slider speedController;
    [SerializeField] private float sliderSpeed = 0f;
    float sliderValue = 0f;
    private bool MovingUp = true, clickable = false, startslider = true, isUnderWater = false;

    private Text feedback;

    private Text timerText;
    public float sec = 0f, second = 0f;
    public int min = 0;
    private float currentScore;

    private GameObject EndTrigger;
    private bool GameDone = false;

    private bool hiddenSlowUI = true;
    public GameObject SlowUI;
    [SerializeField] private GameObject PlayerUI;

    public GameObject endswim;
    public GameObject disableui;
    public GameObject farEndUI;
    public GameObject diveUi;
    public GameObject AudioManager;
    
    [SerializeField]
    private GameObject prevHighScore;
    private string prevhscore;

    AudioManager audioManager;
    Timerswim timerswim;

    public Text Hscore;
    public int changedir;

    bool await = false;
    private bool isScoreUpdated = false;

    private void Awake()
    {
    }
    private void Start()
    {
        timerswim = gameObject.GetComponent<Timerswim>();
        audioManager = AudioManager.GetComponent<AudioManager>();
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();
        Controller.detectCollisions = false;
        Velocity = Vector3.zero;
        IsFallAnim = false;
        fordive = false;
        speedController = GameObject.Find("SpeedController").GetComponent<Slider>();
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        feedback = GameObject.Find("Feedback").GetComponent<Text>();
        speedController.minValue = 0f;
        speedController.maxValue = 200f;
        sliderSpeed = 0f;
        prevhscore = DBManager.swim.ToString(); 
        EndTrigger = GameObject.FindGameObjectWithTag("NearEnd");
        EndTrigger.SetActive(false);

        PlayerUI.SetActive(false);
        SlowUI.SetActive(false);
        audioManager.Play("back_ground");
        changedir = -1;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        move();

        if (!GameDone)
        {
            moveSlider();
            Timer();
        }
        if(await){

        Vector3 correct = new Vector3(transform.position.x, 0.09999f, transform.position.z);
        transform.position = correct;
        }
    }


    #region METHODS
    private void move()
    {
        //InpuX = Input.GetAxisRaw("Horizontal");
        //InpuZ = Input.GetAxisRaw("Vertical");
        //Movedir = new Vector3(InpuX, 0f, InpuZ).normalized;

        if (Input.GetKey(KeyCode.W) && await)
        {
            InpuZ = 1f;
            Debug.Log("4");
        }
        else
        {
            InpuZ = 0f;
        }

        Movedir = new Vector3(0f, 0f, InpuZ);


        if (Movedir != Vector3.zero && isUnderWater)
        {
            Walk();
            MoveHere();
        }

        if (Movedir == Vector3.zero && isUnderWater)
        {
            Idle();
        }

        if (timerswim.Timetodive == true)
        {
            diveUi.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                //Barrier.GetComponent<BoxCollider>().isTrigger = true;
                diveUi.SetActive(false);
                Dive();
                timerswim.Timetodive = false;

            }
        }



        if (IsFallAnim)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                transform.Translate(Vector3.down * downward);
                transform.Translate(Vector3.forward * forward);
                IsFallAnim = false;
                isUnderWater = true;
            }
        }

    }

    private void MoveHere()
    {
        /*float targetangle = Mathf.Atan2(Movedir.x, Movedir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref refsmoothvelocity, smoothingcam);
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveincamdir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
        //Controller.Move(moveincamdir.normalized * MovespeedM * Time.deltaTime);*/


        Controller.Move(changedir * Movedir * MovespeedM * Time.deltaTime);
    }

    private void Walk()
    {
        MovespeedM = Walkspeed;
        Anim.SetBool("Walk", true);
        //Anim.SetBool("Run", false);    
        //audioManager.Play("swim_motion");
    }

    private void Idle()
    {
        //Anim.SetBool("Run", false);
        Anim.SetBool("Walk", false);
    }

    private void Dive()
    {

        //Anim.SetBool("Dive", true);
        // Debug.Log("Dive");
        IsFallAnim = true;
        Anim.runtimeAnimatorController = swim;
        StartCoroutine(dive_delay());
        Debug.Log("2");

        //move

        //Controller.Move(Vector3.Lerp(Vector3.down * 12f, Vector3.forward * 2.3f, 0.1f));


    }

    IEnumerator dive_delay()
    {
        yield return new WaitForSeconds(1);
        audioManager.Play("Dive_splash");
        Debug.Log("3");

        Vector3 correct = new Vector3(transform.position.x, 0.09899f, transform.position.z);
        transform.position = correct;

        await = true;
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {

        /*if (collision.gameObject.CompareTag("SwimBarrier"))
        {
            Debug.Log("in");
            fordive = true;

            if (timerswim.Timetodive == true)
            {
                diveUi.SetActive(true);
            }
            //StartCoroutine(HideDiveUi());
        }*/

        if (collision.gameObject.CompareTag("FarEnd"))
        {
            // Debug.Log("FarEnd Approached");
            EndTrigger.SetActive(true);
            farEndUI.SetActive(true);
            StartCoroutine(HidefarEndUI());
            changedir = 1;

        }

        if (collision.gameObject.CompareTag("NearEnd"))
        {
            Debug.Log("NearEnd Approached");
            GameDone = true;
            speedController.value = 0f;
            endswim.SetActive(true);
            disableui.SetActive(false);
            Hscore.text = timerText.text;
            if(!isScoreUpdated)
            {
                currentScore = sec + (float)min * 60;
                UpdateScoreinDB(currentScore.ToString());
                isScoreUpdated = true;
            }
            prevHighScore.GetComponent<Text>().text = prevhscore;
            Debug.Log(DBManager.swim);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("FarEnd"))
        {
            // Debug.Log("out");
            transform.Rotate(0f, 180f, 0f);

        }
    }

    /*private void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.CompareTag("FarEnd"))
        {
            Debug.Log("FarEnd Approached");
            EndTrigger.SetActive(true);
            farEndUI.SetActive(true);
            StartCoroutine(HidefarEndUI());
        }
        if (other.gameObject.CompareTag("NearEnd"))
        {
            Debug.Log("NearEnd Approached");
            GameDone = true;
            speedController.value = 0f;
            endswim.SetActive(true);
            disableui.SetActive(false);
            Hscore.text=timerText.text;
            Cursor.lockState=CursorLockMode.None;

        }
    }*/

    IEnumerator HidefarEndUI()
    {
        yield return new WaitForSeconds(2f);
        farEndUI.SetActive(false);
    }

    private void moveSlider()
    {
        if (isUnderWater && startslider)
        {
            sliderSpeed = 400f;
            clickable = true;
            startslider = false;
        }

        if (MovingUp && sliderSpeed != 0f)
        {
            sliderValue += Time.deltaTime * sliderSpeed;

            if (sliderValue >= 200f)
            {
                sliderValue = 200f;
                MovingUp = false;
            }
        }

        if (!MovingUp && sliderSpeed != 0f)
        {
            sliderValue -= Time.deltaTime * sliderSpeed;

            if (sliderValue <= 0f)
            {
                sliderValue = 0f;
                MovingUp = true;
            }
        }

        if (speedController != null)
        {
            speedController.value = sliderValue;
        }
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        if (Input.GetMouseButtonDown(0) && clickable)
        {
            sliderSpeed = 0f;
            float changeSpeed = sliderValue - 100f;
            changeSpeed = Math.Abs(changeSpeed);
            changeSpeed = 100f - changeSpeed;
            float absoluteValue = changeSpeed;
            changeSpeed *= 0.012f;
            Walkspeed += changeSpeed;
            // Debug.Log(Walkspeed);

            string msg;
            if (absoluteValue >= 90)
            {
                msg = "Perfect!";
            }
            else if (absoluteValue >= 70 && absoluteValue < 90)
            {
                msg = "Nice!";
            }
            else
            {
                msg = "Poor!";
            }

            feedback.text = msg;
            clickable = false;
            float delay = UnityEngine.Random.Range(2f, 3f);
            // Debug.Log(delay);
            StartCoroutine(StartSliderAgain(delay));
        }
    }

    IEnumerator StartSliderAgain(float delay)
    {
        yield return new WaitForSeconds(delay);
        sliderSpeed = 400f;
        clickable = true;
    }

    private void Timer()
    {
        if (timerswim.racebegin)
        {
            if (sec >= 60f)
            {
                min += 1;
                sec = 0f;

            }
            sec += Time.deltaTime;

            SpeedDecaywithTime();
        }

        if (timerText != null)
        {
            timerText.text = min.ToString() + ":" + sec.ToString();
        }
    }

    private void SpeedDecaywithTime()
    {
        if (second >= 0.5f)
        {
            Walkspeed -= 0.1f;
            second = 0f;

            if (Walkspeed <= 0f)
            {
                Walkspeed = 0f;
                if (hiddenSlowUI)
                {
                    SlowUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    PlayerUI.SetActive(false);
                }
            }
            //Debug.Log(Walkspeed);
        }
        second += Time.deltaTime;
    }

    private void UpdateScoreinDB(string score)
    {
        WWWForm setScoreForm = DBops.SetScoreForm(DBManager.email, "swim", score, "0");
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
            DBManager.swim = float.Parse(score);
        }
    }

}

