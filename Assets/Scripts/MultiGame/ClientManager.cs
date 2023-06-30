using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClientManager : NetworkBehaviour
{
    public NetworkManager networkManager;

    private void Start()
    {
        // ���ӵ��Ʒ�����
        NetworkManager.singleton.networkAddress = "http://101.43.168.188";
        //NetworkManager.singleton.networkPort = 7777; // Mirror�����������Ķ˿ں�
        NetworkManager.singleton.StartClient();
        if (networkManager != null)
        {
            if (isServer)
            {
                // ����Ƿ�����������ΪHost����
                networkManager.StartHost();
            }
            else
            {
                // ������Ƿ�����������ΪClient����
                networkManager.StartClient();
            }
        }
    }

}
