using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameHandler : MonoBehaviour
{   
    public List<GameObject> nodeObjects;
    public int nodeCount;
    public GameObject nodePrefab;
    [SerializeField] private MatrixHandler matrixHandler;
    private static bool makeGraph = true;
    public bool isPaused = true;

    // Start is called before the first frame update
    void Start()
    {  
        GenerateSpheres();
        StartCoroutine(matrixHandler.FruchtermanReingold(nodeObjects, 1200, 30.0f, 0.95f, 60f, 60f, 60f, 0f));
        isPaused = false;
        
        /*
         for (int i=0; i<nodeObjects.Count-1; i++){
            Debug.Log("node " + i + " pos: " + nodeObjects[i].transform.position);
         }
        
        */



    }

    // Update is called once per frame
    void Update()
    {
        if (makeGraph)
        {
            
            makeGraph = false;
        }
    }
    private void GenerateSpheres(){
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
}
