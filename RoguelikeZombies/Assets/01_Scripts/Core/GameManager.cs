using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerHealthComp;
    private IDamageable player;

    void Awake() { player = playerHealthComp as IDamageable; HudPresenter.Score = 0; }
    void Update()
    {
        if (player != null && !player.IsAlive && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
