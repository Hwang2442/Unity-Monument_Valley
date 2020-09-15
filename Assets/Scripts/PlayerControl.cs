using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    [Space]
    // 현재 위치한 큐브
    public Transform currentCube;
    // 마우스 클릭한 큐브
    public Transform clickedCube;
    // 인디케이터?
    public Transform indicator;

    [Space]
    // 플레이어가 실제 이동할 경로
    public List<Transform> finalPath = new List<Transform>();

    // 이건 뭐지
    private float blend;

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

            // 레이캐스트 충돌 했나
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
                }
            }
        }
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

    // 이건 뭐지?
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
                // 이건 잘 모르겠음
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

    // 이건 뭐지
    private void BuildPath()
    {        
        Transform cube = clickedCube;

        // 클릭한 큐브가 현재큐브와 같지 않을 때까지
        while(cube != currentCube)
        {
            // 실제 이동할 경로에 삽입
            finalPath.Add(cube);

            // 클릭한 큐브의 이전큐브가 None일 때
            if(cube.GetComponent<Walkable>().previousBlock != null)
            {
                // 잘 모르겠음
                cube = cube.GetComponent<Walkable>().previousBlock;
            }
            else
            {
                return;
            }

            finalPath.Insert(0, clickedCube);

            FollowPath();
        }
    }

    // 길을 따라 가는 함수인 듯
    private void FollowPath()
    {
        // 시퀀스는 동작들을 모아놓은 배열이라 생각하면 됨
        Sequence seq = DOTween.Sequence();        

        // 경로에 따라 계속 루프
        for(int i = finalPath.Count - 1; i > 0; i--)
        {
            // 계단인지 판단하여 이동속도 조정
            float time = (finalPath[i].GetComponent<Walkable>().isStair) ? (1.5f) : (1.0f);

            Vector3 cubePos = finalPath[i].GetComponent<Walkable>().GetWalkPoint();
            cubePos.y += 1.0f;

            // 큐브로 이동하는 것을 시퀀스에 추가, 부드럽게 하기위해서 Linear를 사용 == add와 같다고 보면 됨
            seq.Append(transform.DOMove(cubePos, 0.2f * time).SetEase(Ease.Linear));

            // 이건 뭔지 잘 모르겠음
            if(!finalPath[i].GetComponent<Walkable>().dontRotate)
            {
                seq.Join(transform.DOLookAt(finalPath[i].position, 0.1f, AxisConstraint.Y, Vector3.up));
            }
        }

        //seq.AppendCallback(() => Clear());
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
        // 플레이어 밑으로 레이캐스트 생성
        Ray playerRay = new Ray(transform.position, -transform.up);
        // 레이캐스트 충돌
        RaycastHit playerHit;

        // 
        if(Physics.Raycast(playerRay, out playerHit))
        {
            // 발판을 밟고 있다면
            if(playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;

                // 계단에 따라서 위치 조정해야될 부분?
                if(playerHit.transform.GetComponent<Walkable>().isStair)
                {                    
                }
            }
        }
    }
}
