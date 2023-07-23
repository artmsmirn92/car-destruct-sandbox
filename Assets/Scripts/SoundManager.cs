using mazing.common.Runtime;using mazing.common.Runtime.Helpers;
using UnityEngine;
using Zenject;

public interface ISoundManager : IInit
{
    void EnableAudio(bool _Enable);
    void MuteAudio(bool   _Mute);
}

public class SoundManager : InitBase, ISoundManager
{
    #region nonpublic members

    private bool m_Enabled;
    private bool m_Muted;

    #endregion

    #region inject

    [Inject] private ISavesController SavesController { get; }

    #endregion

    #region api

    public override void Init()
    {
        m_Enabled = SavesController.SoundOn;
        base.Init();
    }

    public void EnableAudio(bool _Enable)
    {
        m_Enabled = _Enable;
        EnableAudioCore(m_Enabled && !m_Muted);
    }

    public void MuteAudio(bool _Mute)
    {
        m_Muted = _Mute;
        EnableAudioCore(m_Enabled && !m_Muted);
    }

    #endregion

    #region nonpublic methods

    private static void EnableAudioCore(bool _Enable)
    {
        Object.FindObjectOfType<AudioListener>().enabled = _Enable;
    }

    #endregion
}