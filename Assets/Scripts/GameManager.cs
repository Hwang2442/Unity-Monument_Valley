using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Space]
    public Transform clearCube;

    [Space]
    public float clearTime;
    float elapseTime;

    public Image fadeImg;

    private bool isClear;
    private bool isReady;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        fadeImg.gameObject.SetActive(true);

        fadeImg.color = new Color(0, 0, 0, 1.0f);

        StartCoroutine(FadeIn(0.5f));

        isClear = isReady = false;
    }

    void Update()
    {
        //if (!isReady)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        StartCoroutine(FadeIn(0));
        //    }
        //}

        if (isClear)
        {
            elapseTime += Time.deltaTime;

            if(elapseTime >= clearTime)
            {
                
            }
        }
    }

    IEnumerator FadeIn(float endAlpha)
    {
        Color fadeColor = fadeImg.color;

        while (fadeImg.color.a > endAlpha)
        {
            fadeColor.a = Mathf.Lerp(fadeImg.color.a, endAlpha, Time.deltaTime * 0.5f);

            fadeImg.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        fadeImg.color = fadeColor;

        yield return null;
    }

    IEnumerator FadeOut(float endAlpha)
    {
        Color fadeColor = fadeImg.color;

        while (fadeImg.color.a < endAlpha)
        {
            fadeColor.a = Mathf.Lerp(fadeImg.color.a, endAlpha, Time.deltaTime * 1.0f);

            fadeImg.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        fadeImg.color = fadeColor;

        yield return null;
    }

    public bool Clear
    {
        get { return isClear; }
        set { isClear = value; }
    }

    public bool Ready
    { 
        get { return isReady; }
        set { isReady = value; }
    }
}