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
    }

    private void CheckNearSameBubble(Bubble.Type type, Vector2Int coord2D)
    {
        AddSameBubble(type, coord2D, 0, 1);// 위쪽
        AddSameBubble(type, coord2D, 0,-1);// 아래
        AddSameBubble(type, coord2D,-1, 0);// 왼쪽
        AddSameBubble(type, coord2D, 1, 0);// 오른족
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
