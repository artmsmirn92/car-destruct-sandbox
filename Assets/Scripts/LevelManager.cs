using System;
using mazing.common.Runtime;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Managers;
using PG;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class LevelManager : MonoBehInitBase
{
    #region singleton

    public static LevelManager Instance { get; private set; }

    #endregion
    
    #region nonpublic members

    private AudioListener           m_AudioListener;
    private Canvas                  m_LevelMenuCanvas;
    private VehicleDamageController m_VehicleDamageController;
    
    #endregion

    #region inject

    [Inject] private ISavesController SavesController { get; }

    #endregion

    #region engine methods

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region api

    public event UnityAction CarDead;
    public event UnityAction CarAlive;
    
    public bool IsCarDeadChecked { get; set; }

    public VehicleDamageController VehicleDamageController
    {
        get => m_VehicleDamageController;
        set
        {
            CarAlive?.Invoke();
            if (m_VehicleDamageController.IsNotNull())
                m_VehicleDamageController.OnDamageAction -= OnDamageAction;
            if (value.IsNotNull())
                value.OnDamageAction += OnDamageAction;
            m_VehicleDamageController = value;
        }
    }

    #endregion

    #region nonpublic methods
    
    private void CheckIfDamageCritical()
    {
        int totalDamagableObjectsCount = VehicleDamageController.DamageableObjects.Length;
        int deadObjectsCount = 0;
        for (int i = 0; i < VehicleDamageController.DamageableObjects.Length; i++)
        {
            var dod = VehicleDamageController.DamageableObjects[i];
            if (!dod.DamageableObject.IsDead)
                continue;
            deadObjectsCount++;
        }
        if (deadObjectsCount < totalDamagableObjectsCount / 2 || IsCarDeadChecked) 
            return;
        CarDead?.Invoke();
        IsCarDeadChecked = true;
    }

    private void OnDamageAction(DamageData _DamageData)
    {
        CheckIfDamageCritical();
    }


    #endregion


}