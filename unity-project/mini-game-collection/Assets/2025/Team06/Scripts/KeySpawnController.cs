using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniGameCollection.Games2025.Team06
{
    public class KeySpawnController : MonoBehaviour
    {
        //Variables
        public float keyTimer;
        public bool key1Spawned = false;
        public bool key2Spawned = false;
        public bool key3Spawned = false;

        //References
        public Transform key1SpawnPoint;
        public Transform key2SpawnPoint;
        public Transform key3SpawnPoint;
        public GameObject keyPrefab;

        // Update is called once per frame
        void Update()
        {
            //Spawn keys over time.
            keyTimer += Time.deltaTime;

            if (keyTimer >= 20 && !key1Spawned)
            {
                Instantiate(keyPrefab, key1SpawnPoint.position, Quaternion.identity);

                key1Spawned = true;
            }
            if (keyTimer >= 30 && !key2Spawned)
            {
                Instantiate(keyPrefab, key2SpawnPoint.position, Quaternion.identity);

                key2Spawned = true;
            }
            if (keyTimer >= 40 && !key3Spawned)
            {
                Instantiate(keyPrefab, key3SpawnPoint.position, Quaternion.identity);

                key3Spawned = true;
            }
        }
    }

}