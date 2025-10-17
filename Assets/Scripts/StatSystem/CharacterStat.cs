using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// This class that represents a single stat on a character, including its base value and modifiers.
[System.Serializable]
public class CharacterStat
{
    // A reference to the ScriptableObject that defines this stat.
    public StatDefinition definition;

    [SerializeField] private float baseValue;

    // A list of all modifiers currently affecting this stat.
    private readonly List<StatModifier> _modifiers = new List<StatModifier>();

    /// The final, calculated value of the stat.
    /// It's a read-only property that calculates the value on the fly.
    public float Value
    {
        get { return baseValue + _modifiers.Sum(mod => mod.value); }
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

/// Represents a temporary or conditional modification to a stat./// </summary>
[System.Serializable]
public class StatModifier
{
    public float value;
    // This can be expanded later to include a source (e.g., "From Fire Sword")
    // or a type (e.g., Flat vs. Percentage).
}