using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [Header("�����ֱ��ʿ��")]
    public float designWidth = 1920f;
    public float designHeight = 1080f;

    [Header("�����������С")]
    public float designOrthographicSize = 5f;

    private Camera m_camera;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        AdjustCameraSize();
    }

    private void AdjustCameraSize()
    {
        float designAspect = designWidth / designHeight;
        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect < designAspect)
        {
            float scale = designAspect / screenAspect;
            m_camera.orthographicSize = designOrthographicSize * scale;
        }
        else
        {
            m_camera.orthographicSize = designOrthographicSize;
        }
    }
}
