namespace Oshi {

    public enum AllyStatus {

        None = 0,
        Player = 1,
        Enemy = 2,

    }

    public static class AllyStatusExtension {
        public static AllyStatus GetOpposite(this AllyStatus status) {
            if (status == AllyStatus.Player) {
                return AllyStatus.Enemy;
            } else if (status == AllyStatus.Enemy) {
                return AllyStatus.Player;
            } else {
                return AllyStatus.None;
            }
        }
    }

}