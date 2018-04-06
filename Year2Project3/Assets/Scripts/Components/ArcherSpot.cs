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

    public ParticleSystem setArcherParticle;

    private void Awake()
    {
        shop = FindObjectOfType<Shop>();
    }

    public void SetArcher()
    {
        myArcher = ObjectPooler.instance.GrabFromPool("shopitem archer", transform.position, transform.rotation);
        setArcherParticle.Play();
        occupied = true;
    }

    public void RemoveArcher()
    {
        ObjectPooler.instance.AddToPool("shopitem archer", myArcher);
        myArcher = null;
        occupied = false;

        shop.archerButtonMaxOverlay.SetActive(false);
        shop.archerButton.interactable = true;
    }

    public bool Isfree()
    {
        return !occupied;
    }
}
