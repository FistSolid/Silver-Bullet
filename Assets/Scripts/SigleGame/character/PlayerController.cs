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
    public bool CanPlayAudio = true;//�Ƿ���Բ�����Ч

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


    public void KillEnemy(string enemyName, string id)//������˵����ֺ���������ӵ��ĸ�����id
    {

        if (id.Equals(this.id)) 
        {
            if (enemyName.Equals("Zombie")) { score += 1; killZombie += 1; }
            if (enemyName.Equals("Bat")) { score += 2; killBat += 1; }
            if (enemyName.Equals("Witch")) { score += 3; killWitch += 1; } 
        }

    }
   
    public void ReleaseSkill(float mpConsumption)//�ͷż���
    {
        mp -= mpConsumption;
        slider2.value = mp;
    }

    public void OnAttack(float damage)//�ܵ�����
    { 
        if (canBeAttack)
        {
            AudioManager.instance.PlayBGS("hitted");//����������Ч
            hp -= damage;
            slider1.value = hp;
        }
        else
        {
            return;
        }

        animator.SetTrigger("onHitted");//�ܵ���������
        canBeAttack = false;
        Invoke(nameof(ResetCanbeAttack), 3f);//�����Ժ�����ٴα�����
    }
    public void ResetCanbeAttack()
    {
        canBeAttack = true;
    }

    public void ChangePosition(Vector3 currentPosition)//����λ��
    {
        playerPosition = currentPosition;
    }

    public void PlayerDeath()//��ɫ����
    {
        if(isDead) return;
        animator.SetTrigger("isDead");//������������
        isDead = true;//����ֹͣ�ƶ���ֹͣ����
        if (CanPlayAudio == true)
        {
            AudioManager.instance.PlayBGS("Death");//����������Ч
            CanPlayAudio = false;//��һ֡��ʼ��������Ч
        }
        surviveTime = 0;
        StartCoroutine(SendRequest());
        StartCoroutine(LoadScene());
        UserScore.score = score;
        UserScore.killZombie = killZombie;
        UserScore.killWitch = killWitch;
        UserScore.killBat = killBat;
        //��ɫ����
        //�������ת����
    }

    public void OnSupply(Collider2D collision)//��Ѫƿ����ħƿ
    {
        if (collision.name.Equals("hp bottle") || collision.name.Equals("hp bottle(Clone)")) //Ѫƿ
        {
            Destroy(collision.gameObject);
            hp += 8f;
            slider1.value = hp;
            if (hp >= maxHealth) { hp = maxHealth; slider1.value = hp; }
        }
        if (collision.name.Equals("mp bottle") || collision.name.Equals("mp bottle(Clone)")) //Ѫƿ
        {
            Destroy(collision.gameObject);
            mp += 8f;
            slider2.value = mp;
            if (mp >= maxMp) { mp = maxMp; slider2.value = mp; }
        }
    }

    //��ҩƿ����Ʒ
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
    public IEnumerator LoadScene()//������Ϸ��������
    {
        if (DeathAnime != null)
        {
            DeathAnime.SetBool("End", true);//����ת����Ч
        }
        yield return new WaitForSeconds(5f);//�ӳ��������ת
        SceneManager.LoadSceneAsync("GameOver");
    }
    IEnumerator SendRequest()//���͵�����Ϣ
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
        // ����ѯ������ӵ� UriBuilder ��
        string queryString = string.Join("&", jsonData.Select(kvp => kvp.Key + "=" + kvp.Value));
        builder.Query = queryString;

        // ��ȡ���յ�URL
        string finalUrl = builder.Uri.ToString();

        Debug.Log(finalUrl);

        // ����UnityWebRequest����
        UnityWebRequest webRequest = new(finalUrl, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // ��������ͷ
        webRequest.SetRequestHeader("Content-Type", "application/json");
        /*
        // ��������ת��Ϊbyte���鲢���õ�UnityWebRequest��
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(body);
        */
        // �������󲢵ȴ���Ӧ
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // ��ȡ��Ӧ����
            string response = webRequest.downloadHandler.text;

            Debug.Log("Response: " + response);

        }
        else
        {
            // ����ʧ�ܣ���ӡ������Ϣ
            Debug.Log("Error: " + webRequest.error);
        }

        // ������ɺ��ͷ���Դ
        webRequest.Dispose();
    }
}
    