using System.IO;
using UnityEngine;

public class TestAB : MonoBehaviour
{
    void Start()
    {
        var myLoadedAssetBundle
            = AssetBundle.LoadFromFile("Assets/AssetBundles/prefabs/triangle");
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Triangle");
        Instantiate(prefab);
    }
}
