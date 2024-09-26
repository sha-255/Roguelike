using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

[HideScriptField]
public class BootLoader : Singleton<BootLoader>
{
    [Scene]
    [SerializeField] private string _bootstrapSceneName;
    [SerializeField] private LogsMode _logMode;

    private string _currentSceneName;

    public string CurrentSceneName => _currentSceneName;
    public LogsMode LogMode => _logMode;

    private void Awake()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;

        if (_currentSceneName != _bootstrapSceneName)
            SceneManager.LoadSceneAsync(_bootstrapSceneName);
        else
            SceneManager.LoadSceneAsync(1);
    }

    public enum LogsMode
    {
        CleanUp,
        Debug
    }
}
