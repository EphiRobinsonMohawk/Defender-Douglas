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

        [SerializeField] public bool gunActive;
        [SerializeField] public bool attackReady;
        [SerializeField] public float attackTimer;
        [SerializeField] public float keysCollected;
        [SerializeField] public float dougSpeed;
        [SerializeField] public PlayerID PlayerID;
        [SerializeField] public Rigidbody2D rb2d;
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {
            float axisX = ArcadeInput.Players[(int)PlayerID].AxisX;
            float axisY = ArcadeInput.Players[(int)PlayerID].AxisY;
            movementInput = new UnityEngine.Vector2(axisX, axisY);
            movementInput.Normalize();
            rb2d.velocity = movementInput * dougSpeed;
            Debug.Log(attackTimer);
            Debug.Log(attackReady);

            if (attackReady && ArcadeInput.Players[(int)PlayerID].Action1.Pressed)
            {
                Chomp();
            }

            //Attack timer logic
            if (!attackReady)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= 2)
                {
                    attackReady = true;
                }
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
            if (attackReady)
            {



                attackReady = false;
                attackTimer = 0;
            }
        }
    
    }

}
