using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Transform fireDirTr;
    public float speed = 100;
    float rotateAngle = 0;
    public float maxAngle = 70;
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            rotateAngle = Mathf.Max(-maxAngle, rotateAngle - speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            rotateAngle = Mathf.Min(maxAngle, rotateAngle + speed * Time.deltaTime);

        fireDirTr.rotation = Quaternion.Euler(0, 0, rotateAngle);
    }
}
