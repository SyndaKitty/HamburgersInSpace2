public interface Projectile
{
    public float Weight { get; set; }
    void Init();
    void Damage(EnemyController enemy);
    void Update();
    void Destroy();
}