using System.Collections.Generic;

public class AbilitySystem
{
    private Dictionary<string, IAbility> abilities = new Dictionary<string, IAbility>();

    public void RegisterAbility(string key, IAbility ability)
    {
        abilities[key] = ability;
    }

    public void TryActivate(string key)
    {
        if (abilities.ContainsKey(key))
            abilities[key].Activate();
    }
}

public interface IAbility
{
    void Activate();
}
