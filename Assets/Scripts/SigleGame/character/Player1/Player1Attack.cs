using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Attack : MonoBehaviour
{
    public bool canAttack = true;
    public bool canReleaseBlackHole = true;
    public bool canReleaseMagicBullet = true;

    public float damage = 4f;


    public int attackStatus = 0;//����״̬��0��ͨ������1~5��Ӧ��ͬ����

    public Vector3 attackDirection;
    private Move playerMove;


    //�����ӵ���Ԥ����
    public GameObject bulletPrefab;
    public GameObject blackHolePrefab;
    public GameObject magicBulletPrefab;
    //�����ӵ��Ľű�
    private PlayerBullet plyBlt;
    private BlackHole blackHole;
    private MagicBullet magicBullet;
    //controller�ű�
    private PlayerController playerController;

    //������
    public Animator animator;

    public BlackHoleIcon icon1;
    public MagicBulletIcon icon2;

    public Transform bulletSpawnPos;

    void Start()
    {

        animator = GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerController>();//���controller

       
        plyBlt = bulletPrefab.GetComponent<PlayerBullet>();//����ӵ���Ķ���
        blackHole = blackHolePrefab.GetComponent<BlackHole>();//��ȡ�ڶ�
        magicBullet = magicBulletPrefab.GetComponent<MagicBullet>();//��ȡ�ӵ�

        icon1 = GameObject.Find("Skill1Icon").GetComponent<BlackHoleIcon>();
        icon2 = GameObject.Find("Skill2Icon").GetComponent<MagicBulletIcon>();
    }

    //���������ж��ͷ����ֹ���
    public void Attack()
    {
        if (attackStatus == 0) { NormalAttack(); }//״̬0���չ�
        else if (attackStatus == 1) { ReleaseBlackHole(); }//״̬1���ڶ�
        else if (attackStatus == 2) { ReleaseMagicBullet(); }//״̬2��ħ���ӵ�
    }

    //��ͨ����
    public void NormalAttack()
    {
        canAttack = false;

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();


        //������������
        animator.SetTrigger("onAttack");

        //���Ź�����Ч
        AudioManager.instance.PlayBGS("Bshoot");

        //�����ӵ�
        plyBlt.CreatePlayerBullet(bulletSpawnPos.position, attackDirection, 18f, 4f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        //������
        Invoke(nameof(ResetCanAttack), 0.3f);//ÿ0.3s��һ���ӵ�
    }

    public void ReleaseBlackHole()
    {
        if (playerController.mp < 10f) { return; }
        else { playerController.ReleaseSkill(10f); }
        canAttack = false;
        BlackHole.hasCollided = false;
        //������������
        animator.SetTrigger("onAttack");
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        AudioManager.instance.PlayBGM("Bskill1");

        //�����ӵ�
        plyBlt.CreatePlayerBullet(bulletSpawnPos.position, attackDirection, 18f, 4f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);

        Invoke(nameof(ResetCanAttack), 1f);
    }
    //����������ܹ���
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
        else { playerController.ReleaseSkill(5f); }//����5mpֵ�ͷ�
        canAttack = false;
        animator.SetTrigger("onReleasingSkill2");

        //���Ź�����Ч
        AudioManager.instance.PlayBGS("Askill2");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();

        magicBullet.CreatMagicBullet(bulletSpawnPos.position, attackDirection, 8f, 10f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        Invoke(nameof(ResetCanAttack), 1f);//ÿ1s����һ��ħ���ӵ�
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
   
    //�ڶ�cdת��
    public void ResetCanReleaseBlackHole()
    {
        canReleaseBlackHole = true;
    }

    //����cdת��
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
        
        //��������ͷźڶ�
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
        //��������ͷ�ħ���ӵ�
        if (canReleaseMagicBullet)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                icon2.StartCooldown(16);
                canReleaseMagicBullet = false;//�޷��ͷ���
                attackStatus = 2;//cdת�ã����Ұ���2��
                Invoke(nameof(ResetAttackStatus), 8f);//8s�󹥻�״̬��Ϊ0�����ܳ���ʱ�䣩
                Invoke(nameof(ResetCanReleaseMagicBullet), 16f);//16s��cd�ٴ�ת�ã�����ʱ�����ȴʱ�䣩
            }
        }

    }
}

