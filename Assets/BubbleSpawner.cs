using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Transform fireDirTr;
    public float speed = 100;
    float rotateAngle = 0;
    public float maxAngle = 70;
    public MovingBubble movingBubble;
    public float force = 10;
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            rotateAngle = Mathf.Max(-maxAngle, rotateAngle - speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            rotateAngle = Mathf.Min(maxAngle, rotateAngle + speed * Time.deltaTime);

        fireDirTr.rotation = Quaternion.Euler(0, 0, rotateAngle);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            movingBubble.transform.position = fireDirTr.position;

            var angle = -fireDirTr.rotation.eulerAngles.z;
            var direction = new Vector2(
                Mathf.Sin(Mathf.Deg2Rad * angle),  // x 좌표
                Mathf.Cos(Mathf.Deg2Rad * angle)); // y 좌표 구하기

            movingBubble.Fire(direction, force);
        }
    }
}
