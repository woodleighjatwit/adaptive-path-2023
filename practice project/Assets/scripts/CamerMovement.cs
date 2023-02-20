using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMovement : MonoBehaviour
{

    public Transform player;
    public Transform cameraTransform;
    public Vector3 lockedCameraOffset;
    private Vector3 unlockedCameraOrigin;
    private Vector3 unlockedCameraDifference;
    public bool lockedCamera = true;
    private bool drag = false;
    private Vector3 pos;

    void Start()
    {
        cameraTransform = transform.transform;
        cameraTransform.position = player.transform.position;
        
    }

    void FixedUpdate(){
        if (lockedCamera){
            transform.transform.position = player.transform.position + lockedCameraOffset;
        }
        if (!lockedCamera && Input.GetKey("space")){
            lockedCamera = true;
        }
               
        
        if (Input.GetMouseButton(0)){
            unlockedCameraDifference = Input.mousePosition - cameraTransform.position;
            
            if(!drag){
                lockedCamera = false;
                unlockedCameraOrigin = Input.mousePosition;
                drag = true;
                Debug.Log(Input.mousePosition);
            }
           
        }
        else{
            drag = false;
        }

        if (drag){
            pos = Camera.main.ScreenToViewportPoint(unlockedCameraOrigin - unlockedCameraDifference);
            pos[2] = pos[1];
            pos[1] = 15;
            cameraTransform.position = pos;
          
         
        }

        
    }

}