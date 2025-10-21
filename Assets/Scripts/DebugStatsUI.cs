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

    [Header("Stat Definitions")]
    [SerializeField] private StatDefinition enduranceStatDefinition; // Assign in inspector

    private CharacterStat _enduranceStat;

    private void Start()
    {
        if (statController != null)
        {
            // Cache the stat reference once at the start.
            _enduranceStat = statController.GetStat(enduranceStatDefinition);
        }
    }

    private void Update()
    {
        if (statController == null || staminaController == null) return;

        // Update UI with cached stat.
        if (_enduranceStat != null)
        {
            enduranceText.text = $"Endurance: {_enduranceStat.Value}";
        }

        staminaText.text = $"Stamina: {staminaController.CurrentStamina:F0} / {staminaController.MaxStamina:F0}";
    }

    public void SetTarget(StatController stats, StaminaController stamina)
    {
        statController = stats;
        staminaController = stamina;
        // Recache the stat if the target changes.
        if (statController != null)
        {
            _enduranceStat = statController.GetStat(enduranceStatDefinition);
        }
    }
}
