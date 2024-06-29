using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��Sprite��Custom Physics Shaper����ʽִ�й���Ͷ���жϵ�ͼ��
[RequireComponent(typeof(PolygonCollider2D))]
public class ShapeRaycastImage : Image
{
    private PolygonCollider2D collider2d;

    protected override void OnEnable()
    {
        base.OnEnable();
        AdjustCollider();
        RegisterDirtyVerticesCallback(AdjustCollider);
    }

    protected override void OnDisable()
    {
        UnregisterDirtyVerticesCallback(AdjustCollider);
        base.OnDisable();
    }

    public void AdjustCollider()
    {
        if (overrideSprite == null)
        {
            return;
        }

        // ��Sprite���Զ���������״������PolygonCollider2D
        collider2d = GetComponent<PolygonCollider2D>();
        if (collider2d == null)
        {
            collider2d = gameObject.AddComponent<PolygonCollider2D>();
        }

        collider2d.isTrigger = true;
        collider2d.pathCount = overrideSprite.GetPhysicsShapeCount();
        for (int i = 0; i < collider2d.pathCount; i++)
        {
            List<Vector2> physicsShape = new List<Vector2>();
            int pointCount = overrideSprite.GetPhysicsShape(i, physicsShape);
            Vector2[] points = new Vector2[pointCount];
            for (int j = 0; j < points.Length; j++)
            {
                float x = (physicsShape[j].x / overrideSprite.rect.width * overrideSprite.pixelsPerUnit + 0.5f - rectTransform.pivot.x) * rectTransform.rect.width;
                float y = (physicsShape[j].y / overrideSprite.rect.height * overrideSprite.pixelsPerUnit + 0.5f - rectTransform.pivot.y) * rectTransform.rect.height;
                points[j] = new Vector2(x, y);
            }
            collider2d.SetPath(i, points);
        }
    }

    // ���RectTransform�����PolygonCollider2D�ص�����true��
    public bool IsRectTransformPointOverLapped(Vector2 local)
    {
        Vector2 point = new Vector2(transform.position.x + local.x * rectTransform.lossyScale.x, transform.position.y + local.y * rectTransform.lossyScale.y);
        return collider2d.OverlapPoint(point);
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (overrideSprite == null)
        {
            return true;
        }

        Vector2 local;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local))
        {
            return false;
        }

        if (IsRectTransformPointOverLapped(local))
        {
            return true;
        }

        return false;
    }
}
