using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System;

public class PlayerMovementTE : MonoBehaviour
{
    public float MovespeedM;
    public float Runspeed = 2f;
    public float Walkspeed = 1f;
    public float MouseSentivity = 5f;
    //[SerializeField] GameObject NetworkManager;

    //public GameObject Boost_text;
    //private Text boost_text;
    //private Slider slider;
    [SerializeField] private float smoothingcam, jumpSpeed;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform followTarget;
    //[SerializeField] private bool IsGrounded;
    public bool IsGrounded = false;
    public float Boost;
    public bool boost = false, walk = false, run = false;
    [SerializeField] private float Gravity;
    //[SerializeField] private Text boostField, Roomname;

    public Vector3 Movedir;
    public Vector3 moveincamdir;
    public Vector3 Velocity;

    private float refsmoothvelocity;
    private float InpuX;
    private float InpuZ;
    private float MouseX;
    private float MouseY;
    private CharacterController Controller;
    private GameObject Character;
    private Animator Anim;
    //[SerializeField] private ParticleSystem trail;
    //private ParticleSystem.EmissionModule em;
    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();
        Character = GameObject.FindGameObjectWithTag("male");
        Velocity = Vector3.zero;
        IsGrounded = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        moveCam();
        move();
    }

    private void move()
    {
        InpuX = Input.GetAxisRaw("Horizontal");
        InpuZ = Input.GetAxisRaw("Vertical");
        Movedir = new Vector3(InpuX, 0f, InpuZ).normalized;


        if (Movedir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        else if (Movedir != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && (!Input.GetKey(KeyCode.LeftControl) || Boost <= 0))
        {
            Run();
        }
        else if (Movedir == Vector3.zero)
        {
            Idle();
        }
        else if (Boost > 0 && Movedir != Vector3.zero && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            FastRun();
        }
        if (IsGrounded && Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        if (Movedir != Vector3.zero)
        {
            MoveHere();
        }
        if (!IsGrounded)
        {
            Velocity.y += Gravity * Time.deltaTime;
            Controller.Move(Velocity * Time.deltaTime);
        }

    }

    private void MoveHere()
    {
        float targetangle = Mathf.Atan2(Movedir.x, Movedir.z) * Mathf.Rad2Deg;
        float Smoothingangle = Mathf.SmoothDampAngle(Character.transform.localEulerAngles.y, targetangle, ref refsmoothvelocity, smoothingcam);
        Character.transform.localEulerAngles = new Vector3(0, Smoothingangle, 0);

        targetangle += cam.eulerAngles.y;
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref refsmoothvelocity, smoothingcam);
        //GameObject.Find("Male").transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveincamdir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
        Controller.Move(moveincamdir.normalized * MovespeedM * Time.deltaTime);
        //Controller.Move(Movedir.normalized * MovespeedM * Time.deltaTime);
    }

    private void Walk()
    {
        walk = true;
        boost = false;
        run = false;
        MovespeedM = Walkspeed;
        Anim.SetBool("Walk", true);
        Anim.SetBool("Run", false);
    }

    private void Run()
    {
        walk = false;
        boost = false;
        run = true;
        MovespeedM = Runspeed;
        Anim.SetBool("Run", true);
        Anim.SetFloat("Speed", 1);
    }

    private void Idle()
    {
        walk = false;
        boost = false;
        run = false;
        Anim.SetBool("Run", false);
        Anim.SetBool("Walk", false);
    }

    private void Jump()
    {
        Debug.Log("jump");
        IsGrounded = false;
        Velocity.y = jumpSpeed;
    }

    private void FastRun()
    {
        if (Boost <= 0)
        {
            Run();
        }
        else
        {
            walk = false;
            boost = true;
            run = false;
            MovespeedM = 22;
            Anim.SetBool("Run", true);
            Anim.SetFloat("Speed", 3);
            Boost -= 10 * Time.deltaTime;
            // slider.value=Boost;
        }

    }

    private void moveCam()
    {
        
        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");
        followTarget.rotation *= Quaternion.AngleAxis(MouseX * MouseSentivity, Vector3.up);
        followTarget.rotation *= Quaternion.AngleAxis(MouseY * MouseSentivity, Vector3.right);

        var angles = followTarget.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.localEulerAngles.x;

        if(angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 30)
        {
            angles.x = 30;
        }

        
        
        followTarget.localEulerAngles = angles;

        transform.rotation = Quaternion.Euler(0, followTarget.rotation.eulerAngles.y, 0);
        followTarget.localEulerAngles = new Vector3(angles.x, 0, 0);
    }
}
