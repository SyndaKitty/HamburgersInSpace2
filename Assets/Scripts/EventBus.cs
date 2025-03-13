namespace Assets.Scripts
{
    public class EventBus
    {
        public delegate void _EnemyKilled(EnemyController enemy);
        public static event _EnemyKilled OnEnemyKilled;
        public static void EnemyKilled(EnemyController enemy)
        {
            OnEnemyKilled?.Invoke(enemy);
        }

        public delegate void _NextWave(int waveNumber);
        public static event _NextWave OnNextWave;
        public static void NextWave(int waveNumber)
        {
            OnNextWave?.Invoke(waveNumber);
        }

        public delegate void _EnemySpawned(EnemyController enemy);
        public static event _EnemySpawned OnEnemySpawned;
        public static void EnemySpawned(EnemyController enemy)
        {
            OnEnemySpawned?.Invoke(enemy);
        }

        public delegate void _PlayerDied();
        public static event _PlayerDied OnPlayerDied;
        public static void PlayerDied()
        {
            OnPlayerDied?.Invoke();
        }
    }
}
