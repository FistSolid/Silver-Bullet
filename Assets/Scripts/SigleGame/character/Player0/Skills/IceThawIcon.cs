using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IceThawIcon : MonoBehaviour
{
    //��ȡ����ͼ��
    public Image skillIcon;
    //��ȡ����ʱ�ı�
    public Text cooldownText;
    //������ȴʱ��
    public float cooldownDuration = 11f;
    //ʣ�µ���ȴʱ��
    private float cooldownTimer = 0f;
    //�ж��Ƿ������ȴ
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

    //���µ���ʱ��ʱ��
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
                // ����ʱ����
                isCooldown = false;
                cooldownTimer = 0f;
                skillIcon.fillAmount = 1f;
                cooldownText.text = "";
            }
        }
    }
}
