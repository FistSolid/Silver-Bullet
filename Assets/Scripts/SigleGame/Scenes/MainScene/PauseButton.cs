using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    private PauseManager ps;
    AsyncOperation operation;

    private void Start()
    {
        ps = GameObject.Find("PauseManager").GetComponent<PauseManager>();
    }
    public void ClickResume()//恢复游戏
    {
        ps.ResumeGame();
    }
    
    public void ClickHome()//返回主菜单
    {

        StartCoroutine(loadScene());
    }
    public IEnumerator loadScene()
    {
        ps.ResumeGame();
        operation = SceneManager.LoadSceneAsync("StartScene");
        yield return operation;
    }
    public void ClickOver()//游戏结束
    {
        ps.ResumeGame();
        Invoke(nameof(GameOver), 0.6f);
    }

    private void GameOver()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.hp = 0;
    }
}
