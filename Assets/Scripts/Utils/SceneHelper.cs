using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[HideScriptField]
public class SceneHelper : MonoBehaviour
{
    [Scene, SerializeField] private string _loadScene;

    public void LoadSceneAsync() => SceneManager.LoadSceneAsync(_loadScene);

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
