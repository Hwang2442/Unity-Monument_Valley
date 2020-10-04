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
            // 검은 화면에 글자만 페이드 인
            fadeText[i].color = new Color(1.0f, 1.0f, 1.0f, 0);
            StartCoroutine(FadeIn(fadeText[i], 1.0f, 0.6f));
        }

        isClear = isReady = false;
    }

    void Update()
    {
        if (!isReady)
        {
            // 글자 페이드 인이 완료되었다면
            if (fadeText[0].color.a == 1.0f && fadeImg.color.a == 1.0f)
            {
                // 검은 화면을 페이드 아웃
                StartCoroutine(FadeOut(fadeImg, 0.35f, 0.4f));
            }

            // 글자와 검은 화면이 모두 세팅되어 있다면
            if (fadeText[0].color.a == 1.0f && fadeImg.color.a == 0.35f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < fadeText.Count; i++)
                    {
                        // 글자 페이드 아웃
                        StartCoroutine(FadeOut(fadeText[i], 0, 0.65f));
                    }

                    // 검은 화면 페이드 아웃
                    StartCoroutine(FadeOut(fadeImg, 0, 0.65f));
                }
            }

            // 글자와 검은 화면이 모두 보이지 않는다면
            if (fadeText[0].color.a == 0 && fadeImg.color.a == 0)
            {
                // 게임 컨트롤 가능
                isReady = true;
            }
        }
        
        if (isClear && isReady)
        {
            StartCoroutine(FadeIn(fadeImg, 1.0f, 0.4f));

            if (fadeImg.color.a == 1.0f)
            {
                SoundManager.instance.stop(SceneManager.GetActiveScene().name + "BGM");

                SceneManager.LoadScene("Intro");
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