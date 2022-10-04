using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private GameObject cheats;
    [SerializeField]
    private List<Transform> positions;
    private bool isPlayerSet  = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cheats = GameObject.Find("CHEATS");
        positions.Clear();
        foreach(Transform cheat in cheats.transform){
            if(cheat.transform == null){
                return;
            }
            positions.Add(cheat.transform);
        }
        // StartCoroutine(SetPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))// && Input.GetKey(KeyCode.LeftShift))
        {
            int n = GetKeyNumber();
            if(n < positions.Count){
                transform.position = positions[n].position;
            }
            
        }
    }

    private int GetKeyNumber()
    {
        if(Input.GetKey(KeyCode.Keypad0))
        {
            return 0;
        }
        
        else if(Input.GetKey(KeyCode.Keypad1))
        {
            return 1;
        }
        
        else if(Input.GetKey(KeyCode.Keypad2))
        {
            return 2;
        }
        
        else if(Input.GetKey(KeyCode.Keypad3))
        {
            return 3;
        }
        
        else if(Input.GetKey(KeyCode.Keypad4))
        {
            return 4;
        }
        
        else if(Input.GetKey(KeyCode.Keypad5))
        {
            return 5;
        }
        
        else if(Input.GetKey(KeyCode.Keypad6))
        {
            return 6;
        }
        
        else if(Input.GetKey(KeyCode.Keypad7))
        {
            return 7;
        }
        
        else if(Input.GetKey(KeyCode.Keypad8))
        {
            return 8;
        }
        
        else if(Input.GetKey(KeyCode.Keypad9))
        {
            return 9;
        }

        else{
            return 99;
        }
    }
}
