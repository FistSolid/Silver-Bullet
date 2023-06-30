/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Muninn;
using Unity.Muninn.Model;
using Unity.Muninn.Transport;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.InteropServices;


public class EventHandler : MuninnBehaviour
{
    private string userID = Guid.NewGuid().ToString(); // ���ID����֤���ظ�
    private uint playerSenderId = 0;
    private uint masterClientId = 0;
    private string playerName = "����С�м�";

    public GameObject UIController;

    [Tooltip("������ƽ��ڸ��б������ѡ���б�ʹ��','�ָ���")]
    public string playerNames = "";

    [Tooltip("UOS Ӧ�� ID")]
    public string UosAppId;
    [Tooltip("UOS Ӧ�� AppSecret")]
    public string UosAppSecret;
    [Tooltip("��������ID")]
    public string RoomProfileUUID;
    [Tooltip("ѡ��ͬ������Э��")]
    public MuninnTransportType TransportType;

    public string publicPlayerName
    {
        get { return playerName; }
    }

    public uint publicPlayerSenderId
    {
        get { return playerSenderId; }
    }


    [Serializable]
    public class MuninnSettingsFromWeb
    {
        public string LobbyDomain;
        public string UosAppId;
        public string UosAppSecret;
        public string RoomProfileUuid;
        public string WebsocketProxy;
        public int ClientId;
    }

    [DllImport("__Internal")]
    private static extern void SetRoomIdToWeb(string roomId);

    [DllImport("__Internal")]
    private static extern string GetMuninnSettingsFromWeb();

    private void Start()
    {
        // ����ѡ��ͬ��Э��
        MuninnSettings.TransportType = TransportType;

        MuninnSettings.UosAppId = UosAppId;
        MuninnSettings.UosAppSecret = UosAppSecret;
        MuninnSettings.RoomProfileUUID = RoomProfileUUID;

        // ��ǰ����û���Ϣ������ 
        MuninnNetwork.PlayerInfo = new MuninnPlayerInfo()
        {
            Id = userID,
            Name = playerName,
            Properties = new Dictionary<String, String>
            {
                ["key1"] = "val1",
                ["��"] = "ֵ1",
            },
        };

#if UNITY_WEBGL && !UNITY_EDITOR
    Debug.Log("in webgl");
    setupMuninnSettings();
#endif
    }

    // 1. �����¼��ص�

    /// <summary>
    /// ���뷿��ɹ�
    /// </summary>
    /// <param name="muninnRoomView"></param>
    public override void OnJoinedRoom(MuninnRoomView muninnRoomView)
    {
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("��ӭ���뷿�� {0}��", muninnRoomView.Room.Id), 0, MessageType.systemMessage);

        foreach (MuninnCachedEvent e in muninnRoomView.CachedEvents)
        {
            // ÿ�� cached ����չʾ
            UIController.GetComponent<UIController>().AddMessageItem(Encoding.UTF8.GetString(e.Data, 0, e.Data.Length), e.SenderId, MessageType.otherMessage);
        }

        // ��ʼ������ id �� ��� id
        if (muninnRoomView.MasterClientId != null)
        {
            this.masterClientId = muninnRoomView.MasterClientId;
        }
        playerSenderId = muninnRoomView.SenderId;
        UpdatePlayerList();

        UIController.GetComponent<UIController>().JoinedRoomHandler();

#if UNITY_WEBGL && !UNITY_EDITOR
    SetRoomIdToWeb(muninnRoomView.Room.Id);
#endif
    }

    /// <summary>
    /// ���뷿��ʧ��
    /// </summary>
    /// <param name="err"></param>
    public override void OnJoinRoomFailed(MuninnError error)
    {
        UIController.GetComponent<UIController>().OnError.Invoke(MuninnCodeLocalize.GetCodeName(error.Code));
    }

    /// <summary>
    /// �Ͽ�����
    /// </summary>
    public override void OnDisconnected()
    {
        UIController.GetComponent<UIController>().LeaveRoomHandler();
    }

    /// <summary>
    /// ��ҽ��뷿��
    /// </summary>
    /// <param name="muninnPlayer"></param>
    public override void OnPlayerEnteredRoom(MuninnPlayer muninnPlayer)
    {
        // ��ȡ��ҵ�ͷ������
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("{0}{1} ���뷿�䡣", muninnPlayer.Name, muninnPlayer.SenderId), 0, MessageType.systemMessage);
        UpdatePlayerList();
    }

    /// <summary>
    /// ����뿪����
    /// </summary>
    /// <param name="muninnPlayer"></param>
    public override void OnPlayerLeftRoom(MuninnPlayer muninnPlayer)
    {
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("{0}{1} �뿪���䡣", muninnPlayer.Name, muninnPlayer.SenderId), 0, MessageType.systemMessage);
        UpdatePlayerList();
    }

    /// <summary>
    /// master client ���
    /// </summary>
    /// <param name="masterClientId"></param>
    public override void OnMasterClientChanged(uint masterClientId)
    {
        base.OnMasterClientChanged(masterClientId);
        this.masterClientId = masterClientId;
        this.UpdatePlayerList();
    }

    /// <summary>
    /// ���յ�һ���¼���������Ϣ
    /// </summary>
    /// <param name="e"></param>
    public override void OnEvent(MuninnEvent e)
    {
        UIController.GetComponent<UIController>().AddMessageItem(Encoding.UTF8.GetString(e.Data), e.SenderId,
        playerSenderId == e.SenderId ? MessageType.myMessage : MessageType.otherMessage);
    }

    /// <summary>
    /// ���ķ���ɹ�
    /// </summary>
    /// <param name="rsp"></param>
    public override void OnSubscribeGroups(MuninnSubscribeGroupResponse rsp)
    {
        base.OnSubscribeGroups(rsp);
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("�������ɹ���"), 0, MessageType.systemMessage);
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("Tips: ��������ͷ�����Ϣ����Ⱥ�������ͨ�Űɣ�"), 0, MessageType.systemMessage);
        UIController.GetComponent<UIController>().ShowUnsubGroupButton();
    }

    /// <summary>
    /// ���ķ���ʧ��
    /// </summary>
    /// <param name="error"></param>
    public override void OnSubscribeGroupsFailed(MuninnError error)
    {
        Debug.LogWarningFormat("[Muninn]: OnSubscribeGroupsFailed() was called by Muninn");
        UIController.GetComponent<UIController>().AddMessageItem(MuninnCodeLocalize.GetCodeName(error.Code), 0, MessageType.systemMessage);
    }

    /// <summary>
    /// ȡ�����ķ���ɹ�
    /// </summary>
    /// <param name="rsp"></param>
    public override void OnUnsubscribeGroups(MuninnUnsubscribeGroupsResponse rsp)
    {
        base.OnUnsubscribeGroups(rsp);
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("�˳�����ɹ���"), 0, MessageType.systemMessage);
        UIController.GetComponent<UIController>().ShowSubscribeGroupButton();

    }

    /// <summary>
    /// ȡ�����ķ���ʧ��
    /// </summary>
    /// <param name="error"></param>
    public override void OnUnsubscribeGroupsFailed(MuninnError error)
    {
        Debug.LogWarningFormat("[Muninn]: OnUnsubscribeGroupsFailed() was called by Muninn");
        UIController.GetComponent<UIController>().AddMessageItem(MuninnCodeLocalize.GetCodeName(error.Code), 0, MessageType.systemMessage);
    }

    /// <summary>
    /// ���յ� server call
    /// </summary>
    /// <param name="e"></param>
    public override void OnServerCall(MuninnEvent e)
    {
        UIController.GetComponent<UIController>().AddMessageItem(Encoding.UTF8.GetString(e.Data), e.SenderId, playerSenderId == e.SenderId ? MessageType.myMessage : MessageType.otherMessage);
    }


    /// <summary>
    /// �ɹ��������
    /// </summary>
    /// <param name="rsp"></param>
    public override void OnKickedPlayer(MuninnKickPlayerResponse rsp)
    {
        Debug.Log("kicked success");
    }

    /// <summary>
    /// �������ʧ��
    /// </summary>
    /// <param name="error"></param>
    public override void OnKickPlayerFailed(MuninnError error)
    {
        Debug.Log("kicked fail");
        UIController.GetComponent<UIController>().AddMessageItem(MuninnCodeLocalize.GetCodeName(error.Code), 0, MessageType.systemMessage);
    }

    // 2. һЩ���ߺ�����

    /// <summary>
    /// �� web ��ȡ app ��Ϣ������Э�顢�ͻ��� id ����Ϣ
    /// </summary>
    public void setupMuninnSettings()
    {
        string jsonString = GetMuninnSettingsFromWeb();
        MuninnSettingsFromWeb settings = JsonUtility.FromJson<MuninnSettingsFromWeb>(jsonString);
        if (settings.LobbyDomain != null && settings.LobbyDomain != "")
        {
            MuninnSettings.LobbyDomain = settings.LobbyDomain;
        }
        MuninnSettings.UosAppId = settings.UosAppId;
        MuninnSettings.UosAppSecret = settings.UosAppSecret;
        MuninnSettings.RoomProfileUUID = settings.RoomProfileUuid;
        if (settings.WebsocketProxy != null && settings.WebsocketProxy != "")
        {
            MuninnSettings.WebsocketProxy = settings.WebsocketProxy;
        }
        MuninnNetwork.PlayerInfo.Name = GetPlayerNameByClientid(settings.ClientId);
        playerName = MuninnNetwork.PlayerInfo.Name;
    }

    /// <summary>
    /// ͨ���ͻ��� id �����ȡһ���������
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetPlayerNameByClientid(int index)
    {
        string[] list = playerNames.Split(',');
        return list[index % list.Length];
    }

    /// <summary>
    /// ���� senderId ��ѯ���
    /// </summary>
    /// <param name="senderId"></param>
    /// <returns></returns>
    public MuninnPlayer GetPlayerBySenderId(uint senderId)
    {
        MuninnRoomView view = GetMuninnRoomView();
        return view.GetPlayerBySenderId(senderId);
    }

    /// <summary>
    /// ��������б�
    /// </summary>
    private void UpdatePlayerList()
    {
        MuninnRoomView view = GetMuninnRoomView();
        if (view == null) return;

        UIController.GetComponent<UIController>().ReplacePlayerList(view.Players, view.SenderId, view.MasterClientId);

    }


}
*/