using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;




public class Move : MonoBehaviour
{
    public float moveSpeed = 6f;
    public Vector3 currentDirection = new(1, 0, 0);
    public Vector3 direction;
    public int faceDir = 1;//��ɫ����

    //������Ƶ����
    public float timer = 0f;//��ֹ��Ƶ���������ļ�ʱ��
    private bool startrun = true;
    public float timer2 = 0f;//��ֹ��ʼ�ܲ��ظ��ж�

    private Animator animator;

    private PlayerController playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerController = gameObject.GetComponent<PlayerController>();
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  // ��ȡˮƽ����
        float vertical = Input.GetAxisRaw("Vertical");  // ��ȡ��ֱ����
        direction = new(horizontal, vertical, 0);
        Vector3 dir = new(horizontal, vertical, 0);

        //����
        if (dir != Vector3.zero&& playerController.hp>0)
        {

            //����״̬Ϊ����
            animator.SetBool("isRun", true);
            transform.localPosition += moveSpeed * Time.deltaTime * direction.normalized;
            playerController.ChangePosition(transform.localPosition);

            //�����ܲ���Ч�����ж��Ƿ��ǴӴ�����ʼ�ܲ�
            runbgs();
            startrun = false;

            if (dir.x >= 0)//��ɫ�����ұ�
            {
                faceDir = 1;
                transform.localScale = new(faceDir, 1, 1);


            }
            else if (dir.x < 0)//��ɫ�������
            {
                faceDir = -1;
                transform.localScale = new(faceDir, 1, 1);

            }
           

        }
        else
        {
            animator.SetBool("isRun", false);
            if (timer2 >= 0.5)
            {
                timer2 = 0f; // ��ʱ
                startrun = true;
            }

        }
        //վ��
        if (direction.normalized != Vector3.zero)
        {
            currentDirection = direction.normalized;
        }
    }

    private void runbgs()//�����ܲ���Ч
    {
        if (startrun == true)//���ο�ʼ�ܲ��ж�
        {
            AudioManager.instance.PlayBGS("run");
        }
        timer += Time.deltaTime;//��ʼ��ʱ
        if (timer >= 0.5)
        {
            AudioManager.instance.PlayBGS("run");//�����ܲ���Ч
            timer = 0f; // ��ʱ
            startrun =false;
        }
    }
}
