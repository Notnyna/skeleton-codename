using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class PlatformMovement : MonoBehaviour
    {

        public Transform Button;
        public Transform MoveToTransform;
        public float speed;

        private Vector3 posA, posB, nextPos;

        private bool doMove;
        private bool checkbutton;

        void Start()
        {
            if (Button.GetComponent<ButtonScript>() == null)
            {
                doMove = true;
                checkbutton = false;
            }
            else { checkbutton = true; }
            posA = transform.position;
            posB = MoveToTransform.position;
            nextPos = posB;
        }

        // Update is called once per frame
        void Update()
        {
            if (doMove)
            {
                Move();
            }
            if (checkbutton)
            {
                doMove = Button.GetComponent<ButtonScript>().button;
            }
        }

        public void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, nextPos) <= 0.01)
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

}