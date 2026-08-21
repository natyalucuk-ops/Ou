using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;

    [Header("Bullet Visual")]
    public Color bulletColor = new Color(1f, 0.8f, 0.2f, 1f);
    public float bulletSize = 0.2f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        // Buat bullet berbentuk runcing (seperti cahaya)
        CreateBulletSprite();

        Destroy(gameObject, lifeTime);
    }

    void CreateBulletSprite()
    {
        Texture2D tex = new Texture2D(32, 32);
        Color[] colors = new Color[32 * 32];
        for (int i = 0; i < colors.Length; i++) colors[i] = Color.clear;

        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float dx = (x - 16) / 16f;
                float dy = (y - 16) / 16f;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);

                // Bentuk peluru: oval memanjang dengan ujung runcing
                float shape = Mathf.Abs(dy) * 1.5f + Mathf.Abs(dx) * 0.5f;
                if (shape < 0.8f && dist < 0.9f)
                {
                    float alpha = 1f - dist * 0.5f;
                    colors[y * 32 + x] = bulletColor * alpha;
                }
            }
        }

        tex.SetPixels(colors);
        tex.Apply();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        transform.localScale = Vector3.one * bulletSize;
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
