using System.Collections;
using System.Collections.Generic;
using MiniGameCollection.Games2025.Team06;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class BulletCollision : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RatTag>() != null)
            {
                if(collision.GetComponent<RatPackBehaviour>() != null)
                {
                    RatPackBehaviour ratToDestroy = collision.GetComponent<RatPackBehaviour>();
                    ratToDestroy.defeated = true;
                }
                if(collision.GetComponent<RatController>() != null)
                {
                    RatController ratToDestroy = collision.GetComponent<RatController>();
                    ratToDestroy.defeated = true;
                }
            }
        }
    }

}