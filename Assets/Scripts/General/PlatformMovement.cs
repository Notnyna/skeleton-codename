using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlatformMovement : MonoBehaviour
    {

    // Use this for initialization
    public static bool buttonTouch;
    private Vector3 posA, posB, nextPos;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transformB;

    void Start()
    {
        buttonTouch = false;
        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        nextPos = posB;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonTouch)
        {
            move();
        }
    }
    public void move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nextPos, speed * Time.deltaTime);
        if (Vector3.Distance(childTransform.localPosition, nextPos) <= 0.01)
        {
            changeDirection();
        }
    }

    public void changeDirection()
    {
        if (nextPos != posA)
            nextPos = posA;
        else
            nextPos = posB;
    }
}

