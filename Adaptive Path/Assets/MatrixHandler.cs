using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixHandler : MonoBehaviour
{
    public static int[,] connectionMatrix;
    public static int[,] lineMatrix;
    // Start is called before the first frame update
    void Start()
    {   
        connectionMatrix = new int[4, 4] {{0, 1, 0, 1}, {1, 0, 1, 0}, {0, 1, 0, 1}, {1, 0, 1, 0}};
        lineMatrix = new int[4, 4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool checkConnection(int s1, int s2){
        return connectionMatrix[s1, s2] == 1;
    }

    public static void createConnection(int s1, int s2){
        lineMatrix[s1, s2] = 1;
        lineMatrix[s2, s1] = 1;
    }

    public static void removeConnection(int s1, int s2){
        lineMatrix[s1, s2] = 0;
        lineMatrix[s2, s1] = 0;
    }
}
