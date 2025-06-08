using UnityEditor;
using UnityEngine;
using System.IO;

namespace YSH.Framework.EditorExtensions
{
    public class AssetBundleBuilder
    {
        private const string assetBundleOutputPath = "AssetBundles";

        [MenuItem("YSHFramework/Build AssetBundles/Build All")]
        public static void BuildAllAssetBundles()
        {
            // 创建输出目录
            if (!Directory.Exists(assetBundleOutputPath))
                Directory.CreateDirectory(assetBundleOutputPath);

            // 自动设置 AssetBundle 名称
            SetAllAssetBundleNames();

            // 构建 AssetBundles
            BuildPipeline.BuildAssetBundles(
                assetBundleOutputPath,
                BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget
            );

            Debug.Log("AssetBundles 打包完成！");
        }

        /// <summary>
        /// 自动为 Bundles 目录下的资源设置 AssetBundle 名称
        /// </summary>
        private static void SetAllAssetBundleNames()
        {
            string assetsPath = "Assets/Bundles";
            DirectoryInfo dir = new DirectoryInfo(assetsPath);
            FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                if (file.Extension == ".meta") continue;

                string assetPath = file.FullName.Replace('\\', '/');
                assetPath = assetPath.Substring(assetPath.IndexOf("Assets/"));

                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer != null)
                {
                    string bundleName = GetAssetBundleName(assetPath);
                    importer.assetBundleName = bundleName;
                }
            }

            Debug.Log("AssetBundle 名称已设置完毕。");
        }

        /// <summary>
        /// 生成 AB 名称，例如 Assets/Bundles/Textures/logo.png → textures/logo
        /// </summary>
        private static string GetAssetBundleName(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            string dir = Path.GetDirectoryName(path).Replace("Assets/Bundles/", "").Replace("\\", "/").ToLower();
            return string.IsNullOrEmpty(dir) ? name.ToLower() : $"{dir}/{name.ToLower()}";
        }
    }
}
