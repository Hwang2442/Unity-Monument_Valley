using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObject : MonoBehaviour
{

    [Header("Rotate Option")]
    public Transform rotateObj;
    public List<PathCondition> pathCubes = new List<PathCondition>();

    [Space]
    public float rotSpeed;

    bool isRotate;

    public float originAngle;

    // Start is called before the first frame update
    void Start()
    {
        isRotate = false;
    }

    // Update is called once per frame
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

                    //Debug.LogError("오오오");

                    Vector2 mousePos = (Vector2)Input.mousePosition;
                    Vector2 leverPos = (Vector2)transform.position;

                    originAngle = Vector2.Angle(leverPos, mousePos);
                }
            }
        }

        if (isRotate)
        {
            //Vector2 mousePos = (Vector2)Input.mousePosition;
            //Vector2 leverPos = (Vector2)transform.position;

            //float currentAngle = Vector2.Angle(leverPos, mousePos);

            //currentAngle -= originAngle;

            //transform.Rotate(new Vector3(0, 0, currentAngle));
            //rotateObj.Rotate(new Vector3(0, 0, currentAngle));

            float x = Input.GetAxis("Mouse X") * rotSpeed;
            float y = Input.GetAxis("Mouse Y") * rotSpeed;

            transform.Rotate(0, 0, x + y);

            

            if (Input.GetMouseButtonUp(0))
            {
                isRotate = false;
            }
        }
    }
}

[System.Serializable]
public class PathCondition
{
    public Vector3 angle;
    public List<Transform> cubes = new List<Transform>();
};