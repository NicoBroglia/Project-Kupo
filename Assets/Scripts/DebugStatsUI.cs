using UnityEngine;
using TMPro;

public class DebugStatsUI : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private StatController statController;
    [SerializeField] private StaminaController staminaController;

    [Header("UI Text Fields")]
    [SerializeField] private TextMeshProUGUI enduranceText;
    [SerializeField] private TextMeshProUGUI staminaText;

    private void Update()
    {
        // Ensure we have the references before trying to update the UI
        if (statController == null || staminaController == null) return;

        // Find the Endurance StatDefinition to get its value
        // Note: This is not super efficient to do in Update, but for a debug UI, it's fine.
        // A better way would be to cache the StatDefinition.
        CharacterStat enduranceStat = null;
        foreach (var stat in statController.stats)
        {
            if (stat.definition.statName == "Endurance")
            {
                enduranceStat = stat;
                break;
            }
        }

        // Update the UI text fields
        if (enduranceStat != null)
        {
            enduranceText.text = $"Endurance: {enduranceStat.Value}";
        }

        staminaText.text = $"Stamina: {staminaController.CurrentStamina:F0} / {staminaController.MaxStamina:F0}";
    }

    // A public method to easily set the target controller from another script if needed
    public void SetTarget(StatController stats, StaminaController stamina)
    {
        statController = stats;
        staminaController = stamina;
    }
}