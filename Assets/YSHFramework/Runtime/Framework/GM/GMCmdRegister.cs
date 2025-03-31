using UnityEngine;

namespace YSH.Framework
{
    //��Ϸ��GM��������ע�ᣬע���ƶ��˵�Ĭ�ϲ�֧������
    public partial class GMCmd
    {
        public GMCmd()
        {
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            gmCommands.Add("AddGold", new GMCommand("AddGold", "AddGold", args =>
            {
                if (args.Length > 0) Debug.Log($"AddGold {args[0]}");

                string content = "AddGold";
                for (int i = 0; i < args.Length; i++)
                {
                    content += " " + args[i];
                }
                DebugUIMgr.Instance.ShowPopupPanel(content);
            }));

            gmCommands.Add("SetLevel", new GMCommand("SetLevel", "SetLevel", args =>
            {
                if (args.Length >= 2)
                    Debug.Log($"���õȼ�: {args[0]}��ְҵ: {args[1]}");
            }));

            gmCommands.Add("UnlockAll", new GMCommand("UnlockAll", "UnlockAll", args =>
            {
                Debug.Log("������������");
            }));

            gmCommands.Add("GodMode", new GMCommand("GodMode", "GodMode", args =>
            {
                Debug.Log("�޵�ģʽ����");
            }));

            gmCommands.Add("AddDiamond", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond2", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond3", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond4", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond5", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond6", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond7", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond8", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond9", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));
        }
    }
}

