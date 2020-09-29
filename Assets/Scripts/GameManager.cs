using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Space]
    public Transform clearCube;

    [Space]
    public float clearTime;
    float elapseTime;

    private bool isClear;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            elapseTime = 0;
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
        if(isClear)
        {
            elapseTime += Time.deltaTime;

            if(elapseTime >= clearTime)
            {
                ChangeIntroScene();
            }
        }
    }

    public bool Clear
    {
        get { return isClear; }
        set { isClear = value; }
    }
}