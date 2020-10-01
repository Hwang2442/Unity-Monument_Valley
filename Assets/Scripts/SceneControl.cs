﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void RoadStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void RoadStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}