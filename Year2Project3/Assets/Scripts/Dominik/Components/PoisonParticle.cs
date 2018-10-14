using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonParticle : MonoBehaviour 
{

    private List<Enemy> damagedEnemies = new List<Enemy>();

    public int damage;

    private void OnParticleCollision(GameObject other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (!damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    public void OnParticleSystemStopped()
    {
        damagedEnemies.Clear();
    }
}
