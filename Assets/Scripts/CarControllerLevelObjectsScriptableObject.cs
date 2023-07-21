using System;
using mazing.common.Runtime.Helpers;
using UnityEngine;

[Serializable]
public class CarControllerLevelObjectsSet : ReorderableArray<GameObject> { }

[CreateAssetMenu(fileName = "Car Controller Level Objects", menuName = "CDS/Car Controller Level Objects")]
public class CarControllerLevelObjectsScriptableObject : ScriptableObject
{
    public CarControllerLevelObjectsSet rccLevelObjects;
    public CarControllerLevelObjectsSet uccLevelObjects;
}