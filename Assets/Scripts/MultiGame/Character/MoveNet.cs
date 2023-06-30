using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Mirror;
using UnityEngine;




public class MoveNet : NetworkBehaviour
{
    public float moveSpeed = 6f;
    public Vector3 currentDirection = new(1, 0, 0);
    public Vector3 direction;
    public int faceDir = 1;//��ɫ����

    private Animator animator;

    private PlayerControllerNet playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerController = gameObject.GetComponent<PlayerControllerNet>();
    }


    void Update()
    {
        if (isLocalPlayer)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");  // ��ȡˮƽ����
            float vertical = Input.GetAxisRaw("Vertical");  // ��ȡ��ֱ����
            direction = new(horizontal, vertical, 0);
            Vector3 dir = new(horizontal, vertical, 0);

            //����
            if (dir != Vector3.zero)
            {

                //����״̬Ϊ����
                animator.SetBool("isRun", true);
                transform.localPosition += moveSpeed * Time.deltaTime * direction.normalized;
                playerController.ChangePosition(transform.localPosition);

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
            }
            //վ��
            if (direction.normalized != Vector3.zero)
            {
                currentDirection = direction.normalized;
            }

        }
    }
}
