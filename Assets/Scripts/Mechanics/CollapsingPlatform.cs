using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    public float collapseDelay = 1.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Invoke(nameof(Collapse), collapseDelay);
        }
    }

    private void Collapse()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
