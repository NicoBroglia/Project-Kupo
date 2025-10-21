using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatController : MonoBehaviour
{
    public List<CharacterStat> stats = new List<CharacterStat>();

    public CharacterStat GetStat(StatDefinition statDef)
    {
        return stats.FirstOrDefault(s => s.definition == statDef);
    }

    public void AddStatModifier(StatDefinition statDef, StatModifier modifier)
    {
        CharacterStat stat = GetStat(statDef);
        if (stat != null)
        {
            stat.AddModifier(modifier);
            Debug.Log($"Added modifier of {modifier.value} to {statDef.statName}. New value: {stat.Value}");
        }
    }

    public void RemoveStatModifier(StatDefinition statDef, StatModifier modifier)
    {
        CharacterStat stat = GetStat(statDef);
        if (stat != null)
        {
            stat.RemoveModifier(modifier);
            Debug.Log($"Removed modifier of {modifier.value} from {statDef.statName}. New value: {stat.Value}");
        }
    }
}