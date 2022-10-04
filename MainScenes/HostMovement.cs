using System.Collections;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.UI;

public class HostMovement : NetworkBehaviour
{
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
    private Animator Anim;
    [SerializeField] private ParticleSystem trail;
    //private ParticleSystem.EmissionModule em;
    float initialangle;
    [SerializeField]
    CinemachineFreeLook rightClick;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsLocalPlayer)
        {
            gameObject.GetComponentInChildren<AudioListener>().enabled = false;
            gameObject.GetComponentInChildren<Camera>().enabled = false;
            gameObject.GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
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
            // Roomname.text = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>().RoomName;
            ExitUI = GameObject.Find("ExitUI");
            LoadingUI = GameObject.Find("LoadingUI");
            PlayerUI = GameObject.Find("PlayerUI");
            ExitUI.SetActive(false);
            LoadingUI.SetActive(false);
        }
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
