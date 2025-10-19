using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// Represents a single stat on a character, calculating its final value from a base value and a list of modifiers.

[System.Serializable]
public class CharacterStat
{
    // A reference to the ScriptableObject that defines this stat.
    public StatDefinition definition;
    [SerializeField] private float _baseValue;

    // We make the list readonly to prevent external scripts from replacing it,
    // but we can still add/remove modifiers through methods.
    private readonly List<StatModifier> _modifiers = new List<StatModifier>();

    /// Calculates the final value of the stat by applying all modifiers.
    /// This property ensures the value is always up-to-date.
    public float Value
    {
        get
        {
            float finalValue = _baseValue;
            // First, apply all flat modifiers.
            _modifiers.Where(m => m.type == StatModType.Flat)
                      .ToList()
                      .ForEach(m => finalValue += m.value);

            // Then, apply all percentage-based modifiers to the modified flat value.
            float percentSum = 0;
            _modifiers.Where(m => m.type == StatModType.Percent)
                      .ToList()
                      .ForEach(m => percentSum += m.value);

            finalValue *= 1 + (percentSum / 100);

            return finalValue;
        }

        /*
        LEGACY CODE
        get { return baseValue + _modifiers.Sum(mod => mod.value); }
        */
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
    }
}

/// Represents a modification to a stat, which can be flat or percentage-based.
public enum StatModType
{
    Flat,
    Percent
}

/// Represents a temporary or conditional modification to a stat.
[System.Serializable]
public class StatModifier
{
    public float value;
    public StatModType type;
    // This can be expanded later to include a source (e.g., "From Fire Sword")
    // or a type (e.g., Flat vs. Percentage).
}