
[System.Serializable]
public class Stats
{
    public Stat health;
    public Stat damage;

    public void ChangeStats(float _healthMultiplier, float _damageMultiplier)
    {
        health.currentValue = health.baseValue * _healthMultiplier;
        damage.currentValue = damage.baseValue * _damageMultiplier;
    }
}