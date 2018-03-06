using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleWeapon_Catapult : CastleWeapon
{

    [Header("Ballista Setup")]
    public Slider forceSlider;

    public float minForce;
    public float maxFloat;

    public override void Awake()
    {
        base.Awake();

        forceSlider.minValue = minForce;
        forceSlider.maxValue = maxFloat;
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
        GameObject newProjectile = Instantiate(projectile, projectileSpawns[0].position, projectileSpawns[0].rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * forceSlider.value);
    }
}
