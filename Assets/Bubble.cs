using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public enum Type
    {
        None,
        Blue,
        DarkBlue,
        Green,
        Grey,
        Orange,
        Pupple,
        Red,
        Yellow,
    }
    public Type type;
    public Collider2D col;
    public void Awake()
    {
        col = GetComponent<Collider2D>();
        textMesh = Instantiate(Resources.Load<TextMesh>("TextMesh")
            , transform);
        textMesh.transform.localPosition = Vector3.zero;
    }
    public TextMesh textMesh;
    public Vector2Int coord2D;
    internal void SetCoord(Vector2Int coord2D)
    {
        this.coord2D = coord2D;
        textMesh.text = $"{coord2D.x},{coord2D.y}";
    }
}
