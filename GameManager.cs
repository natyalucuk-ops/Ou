using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    private float timer = 0f;
    private PlayerController player;

    [Header("Custom Background")]
    public Sprite customBackground; // Drag foto gedung malam di sini

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // ===== SET BACKGROUND =====
        if (customBackground != null)
        {
            Camera.main.gameObject.AddComponent<SpriteRenderer>();
            Camera.main.GetComponent<SpriteRenderer>().sprite = customBackground;
            Camera.main.GetComponent<SpriteRenderer>().sortingOrder = -10;
            // Atur ukuran background agar muat
            float screenRatio = (float)Screen.width / Screen.height;
            float bgRatio = customBackground.bounds.size.x / customBackground.bounds.size.y;
            float orthoSize = 5f;
            if (bgRatio > screenRatio)
            {
                orthoSize = 5f * bgRatio / screenRatio;
            }
            Camera.main.orthographicSize = orthoSize;
        }
        else
        {
            // Fallback: background gelap
            Camera.main.backgroundColor = new Color(0.05f, 0.02f, 0.01f);
        }
    }

    void Update()
    {
        if (player == null || player.currentHealth <= 0) return;

        timer += Time.deltaTime;
        float currentSpawnInterval = Mathf.Max(0.3f, spawnInterval - player.level * 0.1f);
        if (timer >= currentSpawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
