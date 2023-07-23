using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Managers;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehInitBase
{
    #region nonpublic members

    private AudioListener m_AudioListener;
    private Canvas        m_LevelMenuCanvas;
    
    #endregion

    #region inject

    [Inject] private ISavesController SavesController { get; }

    #endregion

    #region engine methods

    private void Awake()
    {
    }

    #endregion


}