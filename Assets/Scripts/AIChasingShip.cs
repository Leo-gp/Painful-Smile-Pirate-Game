using UnityEngine;

public class AIChasingShip : Ship
{
    [field: SerializeField] public float Damage { get; private set; }

    private Ship targetShip;
    protected bool exploded;

    private void Start()
    {
        targetShip = FindObjectOfType<ShootingControllableShip>();
    }

    public override void HandleMovement()
    {
        rb.AddForce(-transform.up * BaseMoveSpeed);
    }

    public override void HandleRotation()
    {
        Vector2 targetDirection = targetShip.transform.position - transform.position;
        transform.up = Vector3.RotateTowards(transform.up, targetDirection, -BaseRotateSpeed * Time.deltaTime, 0.0f);
    }

    public override void Die()
    {
        base.Die();
        if (!exploded)
        {
            GameManager.Instance.IncreaseScore(GameSessionConfiguration.Instance.ScorePerKill);
        }
    }

    private void Explode()
    {
        exploded = true;
        targetShip.TakeDamage(Damage);
        TakeDamage(currentHealth);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject == targetShip.gameObject)
        {
            Explode();
        }
    }
}