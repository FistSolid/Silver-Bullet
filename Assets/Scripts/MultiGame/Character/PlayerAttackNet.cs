using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttackNet : NetworkBehaviour
{

    public bool canAttack = true;
    public bool canReleaseFireBall = true;
    public bool canReleaseIceThaw = true;

    public float damage = 4f;

    public int attackStatus = 0;//攻击状态，0普通攻击，1~5对应不同技能

    public Vector3 attackDirection;
    private MoveNet playerMove;

    //各种子弹的预制体
    public GameObject bulletPrefab;
    public GameObject fireballPrefab;
    public GameObject iceThawPrefab;
    //各种子弹的脚本
    private PlayerBulletNet plyBlt;
    private FireBall fireBall;
    private IceThaw iceThaw;
    //controller脚本
    private PlayerControllerNet playerController;

    //动画器
    public Animator animator;

    public FireBallIcon icon1;
    public IceThawIcon icon2;

    public Transform bulletSpawnPos;

    void Start()
    {

        animator = GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerControllerNet>();//获得controller

        plyBlt = bulletPrefab.GetComponent<PlayerBulletNet>();//获得子弹类的对象
        fireBall = fireballPrefab.GetComponent<FireBall>();//获得火球类的对象
        iceThaw = iceThawPrefab.GetComponent<IceThaw>();//获取冰融

        icon1 = GameObject.Find("Skill1Icon").GetComponent<FireBallIcon>();
        icon2 = GameObject.Find("Skill2Icon").GetComponent<IceThawIcon>();
    }

    //攻击，并判断释放哪种攻击
    [Command]
    public void CmdAttack(Vector3 direction)
    { 
        if (attackStatus == 0) { RpcNormalAttack(direction);}//状态0，普攻
        else if (attackStatus == 1) { RpcReleaseFireBall(direction); }//状态1，火球
        else if (attackStatus == 2) { RpcReleaseIceThaw(direction); }//状态2：冰融
    }



    [ClientRpc]
    void RpcNormalAttack(Vector3 direction)
    {
        canAttack = false;

        playerMove = gameObject.GetComponent<MoveNet>();


        //触发攻击动画
        animator.SetTrigger("onAttack");

        plyBlt.CreatePlayerBulletNet(bulletSpawnPos.position, direction, 10f, 8f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);

        //射击间隔
        Invoke(nameof(ResetCanAttack), 0.1f);//每0.1s射一发子弹
    }
    [ClientRpc]
    public void RpcReleaseFireBall(Vector3 direction)
    {
        if (playerController.mp < 5f) { return; }
        else { playerController.ReleaseSkill(5f); }//消耗5mp值释放
        canAttack = false;
        animator.SetTrigger("onReleasingSkill1");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveNet>();

        Vector3 vec = new(playerMove.faceDir * 1.75f, 1f, 0f);
        Vector3 spawnPosition = transform.position + vec;

        fireBall.CreatFireBall(spawnPosition, direction, 8f, 10f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        Invoke(nameof(ResetCanAttack), 0.75f);//每0.5s能射一发火球
    }

    [ClientRpc]
    public void RpcReleaseIceThaw(Vector3 direction)
    {
        if (playerController.mp < 10f) { return; }
        else { playerController.ReleaseSkill(10f); }
        canAttack = false;
        animator.SetTrigger("onReleasingSkill2");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveNet>();

        Vector3 vec = new(playerMove.faceDir * 1.75f, 1f, 0f);
        Vector3 spawnPosition = transform.position;

        iceThaw.CreatIceThaw(spawnPosition, 10f, 5f, playerController.id);
        Invoke(nameof(ResetCanAttack), 1f);

    }
    //普通攻击CD
    public void ResetCanAttack()
    {
        canAttack = true;
    }
    //技能结束后将攻击状态重置
    public void ResetAttackStatus()
    {
        attackStatus = 0;
    }
    //火球cd转好
    public void ResetCanReleaseFireball()
    {
        canReleaseFireBall = true;
    }
    //冰融cd转好
    public void ResetCanReleaseIceThaw()
    {
        canReleaseIceThaw = true;
    }

    void Update()
    {
        if(isLocalPlayer) { 
            if (canAttack)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    Vector3 center = new(Screen.width / 2, Screen.height / 2, 0);
                    attackDirection = (Input.mousePosition - center).normalized;
                    CmdAttack(attackDirection);
                }
            }
            //如果可以释放火球
            if (canReleaseFireBall)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    icon1.StartCooldown(16);
                    canReleaseFireBall = false;//无法释放火球了
                    attackStatus = 1;//cd转好，并且按下1键
                    Invoke(nameof(ResetAttackStatus), 8f);//8s后攻击状态变为0（技能持续时间）
                    Invoke(nameof(ResetCanReleaseFireball), 16f);//16s后cd再次转好（持续时间加冷却时间）
                }
            }

            //如果可以释放冰融
            if (canReleaseIceThaw)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    icon2.StartCooldown(20);
                    canReleaseIceThaw = false;
                    attackStatus = 2;
                    Invoke(nameof(ResetAttackStatus), 1f);
                    Invoke(nameof(ResetCanReleaseIceThaw), 20f);
                }
            }

        }
    }
}
