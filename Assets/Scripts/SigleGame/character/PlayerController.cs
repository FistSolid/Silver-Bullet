using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    private Slider slider1;
    private Slider slider2;
    //public FireBallIcon skillCooldown;
    public float hp;
    public float mp;
    public float speed = 6f;
    public float maxHealth = 100f;
    public float maxMp = 100f;
    public float maxSpeed = 12f;

    private float timer = 0f;
    private float surviveTime = 0f;
    AsyncOperation operation;

    public string id = "111";
    public string character = "Angelica";

    public int score = 0;
    public int killZombie = 0;
    public int killBat = 0;
    public int killWitch = 0;

    private bool canBeAttack = true;
    public bool isDead = false;
    public bool CanPlayAudio = true;//是否可以播放音效

    public Vector3 playerPosition;

    public Animator animator;
    public Animator DeathAnime;


    private void Start()
    {
        id = UserData.id;
        character = UserData.character;

        hp = maxHealth;
        mp = maxMp;
        animator = gameObject.GetComponent<Animator>();
        slider1 = GameObject.Find("HP").GetComponent<Slider>();
        slider2 = GameObject.Find("MP").GetComponent<Slider>();
        slider1.value = maxHealth;
        slider2.value = maxMp;

       DeathAnime = GameObject.Find("TransitionToEND").GetComponent<Animator>();
    }


    public void KillEnemy(string enemyName, string id)//传入敌人的名字和命中其的子弹的父物体id
    {

        if (id.Equals(this.id)) 
        {
            if (enemyName.Equals("Zombie")) { score += 1; killZombie += 1; }
            if (enemyName.Equals("Bat")) { score += 2; killBat += 1; }
            if (enemyName.Equals("Witch")) { score += 3; killWitch += 1; } 
        }

    }
   
    public void ReleaseSkill(float mpConsumption)//释放技能
    {
        mp -= mpConsumption;
        slider2.value = mp;
    }

    public void OnAttack(float damage)//受到攻击
    { 
        if (canBeAttack)
        {
            AudioManager.instance.PlayBGS("hitted");//播放受伤音效
            hp -= damage;
            slider1.value = hp;
        }
        else
        {
            return;
        }

        animator.SetTrigger("onHitted");//受到攻击动画
        canBeAttack = false;
        Invoke(nameof(ResetCanbeAttack), 3f);//三秒以后可以再次被攻击
    }
    public void ResetCanbeAttack()
    {
        canBeAttack = true;
    }

    public void ChangePosition(Vector3 currentPosition)//更换位置
    {
        playerPosition = currentPosition;
    }

    public void PlayerDeath()//角色死亡
    {
        if(isDead) return;
        animator.SetTrigger("isDead");//触发死亡动画
        isDead = true;//控制停止移动、停止攻击
        if (CanPlayAudio == true)
        {
            AudioManager.instance.PlayBGS("Death");//播放死亡音效
            CanPlayAudio = false;//下一帧开始不播放音效
        }
        surviveTime = 0;
        StartCoroutine(SendRequest());
        StartCoroutine(LoadScene());
        UserScore.score = score;
        UserScore.killZombie = killZombie;
        UserScore.killWitch = killWitch;
        UserScore.killBat = killBat;
        //角色死亡
        //并完成跳转场景
    }

    public void OnSupply(Collider2D collision)//捡到血瓶或者魔瓶
    {
        if (collision.name.Equals("hp bottle") || collision.name.Equals("hp bottle(Clone)")) //血瓶
        {
            Destroy(collision.gameObject);
            hp += 8f;
            slider1.value = hp;
            if (hp >= maxHealth) { hp = maxHealth; slider1.value = hp; }
        }
        if (collision.name.Equals("mp bottle") || collision.name.Equals("mp bottle(Clone)")) //血瓶
        {
            Destroy(collision.gameObject);
            mp += 8f;
            slider2.value = mp;
            if (mp >= maxMp) { mp = maxMp; slider2.value = mp; }
        }
    }

    //捡到药瓶补给品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Supplies"))
        {
            OnSupply(collision);
        }

    }

    public void Update() 
    {

        if (hp <= 0) 
        {
            PlayerDeath();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public IEnumerator LoadScene()//加载游戏结束场景
    {
        if (DeathAnime != null)
        {
            DeathAnime.SetBool("End", true);//播放转场特效
        }
        yield return new WaitForSeconds(5f);//延迟五秒后跳转
        SceneManager.LoadSceneAsync("GameOver");
    }
    IEnumerator SendRequest()//发送当局信息
    {
        UriBuilder builder = new UriBuilder("http://101.201.39.2:9090/single/send_result");
        //string url = "http://101.201.39.2:9090/single/send_result"; // URL
        Dictionary<string, object> jsonData = new Dictionary<string, object>
        {
            { "id", UserData.id },
            { "username", UserData.name },
            { "password", UserData.password },
            { "grade", score },
            { "survive_time", surviveTime.ToString() },
            { "time", DateTime.Now.ToString() },
            { "role", character },
            { "enemy1_kill", killZombie },
            { "enemy2_kill", killBat },
            { "enemy3_kill", killWitch },
            { "total_kill", killZombie + killBat + killWitch }
        };
        // 将查询参数添加到 UriBuilder 中
        string queryString = string.Join("&", jsonData.Select(kvp => kvp.Key + "=" + kvp.Value));
        builder.Query = queryString;

        // 获取最终的URL
        string finalUrl = builder.Uri.ToString();

        Debug.Log(finalUrl);

        // 创建UnityWebRequest对象
        UnityWebRequest webRequest = new(finalUrl, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // 设置请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        /*
        // 将请求体转换为byte数组并设置到UnityWebRequest中
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(body);
        */
        // 发送请求并等待响应
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // 获取响应数据
            string response = webRequest.downloadHandler.text;

            Debug.Log("Response: " + response);

        }
        else
        {
            // 请求失败，打印错误信息
            Debug.Log("Error: " + webRequest.error);
        }

        // 请求完成后释放资源
        webRequest.Dispose();
    }
}
    