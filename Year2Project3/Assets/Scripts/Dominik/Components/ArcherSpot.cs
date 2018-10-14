using UnityEngine;

public class ArcherSpot : MonoBehaviour 
{

    private Shop shop;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    private bool occupied;

    private GameObject myArcher;

    private void Awake()
    {
        shop = FindObjectOfType<Shop>();
    }

    public void SetArcher()
    {
        myArcher = ObjectPooler.instance.GrabFromPool("shopitem archer", transform.position, transform.rotation);
        myArcher.GetComponent<StationaryBowManz>().mySpot = this;
        ObjectPooler.instance.GrabFromPool("buy stationary archer particle", transform.position, transform.rotation);
        occupied = true;
    }

    public void RemoveArcher()
    {
        ObjectPooler.instance.AddToPool("shopitem archer", myArcher);
        myArcher = null;
        occupied = false;

        ObjectPooler.instance.GrabFromPool("demolish particle", transform.position, transform.rotation);

        shop.archerButtonMaxOverlay.SetActive(false);
        shop.archerButton.interactable = true;
    }

    public bool Isfree()
    {
        return !occupied;
    }
}
