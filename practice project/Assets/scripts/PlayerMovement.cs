using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  // Start is called before the first frame update
    public GameObject plane;
    public float RotationAmount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameControls.pause){
            if(Input.GetKey("w")){
            plane.transform.eulerAngles += new Vector3(RotationAmount * Time.deltaTime, 0f, 0f);
            }
            if(Input.GetKey("a")){
                plane.transform.eulerAngles += new Vector3(0f, 0f, RotationAmount* Time.deltaTime);
            }
            if(Input.GetKey("s")){
                plane.transform.eulerAngles += new Vector3(-RotationAmount* Time.deltaTime, 0f, 0f);
            }
            if(Input.GetKey("d")){
                plane.transform.eulerAngles += new Vector3(0f, 0f, -RotationAmount* Time.deltaTime);
            }
        }
        
    }
}
