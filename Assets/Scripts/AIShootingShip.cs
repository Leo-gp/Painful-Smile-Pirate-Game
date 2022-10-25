using UnityEngine;

public class AIShootingShip : ShootingShip
{
    [field: SerializeField] public float ShootingRange { get; private set; }

    private Ship targetShip;

    private void Start()
    {
        targetShip = FindObjectOfType<ShootingControllableShip>();
        remainingTimeForFrontalShoot = FrontalShootCooldown;
        remainingTimeForSideShoot = SideShootCooldown;
    }

    protected override void Update()
    {
        base.Update();
        if (TargetIsNear() && !isDead)
        {
            FrontalShoot();
        }
    }

    public override void HandleMovement()
    {
        if (!TargetIsNear())
        {
            rb.AddForce(-transform.up * BaseMoveSpeed);
        }
    }

    public override void HandleRotation()
    {
        Vector2 targetDirection = targetShip.transform.position - transform.position;
        transform.up = Vector3.RotateTowards(transform.up, targetDirection, -BaseRotateSpeed * Time.deltaTime, 0.0f);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.IncreaseScore(GameSessionConfiguration.Instance.ScorePerKill);
    }

    private bool TargetIsNear()
    {
        return Vector2.Distance(transform.position, targetShip.transform.position) <= ShootingRange;
    }
}