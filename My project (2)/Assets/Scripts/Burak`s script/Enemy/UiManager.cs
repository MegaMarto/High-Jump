using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyHealthText;

    private void OnEnable()
    {
        EventManager.OnEnemyHealthChanged += UpdateEnemyHealthUI;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyHealthChanged -= UpdateEnemyHealthUI;
    }

    private void UpdateEnemyHealthUI(float currentHealth, float maxHealth)
    {
        enemyHealthText.text = $"Enemy Health: {currentHealth}/{maxHealth}";
    }
}

