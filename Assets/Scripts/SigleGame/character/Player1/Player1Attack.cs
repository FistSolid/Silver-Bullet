using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Attack : MonoBehaviour
{
    public bool canAttack = true;
    public bool canReleaseBlackHole = true;
    public bool canReleaseMagicBullet = true;

    public float damage = 4f;


    public int attackStatus = 0;//攻击状态，0普通攻击，1~5对应不同技能

    public Vector3 attackDirection;
    private Move playerMove;


    //各种子弹的预制体
    public GameObject bulletPrefab;
    public GameObject blackHolePrefab;
    public GameObject magicBulletPrefab;
    //各种子弹的脚本
    private PlayerBullet plyBlt;
    private BlackHole blackHole;
    private MagicBullet magicBullet;
    //controller脚本
    private PlayerController playerController;

    //动画器
    public Animator animator;

    public BlackHoleIcon icon1;
    public MagicBulletIcon icon2;

    public Transform bulletSpawnPos;

    void Start()
    {

        animator = GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerController>();//获得controller

       
        plyBlt = bulletPrefab.GetComponent<PlayerBullet>();//获得子弹类的对象
        blackHole = blackHolePrefab.GetComponent<BlackHole>();//获取黑洞
        magicBullet = magicBulletPrefab.GetComponent<MagicBullet>();//获取子弹

        icon1 = GameObject.Find("Skill1Icon").GetComponent<BlackHoleIcon>();
        icon2 = GameObject.Find("Skill2Icon").GetComponent<MagicBulletIcon>();
    }

    //攻击，并判断释放哪种攻击
    public void Attack()
    {
        if (attackStatus == 0) { NormalAttack(); }//状态0，普攻
        else if (attackStatus == 1) { ReleaseBlackHole(); }//状态1：黑洞
        else if (attackStatus == 2) { ReleaseMagicBullet(); }//状态2：魔法子弹
    }

    //普通攻击
    public void NormalAttack()
    {
        canAttack = false;

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();


        //触发攻击动画
        animator.SetTrigger("onAttack");

        //播放攻击音效
        AudioManager.instance.PlayBGS("Bshoot");

        //生成子弹
        plyBlt.CreatePlayerBullet(bulletSpawnPos.position, attackDirection, 18f, 4f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        //射击间隔
        Invoke(nameof(ResetCanAttack), 0.3f);//每0.3s射一发子弹
    }

    public void ReleaseBlackHole()
    {
        if (playerController.mp < 10f) { return; }
        else { playerController.ReleaseSkill(10f); }
        canAttack = false;
        BlackHole.hasCollided = false;
        //触发攻击动画
        animator.SetTrigger("onAttack");
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        AudioManager.instance.PlayBGM("Bskill1");

        //生成子弹
        plyBlt.CreatePlayerBullet(bulletSpawnPos.position, attackDirection, 18f, 4f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);

        Invoke(nameof(ResetCanAttack), 1f);
    }
    //如果死亡则不能攻击
    public void Dead()
    {
        if (playerController.isDead == true)
        {
            canAttack = false;
        }
    }
   
    public void ReleaseMagicBullet()
    {
        if (playerController.mp < 5f) { return; }
        else { playerController.ReleaseSkill(5f); }//消耗5mp值释放
        canAttack = false;
        animator.SetTrigger("onReleasingSkill2");

        //播放攻击音效
        AudioManager.instance.PlayBGS("Askill2");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        magicBullet.CreatMagicBullet(bulletSpawnPos.position, attackDirection, 8f, 10f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        Invoke(nameof(ResetCanAttack), 1f);//每1s能射一发魔力子弹
    }


    public void ResetCanAttack()
    {
        canAttack = true;
    }
    //技能结束后将攻击状态重置
    public void ResetAttackStatus()
    {
        attackStatus = 0;
    }
   
    //黑洞cd转好
    public void ResetCanReleaseBlackHole()
    {
        canReleaseBlackHole = true;
    }

    //闪现cd转好
    public void ResetCanReleaseMagicBullet()
    {
        canReleaseMagicBullet = true;
    }
    void Update()
    {
        Dead();
        if (canAttack)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 center = new(Screen.width / 2, Screen.height / 2, 0);
                attackDirection = (Input.mousePosition - center).normalized;
                Attack();
            }
        }
        
        //如果可以释放黑洞
        if (canReleaseBlackHole)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                icon1.StartCooldown(10);
                canReleaseBlackHole = false;
                attackStatus = 1;
                Invoke(nameof(ResetAttackStatus), 1f);
                Invoke(nameof(ResetCanReleaseBlackHole), 10f);
            }
        }
        //如果可以释放魔力子弹
        if (canReleaseMagicBullet)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                icon2.StartCooldown(16);
                canReleaseMagicBullet = false;//无法释放了
                attackStatus = 2;//cd转好，并且按下2键
                Invoke(nameof(ResetAttackStatus), 8f);//8s后攻击状态变为0（技能持续时间）
                Invoke(nameof(ResetCanReleaseMagicBullet), 16f);//16s后cd再次转好（持续时间加冷却时间）
            }
        }

    }
}

