namespace Utilsf {
    public class Score {
        private int enemyKills;
        private int repairedTiles;

        #region public
        public void reset() {
            enemyKills = 0;
            repairedTiles = 0;
        }

        public void tileRepaired() {
            repairedTiles++;
        }

        public void enemyKilled() {
            enemyKills++;
        }

        public int total() {
            int total = 0;
            total += (enemyKills * 2);
            total += (repairedTiles * 5);
            return total;
        }
        #endregion
    }
}
