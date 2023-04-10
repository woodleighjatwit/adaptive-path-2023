using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameHandler : MonoBehaviour
{   
    public List<GameObject> nodeObjects;
    public List<GameObject> lineObjects;
    public int nodeCount;
    public GameObject nodePrefab;
    [SerializeField] private MatrixHandler matrixHandler;
    [SerializeField] private Drawer drawer;
    public bool isPaused = true;
    [SerializeField] private Material failLineMat;
    public static bool gameOver = false;


    IEnumerator Start()
    {
        GenerateNodes();
        yield return StartCoroutine(matrixHandler.FruchtermanReingold(nodeObjects, 1200, 30.0f, 0.95f, 60f, 60f, 60f, 5f));
        yield return StartCoroutine(matrixHandler.centerNodes(nodeObjects));
        drawer.drawConnectionLines();
        drawer.selectedNode = nodeObjects[0];
        nodeObjects[0].GetComponent<Node>().toggleCore(true);
        nodeObjects[nodeObjects.Count-1].GetComponent<Node>().setEndCore();

        isPaused = false;
   

    }

    private void Update()
    {
        
    }

    private void GenerateNodes(){
        for(int i=0; i<matrixHandler.getSize(); i++){
            nodeObjects.Add(Instantiate(nodePrefab, generateCords(-10, 10, -10, 10, -10, 10), Quaternion.identity));
            nodeObjects[i].name = "Node " + i.ToString();
            nodeObjects[i].GetComponent<Node>().nodeIndex = i;
            nodeObjects[i].GetComponent<Node>().position = nodeObjects[i].transform.position;
        }
    }

    public Vector3 generateCords(int min_x, int max_x, int min_y, int max_y, int min_z, int max_z){
        float x = Random.Range(min_x, max_x);
        float y = Random.Range(min_y, max_y);
        float z = Random.Range(min_z, max_z);
        return new Vector3(x, y, z);
    }


    public void checkGameFinish()
    {

        for(int i=0; i<nodeObjects.Count; i++)
        {
            if (!nodeObjects[i].GetComponent<Node>().active ) {
                failedEnd();
                break;
            }
        }
        isPaused = true;
        
        StartCoroutine(matrixHandler.moveNodesToCenter(nodeObjects));
        


    }
    
    public void failedEnd()
    {
        for (int i = 0; i < nodeObjects.Count; i++)
        {
            nodeObjects[i].GetComponent<Node>().setEndCore();
        }

        for (int i = 0; i < lineObjects.Count; i++)
        {
            lineObjects[i].GetComponent<LineRenderer>().material = failLineMat;
        }
        isPaused = true;
        
    }

    
}
