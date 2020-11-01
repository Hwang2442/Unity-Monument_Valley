using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotObject : MonoBehaviour
{
    public enum AxisOfRotate
    {
        X, Y, Z
    }

    [Header("Rotate Object")]
    public Transform rotateObj;
    public List<PathCondition> pathCubes = new List<PathCondition>();

    [Space]
    public AxisOfRotate axisOfRotate;

    [Space]
    public float rotSpeed;

    bool isRotate;
    float pastAngle;
    int soundNum;

    void Start()
    {
        isRotate = false;
        soundNum = 1;
    }

    void Update()
    {
        // 왼쪽 버튼을 클릭하면 && 게임매니저 플래그 체크
        if(Input.GetMouseButtonDown(0) && GameManager.instance.Ready)
        {
            // 마우스 포인트 기준 레이캐스트 생성
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            // 레이 발사!!
            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                // 충돌한 오브젝트가 레버인지 검사
                if (mouseHit.transform == transform)
                {
                    isRotate = true;

                    pastAngle = getAngle();

                    soundNum = 0;
                }
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (mouseHit.transform == transform.GetChild(i))
                        {
                            isRotate = true;

                            pastAngle = getAngle();

                            soundNum = 0;

                            break;
                        }
                    }
                }
            }
        }

        // 마우스 클릭 중인 상태로 레버와 오브젝트 회전
        if (isRotate)
        {
            Vector2 rot = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            rot *= rotSpeed;

            // 레버 회전
            transform.Rotate(((axisOfRotate == AxisOfRotate.X) ? (rot.x) : (0)),
                    ((axisOfRotate == AxisOfRotate.Y) ? (rot.x) : (0)),
                    ((axisOfRotate == AxisOfRotate.Z) ? (rot.x) : (0)));

            // 오브젝트 회전
            rotateObj.Rotate(((axisOfRotate == AxisOfRotate.X) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Y) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Z) ? (rot.x) : (0)));

            if (Mathf.Abs(getAngle() - pastAngle) >= 30.0f)
            {
                pastAngle = getAngle();

                SoundManager.instance.play("RotateSound_" + soundNum.ToString());

                soundNum = (soundNum > 5) ? (0) : (soundNum + 1);
            }

            // 각도 확인 후 큐브 경로 설정
            for (int i = 0; i < pathCubes.Count; i++)
            {
                for (int j = 0; j < pathCubes[i].path.Count; j++)
                {
                    pathCubes[i].path[j].block.possiblePaths[pathCubes[i].path[j].index - 1].active =
                        transform.eulerAngles.Equals(pathCubes[i].angle);
                }
            }

            // 마우스를 떼면 더 이상 움직이지 않음
            if (Input.GetMouseButtonUp(0))
            {
                isRotate = false;

                float currentAngle = getAngle();
                float targetAngle;

                // 자동으로 맞출 각도 찾기
                for (int i = 270; i >= 0; i -= 90)
                {
                    if (currentAngle > i)
                    {
                        if (currentAngle > i + 45)
                        {
                            targetAngle = i + 90;
                        }
                        else
                        {
                            targetAngle = i;
                        }

                        // 각도 자동맞춤 시작
                        StartCoroutine(Rotate(currentAngle, targetAngle, true));

                        break;
                    }
                }
            }
        }
    }

    IEnumerator Rotate(float startAngle, float finalAngle, bool outBack)
    {
        float timing = 0;

        // 튕길 각도
        float firstAngle = (finalAngle > startAngle) ? (finalAngle + 12.5f) : (finalAngle - 12.5f);
        // 다음 각도
        float nextAngle = (outBack) ? (firstAngle) : (finalAngle);

        int soundNum = 1;

        // 회전
        while (timing < 1.0f)
        {
            float angle = Mathf.Lerp(startAngle, nextAngle, timing);

            setAngle(angle);

            timing += Time.deltaTime * rotSpeed;

            if (timing >= 1.0f)
            {
                setAngle(nextAngle);

                SoundManager.instance.play("RotateSound_" + soundNum.ToString(), 0.5f);
                soundNum++;

                // 한 번 튕긴 경우
                if (nextAngle != finalAngle)
                {
                    startAngle = nextAngle;
                    nextAngle = finalAngle;
                    timing = 0;
                }
            }

            yield return null;
        }

        // 360도 넘어가는 경우
        setAngle((finalAngle >= 360) ? (finalAngle - 360) : (finalAngle));

        // 각도 확인 후 큐브 경로 설정
        for (int i = 0; i < pathCubes.Count; i++)
        {
            for (int j = 0; j < pathCubes[i].path.Count; j++)
            {
                pathCubes[i].path[j].block.possiblePaths[pathCubes[i].path[j].index - 1].active =
                    transform.eulerAngles.Equals(pathCubes[i].angle);
            }
        }

        yield return null;
    }

    public void autoRotate(float targetAngle, bool outBack)
    {
        StartCoroutine(Rotate(getAngle(), targetAngle, outBack));
    }

    private void setAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            (axisOfRotate == AxisOfRotate.X) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Y) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Z) ? angle : 0));

        rotateObj.rotation = Quaternion.Euler(new Vector3(
            (axisOfRotate == AxisOfRotate.X) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Y) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Z) ? angle : 0));
    }

    private float getAngle()
    {
        float ret = 0;

        switch (axisOfRotate)
        {
            case AxisOfRotate.X: ret = transform.eulerAngles.x; break;
            case AxisOfRotate.Y: ret = transform.eulerAngles.y; break;
            case AxisOfRotate.Z: ret = transform.eulerAngles.z; break;
        }

        return ret;
    }
}

[System.Serializable]
public class PathCondition
{
    public Vector3 angle;
    public List<SinglePath> path;
}

[System.Serializable]
public class SinglePath
{
    public Walkable block;
    public int index;
}