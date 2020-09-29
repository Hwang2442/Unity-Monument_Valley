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
    // 플레이어가 바라보는 방향 변경 유무
    public bool dontRotate;

    [Space]
    [Header("Offset")]
    public float defaultOffset = 0.5f;
    public float stairOffset = 0.4f;

    // 걷는 좌표 구하는 함수?
    public Vector3 GetWalkPoint()
    {
        float stair = (isStair) ? (stairOffset) : (0);

        return transform.position + transform.up * 0.5f - transform.up * stair;
    }

    private void OnDrawGizmos()
    {
        // 큐브 위에 타겟 그리기
        Gizmos.color = Color.white;
        Gizmos.DrawCube(GetWalkPoint(), new Vector3(0.1f, 0.1f, 0.1f));

        if(possiblePaths == null)
        {
            return;
        }

        for(int i = 0; i < possiblePaths.Count; i++)
        {
            if(possiblePaths[i].target == null)
            {
                continue;
            }

            Gizmos.color = (possiblePaths[i].active) ? (Color.green) : (Color.clear);
            Gizmos.DrawLine(GetWalkPoint(), possiblePaths[i].target.GetComponent<Walkable>().GetWalkPoint());
        }
    }
}

[System.Serializable]
public class WalkPath
{
    public Transform target;
    public bool active = true;
}