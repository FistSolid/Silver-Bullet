using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player0Attack : MonoBehaviour
{

    public bool canAttack = true;
    public bool canReleaseFireBall = true;
    public bool canReleaseIceThaw = true;
    
   

    public float damage= 4f;

    
    public int attackStatus = 0;//����״̬��0��ͨ������1~5��Ӧ��ͬ����

    public Vector3 attackDirection;
    private Move playerMove;

    //�����ӵ���Ԥ����
    public GameObject bulletPrefab;
    public GameObject fireballPrefab;
    public GameObject iceThawPrefab;
    
    //�����ӵ��Ľű�
    private PlayerBullet plyBlt;
    private FireBall fireBall;
    private IceThaw iceThaw;
    
    //controller�ű�
    private PlayerController playerController;

    //������
    public Animator animator;


    public FireBallIcon icon1;
    public IceThawIcon icon2;

    public Transform bulletSpawnPos;
    

    void Start()
    {

        animator = GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerController>();//���controller

        plyBlt = bulletPrefab.GetComponent<PlayerBullet>();//����ӵ���Ķ���
        fireBall = fireballPrefab.GetComponent<FireBall>();//��û�����Ķ���
        iceThaw = iceThawPrefab.GetComponent<IceThaw>();//��ȡ����
       

        icon1 = GameObject.Find("Skill1Icon").GetComponent<FireBallIcon>();
        icon2 = GameObject.Find("Skill2Icon").GetComponent<IceThawIcon>();
        
    }

    //���������ж��ͷ����ֹ���
    public void Attack()
    {
        if (attackStatus == 0) { NormalAttack(); }//״̬0���չ�
        else if (attackStatus == 1){ ReleaseFireBall(); }//״̬1������
        else if (attackStatus == 2) { ReleaseIceThaw(); }//״̬2������
        
    }


    //��ɫ�������ܹ���
    public void Dead()
    {
        if (playerController.isDead==true)
        {
            canAttack = false;
        }
    }
    
    //��ͨ����
    public void NormalAttack()
    {
        canAttack = false;

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        
        //������������
        animator.SetTrigger("onAttack");

        //���Ź�����Ч
        AudioManager.instance.PlayBGS("Ashoot");

        //�����ӵ�
        plyBlt.CreatePlayerBullet(bulletSpawnPos.position, attackDirection, 18f, 4f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        //������
        Invoke(nameof(ResetCanAttack), 0.3f);//ÿ0.5s��һ���ӵ�
    }

    public void ReleaseFireBall()
    {
        if (playerController.mp < 5f) { return; }
        else { playerController.ReleaseSkill(5f); }//����5mpֵ�ͷ�
        canAttack = false;
        animator.SetTrigger("onReleasingSkill1");

        //���Ź�����Ч
        AudioManager.instance.PlayBGS("Askill1");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        fireBall.CreatFireBall(bulletSpawnPos.position, attackDirection, 8f, 10f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        Invoke(nameof(ResetCanAttack), 1f);//ÿ0.5s����һ������
    }

    public void ReleaseIceThaw()
    {
        if (playerController.mp < 10f) { return; }
        else { playerController.ReleaseSkill(10f); }
        canAttack = false;
        animator.SetTrigger("onReleasingSkill2");

        //���Ź�����Ч
        AudioManager.instance.PlayBGS("Askill2");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        iceThaw.CreatIceThaw(bulletSpawnPos.position, 10f, 5f, playerController.id);
        Invoke(nameof(ResetCanAttack), 1f);

    }

  
    public void ResetCanAttack()
    {
        canAttack = true;
    }
    //���ܽ����󽫹���״̬����
    public void ResetAttackStatus()
    {
        attackStatus = 0;
    }
    //����cdת��
    public void ResetCanReleaseFireball()
    {
        canReleaseFireBall = true;
    }
    //����cdת��
    public void ResetCanReleaseIceThaw()
    {
        canReleaseIceThaw = true;
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
        //��������ͷŻ���
        if (canReleaseFireBall)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                icon1.StartCooldown(16);
                canReleaseFireBall = false;//�޷��ͷŻ�����
                attackStatus = 1;//cdת�ã����Ұ���1��
                Invoke(nameof(ResetAttackStatus), 8f);//8s�󹥻�״̬��Ϊ0�����ܳ���ʱ�䣩
                Invoke(nameof(ResetCanReleaseFireball), 16f);//16s��cd�ٴ�ת�ã�����ʱ�����ȴʱ�䣩
            }
        }

        //��������ͷű���
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
