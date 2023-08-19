using Lean.Localization;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

public class MenuStadiumButtonView : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    #region serialized fields

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button          button;
    [SerializeField] private Image           image;
    [SerializeField] private Button          unlockButton;

    [SerializeField] private LeanLocalizedTextMeshProUGUI leanLocalized;

    #endregion

    #region nonpublic members

    private int         m_LevelIndex;
    private UnityAction m_OnPointerEnterAction;
    private bool        m_IsMobile;

    #endregion

    #region engine methods

    private void Start()
    {
        YandexGame.RewardVideoEvent += OnRewardedVideoShown;
        unlockButton.SetOnClick(OnUnlockLevelButtonClick);
        transform.localScale = Vector3.one;
        m_IsMobile = CommonUtils.IsOnMobileWebGl();
    }

    private void OnDestroy()
    {
        YandexGame.RewardVideoEvent -= OnRewardedVideoShown;
    }

    public void OnPointerEnter(PointerEventData _EventData)
    {
        if (m_IsMobile)
            m_OnPointerEnterAction?.Invoke();
    }
    
    public void OnPointerDown(PointerEventData _EventData)
    {
        if (!m_IsMobile)
            m_OnPointerEnterAction?.Invoke();
    }

    #endregion

    #region api

    public Button Button => button;

    public event UnityAction<int> LevelUnlocked;

    public MenuStadiumButtonView SetLevelIndex(int _LevelIndex)
    {
        m_LevelIndex = _LevelIndex;
        return this;
    }

    public MenuStadiumButtonView SetTitle(string _LocalizationKey)
    {
        leanLocalized.TranslationName = _LocalizationKey;
        return this;
    }

    public MenuStadiumButtonView SetImage(Sprite _Sprite)
    {
        image.sprite = _Sprite;
        return this;
    }

    public MenuStadiumButtonView SetOnClickAction(UnityAction _Action)
    {
        button.SetOnClick(_Action);
        return this;
    }

    public MenuStadiumButtonView SetOnPointerEnterAction(UnityAction _Action)
    {
        m_OnPointerEnterAction = _Action;
        return this;
    }

    public MenuStadiumButtonView SetIsLocked(bool _IsLocked)
    {
        unlockButton.SetGoActive(_IsLocked);
        button.interactable = !_IsLocked;
        return this;
    }
    
    #endregion

    #region nonpublic methods

    private void OnUnlockLevelButtonClick()
    {
        YandexGame.RewVideoShow(m_LevelIndex);
        Time.timeScale = 0f;
    }

    private void OnRewardedVideoShown(int _RewardIndex)
    {
        if (_RewardIndex != m_LevelIndex)
            return;
        
        SetIsLocked(false);
        LevelUnlocked?.Invoke(m_LevelIndex);
    }

    #endregion
}
