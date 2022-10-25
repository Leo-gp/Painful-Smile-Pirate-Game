using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int MaxLifeTime { get; private set; }
    [field: SerializeField] public AnimationClip ExplosionClip { get; private set; }

    public Vector2 Direction { get; set; }
    public float Damage { get; set; }
    public Ship Owner { get; set; }

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        Direction = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.velocity = Direction * Speed;
        }
    }

    private void Explode()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        col.enabled = false;
        animator.SetTrigger("Explode");
        Destroy(gameObject, ExplosionClip.length);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship ship = collision.gameObject.GetComponent<Ship>();
            if (ship != null && ship == Owner)
            {
                return;
            }
        }
        else if (collision.gameObject.CompareTag("CannonBall"))
        {
            Physics2D.IgnoreCollision(col, collision.collider);
            return;
        }
        Explode();
    }
}