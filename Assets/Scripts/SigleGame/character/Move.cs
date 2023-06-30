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
    public int faceDir = 1;//角色朝向

    //关于音频的量
    public float timer = 0f;//防止音频出现杂音的计时器
    private bool startrun = true;
    public float timer2 = 0f;//防止开始跑步重复判定

    private Animator animator;

    private PlayerController playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerController = gameObject.GetComponent<PlayerController>();
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  // 获取水平输入
        float vertical = Input.GetAxisRaw("Vertical");  // 获取垂直输入
        direction = new(horizontal, vertical, 0);
        Vector3 dir = new(horizontal, vertical, 0);

        //行走
        if (dir != Vector3.zero&& playerController.hp>0)
        {

            //设置状态为奔跑
            animator.SetBool("isRun", true);
            transform.localPosition += moveSpeed * Time.deltaTime * direction.normalized;
            playerController.ChangePosition(transform.localPosition);

            //调用跑步音效，并判定是否是从待机开始跑步
            runbgs();
            startrun = false;

            if (dir.x >= 0)//角色朝向右边
            {
                faceDir = 1;
                transform.localScale = new(faceDir, 1, 1);


            }
            else if (dir.x < 0)//角色朝向左边
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
                timer2 = 0f; // 定时
                startrun = true;
            }

        }
        //站立
        if (direction.normalized != Vector3.zero)
        {
            currentDirection = direction.normalized;
        }
    }

    private void runbgs()//播放跑步音效
    {
        if (startrun == true)//初次开始跑步判定
        {
            AudioManager.instance.PlayBGS("run");
        }
        timer += Time.deltaTime;//开始计时
        if (timer >= 0.5)
        {
            AudioManager.instance.PlayBGS("run");//播放跑步音效
            timer = 0f; // 定时
            startrun =false;
        }
    }
}
