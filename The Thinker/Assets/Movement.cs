using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 50;
    public float sensitivity = 1.1f;
    
    
    float speedAdjusted;
    
    float verticalInput = 0f;
    float timeSinceVerticalDown = 0f;
    float turnX;
    float turnY;
    Transform cam;
    // Update is called once per frame
    void Start ()
    {
        cam = transform.Find("Camera");
    }


    void Update()
    {
        
        HandleVerticalInput();
        speedAdjusted = Time.deltaTime * speed;
        transform.localPosition += speedAdjusted * (Input.GetAxis("Horizontal") * cam.transform.right + verticalInput * timeSinceVerticalDown * cam.transform.up + Input.GetAxis("Vertical") * cam.transform.forward);
        if(Input.GetMouseButton(1))
        {
            turnX += Input.GetAxis("Mouse X") * sensitivity;
            turnY += Input.GetAxis("Mouse Y") * sensitivity;
        }
            
        cam.localRotation = Quaternion.Euler(-turnY, turnX, 0);

    }

    void HandleVerticalInput()
    {
        verticalInput = 0f;
        verticalInput = Mathf.Clamp(verticalInput, -1, 1);


        
        
        if(Input.GetKey("q"))
        {
            verticalInput -= 1f;
        }
        if(Input.GetKey("e"))
        {
            verticalInput += 1f;
        }
        //Time Since Vertical Down is used to make it so that up and down gradually increases in speed like the other directions by multiplying it by a number 0 to 1 that maxes at 1 after a second
        if(verticalInput == 0f){
            timeSinceVerticalDown -= Time.deltaTime;
        } else
        {
            timeSinceVerticalDown += 2 * Time.deltaTime;
        }
        timeSinceVerticalDown = Mathf.Clamp(timeSinceVerticalDown, 0, 1);
    }
}
