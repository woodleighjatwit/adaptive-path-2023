using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{   
    public List<GameObject> nodeObjects;
    public int nodeCount;
    public GameObject nodePrefab;
    [SerializeField] private MatrixHandler matrixHandler; 

    // Start is called before the first frame update
    void Start()
    {  
        GenerateSpheres();
        /*
         for (int i=0; i<nodeObjects.Count-1; i++){
            Debug.Log("node " + i + " pos: " + nodeObjects[i].transform.position);
         }
        
        */
        matrixHandler.FruchtermanReingold(nodeObjects, 1000, 5.0f, 0.99f, 30f, 30f, 30f);
        for (int i = 0; i < nodeObjects.Count - 1; i++)
        {
            for (int t = i + 1; t < nodeObjects.Count; t++)
            {
                Debug.Log("Dist" + i + " from " + t + ": " + Vector3.Distance(nodeObjects[i].transform.position, nodeObjects[t].transform.position));
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GenerateSpheres(){
        for(int i=0; i<nodeCount; i++){
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
