using UnityEngine;

public interface ISavesController
{
    bool SoundOn             { get; set; }
    bool HighQualityGraphics { get; set; }
}
    
public class SavesController : ISavesController
{
    public bool SoundOn             { get; set; }
    public bool HighQualityGraphics { get; set; }
}