using UnityEngine;

namespace YSH.Framework
{
    public class RedPointUI : MonoBehaviour
    {
        //·��
        public string Path;

        //���
        private GameObject redPointObject;

        public void SetRedPointPath(string path)
        {
            Path = path;
            AddListener();
        }

        private void AddListener()
        {
            //��Ӽ���ǰ�ȸ���һ��
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
                //������ʾ���
                if (redPointObject == null)
                {
                    ShowRedPointObject();
                }
            }
            else
            {
                //���غ��
                if (redPointObject != null)
                {
                    HideRedPointObject();
                }
            }
        }

        //��ʾ���
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

        //���غ��
        private void HideRedPointObject()
        {
            PoolMgr.Instance.Release(redPointObject);
            redPointObject = null;
        }
    }
}

