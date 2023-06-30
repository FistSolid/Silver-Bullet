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
    private string userID = Guid.NewGuid().ToString(); // 玩家ID，保证不重复
    private uint playerSenderId = 0;
    private uint masterClientId = 0;
    private string playerName = "卖报小行家";

    public GameObject UIController;

    [Tooltip("玩家名称将在该列表中随机选择。列表使用','分隔。")]
    public string playerNames = "";

    [Tooltip("UOS 应用 ID")]
    public string UosAppId;
    [Tooltip("UOS 应用 AppSecret")]
    public string UosAppSecret;
    [Tooltip("房间配置ID")]
    public string RoomProfileUUID;
    [Tooltip("选择不同的连接协议")]
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
        // 可以选择不同的协议
        MuninnSettings.TransportType = TransportType;

        MuninnSettings.UosAppId = UosAppId;
        MuninnSettings.UosAppSecret = UosAppSecret;
        MuninnSettings.RoomProfileUUID = RoomProfileUUID;

        // 当前玩家用户信息的配置 
        MuninnNetwork.PlayerInfo = new MuninnPlayerInfo()
        {
            Id = userID,
            Name = playerName,
            Properties = new Dictionary<String, String>
            {
                ["key1"] = "val1",
                ["键"] = "值1",
            },
        };

#if UNITY_WEBGL && !UNITY_EDITOR
    Debug.Log("in webgl");
    setupMuninnSettings();
#endif
    }

    // 1. 处理事件回调

    /// <summary>
    /// 加入房间成功
    /// </summary>
    /// <param name="muninnRoomView"></param>
    public override void OnJoinedRoom(MuninnRoomView muninnRoomView)
    {
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("欢迎加入房间 {0}。", muninnRoomView.Room.Id), 0, MessageType.systemMessage);

        foreach (MuninnCachedEvent e in muninnRoomView.CachedEvents)
        {
            // 每条 cached 单独展示
            UIController.GetComponent<UIController>().AddMessageItem(Encoding.UTF8.GetString(e.Data, 0, e.Data.Length), e.SenderId, MessageType.otherMessage);
        }

        // 初始化房主 id 和 玩家 id
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
    /// 加入房间失败
    /// </summary>
    /// <param name="err"></param>
    public override void OnJoinRoomFailed(MuninnError error)
    {
        UIController.GetComponent<UIController>().OnError.Invoke(MuninnCodeLocalize.GetCodeName(error.Code));
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public override void OnDisconnected()
    {
        UIController.GetComponent<UIController>().LeaveRoomHandler();
    }

    /// <summary>
    /// 玩家进入房间
    /// </summary>
    /// <param name="muninnPlayer"></param>
    public override void OnPlayerEnteredRoom(MuninnPlayer muninnPlayer)
    {
        // 获取玩家的头像、名称
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("{0}{1} 进入房间。", muninnPlayer.Name, muninnPlayer.SenderId), 0, MessageType.systemMessage);
        UpdatePlayerList();
    }

    /// <summary>
    /// 玩家离开房间
    /// </summary>
    /// <param name="muninnPlayer"></param>
    public override void OnPlayerLeftRoom(MuninnPlayer muninnPlayer)
    {
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("{0}{1} 离开房间。", muninnPlayer.Name, muninnPlayer.SenderId), 0, MessageType.systemMessage);
        UpdatePlayerList();
    }

    /// <summary>
    /// master client 变更
    /// </summary>
    /// <param name="masterClientId"></param>
    public override void OnMasterClientChanged(uint masterClientId)
    {
        base.OnMasterClientChanged(masterClientId);
        this.masterClientId = masterClientId;
        this.UpdatePlayerList();
    }

    /// <summary>
    /// 接收到一般事件、分组消息
    /// </summary>
    /// <param name="e"></param>
    public override void OnEvent(MuninnEvent e)
    {
        UIController.GetComponent<UIController>().AddMessageItem(Encoding.UTF8.GetString(e.Data), e.SenderId,
        playerSenderId == e.SenderId ? MessageType.myMessage : MessageType.otherMessage);
    }

    /// <summary>
    /// 订阅分组成功
    /// </summary>
    /// <param name="rsp"></param>
    public override void OnSubscribeGroups(MuninnSubscribeGroupResponse rsp)
    {
        base.OnSubscribeGroups(rsp);
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("加入分组成功。"), 0, MessageType.systemMessage);
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("Tips: 点击「发送分组消息」跟群组内玩家通信吧！"), 0, MessageType.systemMessage);
        UIController.GetComponent<UIController>().ShowUnsubGroupButton();
    }

    /// <summary>
    /// 订阅分组失败
    /// </summary>
    /// <param name="error"></param>
    public override void OnSubscribeGroupsFailed(MuninnError error)
    {
        Debug.LogWarningFormat("[Muninn]: OnSubscribeGroupsFailed() was called by Muninn");
        UIController.GetComponent<UIController>().AddMessageItem(MuninnCodeLocalize.GetCodeName(error.Code), 0, MessageType.systemMessage);
    }

    /// <summary>
    /// 取消订阅分组成功
    /// </summary>
    /// <param name="rsp"></param>
    public override void OnUnsubscribeGroups(MuninnUnsubscribeGroupsResponse rsp)
    {
        base.OnUnsubscribeGroups(rsp);
        UIController.GetComponent<UIController>().AddMessageItem(String.Format("退出分组成功。"), 0, MessageType.systemMessage);
        UIController.GetComponent<UIController>().ShowSubscribeGroupButton();

    }

    /// <summary>
    /// 取消订阅分组失败
    /// </summary>
    /// <param name="error"></param>
    public override void OnUnsubscribeGroupsFailed(MuninnError error)
    {
        Debug.LogWarningFormat("[Muninn]: OnUnsubscribeGroupsFailed() was called by Muninn");
        UIController.GetComponent<UIController>().AddMessageItem(MuninnCodeLocalize.GetCodeName(error.Code), 0, MessageType.systemMessage);
    }

    /// <summary>
    /// 接收到 server call
    /// </summary>
    /// <param name="e"></param>
    public override void OnServerCall(MuninnEvent e)
    {
        UIController.GetComponent<UIController>().AddMessageItem(Encoding.UTF8.GetString(e.Data), e.SenderId, playerSenderId == e.SenderId ? MessageType.myMessage : MessageType.otherMessage);
    }


    /// <summary>
    /// 成功踢走玩家
    /// </summary>
    /// <param name="rsp"></param>
    public override void OnKickedPlayer(MuninnKickPlayerResponse rsp)
    {
        Debug.Log("kicked success");
    }

    /// <summary>
    /// 踢走玩家失败
    /// </summary>
    /// <param name="error"></param>
    public override void OnKickPlayerFailed(MuninnError error)
    {
        Debug.Log("kicked fail");
        UIController.GetComponent<UIController>().AddMessageItem(MuninnCodeLocalize.GetCodeName(error.Code), 0, MessageType.systemMessage);
    }

    // 2. 一些工具函数：

    /// <summary>
    /// 从 web 获取 app 信息、网络协议、客户端 id 等信息
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
    /// 通过客户端 id 随机获取一个玩家名称
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetPlayerNameByClientid(int index)
    {
        string[] list = playerNames.Split(',');
        return list[index % list.Length];
    }

    /// <summary>
    /// 根据 senderId 查询玩家
    /// </summary>
    /// <param name="senderId"></param>
    /// <returns></returns>
    public MuninnPlayer GetPlayerBySenderId(uint senderId)
    {
        MuninnRoomView view = GetMuninnRoomView();
        return view.GetPlayerBySenderId(senderId);
    }

    /// <summary>
    /// 更新玩家列表
    /// </summary>
    private void UpdatePlayerList()
    {
        MuninnRoomView view = GetMuninnRoomView();
        if (view == null) return;

        UIController.GetComponent<UIController>().ReplacePlayerList(view.Players, view.SenderId, view.MasterClientId);

    }


}
*/