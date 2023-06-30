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
            // 获取系统支持的所有分辨率
            resolutions = Screen.resolutions;

            // 清空下拉菜单中的选项
           dropdown.ClearOptions();

            // 创建分辨率选项列表
            List<string> resolutionOptions = new List<string>();

            // 添加每个分辨率作为选项
            foreach (Resolution resolution in resolutions)
            {
                string option = resolution.width + " x " + resolution.height;
                resolutionOptions.Add(option);
            }

            // 将分辨率选项列表添加到下拉菜单中
            dropdown.AddOptions(resolutionOptions);
        }

        public void SetResolution(int resolutionIndex)
        {
            // 获取所选分辨率
            Resolution resolution = resolutions[resolutionIndex];

            // 设置游戏分辨率
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }



        // Update is called once per frame
        void Update()
        {

        }
    }

