using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class AllyInfoPopup : MonoBehaviour
{

    public static AllyInfoPopup instance;

    private Allie target;
    private Transform mainCam;

    public Animator anim;
    public Animator pointerAnim;

    private int listIndex;
    private bool panelActive;

    public float closePanelMouseDistance;

    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI damageText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        if (target != null)
        {
            healthText.text = ((int)target.myStats.health.currentValue).ToString();
            damageText.text = ((int)target.myStats.damage.currentValue).ToString();

            transform.position = target.transform.position;
            transform.LookAt(mainCam);
        }

        if (panelActive)
        {
            float mouseDistance = Vector3.Distance(healthText.transform.position, Input.mousePosition);

            if (mouseDistance > closePanelMouseDistance)
            {
                anim.ResetTrigger("Show");
                anim.SetTrigger("Hide");
                panelActive = false;
            }
        }
    }

    public void SetTarget(Allie ally)
    {
        target = ally;
    }

    public void SelectNextTarget(bool increase)
    {
        if (WaveManager.instance.alliesInScene.Count == 0)
        {
            return;
        }

        if (listIndex > WaveManager.instance.alliesInScene.Count - 1)
        {
            listIndex = 0;
        }

        if (increase)
        {
            if (listIndex == WaveManager.instance.alliesInScene.Count - 1)
            {
                listIndex = 0;
            }
            else
            {
                listIndex++;

            }
        }
        else
        {
            if (listIndex == 0)
            {
                listIndex = WaveManager.instance.alliesInScene.Count - 1;
            }
            else
            {
                listIndex--;
            }
        }

        SetTarget(WaveManager.instance.alliesInScene[listIndex]);

        if (!pointerAnim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            pointerAnim.SetTrigger("Open");
        }

        #region OLD SHIT
        //float myX = target.transform.position.x;

        //Dictionary<float, Allie> distances = new Dictionary<float, Allie>();

        //if (increase)
        //{
        //    for (int i = 0; i < WaveManager.instance.alliesInScene.Count; i++)
        //    {
        //        if (WaveManager.instance.alliesInScene[i] != target)
        //        {
        //            if (Mathf.Abs(WaveManager.instance.alliesInScene[i].transform.position.x) > target.transform.position.x)
        //            {
        //                distances.Add(Vector3.Distance(WaveManager.instance.alliesInScene[i].transform.position, target.transform.position), WaveManager.instance.alliesInScene[i]);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < WaveManager.instance.alliesInScene.Count; i++)
        //    {
        //        if (WaveManager.instance.alliesInScene[i] != target)
        //        {
        //            if (Mathf.Abs(WaveManager.instance.alliesInScene[i].transform.position.x) < target.transform.position.x)
        //            {
        //                distances.Add(Vector3.Distance(WaveManager.instance.alliesInScene[i].transform.position, target.transform.position), WaveManager.instance.alliesInScene[i]);
        //            }
        //        }
        //    }
        //}

        //float closest = distances.Keys.Min();
        //SetTarget(distances[closest]);
        #endregion
    }

    public void SetPanelActive()
    {
        anim.ResetTrigger("Hide");
        anim.SetTrigger("Show");
        panelActive = true;
    }
}
