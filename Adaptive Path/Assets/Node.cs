using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;
    public int nodeIndex;
 
    // hard lock list

    public Node(){

    }

    public void setPosition(Vector3 pos){
        position = pos;
        transform.position = pos;
    }

    public Vector3 getPosition(){
        return position;
    }



    /*
 public bool isLocked = false;
       public List<Vector3>[,] avaliableSpace; 
    public List<Node> connectedNodes;
    public bool hardLock;
    // gets radius between two points
    public float getRadiusFrom(Vector3 p1, Vector3 p2){
        return Vector3.Distance(p1, p2);
    }

    // gets a point for every degree along a r radius
    public List<Vector3> getPointsOnRadius(int radius, Vector3 position){
        List<Vector3> points = new List<Vector3>(); 
        for (int i=0; i<360; i++){
            points.Add(position + new Vector3(Mathf.Cos(i)*radius, 0, Mathf.Sin(i)*radius));
        }
        return points;
    }


    public List<Vector3> getRadiusPointsOverlap (List<List<Vector3>> listOfLists){
        // Putting all points into a single list
        List<Vector3> allpoints = new List<Vector3>(); 
        foreach(List<Vector3> list in listOfLists){
            allpoints.AddRange(allpoints);
        }
        

        // Creating dictionary to check for matches and if matching place in overlap
        var matchDict = new Dictionary<Vector3, int>(allpoints.Count);
        List<Vector3> overlap = new List<Vector3>(); 

        foreach(Vector3 point in allpoints){
            if(matchDict.ContainsKey(point)){
                if (matchDict[point] == 1){         // For the second occurrence only it will place into overlap 
                    overlap.Add(point);
                }
            }
            else{
                matchDict.Add(point, 1);        // First occurrence will add to matchDict
            }

        }
        return overlap;
    }

    // need function to get overlap instead check for two points that are in radius and x distance apart
    public void getOverLappingPointsWithDistance (List<List<Vector3>> listOfLists){

    }

    // test for hard lock
    public bool testForHardLock(){
        return false;
    }
    */
}
