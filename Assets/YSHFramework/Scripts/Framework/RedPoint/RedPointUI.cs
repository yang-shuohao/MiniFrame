using UnityEngine;

namespace YSH.Framework
{
    public class RedPointUI : MonoBehaviour
    {
        //路径
        public string Path;

        //红点
        private GameObject redPointObject;

        public void SetRedPointPath(string path)
        {
            Path = path;
            AddListener();
        }

        private void AddListener()
        {
            //添加监听前先更新一下
            RedPointCallback(RedPointMgr.Instance.GetValue(Path));
            RedPointMgr.Instance.AddListener(Path, RedPointCallback);
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                AddListener();
            }
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                RedPointMgr.Instance.RemoveListener(Path, RedPointCallback);
            }
        }

        private void RedPointCallback(int value)
        {
            if (value > 0)
            {
                //创建显示红点
                if (redPointObject == null)
                {
                    ShowRedPointObject();
                }
            }
            else
            {
                //隐藏红点
                if (redPointObject != null)
                {
                    HideRedPointObject();
                }
            }
        }

        //显示红点
        private void ShowRedPointObject()
        {
            PoolMgr.Instance.Get("RedPoint", res =>
            {
                if (redPointObject)
                {
                    HideRedPointObject();
                }

                redPointObject = res;

                redPointObject.transform.SetParent(this.transform);

                RectTransform redPointRectTransform = redPointObject.transform as RectTransform;
                redPointRectTransform.anchoredPosition3D = Vector3.zero;
                redPointRectTransform.localScale = Vector3.one;
            });
        }

        //隐藏红点
        private void HideRedPointObject()
        {
            PoolMgr.Instance.Release(redPointObject);
            redPointObject = null;
        }
    }
}

