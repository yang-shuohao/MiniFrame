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
            gmCommands.Add("AddGold", new GMCommand("AddGold", "增加指定数量的金币", args =>
            {
                if (args.Length > 0) Debug.Log($"增加金币 {args[0]}");

                GUIMgr.Instance.ShowPopup("增加金币", 2f);
            }));

            gmCommands.Add("SetLevel", new GMCommand("SetLevel", "设置角色等级 (等级 职业)", args =>
            {
                if (args.Length >= 2)
                    Debug.Log($"设置等级: {args[0]}，职业: {args[1]}");
            }));

            gmCommands.Add("UnlockAll", new GMCommand("UnlockAll", "解锁所有功能", args =>
            {
                Debug.Log("解锁所有内容");
            }));

            gmCommands.Add("GodMode", new GMCommand("GodMode", "开启无敌模式", args =>
            {
                Debug.Log("无敌模式开启");
            }));

            gmCommands.Add("AddDiamond", new GMCommand("AddDiamond", "增加钻石", args =>
            {
                if (args.Length > 0) Debug.Log($"增加钻石 {args[0]}");
            }));
        }
    }
}

