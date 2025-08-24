using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public Sprite deathSprite;
    public SpriteRenderer spriteRenderer;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (enabled && deathSprite != null)
        {
            UpdateSprite();
        }
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animated());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;

        if (deathSprite != null)
        {
            spriteRenderer.sprite = deathSprite;
        }
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement = GetComponent<EntityMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (entityMovement != null)
        {
            entityMovement.enabled = false;
        }
    }

    private IEnumerator Animated()
    {
        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector3.up * jumpVelocity;

        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
