using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MiniGameCollection.Games2025.Team06
{
    public class HealthbarFollow : MonoBehaviour
    {
        public Transform playerPOS; 
        public Vector3 verticalBarOffset; 
        public Camera cam;


        void Update()
        {

            if (playerPOS == null) return;

            Vector3 worldPos = playerPOS.position + verticalBarOffset;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }
    }
}