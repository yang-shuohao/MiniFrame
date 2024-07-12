using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!DbManager.Connect("game", "127.0.0.1", 3306, "root", ""))
            {
                return;
            }

            DbManager.CreatePlayer("abc123");
            PlayerData pd = DbManager.GetPlayerData("abc123");
            pd.coin = 256;
            DbManager.UpdatePlayerData("abc123", pd);

            NetManager.StartLoop(8888);
        }
    }
}
