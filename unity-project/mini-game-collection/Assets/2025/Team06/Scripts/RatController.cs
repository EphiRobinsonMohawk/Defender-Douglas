using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using MiniGameCollection.Games2025.Team00;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class RatController : MiniGameBehaviour
    {
        [Header("References")]
        [SerializeField] public Animator animator;
        [SerializeField] TwoPlayerCamera twoPlayerCamera;
        [SerializeField] public float ratSpeed;
        [SerializeField] public PlayerID PlayerID;
        [SerializeField] public Rigidbody2D rb2d;
        [SerializeField] GameObject followerPrefab;
        [SerializeField] public SpriteRenderer sr;
        [SerializeField] public TMP_Text ratCounter;
        public GameObject foodToEat;

        [Header("Rat Attributes")]
        public bool canEat = false;
        UnityEngine.Vector2 movementInput;
        public int ratCount;
        public bool defeated = false;
        public bool deathAnim = false;
        public bool canMove = true;
        public bool lastFlip;

        // Update is called once per frame
        void Update()
        {
            //Death
            if (defeated && !deathAnim)
            {
                animator.SetBool("defeated", true);
                canMove = false;
            }
            //Movement
            if (canMove)
            {
                float axisX = ArcadeInput.Players[(int)PlayerID].AxisX;
                float axisY = ArcadeInput.Players[(int)PlayerID].AxisY;
                movementInput = new UnityEngine.Vector2(axisX, axisY);
                movementInput.Normalize();
                rb2d.velocity = movementInput * ratSpeed;
            }
            //Animations
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

            //Eat logic.
            if (canEat && ArcadeInput.Players[(int)PlayerID].Action1.Pressed)
            {
                EatFood();
            }
        }

        void Defeat()
        {
            Debug.Log("rat death");
            twoPlayerCamera.targets.Remove(transform);
            Destroy(gameObject);
            MiniGameManager.StopGame();
        }

        void EatFood()
        {
            Instantiate(followerPrefab, transform.position, transform.rotation);
            ratCount++;
            ratCounter.text = "x" + ratCount;
            Destroy(foodToEat);
        }

        //Food collision logic
        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Collided with " + collision.gameObject.name);
            Component component = collision.GetComponent<FoodTag>();
            if (component != null)
            {
                Debug.Log("Found FoodTag!");
                canEat = true;
                foodToEat = collision.gameObject;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            Component component = collision.GetComponent<FoodTag>();
            if (component != null)
            {
                Debug.Log("Left FoodTag!");
                canEat = false;
                foodToEat = null;
            }
        }
    }
}
