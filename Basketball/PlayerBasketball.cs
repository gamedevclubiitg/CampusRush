using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasketball : MonoBehaviour
{
    public float MovespeedM;
    public float Runspeed = 2f;
    public float Walkspeed = 1f;

    public bool IsGrounded = false;
    [SerializeField] private float Gravity;

    public Vector3 Movedir;
    public Vector3 Velocity; 

    private float InpuX;
    private float InpuZ;
    private CharacterController Controller;
    private Animator Anim;

    public GameObject Ball;
    public Rigidbody rb;
    public bool holdingball ;
    public GameObject ballpos;
    public GameObject playercamera;


    private void Start()
    {
        Controller = GetComponent<CharacterController>();
        //Anim = GetComponentInChildren<Animator>();
        Controller.detectCollisions = false;
        //Velocity = Vector3.zero;

        rb = Ball.GetComponent<Rigidbody>();
        rb.useGravity = false;
        holdingball = true;
        IsGrounded = true;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        move();
    }


    #region METHODS
    private void move()
    {
        InpuX = Input.GetAxisRaw("Horizontal");
        InpuZ = Input.GetAxisRaw("Vertical");

        Movedir = (transform.right * InpuX + transform.forward * InpuZ).normalized;


        /*if (IsGrounded && transform.position.y >= 0.01)
        {
            IsGrounded = false;
        }*/

        if (Movedir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            walk(); 
        }

        else if (Movedir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            run();
        }

        else if (Movedir == Vector3.zero)
        {
            idle();
        }

        if (Movedir != Vector3.zero)
        {
            MoveHere();
        }

        /*if (!IsGrounded)
        {
            Velocity.y += Gravity * Time.deltaTime;
            Controller.Move(Velocity * Time.deltaTime);
        }*/

    }

    private void MoveHere()
    {
        Controller.Move(Movedir * MovespeedM * Time.deltaTime);       
    }

    private void walk()
    {
        MovespeedM = Walkspeed;
        //Anim.SetBool("Walk", true);
        //Anim.SetBool("Run", false);    
    }

    private void run()
    {
        MovespeedM = Runspeed;
        //Anim.SetBool("Run", true);
        //Anim.SetFloat("Speed", 1);
    }

    private void idle()
    {
        //Anim.SetBool("Run", false);
        //Anim.SetBool("Walk", false);
    }

    #endregion

    #region BASKETBALL
    public void launchball(float force)
    {
       
       holdingball = false;
       rb.useGravity = true;
       rb.AddForce(playercamera.transform.forward * force, ForceMode.VelocityChange);

    }

    public void stopball()
    {
        Ball.transform.position = ballpos.transform.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Ball.transform.rotation = Quaternion.Euler(Vector3.zero);
        
        rb.useGravity = false;
        holdingball = true;
    }



    #endregion

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
            Velocity = Vector3.zero;
        }
    }*/
}

