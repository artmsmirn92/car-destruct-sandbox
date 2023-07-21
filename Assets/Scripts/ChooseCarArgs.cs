using System;
using UnityEngine;

[Serializable]
public class ChooseCarArgs
{
    public Sprite             Sprite;
    public GameObject         Prefab;
    public ECarControllerType CarControllerType;
}