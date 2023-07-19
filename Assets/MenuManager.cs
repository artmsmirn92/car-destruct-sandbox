using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
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

    [SerializeField] private Button          choosePreviousCarButton;
    [SerializeField] private Button          chooseNextCarButton;
    [SerializeField] private Image           carImage;

    [SerializeField] private List<ChooseCarArgs>     chooseCarArgsList;
    [SerializeField] private List<ChooseStadiumArgs> ChooseStadiumArgsList;

    [SerializeField] private Toggle highQualityGraphicsToggle;
    [SerializeField] private Toggle soundOnToggle;
    
    #endregion

    #region nonpublic members

    private List<GameObject> m_PanelsList;

    #endregion

    #region inject

    [Inject] private ISavesController SavesController { get; }

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
    }

    #endregion
    
    #region api

    private event UnityAction LanguageChanged;

    public void OpenStadiumsPanel()
    {
        HideAllPanels();
        stadiumsPanelObj.SetActive(true);
    }

    public void CloseStadiumsPanel()
    {
        HideAllPanelsAndShowMainMenuPanel();
    }

    public void OpenSettingsPanel()
    {
        HideAllPanels();
        settingsPanelObj.SetActive(true);
    }
    
    public void CloseSettingsPanel()
    {
        HideAllPanelsAndShowMainMenuPanel();
    }

    public void LoadStadium(int _StadiumId)
    {
        MainData.StadiumId = _StadiumId;
        HideAllPanels();
        chooseCarPanelObj.SetActive(true);
    }

    public void CloseChooseCarPanel()
    {
        HideAllPanels();
        stadiumsPanelObj.SetActive(true);
    }
    
    public void ChoosePreviousCar()
    {
        MainData.CarId = Mathf.Clamp(MainData.CarId - 1, 0, GetMaxCarId());
        choosePreviousCarButton.interactable = MainData.CarId > 0;
        chooseNextCarButton.interactable = true;
        MainData.chosenCarArgs = chooseCarArgsList.FirstOrDefault(_Args => _Args.Id == MainData.CarId);
        SetCarImageAndTextInMenu();
    }

    public void ChooseNextCar()
    {
        MainData.CarId = Mathf.Clamp(MainData.CarId + 1, 0, GetMaxCarId());
        choosePreviousCarButton.interactable = true;
        chooseNextCarButton.interactable = MainData.CarId < GetMaxCarId();
        MainData.chosenCarArgs = chooseCarArgsList.FirstOrDefault(_Args => _Args.Id == MainData.CarId);
        SetCarImageAndTextInMenu();
    }

    public void StartLevel()
    {
        string sceneName = GetStadiumNameById(MainData.StadiumId);
        SceneManager.LoadScene(sceneName);
    }

    public void SetLangRus()
    {
        MainData.IsEnglish = false;
        LanguageChanged?.Invoke();
    }

    public void SetLangEng()
    {
        MainData.IsEnglish = true;
        LanguageChanged?.Invoke();
    }

    public void SetGraphicsQuality(bool _HighQualityGraphics)
    {
        SavesController.HighQualityGraphics = MainData.HighQualityGraphics = _HighQualityGraphics;
    }

    public void EnableSound(bool _IsOn)
    {
        SavesController.SoundOn = MainData.SoundOn = _IsOn;
        audioListener.enabled = _IsOn;
    }

    #endregion

    #region nonpublic methdos

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

    private void SetCarImageAndTextInMenu()
    {
        bool isEnglish = false;
        carImage.sprite = MainData.chosenCarArgs.Sprite;
    }

    private int GetMaxCarId()
    {
        return chooseCarArgsList.Count - 1;
    }

    private static string GetStadiumNameById(int _StadiumId)
    {
        return _StadiumId switch
        {
            1 => "Stadium 1",
            2 => "Stadium 2",
            3 => "Stadium 3",
            4 => "Stadium 4",
            5 => "Stadium 5",
            _ => throw new SwitchExpressionException(_StadiumId)
        };
    }

    #endregion
}
