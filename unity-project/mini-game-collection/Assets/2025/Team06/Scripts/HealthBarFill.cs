using System.Collections;
using System.Collections.Generic;
using MiniGameCollection.Games2025.Team06;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFill : MonoBehaviour
{

    public Image healthBar;
    public DouglasController dougScript;
    public float dougHealth;
   
    // Update is called once per frame
    void Update()
    {
        dougHealth = (float)dougScript.dougHealth;
        healthBar.fillAmount = dougHealth * 0.02f;
    }
}
