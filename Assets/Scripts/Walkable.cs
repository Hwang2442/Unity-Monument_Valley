using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    // 해당 블록에서 이동할 수 있는 좌표
    public List<WalkPath> possiblePaths = new List<WalkPath>();

    [Space]
    // 이전 블록?
    public Transform previousBlock;

    [Space]
    [Header("Bool")]
    // 계단인가 아닌가
    public bool isStair = false;
    // 움직일 수 있는 땅?
    public bool movingGround = false;
    // 버튼인가?
    public bool isButton;
    // 움직이지 않는가? 이건 잘 모르겠음
    public bool dontRotate;

    [Space]
    [Header("Offset")]
    public float defaultOffset = 0.5f;
    public float stairOffset = 0.4f;

    // 걷는 좌표 구하는 함수?
    public Vector3 GetWalkPoint()
    {
        float stair = isStair ? stairOffset : 0;

        return transform.position + transform.up * 0.5f - transform.up * stair;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;        
        Gizmos.DrawCube(GetWalkPoint(), new Vector3(0.1f, 0.1f, 0.1f));

        if(possiblePaths == null)
        {
            return;
        }

        foreach (WalkPath path in possiblePaths)
        {
            if(path.target == null)
            {
                return;
            }

            Gizmos.color = path.active ? Color.green : Color.clear;
            Gizmos.DrawLine(GetWalkPoint(), path.target.GetComponent<Walkable>().GetWalkPoint());
        }
    }
}

[System.Serializable]
public class WalkPath
{
    public Transform target;
    public bool active = true;
}