using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RatReceiver : MonoBehaviour
{
    public void Defeat()
        {
            Debug.Log("rat death");
            Destroy(gameObject);
        }
}
