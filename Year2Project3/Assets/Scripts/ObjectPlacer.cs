using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour 
{

    private bool canPlace = true;

    [SerializeField]
    private Color canPlaceColor;
    [SerializeField]
    private Color cannotPlaceColor;
    [SerializeField]
    private Material colorToChange;
    [SerializeField]
    private GameObject placeAvailabilityTrigger;
    [SerializeField]
    private DamageZone damageZone;

    private List<Transform> badCollisions = new List<Transform>();

    private void OnEnable()
    {
        canPlace = true;
        placeAvailabilityTrigger.SetActive(true);

        if (damageZone != null)
        {
            damageZone.canDamage = false;
        }
    }

    private void Update()
    {
        if (!canPlace)
        {
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(mousePos.x, 0.6f, 0);

        if (badCollisions.Count > 0)
        {
            colorToChange.color = cannotPlaceColor;
        }
        else
        {
            colorToChange.color = canPlaceColor;

            if (Input.GetMouseButton(0))
            {
                PlaceObject();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cannot Place" || other.tag == "Placable Weapon")
        {
            if (!badCollisions.Contains(other.transform))
            {
                badCollisions.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cannot Place" || other.tag == "Placable Weapon")
        {
            if (badCollisions.Contains(other.transform))
            {
                badCollisions.Remove(other.transform);
            }
        }
    }

    public void PlaceObject()
    {
        placeAvailabilityTrigger.SetActive(false);
        UIManager.instance.placeObjectUI.SetActive(false);
        ObjectPooler.instance.GrabFromPool("build particle", transform.position, Quaternion.identity);
        canPlace = false;

        if (damageZone != null)
        {
            damageZone.canDamage = true;
        }
    }
}
