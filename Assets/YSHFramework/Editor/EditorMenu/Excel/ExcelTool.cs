using Excel;
using System;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace YSH.Framework.Editor
{
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

        [MenuItem("YSHFramework/ExcelTool/Generate Excel Info", priority = 0)]
        private static void GenerateExcelInfo()
        {
            // 获取所有Excel文件并逐一处理
            DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
            FileInfo[] files = dInfo.GetFiles();

            foreach (var file in files)
            {
                // 处理Excel文件
                if (file.Extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                    file.Extension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    ProcessExcelFile(file);
                }
            }
        }

        private static void ProcessExcelFile(FileInfo file)
        {
            DataTableCollection tableCollection;

            using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tableCollection = excelReader.AsDataSet().Tables;
            }

            foreach (DataTable table in tableCollection)
            {
                // 检查Excel数据中的空值
                if (CheckForEmptyCells(table))
                {
                    Debug.LogError($"【错误】表格 {table.TableName} 包含空数据，请检查！");
                    continue;
                }

                // 生成数据结构类、容器类及二进制数据
                GenerateExcelDataClass(table);
                GenerateExcelContainer(table);
                GenerateExcelBinary(table);
            }
        }

        /// <summary>
        /// 检查Excel表中的空数据，并输出错误的行和列
        /// </summary>
        /// <param name="table"></param>
        /// <returns>如果表格中存在空数据，则返回true，否则返回false</returns>
        private static bool CheckForEmptyCells(DataTable table)
        {
            bool hasEmptyCells = false;

            for (int i = BEGIN_INDEX; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (row[j] == DBNull.Value || string.IsNullOrWhiteSpace(row[j]?.ToString()))
                    {
                        Debug.LogError($"【错误】表格 {table.TableName} 的数据存在空值，位置：行 {i + 1} 列 {j + 1}");
                        hasEmptyCells = true;
                    }
                }
            }

            return hasEmptyCells;
        }

        /// <summary>
        /// 生成Excel表对应的数据结构类
        /// </summary>
        /// <param name="table"></param>
        private static void GenerateExcelDataClass(DataTable table)
        {
            var rowName = GetVariableNameRow(table);
            var rowType = GetVariableTypeRow(table);

            EnsureDirectoryExists(DATA_CLASS_PATH);

            StringBuilder classStr = new StringBuilder();
            classStr.AppendLine($"public class {table.TableName}");
            classStr.AppendLine("{");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                classStr.AppendLine($"    public {rowType[i]} {rowName[i]};");
            }

            classStr.AppendLine("}");

            WriteToFile(DATA_CLASS_PATH + table.TableName + ".cs", classStr.ToString());
        }

        /// <summary>
        /// 生成Excel表对应的数据容器类
        /// </summary>
        /// <param name="table"></param>
        private static void GenerateExcelContainer(DataTable table)
        {
            int keyIndex = GetKeyIndex(table);
            var rowType = GetVariableTypeRow(table);

            EnsureDirectoryExists(DATA_CONTAINER_PATH);

            StringBuilder containerStr = new StringBuilder();
            containerStr.AppendLine("using System.Collections.Generic;");
            containerStr.AppendLine($"public class {table.TableName}Container");
            containerStr.AppendLine("{");
            containerStr.AppendLine($"    public Dictionary<{rowType[keyIndex]}, {table.TableName}> dataDic = new Dictionary<{rowType[keyIndex]}, {table.TableName}>();");
            containerStr.AppendLine("}");

            WriteToFile(DATA_CONTAINER_PATH + table.TableName + "Container.cs", containerStr.ToString());
        }

        /// <summary>
        /// 生成excel二进制数据
        /// </summary>
        /// <param name="table"></param>
        private static void GenerateExcelBinary(DataTable table)
        {
            // 确保路径存在
            EnsureDirectoryExists(BinaryDataMgr.Instance.DATA_BINARY_PATH);

            string filePath = Path.Combine(BinaryDataMgr.Instance.DATA_BINARY_PATH, table.TableName);

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                try
                {
                    int dataRowCount = table.Rows.Count - BEGIN_INDEX;
                    if (dataRowCount < 0)
                    {
                        throw new InvalidOperationException("数据行数不足以生成二进制文件。");
                    }

                    fs.Write(BitConverter.GetBytes(dataRowCount), 0, 4);

                    string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
                    byte[] keyNameBytes = Encoding.UTF8.GetBytes(keyName);
                    fs.Write(BitConverter.GetBytes(keyNameBytes.Length), 0, 4);
                    fs.Write(keyNameBytes, 0, keyNameBytes.Length);

                    // TODO: 写入表格数据
                }
                catch (Exception ex)
                {
                    Debug.LogError($"生成Excel二进制文件时发生错误: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 确保指定路径存在
        /// </summary>
        /// <param name="path"></param>
        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 将字符串写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        private static void WriteToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
            AssetDatabase.Refresh();
        }

        // 以下为辅助方法：获取变量名行、类型行及主键索引
        private static DataRow GetVariableNameRow(DataTable table) => table.Rows[0];
        private static DataRow GetVariableTypeRow(DataTable table) => table.Rows[1];
        private static int GetKeyIndex(DataTable table) => Array.IndexOf(table.Rows[0].ItemArray, "ID");  // 假设主键列是 "ID"
    }
}
