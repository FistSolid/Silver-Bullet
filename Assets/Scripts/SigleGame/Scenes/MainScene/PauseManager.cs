using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Image pauseImage;
    public GameObject pauseCanvas;

    public bool isPaused = false;


    private void Start()
    {
        SetAlpha(0.2f);    
    }

    private void Update()
    {
        // 监听ESC键的按下事件
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    // 设置Image的透明度
    public void SetAlpha(float alpha)
    {
        Color color = pauseImage.color;
        color.a = alpha;
        pauseImage.color = color;
    }
    private void PauseGame()
    {
        Time.timeScale = 0f; // 暂停游戏时间流逝
        isPaused = true;
        pauseCanvas.SetActive(true); // 显示菜单界面
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 恢复游戏时间流逝
        isPaused = false;
        pauseCanvas.SetActive(false); // 隐藏菜单界面
    }

   
}
