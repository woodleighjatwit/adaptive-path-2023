using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void UpdateCounter(){
        text.text = GameControls.collectedCubes.ToString() + "/" + GameControls.level.ToString();
        Debug.Log(GameControls.collectedCubes.ToString() + "/" + GameControls.level.ToString());
 
    }
    

}
