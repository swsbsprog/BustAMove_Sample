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
}
