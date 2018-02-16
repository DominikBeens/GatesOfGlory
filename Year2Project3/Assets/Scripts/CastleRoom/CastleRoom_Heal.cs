using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleRoom_Heal : CastleRoom 
{

    [Header("Properties")]
    public Stat useCooldown;
    public Stat healAmount;
    public GameObject healParticle;

    public override void SetupUI()
    {
        base.SetupUI();
    }

    public override void UseRoom()
    {
        base.UseRoom();

        for (int i = 0; i < WaveManager.instance.alliesInScene.Count; i++)
        {
            WaveManager.instance.alliesInScene[i].TakeDamage(-healAmount.currentValue);

            Vector3 particleSpawn = new Vector3(WaveManager.instance.alliesInScene[i].transform.position.x, WaveManager.instance.alliesInScene[i].transform.position.y + 1.5f);
            Instantiate(healParticle, particleSpawn, Quaternion.identity);
        }
    }
}
