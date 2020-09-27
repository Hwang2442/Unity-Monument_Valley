using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform clearCube;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // 스테이지 다시시작
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 인트로 씬으로 돌아감
    public void ChangeIntroScene()
    {
        SceneManager.LoadScene("Intro");
    }

    void Update()
    {
        
    }
}
