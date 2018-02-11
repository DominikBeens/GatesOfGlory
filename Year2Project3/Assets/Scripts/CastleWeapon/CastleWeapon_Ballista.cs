using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleWeapon_Ballista : CastleWeapon 
{

    public float speed;

    public override void Update()
    {
        base.Update();

        if (usingWeapon)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, mouseLayerMask))
            {
                mouseObject.transform.position = hit.point;
                mouseObject.transform.LookAt(rotatableWeapon.transform);

                Quaternion newRotation = Quaternion.Euler(-mouseObject.transform.rotation.eulerAngles);
                newRotation = Quaternion.Euler(Mathf.Clamp(newRotation.eulerAngles.x, minXRotation, maxXRotation), -90, newRotation.eulerAngles.z);

                if (side == Side.Left)
                {
                    if (-mouseObject.transform.rotation.eulerAngles.x + newRotation.eulerAngles.x > 1f)
                    {
                        if (mouseObject.transform.position.x < 0)
                        {
                            rotatableWeapon.transform.rotation = newRotation;
                        }
                    }
                }
            }
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * force.currentValue);
    }
}
