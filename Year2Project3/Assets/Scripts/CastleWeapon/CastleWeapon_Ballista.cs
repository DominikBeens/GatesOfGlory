using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleWeapon_Ballista : CastleWeapon 
{

    public float rotationSpeed;
    public Slider rotationSlider;

    public void Awake()
    {
        rotationSlider.minValue = minXRotation;
        rotationSlider.maxValue = maxXRotation;
    }

    public override void Update()
    {
        base.Update();

        if (usingWeapon)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, 100f, mouseLayerMask))
            //{
            //    mouseObject.transform.position = hit.point;
            //    mouseObject.transform.LookAt(rotatableWeapon.transform);

            //    Quaternion newRotation = Quaternion.Euler(-mouseObject.transform.rotation.eulerAngles);
            //    //newRotation = Quaternion.Euler(Mathf.Clamp(newRotation.eulerAngles.x, minXRotation, maxXRotation), -90, newRotation.eulerAngles.z);

            //    if (side == Side.Left)
            //    {
            //        //if (-mouseObject.transform.rotation.eulerAngles.x + newRotation.eulerAngles.x > 1f)
            //        //{
            //        //    if (mouseObject.transform.position.x < 0)
            //        //    {
            //                rotatableWeapon.transform.rotation = newRotation;
            //        //    }
            //        //}
            //    }
            //    else
            //    {

            //    }
            //}

            Quaternion newRotation = Quaternion.Euler(rotationSlider.value, -90, 0);
            rotatableWeapon.transform.rotation = Quaternion.RotateTowards(rotatableWeapon.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        List<GameObject> newProjectiles = new List<GameObject>();

        switch (amountOfProjectiles)
        {
            case 1:

                GameObject newProjectile = Instantiate(projectile, projectileSpawns[0].position, projectileSpawns[0].rotation);
                newProjectiles.Add(newProjectile);
                break;
            case 2:
                break;
            case 3:

                GameObject newProjectile1 = Instantiate(projectile, projectileSpawns[0].position, projectileSpawns[0].rotation);
                newProjectiles.Add(newProjectile1);

                GameObject newProjectile2 = Instantiate(projectile, projectileSpawns[1].position, projectileSpawns[1].rotation);
                newProjectiles.Add(newProjectile2);

                GameObject newProjectile3 = Instantiate(projectile, projectileSpawns[2].position, projectileSpawns[2].rotation);
                newProjectiles.Add(newProjectile3);
                break;
        }

        for (int i = 0; i < newProjectiles.Count; i++)
        {
            newProjectiles[i].GetComponent<Rigidbody>().AddForce(newProjectiles[i].transform.forward * force.currentValue);
        }
    }
}
