using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleWeapon_Ballista : CastleWeapon 
{

    public float speed;

    public override void Update()
    {
        base.Update();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, mouseLayerMask))
        {
            mouseObject.transform.position = hit.point;
            mouseObject.transform.LookAt(rotatableWeapon.transform);

            // Constrain rotation here.
            rotatableWeapon.transform.rotation = Quaternion.Euler(-mouseObject.transform.rotation.eulerAngles);
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * force.currentValue);
    }
}
