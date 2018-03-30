using UnityEngine;
using TMPro;

public class Notary : PreBuiltCastleRoom
{

    public static int goldAccumulated;
    public static int goldSpent;

    [Space(10)]
    public TextMeshProUGUI goldAccumulatedText;
    public TextMeshProUGUI goldSpentText;

    public override void Update()
    {
        base.Update();

        if (uiPanel.activeInHierarchy)
        {
            goldAccumulatedText.text = goldAccumulated.ToString();
            goldSpentText.text = goldSpent.ToString();
        }
    }
}
