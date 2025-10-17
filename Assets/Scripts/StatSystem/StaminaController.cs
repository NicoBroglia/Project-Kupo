using UnityEngine;

[RequireComponent(typeof(StatController))]
public class StaminaController : MonoBehaviour
{
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }

    [Header("Configuration")]
    [Tooltip("Drag your 'Endurance' StatDefinition Scriptable Object here.")]
    [SerializeField] private StatDefinition enduranceStat;
    [SerializeField] private float staminaRegenRate = 20f;
    [SerializeField] private float staminaRegenDelay = 1.5f;

    private StatController _statController;
    private float _staminaRegenTimer;

    private void Awake()
    {
        if (_statController == null)
        {
            _statController = GetComponent<StatController>();
        }
        else
        {
            Debug.LogWarning("StatController component is missing on the GameObject!", this);
        }
    }

    private void Start()
    {
        // Calculate initial max stamina and set current stamina
        CalculateMaxStamina();
        CurrentStamina = MaxStamina;
    }

    private void Update()
    {
        if (_staminaRegenTimer <= 0f)
        {
            if (CurrentStamina < MaxStamina)
            {
                CurrentStamina += staminaRegenRate * Time.deltaTime;
                CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
            }
        }
        else
        {
            _staminaRegenTimer -= Time.deltaTime;
        }
    }

    public bool TryConsumeStamina(float amount)
    {
        if (CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            _staminaRegenTimer = staminaRegenDelay;
            return true;
        }
        return false;
    }

    private void CalculateMaxStamina()
    {
        CharacterStat endurance = _statController.GetStat(enduranceStat);
        if (endurance != null)
        {
            MaxStamina = 50 + (endurance.Value * 5f);
        }
        else
        {
            Debug.LogWarning("Endurance StatDefinition not assigned on the StaminaController!", this);
            MaxStamina = 50; // Default value
        }
    }
}