using System.Collections;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.UI;
using System;
//using MLAPI.Transports.PhotonRealtime;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    public float MovespeedM;
    public float Runspeed = 2f;
    public float Walkspeed = 1f;
    [SerializeField]
    GameObject networkManager, ExitUI, PlayerUI, LoadingUI, Page3;
    public GameObject Message;
    public GameObject Scores_endpanel, Name;
    //public GameObject Boost_text;
    //private Text boost_text;
    private Slider slider;
    [SerializeField] private float smoothingcam, jumpSpeed, someValue;
    [SerializeField] private Transform cam, cam1, cam2;
    //[SerializeField] private bool IsGrounded;
    public bool IsGrounded = false;
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
    public Animator Anim;
    [SerializeField] private ParticleSystem trail;
    private ParticleSystem.EmissionModule em;
    float initialangle;
    [SerializeField]
    CinemachineFreeLook rightClick;
    float time = 0;
    

    private void Start()
    {
        if (!IsLocalPlayer)
        {
            gameObject.GetComponentInChildren<AudioListener>().enabled = false;
            gameObject.GetComponentInChildren<Camera>().enabled = false;
            gameObject.GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
        }
        else
        {
            Page3 = GameObject.Find("page 3");
            //Boost_text=GameObject.Find("boost_text");
            //boost_text=Boost_text.GetComponent<Text>();
            Controller = GetComponent<CharacterController>();
            //slider = GameObject.Find("slider").GetComponent<Slider>();
            slider = GameObject.Find("BoostBar").GetComponent<Slider>();
            Anim = GetComponent<Animator>();
            Controller.detectCollisions = false;
            Velocity = Vector3.zero;
            IsGrounded = true;
            Boost = 30;
            //boostField = GameObject.Find("Boost").GetComponent<Text>();
            slider.minValue = 0f;
            slider.maxValue = 100f;
            //boost_text.text=Boost.ToString();
            em = trail.emission;
            Roomname = GameObject.Find("Room Name").GetComponent<Text>();
            Roomname.text = GameObject.Find("NetworkManager").GetComponent<RelayManager>().joinCode;
            networkManager = GameObject.Find("NetworkManager");
            ExitUI = GameObject.Find("ExitUI");
            LoadingUI = GameObject.Find("LoadingUI");
            PlayerUI = GameObject.Find("PlayerUI");
            Message = GameObject.Find("message");
            Scores_endpanel = GameObject.Find("Scores");
            StartCoroutine(ApplyName());
            
            //rightClick.LookAt = GameObject.FindGameObjectWithTag("Football").transform;
        }
        Debug.Log("Started");
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            //Debug.Log(Convert.ToInt32(Boost).ToString());
            //boostField.text = Convert.ToInt32(Boost).ToString();
            move();
            if (Boost > 100f)
            {
                Boost = 100f;
            }
            slider.value = gameObject.GetComponent<PlayerMovement>().Boost;
            //boost_text.text=Boost.ToString();
        }

        else
        {
            return;
        }

        if (IsLocalPlayer && boost)
        {
            em.rateOverDistance = 30f;
        }
        else
        {
            em.rateOverDistance = 0f;
        }
        time += Time.deltaTime;
        if(time >= UnityEngine.Random.Range(2f, 7f))
        {
            time = 0;
            Boost += 1;
        }
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.Find("Tutorial").SetActive(false);
        }
        if (!IsClient)
        {

        }
    }


    #region METHODS
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
        if (Input.GetMouseButtonDown(1))
        {
            cam2.gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.Value = cam.eulerAngles.y;
        }
        else if (Input.GetMouseButton(1))
        {
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            cam1.gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.Value = cam.eulerAngles.y;
        }
        else
        {
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
            initialangle = cam.eulerAngles.y;
        }
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
            MovespeedM = 15;
            Anim.SetBool("Run", true);
            Anim.SetFloat("Speed", 1.7f);
            Boost -= 10 * Time.deltaTime;
            // slider.value=Boost;
        }

    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (IsLocalPlayer)
        {
            //Debug.Log("Yop");
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGrounded = true;
                Velocity = Vector3.zero;
            }
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
            if (!ExitUI.activeInHierarchy)
            {
                ExitUI.SetActive(true);
                if (R > L && gameObject.GetComponent<Spawner>().teamID == 1)
                {
                    Message.GetComponent<Text>().text = "You Won!";
                    Scores_endpanel.GetComponent<Text>().text = "Your team " + R.ToString() + " Their team " + L.ToString();
                }
                else if (L > R && gameObject.GetComponent<Spawner>().teamID == 0)
                {
                    Message.GetComponent<Text>().text = "You Won!";
                    Scores_endpanel.GetComponent<Text>().text = "Your team " + R.ToString() + " Their team " + L.ToString();
                }
                else if(L != R)
                {
                    Message.GetComponent<Text>().text = "You Lost!";
                    Scores_endpanel.GetComponent<Text>().text = "Your team " + R.ToString() + " Their team " + L.ToString();
                }
                else
                {
                    Message.GetComponent<Text>().text = "Tied!";
                    Scores_endpanel.GetComponent<Text>().text = "Your team " + R.ToString() + " Their team " + L.ToString();
                }
            }
        }
    }

    IEnumerator ApplyName()
    {
        yield return new WaitForSeconds(10);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = GameObject.Find("Spawn").GetComponent<Gender>().name;
        Debug.Log(GameObject.Find("Spawn").GetComponent<Gender>().name);
        ExitUI.SetActive(false);
        LoadingUI.SetActive(false);
    }
}

