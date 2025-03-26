using LitJson;
using UnityEngine;
using YSH.Framework;

public class NetTest : MonoBehaviour
{
    private void Start()
    {
        NewWorkSocketMgr.Instance.Connect("192.168.17.131", 1011);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Send("����˭");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Send("��Ҫȥ��");
        }
    }

    private void Send(string msg)
    {
        using(ExtendedMemoryStream ms = new ExtendedMemoryStream())
        {
            ms.WriteUTF8String(msg);

            NewWorkSocketMgr.Instance.SendMsg(ms.ToArray());
        }
    }

    private void OnDestroy()
    {
        NewWorkSocketMgr.Instance.DisConnect();
    }

    private void TestPost()
    {
        JsonData jsonStr = new JsonData();
        jsonStr["Type"] = 0;
        jsonStr["UserName"] = "ysh";
        jsonStr["Pwd"] = "123456";

        NetWorkHttpMgr.Instance.Post(GameConstants.WebURL, jsonStr.ToJson(), response =>
        {
            if (response.HasError)
            {
                Debug.LogError("����ʧ��: " + response.ErrorMessage);
            }
            else
            {
                if (response.Data != "null")
                {
                    Debug.Log("����ɹ�: " + response.Data);
                }
            }
        });
    }
}
