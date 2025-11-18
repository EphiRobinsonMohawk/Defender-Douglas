using System;
using System.Collections;
using System.Collections.Generic;
using MiniGameCollection;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    public class FoodSpawner : MiniGameBehaviour
    {
        public List<GameObject> foodSpawns = new List<GameObject>();
        public List<bool> hasSpawned = new List<bool>();
        public GameObject food;
        public int foodLimit = 15;
        public int foodSpawned = 0;

        // Start is called before the first frame update
        void Awake()
        {
            //On start of game, make a list of all spawn points. While under the max spawns for food, it will loop
            //until enough spawns are full.
            for (int i = 0; i < foodSpawns.Count; i++)
            {
                hasSpawned.Add(false);
            }
            while (foodSpawned < foodLimit)
            {
                for (int index = 0; index < foodSpawns.Count; index++)
                {
                    int r = UnityEngine.Random.Range(0, 2);
                    //50/50 on if food spawns in the spawn point. Also checks if food as already spawned here.
                    if (r == 1 && !hasSpawned[index])
                    {
                        Instantiate(food, foodSpawns[index].transform);
                        foodSpawned++;
                        hasSpawned[index] = true;
                    }
                }
            }

        }

    }

}