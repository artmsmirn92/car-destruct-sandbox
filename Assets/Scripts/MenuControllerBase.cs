using System.Collections.Generic;
using System.Linq;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuControllerBase : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private Toggle highQualityGraphicsToggle;
    [SerializeField] private Toggle soundOnToggle;

    #endregion
    
    #region inject

    [Inject] protected ISavesController SavesController  { get; }
    [Inject] protected ISoundManager    SoundManager     { get; }
    [Inject] protected LeanLocalization LeanLocalization { get; }

    #endregion

    #region engine methdos

    protected virtual void Start()
    {
        highQualityGraphicsToggle.isOn = SavesController.HighQualityGraphics;
        soundOnToggle.isOn             = SavesController.SoundOn;
    }

    #endregion

    #region api

    public void EnableSound(bool _IsOn)
    {
        SavesController.SoundOn = _IsOn;
        SoundManager.EnableAudio(_IsOn);
    }
    
    public void SetGraphicsQuality(bool _HighQualityGraphics)
    {
        SavesController.HighQualityGraphics = _HighQualityGraphics;
        QualitySettings.SetQualityLevel(_HighQualityGraphics ? 1 : 0);
    }
    
    public void SetLangRus()
    {
        MainData.IsEnglish = false;
        FindObjectOfType<LeanLocalization>().SetCurrentLanguage("Russian");
    }

    public void SetLangEng()
    {
        MainData.IsEnglish = true;
        FindObjectOfType<LeanLocalization>().SetCurrentLanguage("English");
    }

    #endregion
}