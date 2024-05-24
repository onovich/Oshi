using MortiseFrame.Swing;
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
        public EasingType fadingOut_easingType;
        public EasingMode fadingOut_easingMode;
        public GameResult fadingOut_result;

        public bool mapOver_isEntering;
        public float mapOver_enterTime;
        public GameResult mapOver_result;

        public bool gameOver_isEntering;
        public GameResult gameOver_gameResult;

        public void NotInGame_Enter() {
            Reset();
            status = GameStatus.NotInGame;
            notInGame_isEntering = true;
        }

        public void FadingIn_Enter(float duration) {
            Reset();
            status = GameStatus.FadingIn;
            fadingIn_isEntering = true;
            fadingIn_enterTime = 0f;
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

        public void FadingOut_Enter(float duration, EasingType easingType, EasingMode easingMode, GameResult result) {
            Reset();
            status = GameStatus.FadingOut;
            fadingOut_isEntering = true;
            fadingOut_enterTime = 0f;
            fadingOut_duration = duration;
            fadingOut_result = result;
            fadingOut_easingType = easingType;
            fadingOut_easingMode = easingMode;
        }

        public void FadingOut_IncTimer(float dt) {
            fadingOut_enterTime += dt;
        }

        public void MapOver_Enter(float enterTime, GameResult result) {
            Reset();
            status = GameStatus.MapOver;
            mapOver_isEntering = true;
            mapOver_enterTime = enterTime;
            mapOver_result = result;
        }

        public void MapOver_DecTimer(float dt) {
            mapOver_enterTime -= dt;
            mapOver_enterTime = Mathf.Max(0, mapOver_enterTime);
        }

        public void GameOver_Enter(GameResult result) {
            Reset();
            status = GameStatus.GameOver;
            gameOver_isEntering = true;
            gameOver_gameResult = result;
        }

        public void Reset() {
            notInGame_isEntering = false;
            playerTurn_isEntering = false;
            envirTurn_isEntering = false;
            gameOver_isEntering = false;
            fadingIn_isEntering = false;
            fadingOut_isEntering = false;
            mapOver_isEntering = false;

            fadingIn_enterTime = 0;
            fadingOut_enterTime = 0;
            mapOver_enterTime = 0;
        }

    }

}