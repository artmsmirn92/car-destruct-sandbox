using UnityEngine.Events;

public static class MainData
{
    public static int  CarId;
    public static bool IsEnglish;

    public static ChooseCarArgs     ChosenCarArgs;
    public static ChooseStadiumArgs ChooseStadiumArgs;

    public static bool IsInLevelMenu;

    private static bool _isSoundOn;

    public static bool IsSoundOn
    {
        get => _isSoundOn;
        set
        {
            _isSoundOn = value;
            SoundOnEvent?.Invoke(value);
        }
    }

    public static event UnityAction<bool> SoundOnEvent;
}
