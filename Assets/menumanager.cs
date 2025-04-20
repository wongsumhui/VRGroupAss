using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("Start Menu UI")]
    public GameObject startMenuCanvas;  // Canvas with buttons

    void Start()
    {
        // 确保菜单一开始是开启的
        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(true);
    }

    // 按钮调用：开始游戏
    public void OnStartButtonPressed()
    {
        Debug.Log("Start Game Pressed!");
        // 菜单隐藏（可选）
        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(false);

        // 加载主场景
        SceneManager.LoadScene("MainScene"); // 请替换成你的主场景名
    }

    // 按钮调用：退出游戏
    public void OnQuitButtonPressed()
    {
        Debug.Log("Quit Game Pressed!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 如果在编辑器内运行
#endif
    }
}
