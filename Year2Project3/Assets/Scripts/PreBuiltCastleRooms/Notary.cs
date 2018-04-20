using UnityEngine;
using TMPro;

public class Notary : PreBuiltCastleRoom
{

    public static int goldAccumulated;
    public static int goldSpent;
    public static int goldStolen;

    [Space(10)]
    public TextMeshProUGUI goldAccumulatedText;
    public TextMeshProUGUI goldSpentText;
    public TextMeshProUGUI goldStolenText;

    public override void Awake()
    {
        base.Awake();

        goldAccumulated = 0;
        goldSpent = 0;
        goldStolen = 0;
    }

    public override void Update()
    {
        base.Update();

        if (uiPanel.activeInHierarchy)
        {
            goldAccumulatedText.text = goldAccumulated.ToString();
            goldSpentText.text = goldSpent.ToString();
            goldStolenText.text = goldStolen.ToString();
        }
    }
}
