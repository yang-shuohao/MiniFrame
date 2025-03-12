using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace YSH.Framework
{

    /// <summary>
    /// 2进制数据管理器
    /// </summary>
    public class BinaryDataMgr : Singleton<BinaryDataMgr>
    {
        /// <summary>
        /// 2进制数据存储位置路径
        /// </summary>
        public string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Binary/";

        /// <summary>
        /// 数据存储的位置
        /// </summary>
        private string SAVE_PATH = Application.persistentDataPath + "/Data/";

        /// <summary>
        /// 用于存储所有Excel表数据的容器
        /// </summary>
        private Dictionary<string, object> tableDic = new Dictionary<string, object>();



        public BinaryDataMgr()
        {

        }


        /// <summary>
        /// 加载Excel表的2进制数据到内存中 
        /// </summary>
        /// <typeparam name="T">容器类名</typeparam>
        /// <typeparam name="K">数据结构类类名</typeparam>
        public void LoadTable<T, K>()
        {
            //读取 excel表对应的2进制文件 来进行解析
            using (FileStream fs = File.Open(DATA_BINARY_PATH + typeof(K).Name, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();

                //用于记录当前读取了多少字节了
                int index = 0;

                //读取多少行数据
                int count = BitConverter.ToInt32(bytes, index);
                index += 4;

                //读取主键的名字
                int keyNameLength = BitConverter.ToInt32(bytes, index);
                index += 4;
                string keyName = Encoding.UTF8.GetString(bytes, index, keyNameLength);
                index += keyNameLength;

                //创建容器类对象
                Type contaninerType = typeof(T);
                object contaninerObj = Activator.CreateInstance(contaninerType);
                //得到数据结构类的Type
                Type classType = typeof(K);
                //通过反射 得到数据结构类 所有字段的信息
                FieldInfo[] infos = classType.GetFields();

                //读取每一行的信息
                for (int i = 0; i < count; i++)
                {
                    //实例化一个数据结构类 对象
                    object dataObj = Activator.CreateInstance(classType);
                    foreach (FieldInfo info in infos)
                    {
                        if (info.FieldType == typeof(int))
                        {
                            //相当于就是把2进制数据转为int 然后赋值给了对应的字段
                            info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                            index += 4;
                        }
                        else if (info.FieldType == typeof(float))
                        {
                            info.SetValue(dataObj, BitConverter.ToSingle(bytes, index));
                            index += 4;
                        }
                        else if (info.FieldType == typeof(bool))
                        {
                            info.SetValue(dataObj, BitConverter.ToBoolean(bytes, index));
                            index += 1;
                        }
                        else if (info.FieldType == typeof(string))
                        {
                            //读取字符串字节数组的长度
                            int length = BitConverter.ToInt32(bytes, index);
                            index += 4;
                            info.SetValue(dataObj, Encoding.UTF8.GetString(bytes, index, length));
                            index += length;
                        }
                    }

                    //读取完一行的数据了 应该把这个数据添加到容器对象中
                    //得到容器对象中的 字典对象
                    object dicObject = contaninerType.GetField("dataDic").GetValue(contaninerObj);
                    //通过字典对象得到其中的 Add方法
                    MethodInfo mInfo = dicObject.GetType().GetMethod("Add");
                    //得到数据结构类对象中 指定主键字段的值
                    object keyValue = classType.GetField(keyName).GetValue(dataObj);
                    mInfo.Invoke(dicObject, new object[] { keyValue, dataObj });
                }

                //把读取完的表记录下来
                tableDic.Add(typeof(T).Name, contaninerObj);

                fs.Close();
            }
        }

        /// <summary>
        /// 得到一张表的信息
        /// </summary>
        /// <typeparam name="T">容器类名</typeparam>
        /// <returns></returns>
        public T GetTable<T>() where T : class
        {
            string tableName = typeof(T).Name;
            if (tableDic.ContainsKey(tableName))
            {
                return tableDic[tableName] as T;
            }
            return null;
        }

        /// <summary>
        /// 存储类对象数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public void Save(object obj, string fileName)
        {
            //先判断路径文件夹有没有
            if (!Directory.Exists(SAVE_PATH))
            {
                Directory.CreateDirectory(SAVE_PATH);
            }

            using (FileStream fs = new FileStream(SAVE_PATH + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }

        /// <summary>
        /// 读取2进制数据转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T Load<T>(string fileName) where T : class
        {
            //如果不存在这个文件 就直接返回泛型对象的默认值
            if (!File.Exists(SAVE_PATH + fileName))
            {
                return default(T);
            }

            T obj;
            using (FileStream fs = File.Open(SAVE_PATH + fileName, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs) as T;
                fs.Close();
            }

            return obj;
        }
    }
}
