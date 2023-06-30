using Mirror;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerControllerNet : NetworkBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public FireBallIcon skillCooldown;
    public float hp;
    public float mp;
    public float speed = 6f;
    public float maxHealth = 100f;
    public float maxMp = 100f;
    public float maxSpeed = 12f;


    public string id = "111";
    public string character = "Angelica";

    public int score = 0;
    public int killZombie = 0;
    public int killBat = 0;
    public int killWitch = 0;

    private bool canBeAttack = true;

    public Vector3 playerPosition;

    public Animator animator;


    private void Start()
    {
        hp = maxHealth;
        mp = maxMp;
        animator = gameObject.GetComponent<Animator>();
        slider1 = GameObject.Find("HP").GetComponent<Slider>();
        slider2 = GameObject.Find("MP").GetComponent<Slider>();
        slider1.value = maxHealth;
        slider2.value = maxMp;
        CameraControllerNet cameraController;

        if (isLocalPlayer)
        {
            cameraController = FindObjectOfType<CameraControllerNet>();
            if (cameraController != null)
            {
                cameraController.SetTarget(transform);
            }
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
    
    [Command]
    private void CmdPlayerDeath()//��ɫ����
    {
        Debug.Log("cmdPlayerDie");
        RpcPlayerDied();
    }
    [ClientRpc]
    private void RpcPlayerDied()
    {
        Debug.Log("RpcPlyaerDied");
        if(hp > 0)
        {
            StartCoroutine(LoadScene(true));
        }
        if(hp <= 0) 
        {
            StartCoroutine(LoadScene(false));
        }
    }
    IEnumerator LoadScene(bool isWin)
    {
        if (isWin)
        {
            yield return new WaitForSeconds(0.5f);//�ӳ��������ת
            SceneManager.LoadSceneAsync("Victory");
        }
        if (!isWin)
        {
            yield return new WaitForSeconds(0.5f);//�ӳ��������ת
            SceneManager.LoadSceneAsync("GameOver");
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Supplies"))
        {
            OnSupply(collision);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {

        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (hp <= 0)
        {
            UnityEngine.Debug.Log("Update");
            CmdPlayerDeath();
            //RpcPlayerDied();
        }
    }
}
