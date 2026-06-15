
using Common.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YSH.Framework;

public class TestPanel : BaseUI
{
    private TMP_InputField inf;
    private Button btnTest;

    protected override void Awake()
    {
        base.Awake();
        inf = GetControl<TMP_InputField>(nameof(inf));
        btnTest = GetControl<Button>(nameof(btnTest));
    }

    private void Start()
    {
        NetworkSocketMgr.Instance.Init("127.0.0.1", 8000);
        NetworkSocketMgr.Instance.Connect();
        NetworkSocketMgr.Instance.OnConnect += OnConnect;
    }

    private void OnConnect(int result, string reason)
    {
        Debug.Log("Connected to server successfully.");
        //NetMessage netMessage = new NetMessage();
      
        //NetworkSocketMgr.Instance.SendMessage(netMessage);
    }

    private void OnConnectFail()
    {
        Debug.Log("Connected to server OnConnectFail.");
    }

    private void OnConnectClose()
    {
        Debug.Log("Connected to server OnConnectClose.");
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnTest":
                break;
        }
    }
}
