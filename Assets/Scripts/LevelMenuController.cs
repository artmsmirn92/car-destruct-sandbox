using Lean.Common;
using mazing.common.Runtime.Constants;
using PG;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuController : MenuControllerBase
{
    #region serialized fields

    [SerializeField] private Canvas levelMenuCanvas;

    #endregion

    #region engine methods

    protected override void Start()
    {
        EnableLevelMenu(false);
        base.Start();
    }

    private void Update()
    {
        if (!LeanInput.GetDown(KeyCode.Escape))
            return;
        EnableLevelMenu(!MainData.IsInLevelMenu);
    }

    #endregion

    #region api

    public void ContinuePlay()
    {
        EnableLevelMenu(false);
    }

    public void RepairCar()
    {
        FindObjectOfType<CarControllerInput>().RestoreCar();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneNames.Menu);
    }

    #endregion

    #region nonpublic methods

    private void EnableLevelMenu(bool _Enable)
    {
        levelMenuCanvas.enabled = _Enable;
        MainData.IsInLevelMenu = _Enable;
        Time.timeScale = _Enable ? 0f : 1f;
        SoundManager.MuteAudio(_Enable);
        Cursor.lockState = _Enable ? CursorLockMode.None : CursorLockMode.Locked;
    }

    #endregion
}