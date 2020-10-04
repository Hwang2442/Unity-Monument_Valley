using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void RoadStage1()
    {
        SoundManager.instance.play("StageSelect", 0.5f);

        SoundManager.instance.play("Stage1BGM");

        SceneManager.LoadScene("Stage1");
    }

    public void RoadStage2()
    {
        SoundManager.instance.play("StageSelect", 0.5f);

        SoundManager.instance.play("Stage2BGM");

        SceneManager.LoadScene("Stage2");
    }

    public void ReloadScene()
    {
        SoundManager.instance.play(SceneManager.GetActiveScene().name + "BGM");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}
