using UnityEngine;

public class ExcelTest : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.LogWarning(1);
    }

    private void Start()
    {
        BinaryDataMgr.Instance.LoadTable<PlayerInfoContainer, PlayerInfo>();

        PlayerInfoContainer playerInfoContainer = BinaryDataMgr.Instance.GetTable<PlayerInfoContainer>();

        foreach (var item in playerInfoContainer.dataDic.Values)
        {
            Debug.LogWarning(item.id);
            Debug.LogWarning(item.name);
            Debug.LogWarning(item.age);
            Debug.LogWarning(item.attack);
            Debug.LogWarning(item.health);
            Debug.LogWarning(item.speed);
        }
    }
}
