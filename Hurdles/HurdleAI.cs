using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HurdleAI : MonoBehaviour
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
    [SerializeField]GameObject player;

    public BoxCollider Box, Box2;
    public bool jump;
    [SerializeField] float initJump, finalJump, multiplier, value;
    public bool done, rotate = false;
    [SerializeField]List<GameObject> destinations;
    bool touched;
    int i;
    //[SerializeField] List<Vector3> offcenter;
    //public CapsuleCollider caps;

    private void Start()
    {
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();
        Box = GetComponent<BoxCollider>();
        Controller.detectCollisions = false;
        Velocity = Vector3.zero;
        IsGrounded = true;
        jump = false;
        done = false;
        touched = false;
        i = 0;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        move();
    }

    #region METHODS

    private void move()
    {
        //InpuZ = Input.GetAxisRaw("Vertical");
        //Movedir = (transform.forward * InpuZ).normalized;
        if (!done)
        {
            if (gameObject.GetComponent<NavMeshAgent>().isActiveAndEnabled)
            {
                gameObject.GetComponent<NavMeshAgent>().destination = destinations[i].transform.position;
                //Debug.Log(gameObject.name + " " + gameObject.GetComponent<NavMeshAgent>().destination);
            }
            Run();
            Isruuning = true;
            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward, out hit);
            //Debug.Log(hit.distance);
            if (hit.collider != null && hit.collider.CompareTag("Hurdle") && hit.distance < Random.Range(initJump, finalJump) && IsGrounded)
            {
                Jump();
                IncreaseSpeed(hit.collider.gameObject.transform.position.z - transform.position.z);
            }
            //MoveHere();

            if (!IsGrounded)
            {
                Velocity.y += Gravity * Time.deltaTime;
                Controller.Move(Velocity * Time.deltaTime);
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                MoveHere();
            }
            else
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
        else
        {
            Idle();
        }
        /*if (rotate)
        {
            Rotate();
        }*/

    }

    private void Rotate()
    {
        Vector3 eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0, 90, 0), (Time.deltaTime * multiplier));
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
    private void MoveHere()
    {
        Controller.Move(transform.forward * MovespeedM * Time.deltaTime);
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
        if (collision.gameObject.CompareTag("End"))
        {
            done = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Box.isTrigger = false;
            jump = false;
            Box2.enabled = false;
            //Debug.Log("Trigger Enter");
            IsGrounded = true;
            Velocity = Vector3.zero;
            Anim.SetBool("Jump", false);
        }
        if (other.gameObject.tag == "Destination")
        {
            //Debug.Log("HAA");
            //Debug.Log(i);
            if (!touched)
            {
                StartCoroutine(Increasei());
            }
        }
        /*if (other.gameObject.tag == "Multiplier")
        {
            multiplier /= value;
        }
        if (other.gameObject.tag == "CompleteRotate")
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            multiplier *= value;
            rotate = false;
        }*/
    }
    void IncreaseSpeed(float dist)
    {
        //Debug.Log("Called");
        //Debug.Log(distance + " " + 0.5f / ((Mathf.Abs(distance - 2) * 10) + 5));
        if (dist < 1.5f || dist > 2.5f)
        {
            gameObject.GetComponent<NavMeshAgent>().speed += 0.01f;
            Runspeed += 0.01f;
        }
        else if (dist < 1.8f || dist > 2.2f)
        {
            gameObject.GetComponent<NavMeshAgent>().speed += 0.03f;
            Runspeed += 0.03f;
        }
        else if ((dist > 1.8f && dist < 1.9f) || (dist < 2.2f && dist > 2.1f))
        {
            gameObject.GetComponent<NavMeshAgent>().speed += 0.1f;
            Runspeed += 0.1f;
        }
        else if (dist > 1.9f || dist < 2.1f)
        {
            gameObject.GetComponent<NavMeshAgent>().speed += 0.18f;
            Runspeed += 0.18f;
        }
        else if (dist > 1.98f && dist < 2.02f)
        {
            Debug.Log("U an AI. It's easy for you!");
        }
    }

    IEnumerator Increasei()
    {
        touched = true;
        i++;
        //Debug.Log("i " + i);
        if (i >= destinations.Count)
        {
            Debug.Log("Happened");
            i--;
            Idle();
        }
        if (i == 2)
        {
            gameObject.GetComponent<Ragdoll>().turned = true;
        }
        yield return new WaitForSeconds(0.5f);
        touched = false;
    }
}
