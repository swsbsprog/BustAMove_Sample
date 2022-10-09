using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Collider2D col;
    public void Awake() => col = GetComponent<Collider2D>();
}
