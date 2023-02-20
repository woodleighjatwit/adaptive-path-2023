using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour
{

    public static int level = 1;
    public static int collectedCubes = 0;
    public static bool pause = false;
    public GameObject cube;
    private UIControls uiControls;
    public int min_x, max_x, min_y, max_y;

    // Start is called before the first frame update
    void Start()
    {   
        uiControls = GameObject.Find("GameManager").GetComponent<UIControls>();
        createGame();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    public void createGame(){
        for (int i=0; i<GameControls.level; i++){
            Instantiate(cube, generateCords(min_x, max_x, min_y, max_y), Quaternion.identity);
        }
        Debug.Log(GameControls.level);
        uiControls.UpdateCounter();
    }
    public void reloadGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        uiControls.UpdateCounter();
    }

    public void collisionOccurred(){
        GameControls.collectedCubes += 1;
        uiControls.UpdateCounter();
        if (GameControls.collectedCubes == GameControls.level){
            
            nextLevel();
        }
    }
    public void nextLevel(){
        GameControls.level += 1;
        GameControls.collectedCubes = 0;
        Debug.Log(level);
        reloadGame();
    }
    public Vector3 generateCords(int min_x, int max_x, int min_y, int max_y){
        Vector3 cord;
        do{
            cord = new Vector3(Random.Range(min_x, max_x), 3f, Random.Range(min_y, max_y));
        }while((-1 < cord[0] && cord[0] < 1) && ((-1 < cord[2] && cord[2] < 1)));

        return cord;
    }

    

}
