using System;
using System.Linq;
using UnityEngine;

public class ActorInfo : MonoBehaviour
{
    [SerializeField] private Creature _creatureData;
    [SerializeField] private float _currentHealth;

    public Creature CurrentData => _creatureData;
    public float CurrentHealth => _currentHealth;

    public event Action OnDeath;
    public event Action<float> OnDamageTaken;
    public event Action<float> OnHealed;

    public void Init(Creature creature)
    {
        _creatureData = creature;
        _currentHealth = GetStat(StatsType.Health);
    }

    public float GetStat(StatsType type)
    {
        Stats stat = _creatureData.stats.FirstOrDefault(s => s.type == type);
        return stat != null ? stat.Value : 0f;
    }

    public void ApplyDamage(float amount)
    {
        _currentHealth -= amount;

        OnDamageTaken?.Invoke(amount);

        if(_currentHealth <= 0)
        {
            _currentHealth = 0f;
            OnDeath?.Invoke();
        }
    }

    public void ApplyHeal(float amount)
    {
        _currentHealth += amount;
        float maxHealth = GetStat(StatsType.Health);

        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }

        OnHealed?.Invoke(amount);
    }
}
