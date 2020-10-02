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

    [Space]
    public Image fadeImg;
    public List<Text> fadeText = new List<Text>();

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

        for (int i = 0; i < fadeText.Count; i++)
        {
            fadeText[i].color = new Color(1.0f, 1.0f, 1.0f, 0);
            StartCoroutine(FadeIn(fadeText[i], 1.0f, 0.6f));
        }

        isClear = isReady = false;
    }

    void Update()
    {
        if (!isReady)
        {
            if (fadeText[0].color.a == 1.0f && fadeImg.color.a == 1.0f)
            {
                StartCoroutine(FadeOut(fadeImg, 0.35f, 0.4f));
            }

            if (fadeText[0].color.a == 1.0f && fadeImg.color.a == 0.35f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < fadeText.Count; i++)
                    {
                        StartCoroutine(FadeOut(fadeText[i], 0, 0.5f));
                    }

                    StartCoroutine(FadeOut(fadeImg, 0, 0.5f));
                }
            }

            if (fadeText[0].color.a == 0 && fadeImg.color.a == 0)
            {
                isReady = true;
            }
        }
        
        if (isClear && isReady)
        {
            elapseTime += Time.deltaTime;

            if(elapseTime >= clearTime)
            {
                
            }
        }
    }

    // 투명 >> 불투명
    IEnumerator FadeIn(Image target, float endAlpha, float speed)
    {
        Color fadeColor = target.color;
        Color originColor = target.color;

        float timing = 0;

        while (target.color.a < endAlpha)
        {
            fadeColor.a = Mathf.Lerp(originColor.a, endAlpha, timing);

            target.color = fadeColor;

            timing += Time.deltaTime * speed;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

        yield return null;
    }

    // 불투명 >> 투명
    IEnumerator FadeOut(Image target, float endAlpha, float speed)
    {
        Color fadeColor = target.color;
        Color originColor = target.color;

        float timing = 0;

        while (target.color.a > endAlpha)
        {
            fadeColor.a = Mathf.Lerp(originColor.a, endAlpha, timing);

            target.color = fadeColor;

            timing += Time.deltaTime * speed;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

        yield return null;
    }

    // 투명 >> 불투명
    IEnumerator FadeIn(Text target, float endAlpha, float speed)
    {
        Color fadeColor = target.color;
        Color originColor = target.color;

        float timing = 0;

        while (target.color.a < endAlpha)
        {
            fadeColor.a = Mathf.Lerp(originColor.a, endAlpha, timing);

            target.color = fadeColor;

            timing += Time.deltaTime * speed;

            yield return null;
        }

        fadeColor.a = endAlpha;
        target.color = fadeColor;

        yield return null;
    }
    
    // 불투명 >> 투명
    IEnumerator FadeOut(Text target, float endAlpha, float speed)
    {
        Color fadeColor = target.color;
        Color originColor = target.color;

        float timing = 0;

        while (target.color.a > endAlpha)
        {
            fadeColor.a = Mathf.Lerp(originColor.a, endAlpha, timing);

            target.color = fadeColor;

            timing += Time.deltaTime * speed;

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