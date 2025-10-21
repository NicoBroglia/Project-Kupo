using UnityEngine;
using System;

[RequireComponent(typeof(StatController))]
public class HealthController : MonoBehaviour
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    // Event for the UI to listen to. Passes (currentHealth, maxHealth).
    public event Action<float, float> OnHealthChanged;

    [Header("Configuration")]
    [Tooltip("Drag your 'Vigor' StatDefinition asset here.")]
    [SerializeField] private StatDefinition vigorStatDefinition;
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float healthPerVigorPoint = 15f;

    private StatController _statController;
    private CharacterStat _vigorStat;

    private void Awake()
    {
        _statController = GetComponent<StatController>();
    }

    private void Start()
    {
        _vigorStat = _statController.GetStat(vigorStatDefinition);

        if (_vigorStat != null)
        {
            _vigorStat.OnStatChanged += OnVigorStatChanged;
        }
        else
        {
            Debug.LogError("Vigor StatDefinition not found on StatController!", this);
        }

        // Calculate initial health values first.
        CalculateInitialHealth();
    }

    private void OnDestroy()
    {
        if (_vigorStat != null)
        {
            _vigorStat.OnStatChanged -= OnVigorStatChanged;
        }
    }

    private void CalculateInitialHealth()
    {
        // Calculate the max health based on Vigor.
        MaxHealth = baseHealth + (_vigorStat.Value * healthPerVigorPoint);
        // Set the current health to be full.
        CurrentHealth = MaxHealth;
        // NOW, notify the UI with the correct, final values.
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void OnVigorStatChanged()
    {
        // This method is called only when the Vigor stat itself changes (e.g., leveling up).
        float oldMaxHealth = MaxHealth;
        MaxHealth = baseHealth + (_vigorStat.Value * healthPerVigorPoint);

        // Preserve the same health percentage when max health changes.
        CurrentHealth = CurrentHealth * (MaxHealth / oldMaxHealth);

        // Notify the UI that health has changed.
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        // Add logic for player death here later.
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}