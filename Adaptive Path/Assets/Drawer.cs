using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Line : MonoBehaviour
{
    public GameObject node1;
    public GameObject node2;
    public int nodeIndex1;
    public int nodeIndex2;
    public List<GameObject> directionLines;
    public GameObject drawnLine;

    public void destroyDirectionLine()
    {
        if(directionLines.Count > 0)
        {
            foreach (GameObject l in directionLines)
            {
                
                Destroy(l);
            }
        }        
        
    }
    public void destroyDrawnLine()
    {
        if (drawnLine != null)
        {
            Destroy(drawnLine);
        }
    }

    
}

public class Drawer : MonoBehaviour { 

    // Start is called before the first frame update
    private RaycastHit hit;
    private GameObject newLine;
    private LineRenderer lineRenderer;
    private GameObject firstObject;
    private GameObject secondObject;
    public float timerDelay;
    private float timer;
    public float lineWidth;
    [SerializeField] private MatrixHandler matrixHandler;
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private Material lineMat;
    public GameObject selectedNode;
    public Texture lineTexture;
    
    
    
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameHandler.isPaused)
        {


            // if left mouse click down
            if (Input.GetMouseButtonDown(0))
            {
                firstObject = MouseOverObject();
                if (firstObject.tag != "Node")
                {
                    firstObject = null;
                }

            }
            // if left mouse click up
            else if (Input.GetMouseButtonUp(0))
            {
                firstObject = null;
                secondObject = null;

            }

            // if drag
            if (Input.GetMouseButton(0))
            {
                if (firstObject != null && firstObject.Equals(selectedNode))
                {
                    secondObject = MouseOverObject();

                    if (secondObject != null && secondObject.tag == "Node" && secondObject != firstObject && firstObject.GetComponent<Node>().active)
                    {
                        if(drawLine(firstObject, secondObject)) {

                            if (secondObject.gameObject.GetComponent<Node>().active)
                            {
                                gameHandler.failedEnd();
                            }
                            else
                            {
                                secondObject.gameObject.GetComponent<Node>().toggleCore(true);
                                if (secondObject.GetComponent<Node>().isEndCore)
                                {
                                    gameHandler.checkGameFinish();
                                }
                                
                            }
                            firstObject = secondObject;
                            selectedNode = secondObject;
                            secondObject = null;
                        }
                    
                        


                    }
                    else
                    {
                        secondObject = null;
                    }

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

    public bool drawLine(GameObject s1, GameObject s2)
    {

        foreach(Line l in gameHandler.lines)
        {
            if((l.node1.Equals(s1) && l.node2.Equals(s2)) || (l.node1.Equals(s2) && l.node2.Equals(s1)))
            {
                newLine = new GameObject();
                lineRenderer = newLine.AddComponent<LineRenderer>();
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
                lineRenderer.material = lineMat;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(new Vector3[] { s1.transform.position, s2.transform.position });
                l.drawnLine = newLine;
                l.destroyDirectionLine();

                return true;
            } 

        }
        return false;
        
       
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
                        createDirectionLine(gameHandler.nodeObjects[i], gameHandler.nodeObjects[t], adjMatrix[i, t], Color.grey, i, t);
                    }
                    else
                    {
                        createDirectionLine(gameHandler.nodeObjects[i], gameHandler.nodeObjects[t], adjMatrix[i, t], Color.grey, i, t);

                    }
                }
            }
        }
    }
    public void createDirectionLine(GameObject startObj, GameObject endObj, int distance, Color c, int index1, int index2)
    {
        Line l = new Line();
        List<GameObject> lines = new List<GameObject>();
        Vector3 pos = startObj.transform.position;
        Vector3 iterationDistance = (endObj.transform.localPosition - startObj.transform.localPosition) / (distance*3 + 7);
        l.nodeIndex1 = index1;
        l.nodeIndex2 = index2;
        l.node1 = startObj;
        l.node2 = endObj;


        pos += iterationDistance*4;
        for (int i = 0; i < distance; i++)
        {

            newLine = new GameObject();
            lineRenderer = newLine.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[] { pos, pos+iterationDistance*2 });
            pos += iterationDistance*3;
            lines.Add(newLine);

        }
        l.directionLines = lines;
        gameHandler.lines.Add(l);
       
    }
  
}

