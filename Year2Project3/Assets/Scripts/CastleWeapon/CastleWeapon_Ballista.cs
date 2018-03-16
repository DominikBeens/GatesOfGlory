using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleWeapon_Ballista : CastleWeapon 
{

    [Header("Ballista Setup")]
    public float rotationSpeed;
    public Slider rotationSlider;

    public override void Awake()
    {
        base.Awake();

        rotationSlider.minValue = minXRotation;
        rotationSlider.maxValue = maxXRotation;
    }

    public override void Update()
    {
        base.Update();

        if (usingBuilding)
        {
            Quaternion newRotation;

            if (side == Side.Left)
            {
                newRotation = Quaternion.Euler(rotationSlider.value, -90, 0);
            }
            else
            {
                newRotation = Quaternion.Euler(rotationSlider.value, 90, 0);
            }

            rotatableWeapon.transform.rotation = Quaternion.RotateTowards(rotatableWeapon.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        anim.SetTrigger("Fire");
    }

    public void FireProjectile()
    {
        GameObject newProjectile;

        switch (amountOfProjectiles)
        {
            case AmountOfProjectiles.One:

                newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", projectileSpawns[0].position, projectileSpawns[0].rotation);

                newProjectile.GetComponent<Projectile>().myDamage = damage.currentValue;
                newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * (force.currentValue + Random.Range(-15, 15)));
                break;
            case AmountOfProjectiles.Three:

                for (int i = 0; i < projectileSpawns.Count; i++)
                {
                    newProjectile = ObjectPooler.instance.GrabFromPool("ballista projectile", projectileSpawns[i].position, projectileSpawns[i].rotation);

                    newProjectile.GetComponent<Projectile>().myDamage = damage.currentValue;
                    newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * (force.currentValue + Random.Range(-15, 15)));
                }
                break;
        }
    }
}
