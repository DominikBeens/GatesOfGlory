using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuilder : MonoBehaviour 
{

    public enum Type
    {
        Weapon,
        Room
    }
    public Type type;

    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    public CastleBuild myBuildedObject;

    public List<GameObject> availableBuilds = new List<GameObject>();

    [Header("Buttons")]
    public GameObject buildButton;
    public GameObject useButton;

    public void InteractButton()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            CastleUpgradeManager.instance.OpenUI(this);
        }
    }

    public void BuildWeapon(CastleWeapon.WeaponType type)
    {
        for (int i = 0; i < availableBuilds.Count; i++)
        {
            if (availableBuilds[i].GetComponent<CastleWeapon>().weaponType == type)
            {
                if (!ResourceManager.instance.HasEnoughGold(availableBuilds[i].GetComponent<CastleWeapon>().info.myBuildCost))
                {
                    return;
                }

                GameObject newBuild = Instantiate(availableBuilds[i], transform.position, Quaternion.identity);
                newBuild.transform.SetParent(transform);

                CastleWeapon castleWeaponComponent = newBuild.GetComponent<CastleWeapon>();
                castleWeaponComponent.myBuilder = this;
                myBuildedObject = castleWeaponComponent;

                ObjectPooler.instance.GrabFromPool("build particle", myBuildedObject.transform.position, Quaternion.identity);

                switch (side)
                {
                    case Side.Left:

                        castleWeaponComponent.SetLeftSide();
                        break;
                    case Side.Right:

                        castleWeaponComponent.SetRightSide();
                        break;
                }

                CastleUpgradeManager.instance.allBuiltWeapons.Add(castleWeaponComponent);

                buildButton.SetActive(false);
                useButton.SetActive(true);

                CastleUpgradeManager.instance.CloseAllUI(null);

                ResourceManager.instance.RemoveGold(availableBuilds[i].GetComponent<CastleWeapon>().info.myBuildCost, true);
                return;
            }
        }
    }

    public void BuildRoom(CastleRoom.RoomType type)
    {
        for (int i = 0; i < availableBuilds.Count; i++)
        {
            if (availableBuilds[i].GetComponent<CastleRoom>().roomType == type)
            {
                if (!ResourceManager.instance.HasEnoughGold(availableBuilds[i].GetComponent<CastleRoom>().info.myBuildCost))
                {
                    return;
                }

                GameObject newBuild = Instantiate(availableBuilds[i], transform.position, Quaternion.identity);
                newBuild.transform.SetParent(transform);

                CastleRoom castleRoomComponent = newBuild.GetComponent<CastleRoom>();
                castleRoomComponent.myBuilder = this;
                myBuildedObject = castleRoomComponent;

                ObjectPooler.instance.GrabFromPool("build particle", myBuildedObject.transform.position, Quaternion.identity);

                switch (side)
                {
                    case Side.Left:

                        castleRoomComponent.side = CastleBuild.Side.Left;
                        break;
                    case Side.Right:

                        castleRoomComponent.side = CastleBuild.Side.Right;
                        break;
                }

                CastleUpgradeManager.instance.allBuiltRooms.Add(castleRoomComponent);

                buildButton.SetActive(false);
                useButton.SetActive(true);

                CastleUpgradeManager.instance.CloseAllUI(null);

                ResourceManager.instance.RemoveGold(availableBuilds[i].GetComponent<CastleRoom>().info.myBuildCost, true);
                return;
            }
        }
    }

    public void Demolish()
    {
        if (myBuildedObject != null)
        {
            if (myBuildedObject is CastleWeapon)
            {
                CastleUpgradeManager.instance.allBuiltWeapons.Remove((CastleWeapon)myBuildedObject);
            }
            else if (myBuildedObject is CastleRoom)
            {
                CastleUpgradeManager.instance.allBuiltRooms.Remove((CastleRoom)myBuildedObject);
            }

            ResourceManager.instance.AddGold(myBuildedObject.goldSpentOnThisObject / 2);
        }

        CastleUpgradeManager.instance.CloseAllUI(null);

        ObjectPooler.instance.GrabFromPool("demolish particle", myBuildedObject.transform.position, Quaternion.identity);
        myBuildedObject.anim.SetTrigger("Destroy");
        Invoke("DestroyMyBuildedObject", myBuildedObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    private void DestroyMyBuildedObject()
    {
        Destroy(myBuildedObject.gameObject);
        myBuildedObject = null;

        buildButton.SetActive(true);
    }
}
