public interface IShootingShip : IShip
{
    CannonBall CannonBallPrefab { get; }
    float ShootSpawnOffset { get; }
    float SideShootBallsOffset { get; }
    float CannonBallDamage { get; }
    float FrontalShootCooldown { get; }
    float SideShootCooldown { get; }
    bool DisableFrontalShoot { get; }
    bool DisableRightShoot { get; }
    bool DisableLeftShoot { get; }

    void FrontalShoot();
    void RightShoot();
    void LeftShoot();
}