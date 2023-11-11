namespace Alter {

    public class GameEntity {

        public GameStage stage;
        public System.Random random;
        public float intervalTime;

        public GameEntity() {
            intervalTime = 0;
            stage = GameStage.Prepare;
        }
        public void Reset() {
            stage = GameStage.Prepare;
        }

    }

}