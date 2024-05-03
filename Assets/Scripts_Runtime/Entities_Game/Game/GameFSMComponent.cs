using UnityEngine;

namespace Oshi {

    public class GameFSMComponent {

        public GameStatus status;

        public bool notInGame_isEntering;

        public bool fadingIn_isEntering;
        public float fadingIn_enterTime;
        public float fadingIn_duration;

        public bool playerTurn_isEntering;

        public bool envirTurn_isEntering;

        public bool fadingOut_isEntering;
        public float fadingOut_enterTime;
        public float fadingOut_duration;

        public bool gameOver_isEntering;
        public float gameOver_enterTime;
        public GameResult gameOver_result;

        public void NotInGame_Enter() {
            Reset();
            status = GameStatus.NotInGame;
            notInGame_isEntering = true;
        }

        public void FadingIn_Enter(float enterTime, float duration) {
            Reset();
            status = GameStatus.FadingIn;
            fadingIn_isEntering = true;
            fadingIn_enterTime = enterTime;
            fadingIn_duration = duration;
        }

        public void FadingIn_IncTimer(float dt) {
            fadingIn_enterTime += dt;
        }

        public void PlayerTurn_Enter() {
            Reset();
            status = GameStatus.PlayerTurn;
            playerTurn_isEntering = true;
        }

        public void EnvirTurn_Enter() {
            Reset();
            status = GameStatus.EnvirTurn;
            envirTurn_isEntering = true;
        }

        public void FadingOut_Enter(float enterTime, float duration) {
            Reset();
            status = GameStatus.FadingOut;
            fadingOut_isEntering = true;
            fadingOut_enterTime = enterTime;
            fadingOut_duration = duration;
        }

        public void FadingOut_IncTimer(float dt) {
            fadingOut_enterTime += dt;
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
            playerTurn_isEntering = false;
            envirTurn_isEntering = false;
            gameOver_isEntering = false;
            fadingIn_isEntering = false;
            fadingOut_isEntering = false;
            gameOver_enterTime = 0;
            fadingIn_enterTime = 0;
            fadingOut_enterTime = 0;
        }

    }

}