using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour{
    public float soldierOffset;

    public Transform gateRightInside, gateLeftInside;
    public Transform gateRight,gateLeft;
    public Transform throne;

    public Transform gateRightAlly, GateLeftAlly;
    public static BattleManager instance;
    public List<GameObject> freeEnemys = new List<GameObject>();

    void Awake(){
        instance = this;
    }

    public Transform EnemyGetTarget(float _myPosX){
        if (_myPosX >= 0){
            if (gateRight.GetComponent<CastleDeffensePoint>().gateOpen == true){
                return (throne);
            }
            else if(_myPosX > gateRight.position.x){
                return (gateRight);
            }
            else{
                return (throne);
            }
        }
        else{
            if (gateLeft.GetComponent<CastleDeffensePoint>().gateOpen == true){
                return (throne);
            }
            else if(_myPosX < gateLeft.position.x)
            {
                return (gateLeft);
            }
            else{
                return (throne);
            }
        }
    }

    //finding a path to a enemy is there is none go to closest reachable gate
    public Transform AllyGetTarget(float _myPosX, Allie _me){
        if(_myPosX >= 0){
            if(gateRight.GetComponent<CastleDeffensePoint>().gateOpen){
                GameObject _currentTarget = GetClosest(true,1000,0, _me);
                if(_currentTarget == null){
                    return(gateRightAlly);
                }
                return (_currentTarget.transform);
            }
            else{
                if(_myPosX < gateRight.transform.position.x){
                    GameObject _currentTarget = GetClosest(true, gateRightInside.transform.position.x,0, _me);
                    if(_currentTarget == null){
                        return (gateRightInside);
                    }
                    return (_currentTarget.transform);
                }
                else{
                    GameObject _currentTarget = GetClosest(true, 1000, gateRight.transform.position.x, _me);
                    if(_currentTarget == null){
                        return (gateRightAlly);
                    }
                    return (_currentTarget.transform);
                }
            }
        }
        else{
            if (gateLeft.GetComponent<CastleDeffensePoint>().gateOpen){
                GameObject _currentTarget = GetClosest(true, 0, -1000,_me);
                if (_currentTarget == null){
                    return (GateLeftAlly);
                }
                return (_currentTarget.transform);
            }
            else{
                if (_myPosX > gateLeft.transform.position.x){
                    GameObject _currentTarget = GetClosest(true, 0, gateLeftInside.transform.position.x, _me);
                    if (_currentTarget == null){
                        return (gateLeftInside);
                    }
                    return (_currentTarget.transform);
                }
                else{
                    GameObject _currentTarget = GetClosest(true, gateRight.transform.position.x ,- 1000, _me);
                    if (_currentTarget == null){
                        return (GateLeftAlly);
                    }
                    return (_currentTarget.transform);
                }
            }
        }
    }

    public GameObject GetClosest(bool rightSide, float maxDistance, float minDistance, Allie _me){
        GameObject closest = null;
        foreach(GameObject t in freeEnemys){
            if(t == null){
                freeEnemys.Remove(t);
            }
            else if (rightSide){
                if(t.transform.position.x > minDistance && t.transform.position.x < maxDistance){
                    if(closest == null){
                        closest = t;
                    }
                    else if(t.transform.position.x > closest.transform.position.x){
                        closest = t;
                    }
                }
            }
            else{
                if (t.transform.position.x > -maxDistance && t.transform.position.x < -minDistance){
                    if (closest == null){
                        closest = t;
                    }
                    else if (t.transform.position.x < closest.transform.position.x)
                    {
                        closest = t;
                    }
                }
            }
        }
        if(closest != null){
            closest.GetComponent<Enemy>().AddCounter(_me);
        }
        return (closest);
    }
}
