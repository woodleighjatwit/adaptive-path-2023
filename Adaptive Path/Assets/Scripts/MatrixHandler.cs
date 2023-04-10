using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;

public class Vertex{
    public Vector3 displacement;
    public GameObject nodeObject;
    public Vector3 position;
}

public class Edge{
    public Vertex vert1;
    public Vertex vert2;
    public int length;
}

public class MatrixHandler : MonoBehaviour
{
    public int[,] adjMatrix;
    public int[,] actualMatrix; 
    [SerializeField] private GameHandler gameHandler;
    // Start is called before the first frame update
    void Awake()
    {
        // numbers represent length between nodes 
        // row/col are otherway common representation of adjacency matrices 

        //adjMatrix = new int[4, 4] {{0, 4, 0, 10}, {4, 0, 8, 0}, {0, 8, 0, 4}, {10, 0, 4, 0}};
        adjMatrix = new int[6, 6] { { 0, 6, 18, 16, 10, 8 }, { 6, 0, 10, 12, 10, 0 }, { 18, 10, 0, 18, 10, 16 }, { 16, 12, 18, 0, 8, 8 }, { 10, 10, 10, 8, 0, 2 }, { 8, 0, 16, 8, 2, 0 } };
        

    }

    public IEnumerator FruchtermanReingold(List<GameObject> nodeList, int maxIterations, float initialTemp, float coolingFactor, float x, float y, float z, float time){
        int iteration = 1;
        float averageLength = findAverageLength(adjMatrix);
        Debug.Log("Average length: " + averageLength);
        float temp;
        float newError = 0;
        float oldError = 0;


        List<Vertex> vertexList = new List<Vertex>();
        List<Edge> edgeList = new List<Edge>();

        // creates a list of all vertices 
        for(int i=0; i<nodeList.Count; i++){
            Vertex v = new Vertex();
            v.nodeObject = nodeList[i];
            vertexList.Add(v);
        }

        // create a list of all edges
        for (int i=0; i<adjMatrix.GetLength(0)-1; i++){
            for (int t=i+1; t<adjMatrix.GetLength(1); t++){
                if(adjMatrix[i, t] != 0 && i < t){
                    Edge e = new Edge();
                    e.vert1 = vertexList[i];
                    e.vert2 = vertexList[t];
                    e.length = adjMatrix[i, t];
                    edgeList.Add(e);
                }               
            }
        }

        temp = findError(edgeList);

        while (iteration < maxIterations && temp > 0.00001){
            repulsiveForce(vertexList, adjMatrix, averageLength);   // compute repulsive forces 
            attractiveForce(edgeList); // compute attractive forces 
            vertexPlacement(vertexList, temp, x, y, z); // add displacement to vertices 
            oldError = newError;
             
            
            if ( iteration != 1){
                newError = findError(edgeList); // find current cost/energy of system
                temp = simulatedAnnealing(temp, coolingFactor, newError - oldError, vertexList);  // add cooling to temperature 
            }
            iteration++;
            yield return new WaitForSeconds(time/maxIterations); // add delay for animation 


        }
        
        // displace distance 
        foreach(Edge e in edgeList){
            Debug.Log(e.vert1.nodeObject.name + " and " + e.vert2.nodeObject.name + " are distance " + Vector3.Distance(e.vert1.nodeObject.transform.position, e.vert2.nodeObject.transform.position));
        }
        Debug.Log(findError(edgeList));





    }

    // Calculates a repulsive force on every node base on their distance from every other node.
    private void repulsiveForce(List<Vertex> vertexList, int[,] adjMatrix, float averageLength){
        Vector3 delta;
        float k;
        float mag;
        for(int i=0; i<vertexList.Count; i++){

            // since repulsive force is caculated first reset all vertex displacement values
            vertexList[i].displacement = new Vector3(0, 0 ,0);
            
            for(int t=0; t<vertexList.Count; t++){
                if (i != t){
                    delta = vertexList[i].nodeObject.transform.position - vertexList[t].nodeObject.transform.position; // change in position between  i and t vertex
                    mag = Vector3.Magnitude(delta);
                    // get length to use
                    if (adjMatrix[i, t] == 0){

                        /*
                        if (Mathf.Abs(mag) > averageLength){
                            k = 0;
                        }
                        else{
                            k = averageLength;
                        }
                        */
                        k = averageLength*3*(1/mag);
                    }
                    else{
                        k = adjMatrix[i, t];

                    }
                    vertexList[i].displacement += (delta / mag) * ((k * k) / mag);  // equation used by FR to determine repuslive force

                }
            }
        }
    }

    // Calculates an attractive force on every vertice pair (edges) that pulls them together.
    private void attractiveForce(List<Edge> edgeList){
        Vector3 delta;
        float mag;

        for(int i=0; i<edgeList.Count; i++){
            delta = edgeList[i].vert1.nodeObject.transform.position - edgeList[i].vert2.nodeObject.transform.position;  // change vector 
            mag = Vector3.Magnitude(delta); 

            // FRs two equations 
            edgeList[i].vert1.displacement += -(delta / mag) * ((mag * mag) / edgeList[i].length);
            edgeList[i].vert2.displacement += (delta / mag) * ((mag * mag) / edgeList[i].length);
        }
    }

    // Uses displacement/calculated forces to displace vertices 
    private void vertexPlacement(List<Vertex> vertexList, float temp, float x, float y, float z){
        float mag;

        foreach(Vertex v in vertexList){
            mag = Vector3.Magnitude(v.displacement);

            //limits change based with temp
            Vector3 pos = v.nodeObject.transform.position + ((v.displacement / mag) * Mathf.Min(mag, temp)); // FR equation

            // limiting change to within +/- x/2, y/2, z/2 distance (or within frame)
            pos[0] = Mathf.Min(x/2, Mathf.Max(-x/2, pos[0]));
            pos[1] = Mathf.Min(y/2, Mathf.Max(-y/2, pos[1]));
            pos[2] = Mathf.Min(z/2, Mathf.Max(-z/2, pos[2]));
            
            v.position = pos; 
            
        }
    }

    // Calculates new room temperature with cooling factor. Used function because there are different variants of cooling functions
    private float simulatedAnnealing(float temp, float coolingFactor, float deltaError, List<Vertex> vertexList){

        if (deltaError < 0){
            setPosition(vertexList);
            return temp * coolingFactor;
        }
        else if(Mathf.Exp(deltaError / temp) > UnityEngine.Random.Range(0f, 1f))
        {
            setPosition(vertexList);
            return temp * coolingFactor;
        }
        else{
            return temp;
        }
       
    }
    // keep track of change between two iterations 
    // minimize change and lower temp if being minimized, raise temp if not
    // find to a certain degree of accuracy or max iterations to account for impossible placement 
    //
    public int getSize()
    {
        return adjMatrix.GetLength(0);
    }

    private float findAverageLength(int[,] adjMatrix){
        float length = 0;
        int num = 0;
        for (int i=0; i<adjMatrix.GetLength(0); i++){
            for (int t=0; t<adjMatrix.GetLength(1); t++){
                if (adjMatrix[i, t] != 0){
                    length += adjMatrix[i, t];
                    num++;
                }
            }
        }
        return length/(num);
    }

    public float findError(List<Edge> edgeList){
        float error = 0;
        foreach (Edge e in edgeList) {
            error += Math.Abs(Vector3.Distance(e.vert1.nodeObject.transform.position, e.vert2.nodeObject.transform.position) - e.length);
        }
        return error;
    }

    public void setPosition(List<Vertex> vertexList)
    {
        foreach(Vertex v in vertexList) {
            v.nodeObject.transform.position = v.position;

        }
    }

    public IEnumerator centerNodes(List<GameObject> nodeList)
    {
        Debug.Log("STARTING");
        Vector3 avgVector = new Vector3(0, 0, 0);
        foreach(GameObject node in nodeList)
        {
            avgVector += node.transform.position;

        }
        avgVector = avgVector / nodeList.Count;

        Debug.Log(avgVector);
        for (int i=0; i<100; i++)
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
        yield return new WaitForSeconds(1);
        for (int i = 0; i < gameHandler.lineObjects.Count; i++)
        {
            Destroy(gameHandler.lineObjects[i]);
        }
        List<GameObject> nodes = nodeList;
        List<Vector3> pos = new List<Vector3>(); 
        foreach(GameObject node in nodeList )
        {
            pos.Add(node.transform.position);
        }
        for (int i=0; i<101; i++)
        {
            if (i != 0)
            { 
                for (int j=0; j <nodes.Count;j++)
                {

                    nodeList[j].transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);
                    nodeList[j].transform.position -= (pos[j] / 100);


                }

            }

            yield return new WaitForSeconds(0.001f);

        }
    }
}
