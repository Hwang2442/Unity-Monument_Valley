using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            }
        }
    }
}
