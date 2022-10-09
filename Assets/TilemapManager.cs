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
            bubble.GetComponent<Collider2D>().enabled = true;
    }

    internal void AddBubble(Bubble bubble)
    {
        bubble.transform.parent = transform;

        // 위치 스냅이동
        Vector3Int coord = tilemap.WorldToCell(bubble.transform.position);
        Vector3 worldPos = tilemap.CellToWorld(coord);
        bubble.transform.position = worldPos;

        // 리스에 넣기.
    }
}
