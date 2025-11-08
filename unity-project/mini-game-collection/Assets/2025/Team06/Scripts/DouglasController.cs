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

        [SerializeField] public GameObject bulletPrefab;
        [SerializeField] public GameObject gun;
        [SerializeField] public RatController mainRat;
        [SerializeField] public float originalRatSpeed;
        [SerializeField] public bool gunActive = false;
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
        [SerializeField] public float iFrames;
        [SerializeField] public float iFramesMax;
        [SerializeField] public bool invincible;
        [SerializeField] public int dougHealth;
        [SerializeField] public PlayerID PlayerID;
        [SerializeField] public Rigidbody2D rb2d;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private UnityEngine.Vector2 lastDirection = UnityEngine.Vector2.right;

        [SerializeField] public float offsetRange = 1f;
        [SerializeField] private bool isFiring;
        [SerializeField] private int fireCount;
        [SerializeField] public List<GameObject> ratsInRange = new();


        // Update is called once per frame
        void Update()
        {
            //Death Logic
            if(dougHealth <= 0)
            {
                Die();
            }
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

            if (isFiring)
            {
                fireCount++;
                if (fireCount == 15 || fireCount == 30 || fireCount == 45 || fireCount == 60 || fireCount == 75 || fireCount == 90)
                {
                    Fire();
                }
                else if (fireCount >= 91)
                {
                    isFiring = false;
                    fireCount = 0;
                }
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

            //IFrame timer logic
            if (invincible)
            {
                iFrames += Time.deltaTime;
                if (iFrames >= iFramesMax)
                {
                    invincible = false;
                    iFrames = 0;
                }
            }

            else if (!ratSlowed)
            {
                originalRatSpeed = mainRat.ratSpeed;
            }

            //Gun activation logic
            if (!gunActive)
            {
                gun.SetActive(false);
                if (keysCollected >= 3)
                {
                    gun.SetActive(true);
                    gunActive = true;
                }

            }
        }

        void Chomp()
        {
            if (!gunActive && attackReady && ratsInRange.Count > 0)
            {
                GameObject rat = ratsInRange[0];
                ratsInRange.RemoveAt(0);
                Destroy(rat);

                //Reseting attack timer

                attackReady = false;
                attackTimer = 0;
                Debug.Log("Bite hits");
            }
            else if (gunActive && attackReady)
            {
                //Insantiating bullet + inheriting player's velocity (so you can choose firing direction)
                isFiring = true;
                //Reseting attack timer
                attackReady = false;
                attackTimer = 0;
                Debug.Log("Gun Fires");
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

        void Fire()
        {
            UnityEngine.Vector2 dir = rb2d.velocity;
            if (dir == UnityEngine.Vector2.zero)
            {
                dir = lastDirection;               // use last aim if not moving
            }
            else
            {
                lastDirection = dir.normalized;    // remember this direction
            }
            UnityEngine.Vector2 randomOffset = new UnityEngine.Vector2
            (
            Random.Range(-offsetRange, offsetRange),
            Random.Range(-offsetRange, offsetRange)
            );
            UnityEngine.Vector2 spawnPos = (UnityEngine.Vector2)transform.position + randomOffset;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, UnityEngine.Quaternion.Euler(0f, 0f, angle + 270));
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = rb2d.velocity + dir.normalized * bulletSpeed;
        }

        void Die()
        {
            Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RatTag>() != null)
            {


                //Bite Logic
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

        void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.GetComponent<RatTag>() != null)
            {
                //Damage Logic
                if(!invincible)
                {
                    dougHealth -= 1;
                    invincible = true;
                    Debug.Log("Douglas has " + dougHealth + "HP");
                }
            }
        }
    }
    
        

}
