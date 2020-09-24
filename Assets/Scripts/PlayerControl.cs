using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEditorInternal;

public class PlayerControl : MonoBehaviour
{
    [Space]
    // 현재 위치한 큐브
    public Transform currentCube;
    // 마우스 클릭한 큐브
    public Transform clickedCube;

    public Animator anim;

    [Space]
    // 플레이어가 실제 이동할 경로
    public List<Transform> finalPath = new List<Transform>();

    [Space]
    public float moveSpeed;

    void Start()
    {
        // 플레이어가 밟고 있는 큐브 설정
        RayCastDown();
    }

    void Update()
    {
        // 플레이어가 밟고 있는 큐브 설정
        RayCastDown();

        // 현재 밟고 있는 큐브가 움직이는 땅인 경우
        if(currentCube.GetComponent<Walkable>().movingGround)
        {
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        // 마우스 클릭
        if(Input.GetMouseButtonDown(0))
        {
            // 스크린 터치하고 좌표?
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            // 레이 발사!!
            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                // 클릭한 곳이 오브젝트면
                if(mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    // 클릭한 큐브 위치 설정
                    clickedCube = mouseHit.transform;

                    // 경로 초기화
                    finalPath.Clear();

                    // 길찾기 시작
                    FindPath();

                    anim.SetBool("Walking", true);
                }
            }
        }
        
        FollowPath();
    }

    // 길찾기
    private void FindPath()
    {
        // 다음 이동할 큐브
        List<Transform> nextCubes = new List<Transform>();
        // 이전 큐브
        List<Transform> pastCubes = new List<Transform>();

        // 현재 큐브의 연결된 큐브 갯수만큼 루프
        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            // 경로가 연결되어 있다면
            if(path.active)
            {
                // 다음으로 이동할 큐브 리스트에 추가
                nextCubes.Add(path.target);
                // 해당 큐브를 이전큐브로 등록?
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();       
    }

    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        // 클릭한 큐브와 현재 큐브가 같으면
        // 목표 좌표에 도착한 것
        if(current == clickedCube)
        {
            return;
        }

        // 현재 큐브의 이동 가능한 큐브만큼 반복
        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            // 방문했던 큐브중에 없고 && 길이 활성화되어 있을 때
            if(!visitedCubes.Contains(path.target) && path.active)
            {
                // 다음 이동 큐브에 삽입
                nextCubes.Add(path.target);
                
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }

        // 방문한 큐브 리스트에 현재 큐브를 삽입
        visitedCubes.Add(current);

        // 리스트가 비어있지않다면?
        if(nextCubes.Any())
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }

    // 경로 생성
    private void BuildPath()
    {
        Transform cube = clickedCube;

        // 클릭한 큐브가 현재큐브와 같지 않을 때까지
        while (cube != currentCube)
        {
            // 실제 이동할 경로에 삽입
            finalPath.Add(cube);

            // 클릭한 큐브의 이전큐브가 None일 때
            if (cube.GetComponent<Walkable>().previousBlock != null)
            {                
                cube = cube.GetComponent<Walkable>().previousBlock;
            }
            else
            {
                return;
            }
        }
    }

    // 길을 따라 가는 함수
    private void FollowPath()
    {
        // 이동하지 않음
        if (finalPath.Count == 0)
        {
            return;
        }

        // 다음으로 이동할 큐브
        Walkable moveCube = finalPath[finalPath.Count - 1].GetComponent<Walkable>();
        
        // 방향을 바꾸지 않음
        if(!moveCube.dontRotate)
        {
            transform.LookAt(moveCube.GetWalkPoint());
        }

        // 착시효과가 적용된 큐브는 거리가 실제로 멀기 때문에 보간을 이용
        transform.position = Vector3.Lerp(transform.position, moveCube.GetWalkPoint(), Time.deltaTime * moveSpeed);

        if(Vector3.Distance(transform.position, moveCube.GetWalkPoint()) < 0.01f)
        {
            transform.position = moveCube.GetWalkPoint();

            finalPath.RemoveAt(finalPath.Count - 1);
            
            if(finalPath.Count == 0)
            {
                anim.SetBool("Walking", false);
            }
        }
    }

    private void Clear()
    {
        foreach (Transform form in finalPath)
        {
            form.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
    }

    // 현재 플레이어가 밟고 있는 큐브 찾는 함수
    public void RayCastDown()
    {
        // 플레이어 센터 포지션 생성
        Vector3 rayPos = transform.position;
        rayPos.y += transform.localScale.y * 0.5f;
        
        // 레이 생성, 방향은 아래
        Ray playerRay = new Ray(rayPos, -transform.up);
        // 레이캐스트 충돌
        RaycastHit playerHit;

        // 레이 발사
        if(Physics.Raycast(playerRay, out playerHit))
        {
            // 발판을 밟고 있다면
            if(playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
            }
        }
    }
}
