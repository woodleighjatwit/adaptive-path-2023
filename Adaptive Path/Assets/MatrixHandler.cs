using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Vertex{
    public Vector3 displacement;
    public GameObject nodeObject;
}

public class Edge{
    public Vertex vert1;
    public Vertex vert2;
    public int length;
}

public class MatrixHandler : MonoBehaviour
{
    public static int[,] adjMatrix;
    // Start is called before the first frame update
    void Start()
    {
        //adjMatrix = new int[4, 4] {{0, 4, 0, 10}, {4, 0, 8, 0}, {0, 8, 0, 4}, {10, 0, 4, 0}};
        adjMatrix = new int[6, 6] { { 0, 3, 9, 8, 5, 4 }, { 3, 0, 5, 6, 5, 0 }, { 9, 5, 0, 9, 5, 8, }, { 8, 6, 9, 0, 4, 5 }, { 5, 5, 5, 4, 0, 1 }, { 4, 0, 8, 5, 1, 0 } };
        
    }

    public IEnumerator FruchtermanReingold(List<GameObject> nodeList, int maxIterations, float initialTemp, float coolingFactor, float x, float y, float z, float time){
        int iteration = 1;
        float averageLength = findAverageLength(adjMatrix);
        Debug.Log("Average length: " + averageLength);
        float temp = initialTemp;


        List<Vertex> vertexList = new List<Vertex>();
        List<Edge> edgeList = new List<Edge>();

        for(int i=0; i<nodeList.Count; i++){
            Vertex v = new Vertex();
            v.nodeObject = nodeList[i];
            vertexList.Add(v);
        }

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

        while (iteration < maxIterations && temp > 0){
            repulsiveForce(vertexList, adjMatrix, averageLength);
            attractiveForce(edgeList);
            vertexPlacement(vertexList, temp, x, y, z);
            temp = temperature(temp, coolingFactor, iteration);
            iteration++;
            yield return new WaitForSeconds(time/maxIterations);


        }
        
        foreach(Edge e in edgeList){
            Debug.Log(e.vert1.nodeObject.name + " and " + e.vert2.nodeObject.name + " are distance " + Vector3.Distance(e.vert1.nodeObject.transform.position, e.vert2.nodeObject.transform.position));
        }
        

    }

    private void repulsiveForce(List<Vertex> vertexList, int[,] adjMatrix, float averageLength){
        Vector3 delta;
        float k;
        float mag;
        for(int i=0; i<vertexList.Count; i++){
            vertexList[i].displacement = new Vector3(0, 0 ,0);
            
            for(int t=0; t<vertexList.Count; t++){
                if (i != t){
                    if(adjMatrix[i, t] == 0){
                        k = averageLength;
                    }
                    else{
                        k = adjMatrix[i, t];
                    }
                    delta = vertexList[i].nodeObject.transform.position - vertexList[t].nodeObject.transform.position;
                    mag = Vector3.Magnitude(delta);
                    vertexList[i].displacement += (delta / mag) * ((k * k) / mag);
                }
            }
        }
    }

    private void attractiveForce(List<Edge> edgeList){
        Vector3 delta;
        float mag;
        for(int i=0; i<edgeList.Count; i++){
           delta = edgeList[i].vert1.nodeObject.transform.position - edgeList[i].vert2.nodeObject.transform.position;
           mag = Vector3.Magnitude(delta);
           edgeList[i].vert1.displacement += -(delta / mag) * ((mag * mag) / edgeList[i].length);
           edgeList[i].vert2.displacement += (delta / mag) * ((mag * mag) / edgeList[i].length);
        }
    }

    private void vertexPlacement(List<Vertex> vertexList, float temp, float x, float y, float z){
       // Vector3 pos;
        float mag;

        foreach(Vertex v in vertexList){
            mag = Vector3.Magnitude(v.displacement);
            Vector3 pos = v.nodeObject.transform.position + ((v.displacement / mag) * Mathf.Min(mag, temp));
            pos[0] = Mathf.Min(x/2, Mathf.Max(-x/2, pos[0]));
            pos[1] = Mathf.Min(y/2, Mathf.Max(-y/2, pos[1]));
            pos[2] = Mathf.Min(z/2, Mathf.Max(-z/2, pos[2]));
            
            v.nodeObject.transform.position = pos;
            
        }
    }

    private float temperature(float temp, float coolingFactor, int iteration){
       return temp*coolingFactor;
    }

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
}
