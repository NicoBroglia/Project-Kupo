using UnityEngine;

[RequireComponent(typeof(EquipLoadController))]
public class ActionSpeedController : MonoBehaviour
{
    [System.Serializable]
    public class EquipLoadTier
    {
        [Tooltip("The equip load percentage threshold for this tier (e.g., 30 for fast roll).")]
        public float percentageThreshold;
        [Tooltip("The multiplier for action DURATION. <1 is faster, >1 is slower.")]
        public float actionDurationMultiplier;
    }

    [Header("Configuration")]
    [Tooltip("Define tiers from fastest to slowest. The system will pick the first tier the player qualifies for.")]
    [SerializeField]
    private EquipLoadTier[] _equipLoadTiers = new EquipLoadTier[]
    {
        // Values now represent duration multipliers. Lower is faster.
        new EquipLoadTier { percentageThreshold = 30f, actionDurationMultiplier = 0.7f }, // Fast (70% of base duration)
        new EquipLoadTier { percentageThreshold = 70f, actionDurationMultiplier = 1.0f }, // Medium (normal duration)
        new EquipLoadTier { percentageThreshold = 100f, actionDurationMultiplier = 1.5f } // Slow (150% of base duration)
    };

    public float ActionDurationMultiplier { get; private set; } = 1f;

    private EquipLoadController _equipLoadController;

    private void Awake()
    {
        _equipLoadController = GetComponent<EquipLoadController>();
    }

    private void Update()
    {
        if (_equipLoadController == null) return;

        CalculateActionDuration();
    }

    private void CalculateActionDuration()
    {
        float currentPercentage = _equipLoadController.EquipLoadPercentage;

        // Default to the slowest speed if the player is overencumbered.
        ActionDurationMultiplier = _equipLoadTiers[_equipLoadTiers.Length - 1].actionDurationMultiplier;

        // Iterate through tiers to find the correct one.
        foreach (var tier in _equipLoadTiers)
        {
            if (currentPercentage < tier.percentageThreshold)
            {
                ActionDurationMultiplier = tier.actionDurationMultiplier;
                break; // Exit after finding the first matching tier.
            }
        }
    }
}