
using UnityEngine;
using Unity.Netcode;


public class Spawner : NetworkBehaviour
{

    //Vector3 spawnPostion;
    private GameObject spawnspot;
    private GameObject[] playersbefore, blue, red;

    
    
    
    private int numberOfPlayers;
    private GameObject[] spawnspots;

    [SerializeField]
    GameObject post1, post2, post3, post4;
    private GameObject gameManager;
    [SerializeField]
    public int teamID;
    private bool once = true;
    //private float Red, Green, Blue, Alpha;
    // private class RGBA{
        
    //    public float Red=0f;
    //    public float g=0f;
    //    public float b=0f;
    //    public float a=0f;
    // }

    // RGBA neela = new RGBA();
    //bool once1 = true;
    //[SerializeField] Rigidbody rb;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gamemanager");
        spawnspots = GameObject.FindGameObjectsWithTag("spawnspot");
        playersbefore = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayers = playersbefore.Length;
        spawnspot = spawnspots[numberOfPlayers - 1];
        
        if (IsLocalPlayer)
        {
            post1 = GameObject.Find("RedPost");
            post2 = GameObject.Find("BluePost");
            post3 = GameObject.Find("RedPost2");
            post4 = GameObject.Find("BluePost2");
            Debug.Log(numberOfPlayers);
            transform.position = spawnspot.transform.position;
            transform.rotation = spawnspot.transform.rotation;
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            if(gameObject.transform.position.z < 0)
            {
                post3.SetActive(false);
                post4.SetActive(false);
            }
            else
            {
                post1.SetActive(false);
                post2.SetActive(false);
            }
        }
        if (gameObject.transform.position.z < 0)
        {
            teamID = 1;
        }
        else
        {
            teamID = 0;
        }
        once = true;
        //once1 = true;
    }

    private void Update()
    {

        playersbefore = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(playersbefore.Length);

        if (IsLocalPlayer && playersbefore.Length == FootballManager.players && once)
        {
            Debug.Log("runs");

            //redPlayer.SetActive(true);

            red = GameObject.FindGameObjectsWithTag("Red");
            blue = GameObject.FindGameObjectsWithTag("Blue");
            foreach (GameObject player in red)
            {
                /*if (player.transform.position.z < 0 && transform.position.z < 0)
                {
                    player.SetActive(false);
                }
                else if (player.transform.position.z > 0 && transform.position.z > 0)
                {
                    player.SetActive(false);
                }*/
                if (player.transform.parent.parent.GetComponent<Spawner>().teamID ==
                    gameObject.GetComponent<Spawner>().teamID)
                {
                    player.SetActive(false);
                }
                else
                {
                    player.SetActive(true);
                }
            }
            foreach (GameObject player in blue)
            {
                /*if (player.transform.position.z > 0 && transform.position.z < 0)
                {
                    player.SetActive(false);
                }
                else if (player.transform.position.z < 0 && transform.position.z > 0)
                {
                    player.SetActive(false);
                }*/

                if (player.transform.parent.parent.GetComponent<Spawner>().teamID !=
                    gameObject.GetComponent<Spawner>().teamID)
                {
                    player.SetActive(false);
                }
                else
                {
                    player.SetActive(true);
                }
            }
            // if(gameObject.transform.position.z<0){

            // }
            
            gameObject.GetComponent<PlayerMovement>().enabled = true;   //Enabling player movement here
            
            
            once = false;
        }

        /*foreach (GameObject pole in Poles){
                 
                Renderer PoleColour=pole.GetComponent<Renderer>();
                 
            if(gameObject.transform.position.z*(pole.transform.position.z)>=0){
                Red=0.06666664f;
                Green=0.08968172f;
                Blue=0.772549f;
                    
                   
            }
                
            else{
                    
                Red=0.9716981f;
                Green=0.3276449f;
                Blue=0.2603417f;
                    
            }
            Color rang=new Color(Red,Green,Blue,Alpha);
            PoleColour.material.color=rang;
                
        }*/
    }

    public void ReSpawn()
    {
        if (IsLocalPlayer)
        {
            //Debug.Log("Runs");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = spawnspot.transform.position;
            transform.rotation = spawnspot.transform.rotation;
            //gameManager.GetComponent<GameManager>().football.GetComponent<Football>().respawningPlayer = false;
        }
    }

    /* [ServerRpc(RequireOwnership = false)]
     public void SpawnServerRpc(Vector3 pos)
     {
         Debug.Log("Changing transform in start");
         transform.position = pos;
         //transform.rotation = spawnspot.transform.rotation;
     }*/
}
