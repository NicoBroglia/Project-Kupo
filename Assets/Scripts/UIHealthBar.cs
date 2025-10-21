using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider
using TMPro; // Required for TextMeshPro

[RequireComponent(typeof(Slider))]
public class UIHealthBar : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("Drag the player object (or the object with the HealthController) here.")]
    [SerializeField] private HealthController healthController;
    [Tooltip("Optional: Drag a TextMeshProUGUI element here to display health numbers.")]
    [SerializeField] private TextMeshProUGUI healthText;

    private Slider _healthSlider;

    private void Awake()
    {
        _healthSlider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (healthController != null)
        {
            // Subscribe to the health changed event.
            healthController.OnHealthChanged += UpdateHealthBar;
            // Set initial values for both the slider and the text by calling the update method once.
            UpdateHealthBar(healthController.CurrentHealth, healthController.MaxHealth);
        }
    }

    private void OnDisable()
    {
        if (healthController != null)
        {
            // Unsubscribe to prevent errors.
            healthController.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Update the slider's value (a percentage from 0 to 1).
        if (maxHealth > 0)
        {
            _healthSlider.value = currentHealth / maxHealth;
        }

        // Update the text display if it has been assigned.
        if (healthText != null)
        {
            // We use CeilToInt to show whole numbers, e.g., "85 / 100".
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }
}

