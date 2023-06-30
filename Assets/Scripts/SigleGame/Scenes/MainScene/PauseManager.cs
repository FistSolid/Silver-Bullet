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
        // ����ESC���İ����¼�
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
    // ����Image��͸����
    public void SetAlpha(float alpha)
    {
        Color color = pauseImage.color;
        color.a = alpha;
        pauseImage.color = color;
    }
    private void PauseGame()
    {
        Time.timeScale = 0f; // ��ͣ��Ϸʱ������
        isPaused = true;
        pauseCanvas.SetActive(true); // ��ʾ�˵�����
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // �ָ���Ϸʱ������
        isPaused = false;
        pauseCanvas.SetActive(false); // ���ز˵�����
    }

   
}
