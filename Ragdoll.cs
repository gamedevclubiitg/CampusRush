using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class Ragdoll : NetworkBehaviour
{
    [SerializeField]
    private List<Collider> RagdollParts = new List<Collider>();
    GameObject Armature;
    [SerializeField] bool AI = false;
    [SerializeField] bool IsInFront = false;
    [SerializeField] GameObject player;
    private int Count;
    public bool turned;


    void Start()
    {
        getRagdollParts();
        Armature = transform.Find("Male/Armature").gameObject;
        Armature.SetActive(false);
        IsInFront = false;
        turned = false;
        Count = 0;
    }

    void getRagdollParts()
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.enabled = false;
                RagdollParts.Add(c);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AI && !gameObject.GetComponent<HurdleAI>().done)
        {
            if (((!turned)? transform.position.z > player.transform.position.z : transform.position.x > player.transform.position.x) && !IsInFront)
            {
                Hurdle_UI.rank++;
                IsInFront = true;
            }
            else if (((!turned) ? transform.position.z < player.transform.position.z : transform.position.x < player.transform.position.x) && IsInFront)
            {
                Hurdle_UI.rank--;
                IsInFront = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision with " + collision.gameObject);
        if (collision.gameObject.CompareTag("Hurdle"))
        {
            if (!AI)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            Count++;
            //Debug.Log("Count " + Count);
            if (Count >= 3)
            {
                Debug.Log("Hurdle collision");
                Armature.SetActive(true);
                this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                this.gameObject.GetComponent<CharacterController>().enabled = false;
                if (AI)
                {
                    this.gameObject.GetComponent<HurdleAI>().enabled = false;
                    this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                }
                else
                {
                    this.gameObject.GetComponent<PlayerHurdle>().enabled = false;
                }
                this.gameObject.GetComponentInChildren<Animator>().enabled = false;
                this.gameObject.GetComponentInChildren<Animator>().avatar = null;

                foreach (Collider c in RagdollParts)
                {
                    c.enabled = true;
                    c.attachedRigidbody.velocity = Vector3.zero;
                }
            }
            else
            {
                if (AI)
                {
                    gameObject.GetComponent<NavMeshAgent>().speed -= 0.1f;
                    gameObject.GetComponent<HurdleAI>().Runspeed -= 0.1f;
                    collision.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(900, 900, 900));
                    collision.gameObject.layer = 8;
                }
                else
                {
                    gameObject.GetComponent<PlayerHurdle>().Runspeed -= 0.1f;
                }
            }
        }
    }
}
