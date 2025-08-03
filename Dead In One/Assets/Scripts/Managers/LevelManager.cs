using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerController player;

    public static LevelManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void OnEnable()
    {
        player.PlayerDeathEvent += HandlePlayerDeath;
    }

    void OnDisable()
    {
        player.PlayerDeathEvent -= HandlePlayerDeath;
    }

    void HandlePlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
