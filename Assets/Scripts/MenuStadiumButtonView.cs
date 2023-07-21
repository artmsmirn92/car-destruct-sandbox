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

    #endregion

    #region engine methods

    private void Start()
    {
        image.color = Color.white;
    }

    #endregion

    #region api

    public MenuStadiumButtonView SetTitle(string _Title)
    {
        title.text = _Title;
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
