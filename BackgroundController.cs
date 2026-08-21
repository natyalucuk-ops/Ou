using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float scrollSpeed = 0.2f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Geser background perlahan untuk efek bergerak
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
    }
}
