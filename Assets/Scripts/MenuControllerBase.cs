using Lean.Localization;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
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

    #region nonpublic members

    protected bool IsOnStart;

    #endregion

    #region engine methdos

    protected virtual void Start()
    {
        if (CommonUtils.IsOnMobileWebGl())
            soundOnToggle.SetGoActive(false);
        QualitySettings.SetQualityLevel(SavesController.HighQualityGraphics ? 1 : 0);
        highQualityGraphicsToggle.isOn = SavesController.HighQualityGraphics;
        soundOnToggle.isOn             = SavesController.SoundOn;
    }

    #endregion

    #region api

    public void EnableSound(bool _IsOn)
    {
        if (IsOnStart)
            return;
        SavesController.SoundOn = _IsOn;
        SoundManager.EnableAudio(_IsOn);
    }
    
    public void SetGraphicsQuality(bool _HighQualityGraphics)
    {
        if (IsOnStart)
            return;
        SavesController.HighQualityGraphics = _HighQualityGraphics;
        QualitySettings.SetQualityLevel(_HighQualityGraphics ? 1 : 0);
    }
    
    public void SetLangRus()
    {
        MainData.IsEnglish = false;
        SavesController.Language = "Russian";
        FindObjectOfType<LeanLocalization>().SetCurrentLanguage("Russian");
    }

    public void SetLangEng()
    {
        MainData.IsEnglish = true;
        SavesController.Language = "English";
        FindObjectOfType<LeanLocalization>().SetCurrentLanguage("English");
    }

    #endregion
}