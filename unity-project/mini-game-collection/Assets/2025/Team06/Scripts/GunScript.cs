using System.Collections;
using System.Collections.Generic;
using MiniGameCollection.Games2025.Team06;
using UnityEngine;
namespace MiniGameCollection.Games2025.Team06
{
    public class GunScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "2025-team06-douglas")
            {
                Destroy(gameObject);
            }
        }
    }
}