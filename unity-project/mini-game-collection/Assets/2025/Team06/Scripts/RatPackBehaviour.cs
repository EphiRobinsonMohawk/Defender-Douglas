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
        //References
        [SerializeField] public GameObject ratKing;
        [SerializeField] public float followerSpeed;
        [SerializeField] Rigidbody2D rb2d;
        [SerializeField] public Animator animator;
        [SerializeField] public SpriteRenderer sr;
        public TwoPlayerCamera twoPlayerCamera;

        // Movement
        public Vector2 direction;
        public float maxDistance;
        bool canMove = true;

        //Misc.
        public bool lastFlip;
        public bool defeated = false;

        void Awake()
        {
            //Add player to camera tracking.
            twoPlayerCamera = FindAnyObjectByType<TwoPlayerCamera>();
            ratKing = GameObject.Find("2025-team06-rat-king");
            twoPlayerCamera.targets.Add(transform);
        }

        // Update is called once per frame
        void Update()
        {
            //Handle game loss.
            if (defeated)
            {
                canMove = false;
                animator.SetBool("defeated", true);
            }
            //Handle movement.
            if (ratKing != null && canMove)
            {
                direction.x = ratKing.gameObject.transform.position.x - rb2d.position.x;
                direction.y = ratKing.gameObject.transform.position.y - rb2d.position.y;
                direction.Normalize();
                rb2d.velocity = direction * followerSpeed;
            }
            //Handle animations.
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
            //Remove player from camera tracking.
            twoPlayerCamera.targets.Remove(transform);
        }

        void Defeat()
        {
            //Death logic.
            Debug.Log("ratpack death");
            twoPlayerCamera.targets.Remove(transform);
            Destroy(gameObject);
        }
    }
}
