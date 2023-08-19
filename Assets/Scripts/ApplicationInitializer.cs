using System.Collections;
using mazing.common.Runtime.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class ApplicationInitializer : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (!YandexGame.SDKEnabled)
            yield return null;
        SceneManager.LoadScene(SceneNames.Menu);
    }
}