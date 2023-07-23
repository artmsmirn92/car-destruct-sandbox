using YG;

public interface ISavesController
{
    bool SoundOn             { get; set; }
    bool HighQualityGraphics { get; set; }
}
    
public class SavesControllerYandexGames : ISavesController
{
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
}