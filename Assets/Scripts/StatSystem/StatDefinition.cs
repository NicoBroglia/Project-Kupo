using UnityEngine;

/// A ScriptableObject that defines a stat's core, unchangeable properties.
/// This allows for easy creation and management of stats as assets in the project.
[CreateAssetMenu(fileName = "New Stat Definition", menuName = "Stats/Stat Definition")]
public class StatDefinition : ScriptableObject
{
    [Tooltip("The name of the stat to be displayed in the UI.")]
    public string statName;

    [Tooltip("A description of what the stat does.")]
    [TextArea]
    public string description;
}