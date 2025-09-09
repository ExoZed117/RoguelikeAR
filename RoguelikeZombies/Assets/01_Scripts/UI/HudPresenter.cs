using TMPro;
using UnityEngine;

public class HudPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private MonoBehaviour healthSource; // Componente que implemente IHealthReadable
    private IHealthReadable health;
    public static int Score = 0;

    void Awake() { health = healthSource as IHealthReadable; }
    void Update()
    {
        if (health != null) hpText.text = $"HP: {Mathf.CeilToInt(health.Current)}";
        scoreText.text = $"Score: {Score}";
    }
}
