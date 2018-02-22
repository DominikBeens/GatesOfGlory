using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Waves/Stage")]
public class Stage : ScriptableObject{
    public List<string> soldiers = new List<string>();
}
