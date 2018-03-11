using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleWeapon_Catapult : CastleWeapon
{

    [Header("Ballista Setup")]
    public Slider forceSlider;

    public float minForce;
    public float maxForce;

    public override void Awake()
    {
        base.Awake();

        forceSlider.minValue = minForce;
        forceSlider.maxValue = maxForce;

        forceSlider.value = 0.5f * maxForce;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        base.Shoot();

        anim.SetTrigger("Fire");
    }

    public void FireProjectile()
    {
        GameObject newProjectile = ObjectPooler.instance.GrabFromPool("catapult projectile", projectileSpawns[0].position, projectileSpawns[0].rotation);

        newProjectile.GetComponent<Projectile>().myDamage = damage.currentValue;
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * forceSlider.value, ForceMode.Impulse);
    }
}
