using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Rigidbody2D dougRB;

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = dougRB.velocity;
         float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
