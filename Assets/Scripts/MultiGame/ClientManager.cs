using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClientManager : NetworkBehaviour
{
    public NetworkManager networkManager;

    private void Start()
    {
        // 连接到云服务器
        NetworkManager.singleton.networkAddress = "http://101.43.168.188";
        //NetworkManager.singleton.networkPort = 7777; // Mirror服务器监听的端口号
        NetworkManager.singleton.StartClient();
        if (networkManager != null)
        {
            if (isServer)
            {
                // 如果是服务器，则作为Host启动
                networkManager.StartHost();
            }
            else
            {
                // 如果不是服务器，则作为Client启动
                networkManager.StartClient();
            }
        }
    }

}
