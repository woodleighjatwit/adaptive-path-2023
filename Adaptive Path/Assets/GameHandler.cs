using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{   
    public List<GameObject> sphereList;
    public int sphereCount;
    public GameObject sphere;
    // Start is called before the first frame update
    void Start()
    {
        GenerateSpheres();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GenerateSpheres(){
        for(int i=0; i<sphereCount; i++){
            sphereList.Add(Instantiate(sphere, generateCords(-5, 5, -5, 5), Quaternion.identity));
            sphereList[i].name = "Node " + i.ToString();
        }
    }

    public Vector3 generateCords(int min_x, int max_x, int min_y, int max_y){
        Vector3 cord;
        do{
            cord = new Vector3(Random.Range(min_x, max_x), 0, Random.Range(min_y, max_y));
        }while((-1 < cord[0] && cord[0] < 1) && ((-1 < cord[2] && cord[2] < 1)));

        return cord;
    }
}
