using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using MiniGameCollection.Games2025.Team00;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class DouglasController : MiniGameBehaviour
    {

        UnityEngine.Vector2 movementInput;
        [SerializeField] public RatController mainRat;
        [SerializeField] public float originalRatSpeed;
        [SerializeField] public bool gunActive;
        [SerializeField] private bool attackReady;
        [SerializeField] public float attackCooldown;
        [SerializeField] private float attackTimer;
        [SerializeField] private bool slowReady;
        [SerializeField] public float slowCooldown;
        [SerializeField] private float slowTimer;
        [SerializeField] private bool ratSlowed;
        [SerializeField] private float ratSlowTimer;
        [SerializeField] public float ratSlowDuration;
        [SerializeField] public float ratSlowSpeed;
        [SerializeField] public float keysCollected;
        [SerializeField] public float dougSpeed;
        [SerializeField] public PlayerID PlayerID;
        [SerializeField] public Rigidbody2D rb2d;

        [SerializeField] public List<GameObject> ratsInRange = new();


        // Update is called once per frame
        void Update()
        {
            float axisX = ArcadeInput.Players[(int)PlayerID].AxisX;
            float axisY = ArcadeInput.Players[(int)PlayerID].AxisY;
            movementInput = new UnityEngine.Vector2(axisX, axisY);
            movementInput.Normalize();
            rb2d.velocity = movementInput * dougSpeed;
            ratsInRange.RemoveAll(go => go == null);

            if (ArcadeInput.Players[(int)PlayerID].Action1.Pressed)
            {
                Chomp();
            }

            if (ArcadeInput.Players[(int)PlayerID].Action2.Pressed)
            {
                Bark();
            }

            //Attack timer logic
            if (!attackReady)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    attackReady = true;
                }
            }

            //Slow timer logic
            if (!slowReady)
            {
                slowTimer += Time.deltaTime;
                if (slowTimer >= slowCooldown)
                {
                    slowReady = true;
                }
            }

            //Slow duration logic
            if (ratSlowed)
            {
                mainRat.ratSpeed = ratSlowSpeed;
                ratSlowTimer += Time.deltaTime;
                if (ratSlowTimer >= ratSlowDuration)
                {
                    mainRat.ratSpeed = originalRatSpeed;
                    ratSlowed = false;
                    ratSlowTimer = 0;
                }
            }

            else if (!ratSlowed)
            {
                originalRatSpeed = mainRat.ratSpeed;
            }

            //Gun activation logic
            if (!gunActive)
            {
                if (keysCollected <= 3)
                {
                    gunActive = true;
                }

            }
        }
        
        void Chomp()
        {
            if (attackReady && ratsInRange.Count > 0)
            {
                GameObject rat = ratsInRange[0];
                ratsInRange.RemoveAt(0);
                Destroy(rat);

                //Reseting attack timer

                attackReady = false;
                attackTimer = 0;
                Debug.Log("Bite hits");
            }
            else
            {
                Debug.Log("Bite misses");
            }

        }

        void Bark()
        {
            if (slowReady && ratsInRange.Count > 0)
            {

                //Reseting attack timer
                ratSlowed = true;

                slowReady = false;
                slowTimer = 0;
                Debug.Log("Bark hits");
            }

            else
            {
                Debug.Log("Bark Misses");
            }

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RatTag>() != null)
            {
                GameObject rat = collision.gameObject;
                if (!ratsInRange.Contains(rat)) //prevents adding collided rat twice
                {
                    ratsInRange.Add(rat);   //adding collided rat to ratsInRange list

                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<RatTag>() != null)
            {
                ratsInRange.Remove(collision.gameObject);
            }
        }

    }

}
