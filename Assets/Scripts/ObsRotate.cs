using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObsRotate : MonoBehaviour
{
    Vector3 originMousePos;

    public float rotSpeed = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            

            transform.Rotate(Vector3.left * rotSpeed * Time.deltaTime * mouseX);
            
        }
    }
}
