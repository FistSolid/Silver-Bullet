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

    public int attackStatus = 0;//����״̬��0��ͨ������1~5��Ӧ��ͬ����

    public Vector3 attackDirection;
    private MoveNet playerMove;

    //�����ӵ���Ԥ����
    public GameObject bulletPrefab;
    public GameObject fireballPrefab;
    public GameObject iceThawPrefab;
    //�����ӵ��Ľű�
    private PlayerBulletNet plyBlt;
    private FireBall fireBall;
    private IceThaw iceThaw;
    //controller�ű�
    private PlayerControllerNet playerController;

    //������
    public Animator animator;

    public FireBallIcon icon1;
    public IceThawIcon icon2;

    public Transform bulletSpawnPos;

    void Start()
    {

        animator = GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerControllerNet>();//���controller

        plyBlt = bulletPrefab.GetComponent<PlayerBulletNet>();//����ӵ���Ķ���
        fireBall = fireballPrefab.GetComponent<FireBall>();//��û�����Ķ���
        iceThaw = iceThawPrefab.GetComponent<IceThaw>();//��ȡ����

        icon1 = GameObject.Find("Skill1Icon").GetComponent<FireBallIcon>();
        icon2 = GameObject.Find("Skill2Icon").GetComponent<IceThawIcon>();
    }

    //���������ж��ͷ����ֹ���
    [Command]
    public void CmdAttack(Vector3 direction)
    { 
        if (attackStatus == 0) { RpcNormalAttack(direction);}//״̬0���չ�
        else if (attackStatus == 1) { RpcReleaseFireBall(direction); }//״̬1������
        else if (attackStatus == 2) { RpcReleaseIceThaw(direction); }//״̬2������
    }



    [ClientRpc]
    void RpcNormalAttack(Vector3 direction)
    {
        canAttack = false;

        playerMove = gameObject.GetComponent<MoveNet>();


        //������������
        animator.SetTrigger("onAttack");

        plyBlt.CreatePlayerBulletNet(bulletSpawnPos.position, direction, 10f, 8f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);

        //������
        Invoke(nameof(ResetCanAttack), 0.1f);//ÿ0.1s��һ���ӵ�
    }
    [ClientRpc]
    public void RpcReleaseFireBall(Vector3 direction)
    {
        if (playerController.mp < 5f) { return; }
        else { playerController.ReleaseSkill(5f); }//����5mpֵ�ͷ�
        canAttack = false;
        animator.SetTrigger("onReleasingSkill1");

        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveNet>();

        Vector3 vec = new(playerMove.faceDir * 1.75f, 1f, 0f);
        Vector3 spawnPosition = transform.position + vec;

        fireBall.CreatFireBall(spawnPosition, direction, 8f, 10f, Vector3.Angle(new(1, 0, 0), attackDirection), playerController.id);
        Invoke(nameof(ResetCanAttack), 0.75f);//ÿ0.5s����һ������
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
    //��ͨ����CD
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
}
