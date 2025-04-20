using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections.Generic;

public class PlateCompletionChecker : MonoBehaviour
{
    public string sceneName = "MainScene"; // 要跳转的场景名称
    public float checkInterval = 0.5f;

    public XRSocketInteractor[] sockets;
    public bool hasCompleted = false;

    void Start()
    {
        // 只获取带有 "FoodSocket" 标签的 socket
        var allSockets = GetComponentsInChildren<XRSocketInteractor>();
        var filteredSockets = new List<XRSocketInteractor>();

        foreach (var socket in allSockets)
        {
            if (socket.CompareTag("FoodSocket")) // 只加入标签为 FoodSocket 的
            {
                filteredSockets.Add(socket);
            }
        }

        sockets = filteredSockets.ToArray();

        InvokeRepeating(nameof(CheckAllSockets), 0f, checkInterval);
    }

    void CheckAllSockets()
    {
        if (hasCompleted) return; // 已完成就不再检测

        foreach (var socket in sockets)
        {
            if (!socket.hasSelection)
            {
                return; // 有一个还没放东西就退出
            }
        }

        // 所有 socket 都放了东西，切换场景
        hasCompleted = true;
        Debug.Log("All food sockets are filled. Loading next scene...");
        SceneManager.LoadScene(sceneName);
    }
}