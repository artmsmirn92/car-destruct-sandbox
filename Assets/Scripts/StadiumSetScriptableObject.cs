using System;
using mazing.common.Runtime.Helpers;
using UnityEngine;

[Serializable]
public class StadiumsSet : ReorderableArray<ChooseStadiumArgs> { }

[CreateAssetMenu(fileName = "Stadiums Set", menuName = "CDS/Stadiums Set")]
public class StadiumSetScriptableObject : ScriptableObject
{
    public StadiumsSet stadiumsSet;
}