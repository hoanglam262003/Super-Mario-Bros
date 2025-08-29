using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 10f;

    private bool isInShell;
    private bool pushed;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private EntityMovement entityMovement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        entityMovement = GetComponent<EntityMovement>();
    }

    private void Update()
    {
        if (!isInShell)
        {
            if (entityMovement != null && entityMovement.enabled)
            {
                if (entityMovement.direction.x > 0.01f) spriteRenderer.flipX = true;
                else if (entityMovement.direction.x < -0.01f) spriteRenderer.flipX = false;
            }
            else
            {
                if (rb != null && Mathf.Abs(rb.linearVelocity.x) > 0.01f)
                {
                    spriteRenderer.flipX = rb.linearVelocity.x < 0;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInShell && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.starPower)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInShell && other.CompareTag("Player"))
        {    
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();
                if (player.starPower)
                {
                    Hit();
                }
                else
                {
                    player.Hit();
                }
            }
        }

        else if (!isInShell && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        isInShell = true;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        EntityMovement entityMovement = GetComponent<EntityMovement>();
        entityMovement.direction = direction.normalized;
        entityMovement.speed = 5f;
        entityMovement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }

    //private void OnBecameInvisible()
    //{
    //    if (pushed)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
