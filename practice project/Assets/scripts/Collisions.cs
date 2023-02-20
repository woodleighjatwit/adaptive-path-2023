using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    private GameControls gameControls;
    void Start(){
        gameControls = GameObject.Find("GameManager").GetComponent<GameControls>();
    }
    void OnCollisionEnter (Collision collisionInfo){
        
        if (collisionInfo.collider.tag == "cube"){
            collisionInfo.gameObject.SetActive(false);
            gameControls.collisionOccurred();
        }
    }
}
