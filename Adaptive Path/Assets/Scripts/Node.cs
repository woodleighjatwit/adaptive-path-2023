using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;
    public int nodeIndex;
    [SerializeField] private Material onMat;
    [SerializeField] private Material offMat;
    [SerializeField] private Material endCore;
    [SerializeField] private GameObject core;
    public bool active = false;
    public bool isEndCore = false;

    public Node()
    {

    }

    public void setPosition(Vector3 pos)
    {
        position = pos;
        transform.position = pos;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public void toggleCore(bool turnOn)
    {
        if (turnOn)
        {
            core.GetComponent<MeshRenderer>().material = onMat;
            active = true;
        }
        else
        {
            core.GetComponent<MeshRenderer>().material = offMat;
            active = false;
        }
    }

    public void setEndCore()
    {
        core.GetComponent<MeshRenderer>().material = endCore;
        isEndCore = true;
    }

}
