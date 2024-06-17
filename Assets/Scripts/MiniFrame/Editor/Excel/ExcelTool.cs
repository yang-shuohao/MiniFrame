using Excel;
using System;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExcelTool
{
    /// <summary>
    /// Excel文件存放的路径
    /// </summary>
    public static string EXCEL_PATH = Application.dataPath + "/ArtRes/Excel/";

    /// <summary>
    /// 数据结构类脚本存储位置路径
    /// </summary>
    public static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";

    /// <summary>
    /// 容器类脚本存储位置路径
    /// </summary>
    public static string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/ExcelData/Container/";

    /// <summary>
    /// 真正内容开始的行号
    /// </summary>
    public static int BEGIN_INDEX = 4;

    [MenuItem("Tools/ExcelTool/GenerateExcelInfo")]
    private static void GenerateExcelInfo()
    {
        //记在指定路径中的所有Excel文件 用于生成对应的3个文件
        DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
        //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
        FileInfo[] files = dInfo.GetFiles();
        //数据表容器
        DataTableCollection tableConllection;
        for (int i = 0; i < files.Length; i++)
        {
            //如果不是excel文件就不要处理了
            if (files[i].Extension == ".xlsx" || files[i].Extension == ".xls")
            {
                //打开一个Excel文件得到其中的所有表的数据
                using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                    tableConllection = excelReader.AsDataSet().Tables;
                    fs.Close();
                }

                //遍历文件中的所有表的信息
                foreach (DataTable table in tableConllection)
                {
                    //生成数据结构类
                    GenerateExcelDataClass(table);
                    //生成容器类
                    GenerateExcelContainer(table);
                    //生成2进制数据
                    GenerateExcelBinary(table);
                }
            }
        }
    }

    /// <summary>
    /// 生成Excel表对应的数据结构类
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelDataClass(DataTable table)
    {
        //字段名行
        DataRow rowName = GetVariableNameRow(table);
        //字段类型行
        DataRow rowType = GetVariableTypeRow(table);

        //判断路径是否存在 没有的话 就创建文件夹
        if (!Directory.Exists(DATA_CLASS_PATH))
        {
            Directory.CreateDirectory(DATA_CLASS_PATH);
        }

        //如果我们要生成对应的数据结构类脚本 其实就是通过代码进行字符串拼接 然后存进文件就行了
        string str = "public class " + table.TableName + "\n{\n";

        //变量进行字符串拼接
        for (int i = 0; i < table.Columns.Count; i++)
        {
            str += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
        }

        str += "}";

        //把拼接好的字符串存到指定文件中去
        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", str);

        //刷新Project窗口
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成Excel表对应的数据容器类
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelContainer(DataTable table)
    {
        //得到主键索引
        int keyIndex = GetKeyIndex(table);
        //得到字段类型行
        DataRow rowType = GetVariableTypeRow(table);
        //没有路径创建路径
        if (!Directory.Exists(DATA_CONTAINER_PATH))
        {
            Directory.CreateDirectory(DATA_CONTAINER_PATH);
        }
            
        string str = "using System.Collections.Generic;\n";

        str += "public class " + table.TableName + "Container" + "\n{\n";

        str += "    public Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">";
        str += " dataDic = new " + "Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">();\n";

        str += "}";

        File.WriteAllText(DATA_CONTAINER_PATH + table.TableName + "Container.cs", str);

        //刷新Project窗口
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成excel二进制数据
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelBinary(DataTable table)
    {
        // 确保路径存在
        if (!Directory.Exists(BinaryDataMgr.Instance.DATA_BINARY_PATH))
        {
            Directory.CreateDirectory(BinaryDataMgr.Instance.DATA_BINARY_PATH);
        }

        string filePath = Path.Combine(BinaryDataMgr.Instance.DATA_BINARY_PATH, table.TableName);

        // 创建一个二进制文件进行写入
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            try
            {
                // 存储行数（排除前4行）
                int dataRowCount = table.Rows.Count - 4;
                if (dataRowCount < 0)
                {
                    throw new InvalidOperationException("数据行数不足以生成二进制文件。");
                }
                fs.Write(BitConverter.GetBytes(dataRowCount), 0, 4);

                // 存储主键的变量名
                string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
                byte[] keyNameBytes = Encoding.UTF8.GetBytes(keyName);
                fs.Write(BitConverter.GetBytes(keyNameBytes.Length), 0, 4);
                fs.Write(keyNameBytes, 0, keyNameBytes.Length);

                // 得到类型行，根据类型写入数据
                DataRow rowType = GetVariableTypeRow(table);
                for (int i = BEGIN_INDEX; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        string cellValue = row[j].ToString();
                        switch (rowType[j].ToString())
                        {
                            case "int":
                                if (int.TryParse(cellValue, out int intValue))
                                {
                                    fs.Write(BitConverter.GetBytes(intValue), 0, 4);
                                }
                                break;
                            case "float":
                                if (float.TryParse(cellValue, out float floatValue))
                                {
                                    fs.Write(BitConverter.GetBytes(floatValue), 0, 4);
                                }
                                break;
                            case "bool":
                                if (bool.TryParse(cellValue, out bool boolValue))
                                {
                                    fs.Write(BitConverter.GetBytes(boolValue), 0, 1);
                                }
                                break;
                            case "string":
                                byte[] stringBytes = Encoding.UTF8.GetBytes(cellValue);
                                fs.Write(BitConverter.GetBytes(stringBytes.Length), 0, 4);
                                fs.Write(stringBytes, 0, stringBytes.Length);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"生成二进制文件时发生错误：{ex.Message}");
            }
        }

        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 获取变量名所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[0];
    }

    /// <summary>
    /// 获取变量类型所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[1];
    }

    
    /// <summary>
    /// 获取主键索引
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static int GetKeyIndex(DataTable table)
    {
        DataRow row = table.Rows[2];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
            {
                return i;
            }
        }
        return 0;
    }
}
