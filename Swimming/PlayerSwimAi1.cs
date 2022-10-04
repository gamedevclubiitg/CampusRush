using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class PlayerSwimAi1 : MonoBehaviour
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
    private float InpuZ;
    private CharacterController Controller;
    private Animator Anim;

    public GameObject Barrier;
    public bool IsFallAnimBot;
    public bool fordive;
    public RuntimeAnimatorController swim;

    public float forward, downward;
    private Slider speedController;
    [SerializeField] private float sliderSpeed = 0f;
    float sliderValue = 0f;
    private bool MovingUp = true, clickable = false, startslider = true, isUnderWater = false;

    //private Text feedback;
    /*private Text timerText;
    public float sec = 0f, second = 0f;
    public int min = 0;*/

    private GameObject EndTrigger;
    private bool GameDone = false;
    private bool botreached = false;

    //private bool hiddenSlowUI = true;
    //public GameObject SlowUI;

    //[SerializeField]private GameObject PlayerUI;

    //public GameObject endswim;
    //public GameObject disableui;
    //public GameObject farEndUI;
    //public GameObject diveUi;

    public GameObject AudioManager;

    AudioManager audioManager;

    public Text Hscore;
    Timerswim timerswimBot;
    public int changedir;

    public int buffer=0;
    float delay;

    int number_of_bots=10;
    private void Awake(){
    }
    private void Start()
    {
        timerswimBot = gameObject.GetComponent<Timerswim>();
        audioManager = AudioManager.GetComponent<AudioManager>();
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();
        Controller.detectCollisions = false;
        Velocity = Vector3.zero;
        IsFallAnimBot = false;
        fordive = false;
        //speedController = GameObject.Find("SpeedController").GetComponent<Slider>();
        //timerText = GameObject.Find("TimerText").GetComponent<Text>();
        //feedback = GameObject.Find("Feedback").GetComponent<Text>();
        //speedController.minValue = 0f;
        //speedController.maxValue = 200f;
        //sliderSpeed = 0f;

        /*EndTrigger = GameObject.FindGameObjectWithTag("NearEnd");
        EndTrigger.SetActive(false);*/

        //PlayerUI.SetActive(false);
        //SlowUI.SetActive(false);
        //audioManager.Play("back_ground");
        changedir = -1;

    }

    private void Update()
    {
        // Debug.Log(gameObject.GetComponent<Transform>());
        move();
    }


    #region METHODS
    private void move()
    {
        /*InpuX = Input.GetAxisRaw("Horizontal");
        InpuZ = Input.GetAxisRaw("Vertical");
        Movedir = new Vector3(InpuX, 0f, InpuZ).normalized;*/

        //timer khatam hote he

        
        Movedir = new Vector3(0f, 0f, 1f);


        if ( isUnderWater)
        {
            Walk();
            MoveHere();
        }

        //after he touch nearend trigger
        if (botreached )
        {
            Idle();
            //anim
            //celebration
        }

        if (timerswimBot.botdive1 == true)
        {
            //Barrier.GetComponent<BoxCollider>().isTrigger = true;
            Dive();
            

            timerswimBot.botdive1 = false;
            
        }
        

        if (IsFallAnimBot)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                transform.Translate(Vector3.down * downward);
                transform.Translate(Vector3.forward * forward);
                IsFallAnimBot = false;
                isUnderWater = true;
            }
        }
    }

    private void MoveHere()
    {
        /*float targetangle = Mathf.Atan2(Movedir.x, Movedir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref refsmoothvelocity, smoothingcam);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveincamdir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;*/

        Controller.Move(changedir * Movedir * MovespeedM * Time.deltaTime);
        
    }

    private void Walk()
    {
        
        ;
        MovespeedM = Walkspeed-Random.Range(-2.0f, 2.0f);
        // Debug.Log(MovespeedM);
        Anim.SetBool("Walk", true);
    }

    private void Idle()
    {
        Anim.SetBool("Walk", false);
    }

    //timer khatam hone pe run karwao
    private void Dive()
    {
        IsFallAnimBot = true;
        //isUnderWater = true;
        Anim.runtimeAnimatorController = swim;
        StartCoroutine(dive_delay());
    }

    IEnumerator dive_delay(){
        yield return new WaitForSeconds(1);
        audioManager.Play("Dive_splash");
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        //use mai aa sakta

        /*if (collision.gameObject.CompareTag("SwimBarrier"))
        {
            Debug.Log("D ui");
            fordive = true;
            //diveUi.SetActive(true);
            //StartCoroutine(HideDiveUi());
        }*/

        //far end detect hone pe smoothly modod usko
        if (collision.gameObject.CompareTag("FarEnd"))
        {
            Debug.Log("FarEnd Approached ai");
            //EndTrigger.SetActive(true);
            changedir = 1;
            //farEndUI.SetActive(true);
            //StartCoroutine(HidefarEndUI());
        }

        //near end pe rok do animation jab tak main player naa aajye rank bhi decide karni hai
        if (collision.gameObject.CompareTag("NearEnd"))
        {
            Debug.Log("NearEnd Approached bot");
            botreached = true;
            isUnderWater = false;
            
            //display rank

            //GameDone = true;
            //speedController.value = 0f;
            //endswim.SetActive(true);
            //disableui.SetActive(false);
            //Hscore.text = timerText.text;
            //Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("FarEnd"))
        {
            Debug.Log("rotate bot");
            transform.Rotate(0f, 180f, 0f);

        }
    }

    /*IEnumerator HidefarEndUI()
    {
        yield return new WaitForSeconds(2f);
        farEndUI.SetActive(false);
    }

    IEnumerator HideDiveUi()
    {
        yield return new WaitForSeconds(1.5f);
        diveUi.SetActive(false);
    }*/

    /*private void moveSlider()
    {
        if (isUnderWater && startslider)
        {
            sliderSpeed = 400f;
            clickable = true;
            startslider = false;
        }
        
        if (MovingUp && sliderSpeed!=0f)
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
            Debug.Log(Walkspeed);

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
            Debug.Log(delay);
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
        if (isUnderWater)
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
    }*/

}

