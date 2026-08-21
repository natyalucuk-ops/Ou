using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    public float speed = 1.8f;
    public int damage = 1;
    private Transform player;

    [Header("Ghost Visual")]
    public Color ghostColor = new Color(0.7f, 0.8f, 1f, 0.6f);
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 2f;
    public float glowIntensity = 0.3f;

    private SpriteRenderer sr;
    private float startY;
    private float startX;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        // Buat bentuk hantu secara prosedural (lingkaran dengan ekor)
        CreateGhostSprite();

        // Warna hantu
        sr.color = ghostColor;

        // Spawn dari pinggir layar
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 0));
        spawnPos.z = 0;
        transform.position = spawnPos;
        startY = transform.position.y;
        startX = transform.position.x;

        // Efek glow
        if (sr.material == null) sr.material = new Material(Shader.Find("Sprites/Default"));
        sr.material.SetFloat("_Glow", glowIntensity);
    }

    void CreateGhostSprite()
    {
        // Buat texture 64x64 untuk hantu
        Texture2D tex = new Texture2D(64, 64);
        Color[] colors = new Color[64 * 64];
        for (int i = 0; i < colors.Length; i++) colors[i] = Color.clear;

        // Bentuk hantu: lingkaran + ekor bergelombang
        for (int y = 0; y < 64; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                float dx = (x - 32) / 32f;
                float dy = (y - 32) / 32f;

                // Kepala (lingkaran)
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                if (dist < 0.7f && dy > -0.3f)
                {
                    colors[y * 64 + x] = Color.white;
                }
                // Badan (ekor bergelombang)
                else if (dy < -0.3f && dy > -0.9f && Mathf.Abs(dx) < 0.5f + Mathf.Sin(y * 0.3f) * 0.15f)
                {
                    colors[y * 64 + x] = Color.white;
                }
                // Mata
                else if (dy > 0.1f && dy < 0.4f && Mathf.Abs(dx - 0.2f) < 0.1f)
                {
                    colors[y * 64 + x] = new Color(0.2f, 0.1f, 0.1f, 1f);
                }
                else if (dy > 0.1f && dy < 0.4f && Mathf.Abs(dx + 0.2f) < 0.1f)
                {
                    colors[y * 64 + x] = new Color(0.2f, 0.1f, 0.1f, 1f);
                }
                // Mulut (senyum tipis)
                else if (dy > -0.05f && dy < 0.05f && Mathf.Abs(dx) < 0.2f)
                {
                    colors[y * 64 + x] = new Color(0.3f, 0.2f, 0.2f, 0.8f);
                }
            }
        }

        tex.SetPixels(colors);
        tex.Apply();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        if (player == null) return;

        // Gerakan ke arah player + efek melayang
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * speed * Time.deltaTime;

        // Efek terapung (float)
        float floatOffset = Mathf.Sin(Time.time * floatSpeed + transform.position.x) * floatAmplitude * Time.deltaTime;
        move.y += floatOffset;

        transform.position += move;

        // Rotasi halus
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 3f);

        // Efek glow berdenyut
        if (sr.material != null)
        {
            float glow = glowIntensity + Mathf.Sin(Time.time * 2f) * 0.1f;
            sr.material.SetFloat("_Glow", glow);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            FindObjectOfType<PlayerController>().AddScore(1);
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
