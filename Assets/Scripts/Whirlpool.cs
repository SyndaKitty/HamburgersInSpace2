using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    public EnemyController ToSpawn;

    public float SpawnTime;
    public AnimationCurve RotationSpeed;
    public float ExpandTime;
    public float ExpandAmount;
    public float AppearTime = 1f;

    SpriteRenderer sr;
    bool spawned;
    float t;
    bool appeared;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        t += Time.deltaTime;

        if (!appeared)
        {
            transform.rotation *= Quaternion.Euler(0, 0, -RotationSpeed.Evaluate(0) * Time.deltaTime);
            var color = sr.color;
            color.a = t / AppearTime;
            sr.color = color;
            if (t > AppearTime)
            {
                appeared = true;
                t = 0;
            }
            return;
        }
        transform.rotation *= Quaternion.Euler(0, 0, -RotationSpeed.Evaluate(t / SpawnTime) * Time.deltaTime);

        if (!spawned)
        {
            if (t > SpawnTime)
            {
                var enemy = Instantiate(ToSpawn);
                enemy.transform.position = transform.position;
                spawned = true;
            }
        }
        else
        {
            float newT = t - SpawnTime;
            transform.localScale = Vector3.one * (1 + ExpandAmount * newT / ExpandTime);
            var color = sr.color;
            color.a = 1 - newT / ExpandTime;
            sr.color = color;

            if (newT > ExpandTime)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
