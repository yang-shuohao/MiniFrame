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
            gmCommands.Add("AddGold", new GMCommand("AddGold", "����ָ�������Ľ��", args =>
            {
                if (args.Length > 0) Debug.Log($"���ӽ�� {args[0]}");

                GUIMgr.Instance.ShowPopup("���ӽ��", 2f);
            }));

            gmCommands.Add("SetLevel", new GMCommand("SetLevel", "���ý�ɫ�ȼ� (�ȼ� ְҵ)", args =>
            {
                if (args.Length >= 2)
                    Debug.Log($"���õȼ�: {args[0]}��ְҵ: {args[1]}");
            }));

            gmCommands.Add("UnlockAll", new GMCommand("UnlockAll", "�������й���", args =>
            {
                Debug.Log("������������");
            }));

            gmCommands.Add("GodMode", new GMCommand("GodMode", "�����޵�ģʽ", args =>
            {
                Debug.Log("�޵�ģʽ����");
            }));

            gmCommands.Add("AddDiamond", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond2", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond3", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond4", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond5", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond6", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond7", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond8", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));

            gmCommands.Add("AddDiamond9", new GMCommand("AddDiamond", "������ʯ", args =>
            {
                if (args.Length > 0) Debug.Log($"������ʯ {args[0]}");
            }));
        }
    }
}

