using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Ship : MonoBehaviour, IShip
{
    [field: SerializeField] public float BaseMoveSpeed { get; private set; }
    [field: SerializeField] public float BaseRotateSpeed { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public HealthBar HealthBarPrefab { get; private set; }
    [field: SerializeField] public GameObject FirePrefab { get; private set; }
    [field: SerializeField] public float FireTime { get; private set; }
    [field: SerializeField] public List<ShipDeteriorationLevel> ShipDeteriorationLevels { get; private set; }

    protected float currentHealth;
    protected bool isDead;
    protected Rigidbody2D rb;

    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private HealthBar healthBar;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = MaxHealth;
        ShipDeteriorationLevels = ShipDeteriorationLevels.OrderByDescending(e => e.Health).ToList();
        InstantiateHealthBar();
    }

    protected virtual void FixedUpdate()
    {
        if (!isDead)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    public abstract void HandleMovement();
    public abstract void HandleRotation();

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.DecreaseHealth(this, damage);
        HandleShipDeterioration();
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        isDead = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        col.enabled = false;
        Destroy(healthBar.gameObject);
        var fire = Instantiate(FirePrefab, transform.position, Quaternion.identity, transform);
        Destroy(fire, FireTime);
        Destroy(gameObject, FireTime);
    }

    public void InstantiateHealthBar()
    {
        healthBar = Instantiate(HealthBarPrefab);
        healthBar.name = name + "HealthBar";
        healthBar.Target = this;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            CannonBall cannonBall = collision.gameObject.GetComponent<CannonBall>();
            if (cannonBall != null && cannonBall.Owner != this)
            {
                TakeDamage(cannonBall.Damage);
            }
        }
    }

    private void HandleShipDeterioration()
    {
        foreach (var deteriorationLevel in ShipDeteriorationLevels)
        {
            if (currentHealth <= deteriorationLevel.Health)
            {
                spriteRenderer.sprite = deteriorationLevel.Sprite;
            }
        }
    }
}