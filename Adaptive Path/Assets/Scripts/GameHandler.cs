using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class GameHandler : MonoBehaviour
{   
    public List<GameObject> nodeObjects;
    public List<Line> lines = new List<Line>();
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
        // parameters might need adjusting
        yield return StartCoroutine(matrixHandler.FruchtermanReingold(nodeObjects, matrixHandler.adjMatrix.GetLength(0)*200, matrixHandler.adjMatrix.GetLength(0) * 10f, 0.95f, matrixHandler.adjMatrix.GetLength(0)*10f, matrixHandler.adjMatrix.GetLength(0) * 10f, matrixHandler.adjMatrix.GetLength(0) * 10f, 5f));
        //yield return StartCoroutine(centerNodes(nodeObjects));
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
        int val = matrixHandler.adjMatrix.GetLength(0);
        for(int i=0; i<matrixHandler.getSize(); i++){
            nodeObjects.Add(Instantiate(nodePrefab, generateCords(-1f * val, 1f * val, -1f * val, 1f * val, -1f * val, 1f * val), Quaternion.identity));
            nodeObjects[i].name = "Node " + i.ToString();
            nodeObjects[i].GetComponent<Node>().nodeIndex = i;
            nodeObjects[i].GetComponent<Node>().position = nodeObjects[i].transform.position;
        }
    }

    public Vector3 generateCords(float min_x, float max_x, float min_y, float max_y, float min_z, float max_z){
        float x = Random.Range(min_x, max_x);
        float y = Random.Range(min_y, max_y);
        float z = Random.Range(min_z, max_z);
        return new Vector3(x, y, z);
    }


    public void checkGameFinish()
    {
        bool win = true;
        for(int i=0; i<nodeObjects.Count; i++)
        {
            if (!nodeObjects[i].GetComponent<Node>().active ) {
                win = false;
                break;
            }
        }
        gameEnd(win);
        


    }
    
    public void gameEnd(bool win)
    {
        isPaused = true;
        if (!win)
        
        {
            for (int i = 0; i < nodeObjects.Count; i++)
            {
                nodeObjects[i].GetComponent<Node>().setEndCore();
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].drawnLine != null)
                {
                    lines[i].drawnLine.GetComponent<LineRenderer>().material = failLineMat;
                }

            }
        }
        StartCoroutine(moveNodesToCenter(nodeObjects));
    }
    public IEnumerator centerNodes(List<GameObject> nodeList)
    {
        Vector3 avgVector = new Vector3(0, 0, 0);

        foreach (GameObject node in nodeList)
        {
            avgVector += node.transform.position;

        }
        avgVector = avgVector / nodeList.Count;

        Debug.Log(avgVector);
        for (int i = 0; i < 100; i++)
        {

            foreach (GameObject node in nodeList)
            {
                node.transform.position += (Vector3.zero - avgVector) / 100;
            }

            yield return new WaitForSeconds(0.001f);

        }


    }

    public IEnumerator moveNodesToCenter(List<GameObject> nodeList)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].destroyDirectionLine();

        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].destroyDrawnLine();

        }

        List<GameObject> nodes = nodeList;
        List<Vector3> pos = new List<Vector3>();
        Vector3 center = Vector3.zero;
        foreach (GameObject node in nodeList)
        {
            pos.Add(node.transform.position);
            center += node.transform.position;
        }
        int segements = 100;
        center = center / nodes.Count;
        for (int i = 0; i < segements; i++)
        {
            for (int j = 0; j < nodes.Count; j++)
            {

                nodeList[j].transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);
                nodeList[j].transform.position -= (pos[j] - center) / segements;


            }

            yield return new WaitForSeconds(0.001f);

        }
    }

}
