using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageZone : MonoBehaviour
{

    private Transform mainCam;

    [HideInInspector]
    public bool canDamage;

    public int myDamage;
    private int myHealth;
    public int myMaxHealth;

    public Animator anim;

    public string myObjectPool;

    public GameObject statsPopupPanel;
    public TextMeshProUGUI statsText;

    private void Awake()
    {
        mainCam = Camera.main.transform;
    }

    private void OnEnable()
    {
        myHealth = myMaxHealth;
    }

    private void Update()
    {
        if (statsPopupPanel.activeInHierarchy)
        {
            statsPopupPanel.transform.LookAt(mainCam);
            statsText.text = "Damage: <color=green>" + myDamage + "</color> Health: <color=green>" + myHealth + "</color>/<color=green>" + myMaxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage)
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(myDamage);
            myHealth--;

            if (anim != null)
            {
                anim.SetTrigger("Damage");
            }
        }

        if (myHealth <= 0)
        {
            ObjectPooler.instance.GrabFromPool("destroy particle", transform.position, Quaternion.identity);
            ObjectPooler.instance.AddToPool(myObjectPool, gameObject);
        }
    }

    private void OnMouseEnter()
    {
        if (canDamage)
        {
            statsPopupPanel.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        statsPopupPanel.SetActive(false);
    }
}
