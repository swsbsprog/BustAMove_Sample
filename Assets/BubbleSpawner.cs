using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Transform fireDirTr;
    public Transform nextBubbleTr;
    public float speed = 100;
    float rotateAngle = 0;
    public float maxAngle = 70;
    public float force = 10;
    public List<Bubble> bubbles = new();   
    public MovingBubble movingBubbleBase;
    public MovingBubble movingBubble;
    public MovingBubble nextMovingBubble;

    private void Start()
    {
        movingBubble = GetNextBubble(fireDirTr.position);
        nextMovingBubble = GetNextBubble(nextBubbleTr.position);
    }

    private MovingBubble GetNextBubble(Vector3 position)
    {
        var newMovingBubble = Instantiate(movingBubbleBase);
        newMovingBubble.transform.position = position;
        var selectBubble = bubbles.OrderBy(x => UnityEngine.Random.Range(0, 1f)).FirstOrDefault();
        var colorGo = Instantiate(selectBubble);
        colorGo.transform.parent = newMovingBubble.transform;
        colorGo.transform.localPosition = Vector3.zero;
        return newMovingBubble;
    }

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

            // 다음버블이 현배 버블위치로 이동

            // 다음 버블 생성
        }
    }
}
