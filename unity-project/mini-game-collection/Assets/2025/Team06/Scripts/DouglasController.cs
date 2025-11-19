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
    public class DouglasController : MiniGameBehaviour
    {
        [Header("References")]
        UnityEngine.Vector2 movementInput;
        [SerializeField] public GameObject fence;
        [SerializeField] public GameObject bulletPrefab;
        [SerializeField] public GameObject gun;
        [SerializeField] public RatController mainRat;
        [SerializeField] public PlayerID PlayerID;
        [SerializeField] public Rigidbody2D rb2d;
        [SerializeField] public TMP_Text keyCounter;
        [SerializeField] public Animator animator;
        [SerializeField] public SpriteRenderer sr;
        [SerializeField] public AudioClip dogBite;
        [SerializeField] public AudioClip ratBite;
        [SerializeField] public AudioClip dogBark;
        [SerializeField] public AudioSource dougSource;
        public List<GameObject> ratsInRange = new();
        public TwoPlayerCamera twoPlayerCamera;
        [SerializeField] public GameObject gunToCamera;

        [Header("Doug Attributes")]
        [SerializeField] public float dougSpeed;
        [SerializeField] public float iFrames;
        [SerializeField] public float iFramesMax;
        [SerializeField] public bool invincible;
        [SerializeField] public float dougHealth;
        [SerializeField] public float attackCooldown;
        [SerializeField] private bool attackReady;
        [SerializeField] private float attackTimer;
        [SerializeField] private bool slowReady;
        [SerializeField] public float slowCooldown;
        [SerializeField] private float slowTimer;


        [Header("Key, Door and Gun")]
        [SerializeField] public float keysCollected;
        [SerializeField] public bool gateOpen = false;
        [SerializeField] public bool gunActive = false;
        [SerializeField] private bool opened;
        [SerializeField] public float offsetRange = 1f;
        [SerializeField] private bool isFiring;
        [SerializeField] private int fireCount;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private UnityEngine.Vector2 lastDirection = UnityEngine.Vector2.right;



        [Header("Rat and Misc.")]

        [SerializeField] public float originalRatSpeed;
        [SerializeField] private bool ratSlowed;
        [SerializeField] private float ratSlowTimer;
        [SerializeField] public float ratSlowDuration;
        [SerializeField] public float ratSlowSpeed;
        public bool lastFlip;
        public bool defeated = false;
        public bool canMove = true;


        void Start()
        {
            gunToCamera = GameObject.Find("2025-team06-gun");
            twoPlayerCamera = FindAnyObjectByType<TwoPlayerCamera>();
        }
        // Update is called once per frame
        void Update()
        {
            //Gate Logic
            if (gateOpen && !opened)
            {
                gunToCamera = GameObject.Find("2025-team06-gun");
                twoPlayerCamera.targets.Add(gunToCamera.transform);
                fence.SetActive(false);
                opened = true;
            }

            if (keysCollected >= 3)
            {
                gateOpen = true;
            }

            //Death Logic
            if (dougHealth <= 0)
            {
                defeated = true;
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
                rb2d.velocity = movementInput * dougSpeed;
            }


            //Animation
            animator.SetFloat("velocity", Math.Abs(rb2d.velocity.x + rb2d.velocity.y));

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

            //

            ratsInRange.RemoveAll(go => go == null);

            //Bite
            if (ArcadeInput.Players[(int)PlayerID].Action1.Pressed)
            {
                Chomp();

            }

            //Bark
            if (ArcadeInput.Players[(int)PlayerID].Action2.Pressed)
            {
                Bark();
            }

            //Shooting
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

            //Key UI
            keyCounter.text = "x" + keysCollected;

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

            //Gun activation logic
            if (gunActive)
            {
                gun.SetActive(true);

                twoPlayerCamera.targets.Remove(gunToCamera.transform);
            }

        }

        void Chomp()
        {
            if (!gunActive && attackReady && ratsInRange.Count > 0 && mainRat != null)
            {
                //AUDIO GOES HERE
                dougSource.PlayOneShot(dogBite);
                //------------------
                if (ratsInRange[0] == mainRat)
                {
                    GameObject rat = ratsInRange[1];
                    ratsInRange.RemoveAt(1);
                    mainRat.ratCount -= 1;
                    mainRat.ratCounter.text = "x" + mainRat.ratCount;
                    if (rat.GetComponent<RatPackBehaviour>() != null)
                    {
                        RatPackBehaviour ratPack = rat.GetComponent<RatPackBehaviour>();
                        ratPack.defeated = true;
                    }
                    else if (rat.GetComponent<RatController>() != null)
                    {
                        RatController ratKing = rat.GetComponent<RatController>();
                        ratKing.defeated = true;
                    }
                    else
                    {
                        Debug.Log("Rat script check failed");
                    }
                }
                else if (ratsInRange[0] != mainRat)
                {
                    GameObject rat = ratsInRange[0];
                    ratsInRange.RemoveAt(0);
                    mainRat.ratCount -= 1;
                    mainRat.ratCounter.text = "x" + mainRat.ratCount;
                    if (rat.GetComponent<RatPackBehaviour>() != null)
                    {
                        RatPackBehaviour ratPack = rat.GetComponent<RatPackBehaviour>();
                        ratPack.defeated = true;
                    }
                    else if (rat.GetComponent<RatController>() != null)
                    {
                        RatController ratKing = rat.GetComponent<RatController>();
                        ratKing.defeated = true;
                    }
                    else
                    {
                        Debug.Log("Rat script check failed");
                    }
                }
                else if (ratsInRange[0] == mainRat && ratsInRange.Count < 2)
                {
                    GameObject rat = ratsInRange[0];
                    ratsInRange.RemoveAt(0);
                    mainRat.ratCount -= 1;
                    mainRat.ratCounter.text = "x" + mainRat.ratCount;
                    if (rat.GetComponent<RatPackBehaviour>() != null)
                    {
                        RatPackBehaviour ratPack = rat.GetComponent<RatPackBehaviour>();
                        ratPack.defeated = true;
                    }
                    else if (rat.GetComponent<RatController>() != null)
                    {
                        RatController ratKing = rat.GetComponent<RatController>();
                        ratKing.defeated = true;
                    }
                    else
                    {
                        Debug.Log("Rat script check failed");
                    }
                }

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
            if (slowReady)
            {
                //Reseting attack timer
                ratSlowed = true;
                slowReady = false;
                slowTimer = 0;
                Debug.Log("Bark hits");
                //AUDIO GOES HERE
                dougSource.PlayOneShot(dogBark);
                //---------------
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
            UnityEngine.Random.Range(-offsetRange, offsetRange),
            UnityEngine.Random.Range(-offsetRange, offsetRange)
            );
            UnityEngine.Vector2 spawnPos = (UnityEngine.Vector2)transform.position + randomOffset;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, UnityEngine.Quaternion.Euler(0f, 0f, angle + 270));
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = rb2d.velocity + dir.normalized * bulletSpeed;
        }

        void Defeat()
        {
            Debug.Log("doug death");
            twoPlayerCamera.targets.Remove(transform);
            Destroy(gameObject);
            MiniGameManager.StopGame();
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
            if (collision.gameObject.name == "2025-team06-gun")
            {
                gunActive = true;
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
            if (collision.GetComponent<RatTag>() != null)
            {
                //Damage Logic
                if (!invincible)
                {
                    dougHealth -= 0.1f * ratsInRange.Count;
                    invincible = true;
                    Debug.Log("Douglas has " + dougHealth + "HP");
                    dougSource.PlayOneShot(ratBite);
                }
            }
        }
    }



}
