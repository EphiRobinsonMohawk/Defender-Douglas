    using System.Collections;
using System.Collections.Generic;
using MiniGameCollection.Games2025.Team00;
using MiniGameCollection.Games2025.Team06;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        DouglasController script = collision.GetComponent<DouglasController>();
        if (collision.gameObject.name == "2025-team06-douglas")
        {
            script.keysCollected++;
            Destroy(gameObject);
            Debug.Log("Keys collected: " + script.keysCollected);
        }
    }
}
