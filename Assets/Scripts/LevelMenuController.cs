using System.Collections;
using Lean.Common;
using mazing.common.Runtime.Constants;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using PG;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using Zenject;

public class LevelMenuController : MenuControllerBase
{
    #region serialized fields

    [SerializeField] private Canvas     levelMenuCanvas;
    [SerializeField] private GameObject howToPlayPanelObj;
    [SerializeField] private Canvas     carDeadPanelCanvas;

    #endregion

    #region inject

    [Inject] private LevelManager LevelManager { get; }

    #endregion

    #region engine methods

    protected override void Start()
    {
        YandexGame.CloseFullAdEvent -= OnCloseFullAdEvent;
        YandexGame.CloseFullAdEvent += OnCloseFullAdEvent;
        carDeadPanelCanvas.enabled = false;
        LevelManager.CarDead += LevelManagerOnCarDead;
        LevelManager.CarAlive += LevelManagerOnCarAlive;
        IsOnStart = true;
        howToPlayPanelObj.SetActive(!CommonUtils.IsOnMobileWebGl());
        SetGraphicsQuality(SavesController.HighQualityGraphics);
        Cor.Run(EnableLevelMenuCoroutine(false));
        base.Start();
        IsOnStart = false;
    }

    private void Update()
    {
        if (!LeanInput.GetDown(KeyCode.Escape))
            return;
        if (YandexGame.nowFullAd || YandexGame.nowVideoAd)
            return;
        EnableLevelMenuForced(!MainData.IsInLevelMenu);
    }

    #endregion

    #region api

    public void ContinuePlay()
    {
        EnableLevelMenuForced(false);
    }

    public void RepairCar()
    {
        carDeadPanelCanvas.enabled = false;
        FindObjectOfType<CarControllerInput>().RestoreCar();
        EnableLevelMenuForced(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneNames.Menu);
        Time.timeScale = 1f;
    }

    #endregion

    #region nonpublic methods
    
    private void OnCloseFullAdEvent()
    {
        Time.timeScale = 0f;
        // SoundManager.MuteAudio(true);
    }

    private void LevelManagerOnCarAlive()
    {
        carDeadPanelCanvas.enabled = false;
    }

    private void LevelManagerOnCarDead()
    {
        carDeadPanelCanvas.enabled = true;
    }

    private void EnableLevelMenuForced(bool _Enable)
    {
        Cor.Run(EnableLevelMenuCoroutine(_Enable, true));
    }
    
    private IEnumerator EnableLevelMenuCoroutine(bool _Enable, bool _Forced = false)
    {
        EnableLevelMenuCore(_Enable);
        if (!_Forced)
            yield return Cor.WaitNextFrame(null, _FramesNum: 3);
        SoundManager.MuteAudio(_Enable);
        InitMobileInput();
        yield return null;
    }

    private void EnableLevelMenuCore(bool _Enable)
    {
        if (_Enable)
            YandexGame.FullscreenShow();
        levelMenuCanvas.enabled = _Enable;
        MainData.IsInLevelMenu = _Enable;
        Time.timeScale = _Enable ? 0f : 1f;
        Cursor.lockState = _Enable ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void InitMobileInput()
    {
        if (!CommonUtils.IsOnMobileWebGl())
            return;
        var mobileButtonsPanel = FindObjectOfType<MobileButtonsPanel>();
        if (mobileButtonsPanel.IsNull())
            return;
        mobileButtonsPanel
            .SetButtonPauseAction(() => EnableLevelMenuForced(true))
            .SetButtonSlowdownTimeAction(() => Time.timeScale = MathUtils.Equals(Time.timeScale, 1f) ? 0.2f : 1f);
    }

    #endregion
}