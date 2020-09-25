using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public void RoadStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void RoadStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

}
