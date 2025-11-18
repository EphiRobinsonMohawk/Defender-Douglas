using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class GunRotation : MonoBehaviour
    {
        public Rigidbody2D dougRB;
        void Update()
        {
            //Match gun's angle to Doug's direction.
            Vector2 dir = dougRB.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
