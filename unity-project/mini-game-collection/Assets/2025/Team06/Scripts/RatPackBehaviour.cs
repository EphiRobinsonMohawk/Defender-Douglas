using System.Collections;
using System.Collections.Generic;
using MiniGameCollection;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class RatPackBehaviour : MiniGameBehaviour
    {
        [SerializeField] public GameObject ratKing;
        [SerializeField] public float followerSpeed;
        [SerializeField] Rigidbody2D rb2d;
        public Vector2 direction;
        public float maxDistance;
        
        


        void Awake()
        {
            ratKing = GameObject.Find("2025-team06-rat-king");
        }
        // Update is called once per frame
        void Update()
        {
            if (ratKing != null)
            {
                direction.x = ratKing.gameObject.transform.position.x - rb2d.position.x;
                direction.y = ratKing.gameObject.transform.position.y - rb2d.position.y;
                direction.Normalize();
                rb2d.velocity = direction * followerSpeed;
            }
           
            
        }
    }
}
