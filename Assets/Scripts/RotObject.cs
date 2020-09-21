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

    void Start()
    {
        isRotate = false;
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

            transform.Rotate(((axisOfRotate == AxisOfRotate.X) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Y) ? (rot.x) : (0)), 
                ((axisOfRotate == AxisOfRotate.Z) ? (rot.x) : (0)));

            rotateObj.Rotate(((axisOfRotate == AxisOfRotate.X) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Y) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Z) ? (rot.x) : (0)));

            // 마우스를 떼면 더 이상 움직이지 않음
            if (Input.GetMouseButtonUp(0))
            {
                isRotate = false;
            }
        }
        
        foreach (PathCondition pathCube in pathCubes)
        {
            foreach (SinglePath singlePath in pathCube.path)
            {
                float angle = 0;
                switch (axisOfRotate)
                {
                    case AxisOfRotate.X: angle = transform.eulerAngles.x; break;
                    case AxisOfRotate.Y: angle = transform.eulerAngles.y; break;
                    case AxisOfRotate.Z: angle = transform.eulerAngles.z; break;
                }

                if (angle < 0) angle = 360 + angle;

                

                singlePath.block.possiblePaths[singlePath.index - 1].active = (transform.eulerAngles.Equals(pathCube.angle));
            }
        }
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