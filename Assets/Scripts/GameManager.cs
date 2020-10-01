using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Space]
    public Transform clearCube;

    [Space]
    public float clearTime;
    float elapseTime;

    public Image fadeImg;
    public Transform text;

    private bool isClear;
    private bool isReady;

    private void Awake()
    {
        Transform tr;

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        fadeImg.gameObject.SetActive(true);

        fadeImg.color = new Color(0, 0, 0, 1.0f);

        StartCoroutine(FadeOut(fadeImg, 0.5f));

        //for (int i = 0; i < text.childCount; i++)
        //{
        //    text.GetChild(i).GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, 0);
        //    StartCoroutine(FadeOut(text.GetChild(i).GetComponent<Text>(), 1.0f));
        //}

        //StartCoroutine(FadeOut(text.GetChild(0).GetComponent<Text>(), 1.0f));

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

    IEnumerator FadeIn(Image target, float endAlpha)
    {
        Color fadeColor = target.color;

        while (target.color.a < endAlpha)
        {
            fadeColor.a = Mathf.Lerp(fadeImg.color.a, endAlpha, Time.deltaTime * 0.5f);

            target.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

        yield return null;
    }

    IEnumerator FadeIn(Text target, float endAlpha)
    {
        Color fadeColor = target.color;

        while (target.color.a < endAlpha)
        {
            fadeColor.a = Mathf.Lerp(fadeImg.color.a, endAlpha, Time.deltaTime * 0.5f);

            target.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

        yield return null;
    }

    IEnumerator FadeOut(Image target, float endAlpha)
    {
        Color fadeColor = target.color;

        while (target.color.a > endAlpha)
        {
            fadeColor.a = Mathf.Lerp(fadeImg.color.a, endAlpha, Time.deltaTime * 0.5f);

            target.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

        Debug.Log("ok");

        yield return null;
    }

    IEnumerator FadeOut(Text target, float endAlpha)
    {
        Color fadeColor = target.color;

        while (target.color.a > endAlpha)
        {
            fadeColor.a = Mathf.Lerp(fadeImg.color.a, endAlpha, Time.deltaTime * 0.5f);

            target.color = fadeColor;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

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