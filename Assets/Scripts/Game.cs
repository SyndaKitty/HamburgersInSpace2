using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance;
    
    public float VolumeModifier = 1;
    public PlayerController Player;
    public OneShotSound ActivateCheckPointSound;
    public float PlayerSpawnWaitTime = 2f;
    public float TimeBetweenRounds = 5f;
    
    public AnimationCurve[] EnemySpawnWaitTime;

    public PlayerCheckPoint ActiveCheckpoint;

    public delegate void NextRound(string value);
    public event NextRound OnNextRound;

    public EnemyController BasicBurger;
    public EnemyController BasicBurgerChaser;
    public EnemyController Donut;
    public EnemyController DoubleBurger;
    public EnemyController DoubleBurgerChaser;

    public Whirlpool Whirlpool;
    public EntityController Entity;

    Transform topLeft;
    Transform bottomRight;

    Queue<(EnemyController, int)> ToSpawn = new Queue<(EnemyController, int)>();
    List<EnemyController> SpawnedEnemies = new List<EnemyController>();

    int round;
    int toKill;

    Music music;
    void Awake() {
        Instance = this;
    }

    void Start() {
        topLeft = transform.Find("TopLeft");
        bottomRight = transform.Find("BottomRight");

        Player = FindObjectOfType<PlayerController>();
        
        StartCoroutine(StartNextRound());
        
        music = FindObjectOfType<Music>();
        if (!music) return;
        music.PlayTrack(2);

    }

    IEnumerator StartNextRound() {
        yield return new WaitForSeconds(TimeBetweenRounds);
        QueueSpawns();
        toKill = 0;
        while (ToSpawn.Count > 0) {
            var next = ToSpawn.Dequeue();
            SpawnEnemy(next.Item1);
            toKill++;
            yield return new WaitForSeconds(EnemySpawnWaitTime[next.Item2].Evaluate(round));
        }
        while (toKill > 0) {
            Debug.Log(toKill);
            yield return null;
        }

        // Round Complete
        StartCoroutine(StartNextRound());
    }

    void QueueSpawns() {
        round++;
        OnNextRound?.Invoke(round.ToString());
        
        if (round == 1) {
            QueueSpawn(BasicBurger, 2, 0);
        }
        else if (round == 2) {
            QueueSpawn(BasicBurger, 2, 0);
            QueueSpawn(Donut, 3, 1);
        }
        else if (round == 3) {

        }
        else if (round == 4) {
            
        }
        else if (round == 5) {
            
        }
        else if (round == 6) {
            
        }
        else if (round == 7) {
            
        }
        else if (round == 8) {
            
        }
        else if (round == 9) {
            
        }
        else if (round == 10) {
            SpawnEntity();
        }
        else if (round == 11) {
            
        }
        else if (round == 12) {
            
        }
        else if (round == 13) {
            
        }
        else if (round == 14) {
            
        }
        else if (round == 15) {
            
        }
        else if (round == 16) {
            
        }
        else if (round == 17) {
            
        }
        else if (round == 18) {
            
        }
        else if (round == 19) {
            
        }
        else if (round == 20) {
            SpawnUberBurger();
        }

        OnNextRound?.Invoke(round.ToString());
    }

    void QueueSpawn(EnemyController enemy, int amount, int type) {
        for (int i = 0; i < amount; i++) {
            ToSpawn.Enqueue((enemy, type));
        }
    }

    void SpawnEnemy(EnemyController enemy) {
        var whirlpool = Instantiate(Whirlpool);
        whirlpool.transform.position = GetValidRandomSpawnPosition(Vector2.one);
        whirlpool.ToSpawn = enemy;
    }

    void SpawnEntity() {
        var entity = Instantiate(Entity);

    }

    void SpawnUberBurger() {

    }

    public void Spawned(EnemyController enemy) {
        SpawnedEnemies.Add(enemy);
    }

    public void Killed(EnemyController enemy) {
        SpawnedEnemies.Remove(enemy);
        toKill--;
    }

    public Vector2 GetValidRandomSpawnPosition(Vector2 size) {
        var tl = topLeft.transform.position;
        var br = bottomRight.transform.position;
        while (true) {
            Vector2 pos = new Vector2(Random.Range(tl.x, br.x), Random.Range(br.y, tl.y));
            var col = Physics2D.OverlapBox(pos, size, 0f);
            if (col == null) {
                return pos;
            }
        }
    }

    public void SetActiveCheckPoint(PlayerCheckPoint checkpoint) {
        if (ActiveCheckpoint == checkpoint) {
            return;
        }
        if (ActiveCheckpoint) {
            ActiveCheckpoint.SetInactive();
        }
        checkpoint.SetActive();
        ActiveCheckpoint = checkpoint;
        if (ActivateCheckPointSound) {
            Instantiate(ActivateCheckPointSound);
        }
    }

    public void PlayerDied() {
        StartCoroutine(SpawnPlayerAfterWait());
    }

    IEnumerator SpawnPlayerAfterWait() {
        yield return new WaitForSeconds(PlayerSpawnWaitTime);
        if (ActiveCheckpoint == null) {
            ActiveCheckpoint = FindObjectOfType<PlayerCheckPoint>();
        }
        Player.transform.position = ActiveCheckpoint.transform.position;
        Player.Respawn();
    }
}