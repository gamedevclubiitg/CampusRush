using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Manage : NetworkBehaviour
{
    [SerializeField] private GameObject Playerprefab;
    [SerializeField] private float SpawnRange = 10f;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(Playerprefab, Vector3.zero + new Vector3(0, 2, Random.Range(-SpawnRange, 0)), Quaternion.identity);
        player.GetComponent<NetworkObject>().Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
