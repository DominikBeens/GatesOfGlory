using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour 
{

    [SerializeField]
    private Color canPlaceColor;
    [SerializeField]
    private Color cannotPlaceColor;
    [SerializeField]
    private Material colorToChange;
    [SerializeField]
    private GameObject placeAvailabilityTrigger;

    private List<Transform> badCollisions = new List<Transform>();

    private void Awake()
    {
        placeAvailabilityTrigger.SetActive(true);
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(mousePos.x, 1, 0);

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
        this.enabled = false;
    }
}
