using LitJson;
using System.IO;
using UnityEngine;

namespace YSH.Framework
{
    /// <summary>
    /// Json数据管理器
    /// </summary>
    public class JsonDataMgr : Singleton<JsonDataMgr>
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public void SaveData(object data, string fileName)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            string jsonStr = JsonMapper.ToJson(data);

            File.WriteAllText(path, jsonStr);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T LoadData<T>(string fileName) where T : new()
        {
            string path = Application.persistentDataPath + "/" + fileName;

            if (File.Exists(path))
            {
                string jsonStr = File.ReadAllText(path);

                T data = default(T);

                data = JsonMapper.ToObject<T>(jsonStr);

                return data;
            }
            else
            {
                return new T();
            }
        }
    }
}
