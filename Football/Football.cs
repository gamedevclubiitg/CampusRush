using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
//using MLAPI.Transports.PhotonRealtime;
using Cinemachine;
using Unity.Netcode;


public class Football : NetworkBehaviour
{
    [SerializeField]
    float magnitude;
    Rigidbody rb;
    Vector3 moveincamdir;
    [SerializeField]
    GameObject Goal, networkManager;
    bool once = false;
    public bool respawningBall = false;
    public bool respawningPlayer = false;
    [SerializeField] private float kickEffectDuration = 0.5f;
    [SerializeField] private float kickEffectIntensity = 20f;

    [SerializeField] private ParticleSystem trail;
    [SerializeField] List<AudioClip> audioClips;
    private ParticleSystem.EmissionModule em;
    Text Roomname;

    private void Awake()
    {
        Goal = GameObject.FindWithTag("Goal Text");
        Goal.GetComponent<Text>().enabled = false;
        Roomname = GameObject.Find("Room Name").GetComponent<Text>();
        networkManager = GameObject.Find("NetworkManager");
    }
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        em = trail.emission;
        //StartCoroutine(ApplyName());
    }

    
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Plastic"))
        {
            gameObject.GetComponent<AudioSource>().clip = audioClips[1];
        }
        else
        {
            gameObject.GetComponent<AudioSource>().clip = audioClips[0];
        }
        gameObject.GetComponent<AudioSource>().Play();
        em.rateOverDistance = kickEffectIntensity;
        StartCoroutine(StopKickEffect(kickEffectDuration));
    }

    // IEnumerator ApplyName()
    // {
    //     yield return new WaitForSeconds(0.25f);


    //     Debug.Log(networkManager.GetComponent<PhotonRealtimeTransport>().RoomName);
    //     Roomname.text = networkManager.GetComponent<PhotonRealtimeTransport>().RoomName;
    // }

    [ServerRpc(RequireOwnership = false)]
    public void CollidedServerRpc(Vector3 collision, Vector3 cp, Vector3 camera, bool walk, bool boost, bool run, bool IsGrounded)
    {
        moveincamdir = camera;
        
        //Debug.Log(moveincamdir);

        //ContactPoint cp = collision.GetContact(0);
        if (!boost && !run && !walk && rb.velocity.magnitude < 3f)
        {
            rb.velocity = Vector3.zero;
            magnitude = 0f;
        }
        else if (boost)
        {
            if (Input.GetMouseButton(1))
            {
                magnitude = 50f;
            }
            else
            {
                magnitude = 30f;
            }
        }
        else if (run)
        {
            if (Input.GetMouseButton(1))
            {
                magnitude = Random.Range(24f, 36f);
            }
            else
            {
                magnitude = Random.Range(12f, 18f);
            }
        }
        else if (walk)
        {
            magnitude = 4f;
        }
        if (!IsGrounded && (cp - transform.position).y < 0)
        {
            rb.AddForce(new Vector3(magnitude * moveincamdir.x, magnitude/3, magnitude * moveincamdir.z), ForceMode.VelocityChange);
        }
        else if (!IsGrounded && (cp - transform.position).y > 0)
        {
            rb.AddForce(new Vector3(magnitude * moveincamdir.x, -magnitude / 3, magnitude * moveincamdir.z), ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(new Vector3(magnitude * moveincamdir.x, magnitude / 5, magnitude * moveincamdir.z), ForceMode.VelocityChange);
        }
        //Debug.Log(magnitude);
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        //Goal Done
        if (other.gameObject.CompareTag("GoalR"))
        {
            //Debug.Log("BC");
            //Goal.SetActive(true);
            Goal.GetComponent<Text>().enabled = true;
            StartCoroutine(RespawnPlayers(5));
        }
        if (other.gameObject.CompareTag("GoalL"))
        {
            //Debug.Log("BC");
            Goal.GetComponent<Text>().enabled = true;
            StartCoroutine(RespawnPlayers(5));
        }
    }

    public IEnumerator RespawnPlayers(float delay)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerMovement>().Anim.SetBool("Dance", true);
        }
        yield return new WaitForSeconds(delay);
        respawningBall = true;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            player.GetComponent<Spawner>().ReSpawn();
            player.GetComponent<PlayerMovement>().Anim.SetBool("Dance", false);
        }
        //Goal.SetActive(false);
        Goal.GetComponent<Text>().enabled = false;
        //SceneManager.LoadScene("Football");
    }

    IEnumerator StopKickEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        em.rateOverDistance = 0f;
    }
}
