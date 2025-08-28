using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        GetComponent<EntityMovement>().enabled = false;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCol = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCol = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        physicsCol.enabled = false;
        triggerCol.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + Vector3.up;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;

        rb.bodyType = RigidbodyType2D.Dynamic;
        physicsCol.enabled = true;
        triggerCol.enabled = true;

        GetComponent<EntityMovement>().enabled = true;
    }
}
