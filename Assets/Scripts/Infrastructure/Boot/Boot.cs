using UnityEngine;
using UnityEngine.SceneManagement;

[HideScriptField]
public class Boot : MonoBehaviour
{
    private BootLoader _loader;

    private void Start()
    {
        _loader = Singleton<BootLoader>.Instance;

        if (_loader.LogMode == BootLoader.LogsMode.CleanUp)
            Debug.ClearDeveloperConsole();

        SceneManager.LoadSceneAsync(_loader.CurrentSceneName);
    }
}
