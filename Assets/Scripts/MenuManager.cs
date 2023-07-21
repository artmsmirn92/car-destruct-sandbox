using System.Collections.Generic;
using Lean.Localization;
using mazing.common.Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private AudioListener audioListener;
    
    [SerializeField] private GameObject mainMenuPanelObj;
    [SerializeField] private GameObject stadiumsPanelObj;
    [SerializeField] private GameObject settingsPanelObj;
    [SerializeField] private GameObject chooseCarPanelObj;

    [SerializeField] private Button choosePreviousCarButton;
    [SerializeField] private Button chooseNextCarButton;
    [SerializeField] private Image  carImage;

    [SerializeField] private List<ChooseCarArgs>     chooseCarArgsList;
    [SerializeField] private List<ChooseStadiumArgs> chooseStadiumArgsList;

    [SerializeField] private Toggle highQualityGraphicsToggle;
    [SerializeField] private Toggle soundOnToggle;

    [SerializeField] private GameObject       stadiumButtonPrefab;
    [SerializeField] private ScrollRect       stadiumPanelScrolRect;
    [SerializeField] private LeanLocalization leanLocalization;
    
    #endregion

    #region nonpublic members

    private List<GameObject> m_PanelsList;

    #endregion

    #region inject

    [Inject] private ISavesController SavesController { get; }
    [Inject] private ISoundManager    SoundManager    { get; }

    #endregion

    #region engine methods

    private void Start()
    {
        m_PanelsList = new List<GameObject>
        {
            mainMenuPanelObj,
            stadiumsPanelObj,
            settingsPanelObj,
            chooseCarPanelObj
        };
        highQualityGraphicsToggle.isOn = MainData.HighQualityGraphics = SavesController.SoundOn;
        soundOnToggle.isOn             = MainData.SoundOn             = SavesController.SoundOn;
        HideAllPanelsAndShowMainMenuPanel();
        SetChooseCarPanelState();
        InitStadiumsPanel();
    }

    #endregion
    
    #region api

    private event UnityAction LanguageChanged;

    public void OpenStadiumsPanel()
    {
        HideAllPanels();
        stadiumsPanelObj.SetActive(true);
    }

    public void OpenSettingsPanel()
    {
        HideAllPanels();
        settingsPanelObj.SetActive(true);
    }

    public void CloseStadiumsPanel()
    {
        HideAllPanelsAndShowMainMenuPanel();
    }
    
    public void CloseSettingsPanel()
    {
        HideAllPanelsAndShowMainMenuPanel();
    }
    
    public void CloseChooseCarPanel()
    {
        HideAllPanels();
        stadiumsPanelObj.SetActive(true);
    }
    
    public void ChoosePreviousCar()
    {
        MainData.CarId = Mathf.Clamp(MainData.CarId - 1, 0, GetMaxCarId());
        SetChooseCarPanelState();
    }

    public void ChooseNextCar()
    {
        MainData.CarId = Mathf.Clamp(MainData.CarId + 1, 0, GetMaxCarId());
        SetChooseCarPanelState();
    }

    public void StartLevel()
    {
        string sceneName = MainData.ChooseStadiumArgs.sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void SetLangRus()
    {
        MainData.IsEnglish = false;
        LanguageChanged?.Invoke();
        leanLocalization.SetCurrentLanguage("Russian");
    }

    public void SetLangEng()
    {
        MainData.IsEnglish = true;
        leanLocalization.SetCurrentLanguage("English");
        LanguageChanged?.Invoke();
    }

    public void SetGraphicsQuality(bool _HighQualityGraphics)
    {
        SavesController.HighQualityGraphics = MainData.HighQualityGraphics = _HighQualityGraphics;
        QualitySettings.SetQualityLevel(_HighQualityGraphics ? 1 : 0);
    }

    public void EnableSound(bool _IsOn)
    {
        SavesController.SoundOn = MainData.SoundOn = _IsOn;
        SoundManager.EnableAudio(_IsOn);
        audioListener.enabled = _IsOn;
    }

    #endregion

    #region nonpublic methdos

    private void SetChooseCarPanelState()
    {
        choosePreviousCarButton.interactable = MainData.CarId > 0;
        chooseNextCarButton    .interactable = MainData.CarId < GetMaxCarId();
        MainData.ChosenCarArgs = chooseCarArgsList[MainData.CarId];
        carImage.sprite = MainData.ChosenCarArgs.Sprite;
    }

    private void HideAllPanels()
    {
        foreach (var go in m_PanelsList)
            go.SetActive(false);
    }

    private void HideAllPanelsAndShowMainMenuPanel()
    {
        HideAllPanels();
        mainMenuPanelObj.SetActive(true);
    }

    private int GetMaxCarId()
    {
        return chooseCarArgsList.Count - 1;
    }

    private void InitStadiumsPanel()
    {
        stadiumPanelScrolRect.content.DestroyChildrenSafe();
        foreach (var args in chooseStadiumArgsList)
        {
            var buttonGo = Instantiate(stadiumButtonPrefab);
            buttonGo.SetParent(stadiumPanelScrolRect.content);
            var buttonView = buttonGo.GetComponent<MenuStadiumButtonView>();
            buttonView
                .SetImage(args.sprite)
                .SetAction(() => OnChooseStadiumButtonClick(args))
                .SetTitle(LeanLocalization.GetTranslationText(args.stadiumNameLocKey));
        }
    }
    
    private void OnChooseStadiumButtonClick(ChooseStadiumArgs _Args)
    {
        MainData.ChooseStadiumArgs = _Args;
        HideAllPanels();
        chooseCarPanelObj.SetActive(true);
    }

    #endregion
}
