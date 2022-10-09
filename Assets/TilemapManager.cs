using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;
    private void Awake() => instance = this;
    private void Start()
    {
        var bubbles = GetComponentsInChildren<Bubble>();
        foreach (var bubble in bubbles) 
            bubble.GetComponent<Collider2D>().enabled = true;
    }
}
