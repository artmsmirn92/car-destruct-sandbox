using System.Runtime.CompilerServices;
using mazing.common.Runtime.Exceptions;
using YG;

public interface ISavesController
{
    string Language                            { get; set; }
    bool   SoundOn                             { get; set; }
    bool   HighQualityGraphics                 { get; set; }
    bool   DoNotShowHowToScrollStadiumsInPanel { get; set; }
    bool   IsLevelLocked(int _LevelIndex);
    void   UnlockLevel(int   _LevelIndex);
    void   ResetProgress();
}
    
public class SavesControllerYandexGames : ISavesController
{
    public string Language
    {
        get => YandexGame.savesData.lang;
        set
        {
            YandexGame.savesData.lang = value;
            YandexGame.SaveProgress();
        }
    }

    public bool SoundOn
    {
        get => YandexGame.savesData.soundOn;
        set
        {
            YandexGame.savesData.soundOn = value;
            YandexGame.SaveProgress();
        }
    }

    public bool HighQualityGraphics
    {
        get => YandexGame.savesData.highQualityGraphics;
        set
        {
            YandexGame.savesData.highQualityGraphics = value;
            YandexGame.SaveProgress();
        }
    }

    public bool DoNotShowHowToScrollStadiumsInPanel
    {
        get => YandexGame.savesData.doNotShowHowToScrollStadiumsInPanel;
        set
        {
            YandexGame.savesData.doNotShowHowToScrollStadiumsInPanel = value;
            YandexGame.SaveProgress();
        }
    }

    public bool IsLevelLocked(int _LevelIndex)
    {
        var savedData = YandexGame.savesData;
        return _LevelIndex switch
        {
            0 => !savedData.level01Unlocked,
            1 => !savedData.level02Unlocked,
            2 => !savedData.level03Unlocked,
            3 => !savedData.level04Unlocked,
            4 => !savedData.level05Unlocked,
            5 => !savedData.level06Unlocked,
            _ => throw new SwitchExpressionException(_LevelIndex)
        };
    }

    public void UnlockLevel(int _LevelIndex)
    {
        var savedData = YandexGame.savesData;
        switch (_LevelIndex)
        {
            case 0:  savedData.level01Unlocked = true; break;
            case 1:  savedData.level02Unlocked = true; break;
            case 2:  savedData.level03Unlocked = true; break;
            case 3:  savedData.level04Unlocked = true; break;
            case 4:  savedData.level05Unlocked = true; break;
            case 5:  savedData.level06Unlocked = true; break;
            default: throw new SwitchCaseNotImplementedException(_LevelIndex);
        }
        YandexGame.SaveProgress();
    }

    public void ResetProgress()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}