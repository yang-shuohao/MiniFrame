using UnityEngine;

namespace YSH.Framework
{
    //游戏中GM命令在这注册，注意移动端等默认不支持中文
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
                    Debug.Log($"设置等级: {args[0]}，职业: {args[1]}");
            }));

            gmCommands.Add("UnlockAll", new GMCommand("UnlockAll", "UnlockAll", args =>
            {
                Debug.Log("解锁所有内容");
            }));

            gmCommands.Add("GodMode", new GMCommand("GodMode", "GodMode", args =>
            {
                Debug.Log("无敌模式开启");
            }));

            gmCommands.Add("AddDiamond", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond2", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond3", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond4", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond5", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond6", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond7", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond8", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));

            gmCommands.Add("AddDiamond9", new GMCommand("AddDiamond", "AddDiamond", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));
        }
    }
}

