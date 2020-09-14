using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }
}

[System.Serializable]
public class PathCondition
{

}
