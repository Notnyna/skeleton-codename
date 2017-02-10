using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMouse : MonoBehaviour {

    [Range(100f, 1000f)]
    public float speed=1;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();		
	}

    void DragToMouse() {
        Vector2 MPos = Input.mousePosition;
        Vector2 FDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.LogFormat("1: {0}", FDir.ToString());
        if (Vector2.Distance(transform.position,FDir)>1f)
        {
            FDir = FDir - new Vector2(transform.position.x,transform.position.y);
            FDir = FDir.normalized * speed;
            rb.AddForce(FDir);
        }

    }
	
	// FixedUpdate() is for physics and rigidbodies, used Update() normally
	void FixedUpdate () {
        DragToMouse();
	}
}
