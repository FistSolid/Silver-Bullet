using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IceThawIcon : MonoBehaviour
{
    //获取技能图标
    public Image skillIcon;
    //获取倒计时文本
    public Text cooldownText;
    //技能冷却时间
    public float cooldownDuration = 11f;
    //剩下的冷却时间
    private float cooldownTimer = 0f;
    //判断是否进入冷却
    private bool isCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void StartCooldown(float cd)
    {
        cooldownDuration = cd;
        if (!isCooldown)
        {
            isCooldown = true;
            cooldownTimer = cooldownDuration;
        }
    }

    //更新倒计时的时间
    private void UpdateCooldownUI()
    {
        skillIcon.fillAmount = 1 - cooldownTimer / cooldownDuration;
        cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            UpdateCooldownUI();

            if (cooldownTimer <= 0f)
            {
                // 倒计时结束
                isCooldown = false;
                cooldownTimer = 0f;
                skillIcon.fillAmount = 1f;
                cooldownText.text = "";
            }
        }
    }
}
