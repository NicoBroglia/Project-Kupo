using UnityEngine;

/// <summary>
/// A ScriptableObject that defines a stat's core, unchangeable properties.
/// Create these as assets ("Vigor", "Strength").
/// </summary>
[CreateAssetMenu(fileName = "New Stat Definition", menuName = "Stats/Stat Definition")]
public class StatDefinition : ScriptableObject
{
    [Tooltip("The name of the stat to be displayed in the UI.")]
    public string statName;

    [Tooltip("A description of what the stat does.")]
    [TextArea]
    public string description;
}