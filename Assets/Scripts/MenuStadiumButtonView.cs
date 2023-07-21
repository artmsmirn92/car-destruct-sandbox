using Lean.Localization;
using mazing.common.Runtime.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuStadiumButtonView : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button          button;
    [SerializeField] private Image           image;

    [SerializeField] private LeanLocalizedTextMeshProUGUI leanLocalized;

    #endregion

    #region engine methods

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    #endregion

    #region api

    public MenuStadiumButtonView SetTitle(string _LocalizationKey)
    {
        // var translation = LeanLocalization.GetTranslation(_LocalizationKey);
        leanLocalized.TranslationName = _LocalizationKey;
        // leanLocalized.UpdateTranslation(translation);
        return this;
    }

    public MenuStadiumButtonView SetImage(Sprite _Sprite)
    {
        image.sprite = _Sprite;
        return this;
    }

    public MenuStadiumButtonView SetAction(UnityAction _Action)
    {
        button.SetOnClick(_Action);
        return this;
    }

    #endregion
}
