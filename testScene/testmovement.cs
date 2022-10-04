using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmovement : MonoBehaviour
{
    // Start is called before the first frame update
    float InputX, InputZ;
    Vector3 Movedir;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move(){
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");
        Movedir = new Vector3(InputX, 0f, InputZ).normalized;

        gameObject.transform.position = new Vector3 (transform.position.x + (InputX * speed), transform.position.y ,transform.position.z + (InputZ * speed));

    }
}
