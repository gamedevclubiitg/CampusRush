using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class BoostManager : NetworkBehaviour
{
    [SerializeField] private GameObject Boost;
    private Vector3 currentPosition;
    [SerializeField] private float delay = 5f;

    /*[SerializeField]
    private GameObject NetManager;
    private NetworkManager networkManager;*/

    // Start is called before the first frame update
    void Start()
    {
        //networkManager = NetManager.GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boost") && IsServer)
        {
            //BoostDespawnServerRpc();
            GameObject boost = other.gameObject;
            currentPosition = boost.transform.position;
            boost.SetActive(false);
            boost.GetComponent<NetworkObject>().Despawn();
            StartCoroutine(SpawnBoostAgain(delay, currentPosition));
        }
    }

    IEnumerator SpawnBoostAgain(float delay, Vector3 currentPosition)
    {
        yield return new WaitForSeconds(delay);

        GameObject boost = Instantiate(Boost, currentPosition, Quaternion.identity);
        boost.GetComponent<NetworkObject>().Spawn();
    }

}
