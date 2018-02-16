using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleRoom : MonoBehaviour
{


    public enum RoomType
    {
        Knight,
        Ranger,
        Spearman,
        Gold,
        Heal
    }
    public RoomType roomType;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    [Header("Upgrade Panel")]
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI roomTypeText;

    public GameObject useUI;
    private bool usingRoom;

    public string roomName;
    public int roomLevel;
    public int buildCost;
    public Stat upgradeCost;

    public virtual void Update()
    {
        if (usingRoom)
        {
            useUI.transform.LookAt(Camera.main.transform);
        }
    }

    public virtual void UseRoom()
    {

    }

    public void Upgrade()
    {

    }

    public virtual void SetupUI()
    {
        roomNameText.text = "Level <color=green>" + roomLevel.ToString() + "</color> " + roomName;
        roomTypeText.text = "Type: <color=green>" + roomType + "</color>";
    }

    public virtual void StartUsing()
    {
        usingRoom = true;
        SetupUI();
        useUI.SetActive(true);
    }

    public virtual void StopUsing()
    {
        StartCoroutine(EventStopUsing());
    }

    public IEnumerator EventStopUsing()
    {
        useUI.GetComponent<Animator>().SetTrigger("CloseUI");

        yield return new WaitForSeconds(0.5f);

        usingRoom = false;

        useUI.SetActive(false);
    }
}
