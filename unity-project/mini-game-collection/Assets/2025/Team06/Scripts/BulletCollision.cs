using System.Collections;
using System.Collections.Generic;
using MiniGameCollection.Games2025.Team06;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class BulletCollision : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RatTag>() != null)
            {
                Destroy(collision.gameObject);
            }
        }
    }

}