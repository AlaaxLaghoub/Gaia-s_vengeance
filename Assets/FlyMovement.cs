using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        
        // Move the fly horizontally
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Optionally, make the fly oscillate vertically
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * 0.7f, transform.position.z);
    }
}
