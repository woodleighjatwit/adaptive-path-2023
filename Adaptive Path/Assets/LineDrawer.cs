using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    private RaycastHit hit;
    private GameObject newLine;
    private LineRenderer lineRender;
    private GameObject firstObject;
    private GameObject secondObject;
    public float timerDelay;
    private float timer;
    public float lineWidth;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // if left mouse click down
        if(Input.GetMouseButtonDown(0)){
            firstObject = MouseOverObject();
            Debug.Log(firstObject);
           
        }
        // if left mouse click up
        else if(Input.GetMouseButtonUp(0)){
            firstObject = null;
            secondObject = null;
            Debug.Log(firstObject);
            Debug.Log(secondObject);

        }
        // if drag
        if(Input.GetMouseButton(0)){
            if(firstObject){
                secondObject = MouseOverObject();
                if(secondObject && secondObject != firstObject){
                    DrawFinalLine(firstObject, secondObject);
                    firstObject = secondObject;
                    secondObject = null;
                    Debug.Log(firstObject);
                    Debug.Log(secondObject);
                    
                }
                
            }
            
        }

    }
   
    private GameObject MouseOverObject(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 15f)){
            
            if(hit.transform.tag == "Node"){
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    public void DrawFinalLine(GameObject s1, GameObject s2){
        
        newLine = new GameObject();
        lineRender = newLine.AddComponent<LineRenderer>();
        lineRender.startWidth = lineWidth;
        lineRender.endWidth = lineWidth;
        lineRender.material = new Material(Shader.Find("Sprites/Default"));
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.red;
        lineRender.positionCount = 2;
        lineRender.SetPositions(new Vector3[] {s1.transform.position, s2.transform.position});


    }
    
    
}
