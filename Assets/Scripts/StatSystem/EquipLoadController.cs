using UnityEngine;

[RequireComponent(typeof(StatController))]
public class EquipLoadController : MonoBehaviour
{
    public float MaxEquipLoad { get; private set; }
    public float EquipLoadPercentage { get; private set; }

    [Header("Configuration")]
    [SerializeField] private StatDefinition enduranceStat;
    [SerializeField] private float baseEquipLoad = 0f;
    [SerializeField] private float equipLoadPerEndurance = 2f;

    [Header("Current State (for testing)")]
    [Tooltip("Set the character's current load as a percentage of their maximum.")]
    [Range(0f, 100f)]
    [SerializeField] private float _testEquipLoadPercentage = 50f;

    // We keep the actual float value for other systems, but hide it from the inspector.
    private float _currentEquipLoad;

    private StatController _statController;

    private void Awake()
    {
        _statController = GetComponent<StatController>();
    }

    private void Update()
    {
        if (_statController == null) return;

        // First, calculate the absolute maximum load based on endurance.
        CalculateMaxEquipLoad();

        // Then, calculate the current load based on the percentage slider.
        _currentEquipLoad = MaxEquipLoad * (_testEquipLoadPercentage / 100f);

        // Finally, calculate the final percentage (this will match the slider, but it's good practice to keep the calculation).
        CalculateEquipLoadPercentage();
    }

    private void CalculateMaxEquipLoad()
    {
        CharacterStat endurance = _statController.GetStat(enduranceStat);
        if (endurance != null)
        {
            MaxEquipLoad = baseEquipLoad + (endurance.Value * equipLoadPerEndurance);
        }
        else
        {
            MaxEquipLoad = baseEquipLoad;
        }
    }

    private void CalculateEquipLoadPercentage()
    {
        if (MaxEquipLoad > 0)
        {
            EquipLoadPercentage = (_currentEquipLoad / MaxEquipLoad) * 100f;
        }
        else
        {
            EquipLoadPercentage = 0f;
        }
    }
}