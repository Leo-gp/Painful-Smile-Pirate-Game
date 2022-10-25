using UnityEngine;

public abstract class ShootingShip : Ship, IShootingShip
{
    [field: SerializeField] public CannonBall CannonBallPrefab { get; private set; }
    [field: SerializeField] public float ShootSpawnOffset { get; private set; }
    [field: SerializeField] public float SideShootBallsOffset { get; private set; }
    [field: SerializeField] public float CannonBallDamage { get; private set; }
    [field: SerializeField] public float FrontalShootCooldown { get; private set; }
    [field: SerializeField] public float SideShootCooldown { get; private set; }
    [field: SerializeField] public bool DisableFrontalShoot { get; private set; }
    [field: SerializeField] public bool DisableRightShoot { get; private set; }
    [field: SerializeField] public bool DisableLeftShoot { get; private set; }

    protected float remainingTimeForFrontalShoot;
    protected float remainingTimeForSideShoot;

    protected virtual void Update()
    {
        UpdateShootCooldown();
    }

    public void FrontalShoot()
    {
        if (!CanFrontalShoot())
        {
            return;
        }
        Vector2 localPositionOffset = Vector2.down * ShootSpawnOffset;
        Vector2 direction = -transform.up;
        Shoot(localPositionOffset, direction);
        remainingTimeForFrontalShoot = FrontalShootCooldown;
    }

    public void RightShoot()
    {
        if (!CanRightShoot())
        {
            return;
        }
        for (int i = -1; i <= 1; i++)
        {
            Vector2 localPositionOffset = new Vector2(-ShootSpawnOffset, -i * SideShootBallsOffset);
            Vector2 direction = -transform.right;
            Shoot(localPositionOffset, direction);
        }
        remainingTimeForSideShoot = SideShootCooldown;
    }

    public void LeftShoot()
    {
        if (!CanLeftShoot())
        {
            return;
        }
        for (int i = -1; i <= 1; i++)
        {
            Vector2 localPositionOffset = new Vector2(ShootSpawnOffset, i * SideShootBallsOffset);
            Vector2 direction = transform.right;
            Shoot(localPositionOffset, direction);
        }
        remainingTimeForSideShoot = SideShootCooldown;
    }

    private void Shoot(Vector2 localPositionOffset, Vector2 direction)
    {
        CannonBall cannonBall = Instantiate(CannonBallPrefab, transform);
        cannonBall.transform.localPosition = (Vector2)cannonBall.transform.localPosition + localPositionOffset;
        cannonBall.Direction = direction;
        cannonBall.Owner = this;
        cannonBall.Damage = CannonBallDamage;
        cannonBall.transform.SetParent(null);
    }

    private bool CanFrontalShoot()
    {
        if (DisableFrontalShoot || remainingTimeForFrontalShoot > 0f)
        {
            return false;
        }
        return true;
    }

    private bool CanRightShoot()
    {
        if (DisableRightShoot || remainingTimeForSideShoot > 0f)
        {
            return false;
        }
        return true;
    }

    private bool CanLeftShoot()
    {
        if (DisableLeftShoot || remainingTimeForSideShoot > 0f)
        {
            return false;
        }
        return true;
    }

    protected virtual void UpdateShootCooldown()
    {
        remainingTimeForFrontalShoot = Mathf.Max(remainingTimeForFrontalShoot - Time.deltaTime, 0f);
        remainingTimeForSideShoot = Mathf.Max(remainingTimeForSideShoot - Time.deltaTime, 0f);
    }
}