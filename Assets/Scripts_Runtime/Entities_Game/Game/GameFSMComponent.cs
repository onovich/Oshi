using System.Diagnostics;
using UnityEngine;

namespace Oshi {

    public class GameFSMComponent {

        public GameStatus status;

        public bool notInGame_isEntering;
        public bool gaming_isEntering;
        public float gaming_gameTime;
        public bool gameOver_isEntering;
        public float gameOver_enterTime;
        public GameResult gameOver_result;

        public void NotInGame_Enter() {
            Reset();
            status = GameStatus.NotInGame;
            notInGame_isEntering = true;
        }

        public void Gaming_Enter(float gameTime) {
            Reset();
            status = GameStatus.Gaming;
            gaming_isEntering = true;
            gaming_gameTime = gameTime;
        }

        public void Gaming_DecTimer(float dt) {
            gaming_gameTime -= dt;
        }

        public void GameOver_Enter(float enterTime, GameResult result) {
            Reset();
            status = GameStatus.GameOver;
            gameOver_isEntering = true;
            gameOver_enterTime = enterTime;
            gameOver_result = result;
        }

        public void GameOver_DecTimer(float dt) {
            gameOver_enterTime -= dt;
            gameOver_enterTime = Mathf.Max(0, gameOver_enterTime);
        }

        public void Reset() {
            notInGame_isEntering = false;
            gaming_isEntering = false;
            gameOver_isEntering = false;
            gameOver_enterTime = 0;
        }

    }

}