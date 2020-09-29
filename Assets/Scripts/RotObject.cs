using DG.Tweening;
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
    public float targetAngle;

    void Start()
    {
        isRotate = false;
        targetAngle = 0;
    }

    void Update()
    {
        // 왼쪽 버튼을 클릭하면
        if(Input.GetMouseButtonDown(0))
        {
            // 마우스 포인트 기준 레이캐스트 생성
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            // 레이 발사!!
            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                // 충돌한 오브젝트가 동일하면
                if(mouseHit.transform.gameObject.Equals(gameObject))
                {
                    isRotate = true;
                }
            }
        }

        // 마우스 클릭 중인 상태로 레버와 오브젝트 회전
        if (isRotate)
        {
            Vector2 rot = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            Debug.Log(rot);

            rot *= rotSpeed;

            setAngle(rot.x);

            // 마우스를 떼면 더 이상 움직이지 않음
            if (Input.GetMouseButtonUp(0))
            {
                isRotate = false;

                float currentAngle = getAngle();                

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

                        Vector3 setAngle = new Vector3((axisOfRotate == AxisOfRotate.X) ? (targetAngle - currentAngle) : 0,
                            (axisOfRotate == AxisOfRotate.Y) ? (targetAngle - currentAngle) : 0,
                            (axisOfRotate == AxisOfRotate.Z) ? (targetAngle - currentAngle) : 0);

                        transform.DORotate(setAngle, 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack);
                        rotateObj.DORotate(setAngle, 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack);

                        break;
                    }
                }
            }
        }
        else
        {
            //float currentAngle = getAngle();

            //if (Mathf.Abs(targetAngle - currentAngle) > rotSpeed)
            //{
            //    setAngle(currentAngle + rotSpeed * Time.deltaTime);
            //}
            //else if (Mathf.Abs(targetAngle - currentAngle) <= rotSpeed)
            //{
            //    setAngle(targetAngle);
            //}
        }

        for (int i = 0; i < pathCubes.Count; i++)
        {
            for (int j = 0; j < pathCubes[i].path.Count; j++)
            {
                pathCubes[i].path[j].block.possiblePaths[pathCubes[i].path[j].index - 1].active =
                    transform.eulerAngles.Equals(pathCubes[i].angle);
            }
        }
    }

    private void setAngle(float angle)
    {
        transform.Rotate(((axisOfRotate == AxisOfRotate.X) ? (angle) : (0)),
                ((axisOfRotate == AxisOfRotate.Y) ? (angle) : (0)),
                ((axisOfRotate == AxisOfRotate.Z) ? (angle) : (0)));

        rotateObj.Rotate(((axisOfRotate == AxisOfRotate.X) ? (angle) : (0)),
            ((axisOfRotate == AxisOfRotate.Y) ? (angle) : (0)),
            ((axisOfRotate == AxisOfRotate.Z) ? (angle) : (0)));
    }

    private float getAngle()
    {
        float answer = 0;

        switch (axisOfRotate)
        {
            case AxisOfRotate.X: answer = transform.eulerAngles.x; break;
            case AxisOfRotate.Y: answer = transform.eulerAngles.y; break;
            case AxisOfRotate.Z: answer = transform.eulerAngles.z; break;
        }

        return answer;
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