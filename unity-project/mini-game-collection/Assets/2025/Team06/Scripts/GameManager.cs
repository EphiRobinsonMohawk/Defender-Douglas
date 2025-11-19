using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team06
{
    //This script just tells the mini game manager who wins and loses. 

    public class GameManager : MiniGameBehaviour
    {
        [SerializeField] public RatController rat;
        [SerializeField] public DouglasController douglas;
        [field: SerializeField] public MiniGameManager miniGameManager { get; private set; }

        public void Start()
        {
            MiniGameManager.StartGame();
        }

        protected override void OnGameEnd()
        {
            if (rat.defeated && douglas.defeated)
            {
                miniGameManager.Winner = MiniGameWinner.Draw;
            }
            else if (rat.defeated)
            {
                miniGameManager.Winner = MiniGameWinner.Player2;
            }
            else if (douglas.defeated)
            {
                miniGameManager.Winner = MiniGameWinner.Player1;
            }
            else
            {
                miniGameManager.Winner = MiniGameWinner.Player2;
                Debug.Log("Victory Con not met, rat is " + rat.defeated + "dog is " + douglas.defeated);
            }
        }
    }
}

