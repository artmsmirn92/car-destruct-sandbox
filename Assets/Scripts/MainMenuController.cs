using System.Collections.Generic;
using System.Runtime.CompilerServices;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenuController : MenuControllerBase
{
    #region serialized fields
    
    [SerializeField] private GameObject mainMenuPanelObj;
    [SerializeField] private GameObject stadiumsPanelObj;
    [SerializeField] private GameObject settingsPanelObj;
    [SerializeField] private GameObject chooseCarPanelObj;
    [SerializeField] private GameObject loadingLevelPanelObj;

    [SerializeField] private Button choosePreviousCarButton;
    [SerializeField] private Button chooseNextCarButton;
    [SerializeField] private Image  carImage;

    [SerializeField] private List<ChooseCarArgs>        chooseCarArgsList;
    [SerializeField] private StadiumSetScriptableObject stadiumsSetScrObj;
    
    [SerializeField] private GameObject               stadiumButtonPrefab;
    [SerializeField] private ScrollRect               stadiumPanelScrolRect;
    [SerializeField] private ScrollRectButtonStateFix scrollRectButtonStateFix;

    [SerializeField] private CarControllerLevelObjectsScriptableObject carControllerLevelObjects;
    [SerializeField] private GameObject                                howToScrollStadiumsPanelGo;
    
    #endregion

    #region nonpublic members

    private List<GameObject>    m_PanelsList;
    private List<RectTransform> m_StadiumButtonsRtrs = new();
    private float               m_AspectRatioChecked;

    #endregion

    #region engine methods

    protected override void Start()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
        IsOnStart = true;
        SoundManager.Init();
        base.Start();
        m_PanelsList = new List<GameObject>
        {
            mainMenuPanelObj,
            stadiumsPanelObj,
            settingsPanelObj,
            chooseCarPanelObj,
            loadingLevelPanelObj
        };
        HideAllPanelsAndShowMainMenuPanel();
        SetChooseCarPanelState();
        howToScrollStadiumsPanelGo.SetActive(!SavesController.DoNotShowHowToScrollStadiumsInPanel);
        if (SavesController.Language == "English")
            SetLangEng();
        else SetLangRus();
        Cor.Run(Cor.WaitNextFrame(() => SoundManager.EnableAudio(SavesController.SoundOn)));
        Cor.Run(Cor.WaitNextFrame(InitStadiumsPanel));
        IsOnStart = false;
    }

    private void Update()
    {
        if (!Mathf.Approximately(GraphicUtils.AspectRatio, m_AspectRatioChecked))
            KeepStadiumButtonsAspectRatio();
        m_AspectRatioChecked = GraphicUtils.AspectRatio;
    }

    #endregion
    
    #region api
    
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
        SceneManager.sceneLoaded += OnLevelSceneLoaded;
        string sceneName = MainData.ChooseStadiumArgs.sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void GotItHowToScrollStadiumsInPanel()
    {
        howToScrollStadiumsPanelGo.SetActive(false);
        SavesController.DoNotShowHowToScrollStadiumsInPanel = true;
    }

    public void ClearSaves()
    {
        SavesController.ResetProgress();
    }

    #endregion

    #region nonpublic methdos

    private void KeepStadiumButtonsAspectRatio()
    {
        foreach (var rTr in m_StadiumButtonsRtrs)
        {
            rTr.sizeDelta = rTr.sizeDelta.SetX(stadiumPanelScrolRect.content.rect.height * 0.6f);
        }
    }

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
        int idx = 0;
        foreach (var args in stadiumsSetScrObj.stadiumsSet)
        {
            var buttonGo = Instantiate(stadiumButtonPrefab);
            buttonGo.SetParent(stadiumPanelScrolRect.content);
            var buttonView = buttonGo.GetComponent<MenuStadiumButtonView>();
            scrollRectButtonStateFix.buttons.Add(buttonView.Button);
            buttonView
                .SetImage(args.sprite)
                .SetOnClickAction(() => OnChooseStadiumButtonClick(args))
                .SetOnPointerEnterAction(GotItHowToScrollStadiumsInPanel)
                .SetTitle(args.stadiumNameLocKey)
                .SetLevelIndex(idx)
                .SetIsLocked(args.lockedByDefault && SavesController.IsLevelLocked(idx++));
            buttonView.LevelUnlocked += OnLevelUnlockedByLevelButton;
            m_StadiumButtonsRtrs.Add(buttonView.GetComponent<RectTransform>());
        }
        Cor.Run(Cor.WaitNextFrame(KeepStadiumButtonsAspectRatio));
    }

    private void OnLevelUnlockedByLevelButton(int _LevelIndex)
    {
        Time.timeScale = 1f;
        SavesController.UnlockLevel(_LevelIndex);
    }
    
    private void OnChooseStadiumButtonClick(ChooseStadiumArgs _Args)
    {
        MainData.ChooseStadiumArgs = _Args;
        HideAllPanels();
        loadingLevelPanelObj.SetActive(true);
        Cor.Run(Cor.WaitNextFrame(StartLevel));
    }

    private void OnLevelSceneLoaded(Scene _Scene, LoadSceneMode _LoadSceneMode)
    {
        Cor.Run(Cor.WaitNextFrame(() =>
        {
            var carControllerObjetsToInstantiate = MainData.ChosenCarArgs.CarControllerType switch
            {
                ECarControllerType.RCC => carControllerLevelObjects.rccLevelObjects,
                ECarControllerType.UCC => carControllerLevelObjects.uccLevelObjects,
                _                      => throw new SwitchExpressionException(MainData.ChosenCarArgs.CarControllerType)
            };
            foreach (var go in carControllerObjetsToInstantiate)
                Instantiate(go);
        }));
        SceneManager.sceneLoaded -= OnLevelSceneLoaded;
    }

    #endregion
}
