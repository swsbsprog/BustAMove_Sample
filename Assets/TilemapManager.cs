using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;
    public Tilemap tilemap;
    private void Awake() => instance = this;
    private void Start()
    {
        var bubbles = GetComponentsInChildren<Bubble>();
        foreach (var bubble in bubbles)
            AddBubble(bubble);  
    }
    Dictionary<Vector2Int, Bubble> coordMap = new();
    internal void AddBubble(Bubble bubble)
    {
        bubble.transform.parent = transform;
        bubble.col.enabled = true;

        // 위치 스냅이동
        Vector3Int coord = tilemap.WorldToCell(bubble.transform.position);
        Vector3 worldPos = tilemap.CellToWorld(coord);
        bubble.transform.position = worldPos;

        // 리스트에 넣기.
        var coord2D = new Vector2Int(coord.x, coord.y);
        coordMap[coord2D] = bubble;
        bubble.SetCoord(coord2D);
    }

    internal void DestroyMatch3(Bubble bubble)
    {
        //bubble타입정보, 위치 사용해서 3개이상 연결된 블럭들을 파괴하자.
        CheckNearSameBubble(bubble.type, bubble.coord2D);

        if(sameBubbleList.Count >= 3)
        {
            foreach (var coord in sameBubbleList)
            {
                Destroy(coordMap[coord].gameObject);
                coordMap.Remove(coord);
            }
        }

        sameBubbleList.Clear();
        alreadyCheck.Clear();

        DestroyIsolation();
    }

    public Vector2Int min;
    public Vector2Int max;
    private void DestroyIsolation()
    {
        HashSet<Vector2Int> aliveBubble = new();
        HashSet<Vector2Int> isolateBubble = new();
        // 탑줄 넣기
        for (int x = min.x; x <= max.x; x++)
        {
            Vector2Int coord = new(x, max.y);
            if (coordMap.ContainsKey(coord))
                aliveBubble.Add(coord);
        }
        foreach (var item in coordMap.Values)
        {
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }


        for (int y = max.y -1; y >= min.y; y--)
        {
            for (int x = min.x; x <= max.x; x++)
            {
                Vector2Int coord = new(x, y);
                if (coordMap.ContainsKey(coord) == false)
                    continue;

                // 위줄과 연결 안되어 있고 && 연결된것도 위줄과 연결 안되어 있다면 격리 된거다(파괴대상)
                //bool isConnectUpLine;
                int upY = y + 1;
                var coordTop = new Vector2Int(x, upY);
                if (aliveBubble.Contains(coordTop))
                {
                    aliveBubble.Add(coord);
                    continue;
                }
                if (y % 2 == 0)  // Y가 짝수 일때는 위, 위왼쪽 체크
                {
                    var coordTopLeft = new Vector2Int(x - 1, upY);
                    if (aliveBubble.Contains(coordTopLeft))
                    {
                        aliveBubble.Add(coord);
                        continue;
                    }
                }
                else // Y가 홀수 일때는 위, 위오른쪽 체크 (1, 1)
                {
                    var coordTopRight = new Vector2Int(x + 1, upY);
                    if (aliveBubble.Contains(coordTopRight))
                    {
                        aliveBubble.Add(coord);
                        continue;
                    }
                }

                isolateBubble.Add(coord);
            }
        }

        //foreach (var coord in aliveBubble)
        //{
        //    coordMap[coord].GetComponent<SpriteRenderer>().color = Color.green;
        //}

        foreach (var coord in isolateBubble)
        {
            Destroy(coordMap[coord].gameObject);
            coordMap.Remove(coord);
        }
    }

    private void CheckNearSameBubble(Bubble.Type type, Vector2Int coord2D)
    {
        AddSameBubble(type, coord2D, x: 0, y: 1);// 위쪽
        AddSameBubble(type, coord2D, x: 0, y:-1);// 아래
        AddSameBubble(type, coord2D, x:-1, y: 0);// 왼쪽
        AddSameBubble(type, coord2D, x: 1, y: 0);// 오른족
        //y가 짝수 일때 (-1,1)/(-1, -1)
        if ( coord2D.y %2 == 0)
        { 
            AddSameBubble(type, coord2D,-1, 1);// 위쪽위
            AddSameBubble(type, coord2D,-1,-1);// 왼쪽아래
        }
        else //y가 홀수 일때 (1,1)/(1, -1)
        {
            AddSameBubble(type, coord2D, 1, 1);// 오른쪽위
            AddSameBubble(type, coord2D, 1,-1);// 오른쪽아래
        }
    }

    HashSet<Vector2Int> sameBubbleList = new();
    HashSet<Vector2Int> alreadyCheck = new();
    // 같은 타입일때 리스트에 더하자.
    private void AddSameBubble(Bubble.Type type, Vector2Int coord2D
        , int x, int y)
    {
        var checkPos = new Vector2Int(x, y) + coord2D;
        if (alreadyCheck.Contains(checkPos))
            return; // 이미 체크 했다
        alreadyCheck.Add(checkPos); 
        
        if (coordMap.ContainsKey(checkPos) == false)
            return; // 보드에 버블이 없는 좌표다

        var checkBubble = coordMap[checkPos];
        if (checkBubble.type == type)
        {
            sameBubbleList.Add(checkPos);
            CheckNearSameBubble(type, checkPos);
        }
    }
}
