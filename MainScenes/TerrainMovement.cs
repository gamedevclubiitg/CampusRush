using System.Collections;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

// using Unity.Netcode.Samples;
using Unity.Netcode.Transports;
using Cinemachine;
using UnityEngine.UI;
using System;
using Unity.Collections;
using TMPro;

public class TerrainMovement : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> PlayerName { get; set; } = new NetworkVariable<FixedString32Bytes>();
    public float MovespeedM;
    public float Runspeed = 2f;
    public float Walkspeed = 1f;
    [SerializeField]
    GameObject ExitUI, PlayerUI, LoadingUI;
    //public GameObject Message;
    //public GameObject Scores_endpanel;
    //public GameObject Boost_text;
    //private Text boost_text;
    //private Slider slider;
    [SerializeField] private float smoothingcam, jumpSpeed, someValue;
    [SerializeField] private Transform cam, cam1;

    //[SerializeField] private bool IsGrounded;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float groundcheckdis;
    //[SerializeField] private BoxCollider collider1;
    //[SerializeField] private CapsuleCollider collider2;


    public float Boost;
    public bool boost = false, walk = false, run = false;
    [SerializeField] private float Gravity;
    [SerializeField] private Text Roomname;
    bool once = true;
    public Vector3 Movedir;
    public Vector3 moveincamdir;
    public Vector3 Velocity;
    
    private float refsmoothvelocity;
    private float InpuX;
    private float InpuZ;
    private CharacterController Controller;
    private Animator Anim;
    [SerializeField] private ParticleSystem trail;

    //private ParticleSystem.EmissionModule em;
    float initialangle;
    [SerializeField]
    CinemachineFreeLook rightClick;
    [SerializeField]
    private TextMeshProUGUI playerName;
    float time = 0;

    private void Start()
    {
        if (!IsLocalPlayer)
        {
            gameObject.GetComponentInChildren<AudioListener>().enabled = false;
            gameObject.GetComponentInChildren<Camera>().enabled = false;
            gameObject.GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
            FixedString32Bytes str = PlayerName.Value;
            playerName.text = str.ConvertToString();
        }
        else
        {
            transform.position = new Vector3(UnityEngine.Random.Range(410, 450), 1, UnityEngine.Random.Range(350, 370));
            Controller = GetComponent<CharacterController>();
            Anim = GetComponent<Animator>();
            Controller.detectCollisions = false;
            Velocity = Vector3.zero;
            IsGrounded = true;
            Boost = 30;
            Roomname = GameObject.Find("Room Name").GetComponent<Text>();
            Roomname.text = GameObject.Find("NetworkManager").GetComponent<RelayManager>().joinCode;
            // Roomname.text = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>().RoomName;
            ExitUI = GameObject.Find("ExitUI");
            LoadingUI = GameObject.Find("LoadingUI");
            PlayerUI = GameObject.Find("PlayerUI");
            ExitUI.SetActive(false);
            LoadingUI.SetActive(false);
            TakeNameServerRpc();
        }
        Debug.Log("Started");
    }

    private void Update()
    {
       

        if (IsLocalPlayer)
        {
            move();
            if (Boost > 100f)
            {
                Boost = 100f;
            }
        }
        else
        {
            if (playerName.text == " ")
            {
                FixedString32Bytes str = PlayerName.Value;
                playerName.text = str.ConvertToString();
            }
            return;
        }

        if (IsLocalPlayer && boost)
        {
            //em.rateOverDistance = 30f;
        }
        else
        {
            //em.rateOverDistance = 0f;
        }

        time += Time.deltaTime;

        if(time >= UnityEngine.Random.Range(2, 7))
        {
            time = 0;
            Boost += 1;
            
        }

    }


    #region METHODS

    private void move()
    {
        InpuX = Input.GetAxisRaw("Horizontal");
        InpuZ = Input.GetAxisRaw("Vertical");
        Movedir = new Vector3(InpuX, 0f, InpuZ).normalized;

        //IsGrounded = Physics.CheckSphere(transform.position, groundcheckdis, ground);

        IsGrounded = Physics.CheckSphere(transform.position, groundcheckdis, ground);
        if (IsGrounded && Velocity.y < 0)
        {
            IsGrounded = true;
            Velocity.y = -0.1f;
            Anim.SetBool("IsGrounded", true);
            //collider2.enabled = false;
            //collider1.enabled = true;
        }

        //if (IsGrounded)
        {
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
            if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }


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
        cam1.gameObject.SetActive(true);
        //cam2.gameObject.SetActive(false);
        initialangle = cam.eulerAngles.y;
        float targetangle = Mathf.Atan2(Movedir.x, Movedir.z) * Mathf.Rad2Deg + ((!Input.GetMouseButton(1)) ? cam.eulerAngles.y : initialangle);
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref refsmoothvelocity, smoothingcam);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveincamdir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
        Controller.Move(moveincamdir.normalized * MovespeedM * Time.deltaTime);

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
        //Debug.Log("Idle state is being called");
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
        Anim.SetBool("IsGrounded", false);
        //collider2.enabled = true;
        //collider1.enabled = false;
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

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (IsLocalPlayer)
        {
            /*if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Ground collision done");
                IsGrounded = true;
                Velocity = Vector3.zero;
                Anim.SetBool("IsGrounded", true);
            }*/

            if (collision.gameObject.CompareTag("Football"))
            {
                Debug.Log("Well");
                StartCoroutine(Collide(collision));
            }
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boost")
        {
            Boost += 20;
            // slider.value=Boost;
            Destroy(other.gameObject);
        }
    }

    

    IEnumerator Collide(Collision collision)
    {
        //collision.gameObject.GetComponent<SphereCollider>().isTrigger = true;
        Debug.Log(collision.gameObject.name);
        collision.gameObject.GetComponent<Football>().CollidedServerRpc(gameObject.transform.position,
            collision.GetContact(0).point,
            gameObject.GetComponentInChildren<CinemachineBrain>().transform.forward,
            gameObject.GetComponent<PlayerMovement>().walk,
            gameObject.GetComponent<PlayerMovement>().boost,
            gameObject.GetComponent<PlayerMovement>().run,
            gameObject.GetComponent<PlayerMovement>().IsGrounded);
        yield return new WaitForSeconds(0.25f);
        //collision.gameObject.GetComponent<SphereCollider>().isTrigger = false;
    }


    public void GameDone(float R, float L)
    {
        if (IsLocalPlayer)
        {
            if (PlayerUI.activeInHierarchy)
            {
                PlayerUI.SetActive(false);
            }

            /*if (!ExitUI.activeInHierarchy)
            {
                ExitUI.SetActive(true);
                if (R > L)
                {
                    Message.GetComponent<Text>().text = "Red team won";
                    Scores_endpanel.GetComponent<Text>().text = "Red team " + R.ToString() + " Blue team " + L.ToString();
                }
                else if (L > R)
                {
                    Message.GetComponent<Text>().text = "Blue team won";
                    Scores_endpanel.GetComponent<Text>().text = "Red team " + R.ToString() + " Blue team " + L.ToString();
                }
                else
                {
                    Message.GetComponent<Text>().text = "Match tied";
                    Scores_endpanel.GetComponent<Text>().text = "Red team " + R.ToString() + " Blue team " + L.ToString();
                }
            }*/
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeNameServerRpc()
    {
        int hash = gameObject.GetComponent<NetworkObject>().GetHashCode();
        string[] names = DBManager.name.Split(' ');

        PlayerName.Value = names[0];
        Debug.Log($"Hash : {hash}");
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(transform.position, 0.01f);
    }
}

