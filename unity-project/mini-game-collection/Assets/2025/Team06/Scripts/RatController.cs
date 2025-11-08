using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using MiniGameCollection.Games2025.Team00;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class RatController : MiniGameBehaviour
    {

        public bool canEat = false;
        UnityEngine.Vector2 movementInput;
        [SerializeField] public float ratSpeed;
        [SerializeField] public PlayerID PlayerID;
        [SerializeField] public Rigidbody2D rb2d;
        [SerializeField] GameObject followerPrefab;
        public GameObject foodToEat;
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {
            float axisX = ArcadeInput.Players[(int)PlayerID].AxisX;
            float axisY = ArcadeInput.Players[(int)PlayerID].AxisY;
            movementInput = new UnityEngine.Vector2(axisX, axisY);
            movementInput.Normalize();
            rb2d.velocity = movementInput * ratSpeed;

            if (canEat && ArcadeInput.Players[(int)PlayerID].Action1.Pressed)
            {
                EatFood();
            }
        }
        
        void EatFood()
        {
            Instantiate(followerPrefab, transform.position, transform.rotation);
            Destroy(foodToEat);
        }


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
