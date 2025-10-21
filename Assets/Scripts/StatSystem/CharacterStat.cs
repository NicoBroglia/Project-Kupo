using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System; // Required for Action

/// This class that represents a single stat on a character, including its base value and modifiers.
[System.Serializable]
public class CharacterStat
{
    // A reference to the ScriptableObject that defines this stat.
    public StatDefinition definition;

    // Event that is fired whenever the stat's final value is changed.
    public event Action OnStatChanged;

    [SerializeField] private float baseValue;
    public float BaseValue
    {
        get { return baseValue; }
        set
        {
            baseValue = value;
            OnStatChanged?.Invoke(); // Fire the event when the base value is changed.
        }
    }

    // A list of all modifiers currently affecting this stat.
    private readonly List<StatModifier> _modifiers = new List<StatModifier>();

    /// The final, calculated value of the stat.
    public float Value
    {
        get
        {
            float finalValue = baseValue;
            // First, apply all flat modifiers.
            _modifiers.Where(m => m.type == StatModType.Flat).ToList().ForEach(m => finalValue += m.value);
            // Then, apply all percentage modifiers.
            _modifiers.Where(m => m.type == StatModType.Percent).ToList().ForEach(m => finalValue *= 1 + m.value);
            return finalValue;
        }
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
        OnStatChanged?.Invoke(); // Fire the event when a modifier is added.
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
        OnStatChanged?.Invoke(); // Fire the event when a modifier is removed.
    }
}

// Represents a temporary or conditional modification to a stat.
[System.Serializable]
public class StatModifier
{
    public float value;
    public StatModType type;
}

public enum StatModType
{
    Flat,
    Percent
}

