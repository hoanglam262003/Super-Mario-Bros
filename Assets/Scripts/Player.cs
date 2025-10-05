using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;
    private bool isInvincible;

    public bool isBig => bigRenderer.enabled;
    public bool isSmall => smallRenderer.enabled;
    public bool isDead => deathAnimation.enabled;
    public bool starPower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }
    public void Hit()
    {
        if (!isDead && !starPower && !isInvincible)
        {
            if (isBig)
            {
                Shrink();               
            }
            else
            {
                Die();
            }
        }
    }


    private void Die()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        bigRenderer.enabled = true;
        smallRenderer.enabled = false;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        isInvincible = true;
        float elapsed = 0f;
        float duration = 1.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (Time.frameCount % 6 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
        isInvincible = false;

        Collider2D hit = Physics2D.OverlapBox(transform.position, capsuleCollider.size, 0f, LayerMask.GetMask("Enemy"));
        if (hit != null)
        {
            Die();
        }
    }

    public void StarPower(float duration = 10f)
    {
        StartCoroutine(StarPowerAnimation(duration));

    }

    private IEnumerator StarPowerAnimation(float duration)
    {
        starPower = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starPower = false;
    }
}
