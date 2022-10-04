using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerHurdle : MonoBehaviour
{
    public float MovespeedM;
    public float Runspeed = 2f;
    public float Walkspeed = 1f;

    [SerializeField] private float smoothingcam, jumpSpeed;
    [SerializeField] private Transform cam;
    [SerializeField] private float Gravity;
    public bool IsGrounded = false;

    public Vector3 Movedir;
    public Vector3 moveincamdir;
    public Vector3 Velocity; 

    private float InpuZ;
    private CharacterController Controller;
    private Animator Anim;
    public bool Isruuning;
    public bool hithurdle;
    [SerializeField] float multiplier, value;
    bool rotate = false;


    public BoxCollider Box, Box2;
    public bool jump, once;
    //public CapsuleCollider caps;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();
        Box = GetComponent<BoxCollider>();
        Controller.detectCollisions = false;
        Velocity = Vector3.zero;
        IsGrounded = true;
        jump = false;
        once = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        move();
    }

    #region METHODS

    private void move()
    {
        InpuZ = 1;
        Movedir = (transform.forward * InpuZ).normalized;
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit);
        if (InpuZ == 1)
        {  
            Run();
            Isruuning = true;
        }
        else if (InpuZ == -1)
        {
            RunBack();
        }
        else if (Movedir == Vector3.zero && IsGrounded)
        {
            Idle();
            Isruuning = false;
        }

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space) && Isruuning )
        {
            Jump();
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);
                IncreaseSpeed(hit.distance);
            }
        }
        if(Movedir != Vector3.zero)
        {
            MoveHere();
        }
        if (!IsGrounded)
        {
            Velocity.y += Gravity * Time.deltaTime;
            Controller.Move(Velocity * Time.deltaTime);
        }
        if (rotate)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        Vector3 eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0, 90, 0), (Time.deltaTime * multiplier));
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
    private void MoveHere()
    {
        Controller.Move(Movedir * MovespeedM * Time.deltaTime);      
    }

    private void Run()
    {
        MovespeedM = Runspeed;
        Anim.SetBool("Run", true);
        Anim.SetFloat("Speed", 1);
    }

    private void RunBack()
    {
        MovespeedM = Runspeed;
        Anim.SetBool("Back", true);
        Anim.SetBool("Run", false);
    }

    private void Idle()
    {
        MovespeedM = 0;
        Anim.SetBool("Run", false);
        Anim.SetBool("Back", false);
    }

    private void Jump()
    {
        IsGrounded = false;
        Velocity.y = jumpSpeed;
        Anim.SetBool("Jump", true);
        jump = true;
        Box.isTrigger = true;
        Box2.enabled = true;

    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.CompareTag("Ground"))
         {
            //Debug.Log("Groundtouch");
            IsGrounded = true;
            Velocity = Vector3.zero;
            Anim.SetBool("Jump", false);
         }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground" && Velocity.y < 0)
        {
            Box.isTrigger = false;
            jump = false;
            Box2.enabled = false;
            //Debug.Log("Trigger Enter");
            IsGrounded = true;
            Velocity = Vector3.zero;
            Anim.SetBool("Jump", false);
        }
    }

    void IncreaseSpeed(float dist)
    {
        Debug.Log(dist);
        if (dist < 1.5f || dist > 2.5f)
        {
            Runspeed += 0.01f;
        }
        else if (dist < 1.8f || dist > 2.2f)
        {
            Runspeed += 0.03f;
        }
        else if ((dist > 1.8f && dist < 1.9f) || (dist < 2.2f && dist > 2.1f))
        {
            Runspeed += 0.07f;
        }
        else if (dist > 1.9f || dist < 2.1f)
        {
            Runspeed += 0.1f;
        }
        else if (dist > 1.98f && dist < 2.02f)
        {
            Runspeed += 0.18f;
        }
        StartCoroutine(gameObject.GetComponent<Hurdle_UI>().GiveRemark(dist));
    }

    public void Restart()
    {
        SceneManager.LoadScene("Hurdle");
    }
}

