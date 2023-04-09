using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour { 

    // Start is called before the first frame update
    private RaycastHit hit;
    private GameObject newLine;
    private LineRenderer lineRender;
    private GameObject firstObject;
    private GameObject secondObject;
    public float timerDelay;
    private float timer;
    public float lineWidth;
    [SerializeField] private MatrixHandler matrixHandler;
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private Material lineMat;
    public class Line
    {
        Vector3 pos1;
        Vector3 pos2;

    }
    
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if left mouse click down
        if (Input.GetMouseButtonDown(0))
        {
            firstObject = MouseOverObject();
            Debug.Log(firstObject);

        }
        // if left mouse click up
        else if (Input.GetMouseButtonUp(0))
        {
            firstObject = null;
            secondObject = null;
            Debug.Log(firstObject);
            Debug.Log(secondObject);

        }
        // if drag
        if (Input.GetMouseButton(0))
        {
            if (firstObject)
            {
                secondObject = MouseOverObject();
                if (secondObject && secondObject != firstObject)
                {
                    drawTestLine(firstObject, secondObject);
                    firstObject = secondObject;
                    secondObject = null;
                    Debug.Log(firstObject);
                    Debug.Log(secondObject);

                }

            }

        }

    }

    private GameObject MouseOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 150f))
        {

            if (hit.transform.tag == "Node")
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    public void drawTestLine(GameObject s1, GameObject s2)
    {

        newLine = new GameObject();
        lineRender = newLine.AddComponent<LineRenderer>();
        lineRender.startWidth = lineWidth;
        lineRender.endWidth = lineWidth;
        lineRender.material = lineMat;
        
        lineRender.positionCount = 2;
        lineRender.SetPositions(new Vector3[] { s1.transform.position, s2.transform.position });


    }
    public void drawConnectionLines()
    {
        int[,] adjMatrix = matrixHandler.adjMatrix;
        for (int i = 0; i < adjMatrix.GetLength(0) - 1; i++)
        {
            for (int t = i + 1; t < adjMatrix.GetLength(1); t++)
            {
                if (adjMatrix[i, t] != 0 && i < t)
                {
                    if (adjMatrix[i, t] == adjMatrix[t, i])
                    {
                        //lineCreator(gameHandler.nodeObjects[i], gameHandler.nodeObjects[t], adjMatrix[i, t], arrowPrefab);
                    }
                    else
                    {
                        //lineCreator(gameHandler.nodeObjects[i], gameHandler.nodeObjects[t], adjMatrix[i, t], arrowPrefab);

                    }
                }
            }
        }
    }
    public void lineCreator(GameObject startObj, GameObject endObj, int distance, Color c)
    {
        GameObject line = new GameObject();
        List<GameObject> lines = new List<GameObject>();
        Vector3 pos = startObj.transform.position;
        Vector3 iterationDistance = (endObj.transform.localPosition - startObj.transform.localPosition) / distance;

        for (int i = 0; i < distance; i++)
        {
            newLine = new GameObject();
            lineRender = newLine.AddComponent<LineRenderer>();
            lineRender.startWidth = lineWidth;
            lineRender.endWidth = lineWidth;
            lineRender.material = new Material(Shader.Find("Sprites/Default"));
            lineRender.startColor = c;
            lineRender.endColor = c;
            lineRender.positionCount = 2;
            //lineRender.SetPositions(new Vector3[] { s1.transform.position, s2.transform.position });
        }

    }
    /*
    public void lineCreator(GameObject startObj, GameObject endObj, int distance, GameObject prefab)
    {
        GameObject line = new GameObject();
        List<GameObject> arrows = new List<GameObject>();
        Vector3 pos = startObj.transform.position;
        Vector3 iterationDistance = (endObj.transform.localPosition - startObj.transform.localPosition) / distance;
        for(int i=0; i < distance; i++)
        { 
            var arrow = Instantiate(prefab, pos, Quaternion.RotateTowards(startObj.transform.localRotation, endObj.transform.localRotation, 5));
            pos += iterationDistance;
            arrow.transform.SetParent(line.transform); 
        }

    }
    */
}

