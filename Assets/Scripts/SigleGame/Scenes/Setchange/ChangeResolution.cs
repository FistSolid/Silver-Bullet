using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeResolution : MonoBehaviour
{
    // Start is called before the first frame update

        
        public TMP_Dropdown dropdown;
        private Resolution[] resolutions;

        private void Start()
        {
            // ��ȡϵͳ֧�ֵ����зֱ���
            resolutions = Screen.resolutions;

            // ��������˵��е�ѡ��
           dropdown.ClearOptions();

            // �����ֱ���ѡ���б�
            List<string> resolutionOptions = new List<string>();

            // ���ÿ���ֱ�����Ϊѡ��
            foreach (Resolution resolution in resolutions)
            {
                string option = resolution.width + " x " + resolution.height;
                resolutionOptions.Add(option);
            }

            // ���ֱ���ѡ���б���ӵ������˵���
            dropdown.AddOptions(resolutionOptions);
        }

        public void SetResolution(int resolutionIndex)
        {
            // ��ȡ��ѡ�ֱ���
            Resolution resolution = resolutions[resolutionIndex];

            // ������Ϸ�ֱ���
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }



        // Update is called once per frame
        void Update()
        {

        }
    }

