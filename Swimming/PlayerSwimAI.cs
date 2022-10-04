using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class PlayerSwimAI : MonoBehaviour
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



    public GameObject AudioManager;
    public Transform AI;
    AudioManager audioManager;

    public Text Hscore;
    Timerswim timerswimBot;
    public int changedir;

    public int buffer = 0;
    public float delay_AI;

    int number_of_bots = 10;
    bool await=false;
    private void Awake()
    {
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
        changedir = -1;

    }

    private void Update()
    {

        move();
        if(await){

        Vector3 correct = new Vector3(transform.position.x, 0.09999f, transform.position.z);
        transform.position = correct;
        }
    }


    #region METHODS
    private void move()
    {


        //timer khatam hote he


        Movedir = new Vector3(0f, 0f, 1f);


        if (isUnderWater)
        {
            Walk();
            MoveHere();
        }

        //after he touch nearend trigger
        if (botreached)
        {
            Idle();
            //anim
            //celebration
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

        Controller.Move(changedir * Movedir * MovespeedM * Time.deltaTime);

    }

    private void Walk()
    {

        ;
        MovespeedM = Walkspeed - Random.Range(-2.0f, 2.0f) * delay_AI;
        // Debug.Log(delay_AI+ " "+ MovespeedM);
        Anim.SetBool("Walk", true);
    }

    private void Idle()
    {
        Anim.SetBool("Walk", false);
    }

    //timer khatam hone pe run karwao
    public void Dive()
    {
        IsFallAnimBot = true;
        //isUnderWater = true;
        Anim.runtimeAnimatorController = swim;
        StartCoroutine(dive_delay());
    }

    IEnumerator dive_delay()
    {
        yield return new WaitForSeconds(1);
        audioManager.Play("Dive_splash");

        Vector3 correct = new Vector3(transform.position.x, 0.09999f, transform.position.z);
        transform.position = correct;
        await=true;

    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        //use mai aa sakta


        //far end detect hone pe smoothly modod usko
        if (collision.gameObject.CompareTag("FarEnd"))
        {
            Debug.Log("FarEnd Approached ai");

            changedir = 1;
        }

        //near end pe rok do animation jab tak main player naa aajye rank bhi decide karni hai
        if (collision.gameObject.CompareTag("NearEnd"))
        {
            Debug.Log("NearEnd Approached bot");
            botreached = true;
            isUnderWater = false;


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



}

