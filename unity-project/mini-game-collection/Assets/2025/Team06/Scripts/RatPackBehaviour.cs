using System;
using System.Collections;
using System.Collections.Generic;
using MiniGameCollection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MiniGameCollection.Games2025.Team06
{
    public class RatPackBehaviour : MiniGameBehaviour
    {
        [SerializeField] public GameObject ratKing;
        [SerializeField] public float followerSpeed;
        [SerializeField] Rigidbody2D rb2d;
        [SerializeField] public Animator animator;
        [SerializeField] public SpriteRenderer sr;
        public TwoPlayerCamera twoPlayerCamera;
        public Vector2 direction;
        public float maxDistance;
        public bool defeated = false;
        bool canMove = true;
        public bool lastFlip;

        
        


        void Awake()
        {
            twoPlayerCamera = FindAnyObjectByType<TwoPlayerCamera>();
            ratKing = GameObject.Find("2025-team06-rat-king");
            twoPlayerCamera.targets.Add(transform);
        }
        // Update is called once per frame
        void Update()
        {
            if(defeated)
            {
                canMove = false;
                animator.SetBool("defeated", true);
            }
            if (ratKing != null && canMove)
            {
                direction.x = ratKing.gameObject.transform.position.x - rb2d.position.x;
                direction.y = ratKing.gameObject.transform.position.y - rb2d.position.y;
                direction.Normalize();
                rb2d.velocity = direction * followerSpeed;
            }
            if (rb2d.velocity.x > 0)
            {
                sr.flipX = false;
                lastFlip = false;
            }
            else if (rb2d.velocity.x < 0)
            {
                sr.flipX = true;
                lastFlip = true;
            }
            else
            {
                sr.flipX = lastFlip;
            }

            animator.SetFloat("velocity", Math.Abs(rb2d.velocity.x + rb2d.velocity.y));
        }

        void OnDestroy()
        {
            twoPlayerCamera.targets.Remove(transform);
        }

        void Defeat()
        {
            Debug.Log("ratpack death");
            twoPlayerCamera.targets.Remove(transform);
            Destroy(gameObject);
        }
    }
}
