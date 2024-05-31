using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public RectTransform testRectTransform;

    private void Start()
    {
        Canvas.willRenderCanvases += MyRebuild;
    }
    private void OnDestroy()
    {
        Canvas.willRenderCanvases -= MyRebuild;
    }
    // Do the same thing as Graphic.Rebuild()
    void MyRebuild()
    {
        var canvasRenderer = gameObject.GetComponent<CanvasRenderer>();
        if (canvasRenderer == null)
        {
            canvasRenderer = gameObject.AddComponent<CanvasRenderer>();
        }
        // List of vertices to be drawn
        List<UIVertex> verts = new List<UIVertex>();
        verts.Add(new UIVertex
        {
            position = new Vector3(-50, -50, 0),
            uv0 = new Vector2(0, 0),
            color = new Color32(255, 0, 0, 255),
        });
        verts.Add(new UIVertex
        {
            position = new Vector3(-50, 50, 0),
            uv0 = new Vector2(0, 1),
            color = new Color32(0, 255, 0, 255),
        });
        verts.Add(new UIVertex
        {
            position = new Vector3(50, 50, 0),
            uv0 = new Vector2(1, 1),
            color = new Color32(0, 0, 255, 255),
        });
        // List of indices to be drawn
        List<int> indices = new List<int>();
        indices.Add(0);
        indices.Add(1);
        indices.Add(2);
        // List received from AddUIVertexStream
        List<Vector3> positions = new List<Vector3>();
        List<Color32> colors = new List<Color32>();
        List<Vector4> uv0s = new List<Vector4>();
        List<Vector4> uv1s = new List<Vector4>();
        List<Vector4> uv2s = new List<Vector4>();
        List<Vector4> uv3s = new List<Vector4>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector4> tangents = new List<Vector4>();
        // Split into a list of verts
        CanvasRenderer.AddUIVertexStream(verts, positions, colors, uv0s, uv1s, uv2s, uv3s, normals, tangents);
        // Mesh to be created from the list received from AddUIVertexStream
        Mesh mesh = new Mesh();
        mesh.SetVertices(positions);
        mesh.SetColors(colors);
        mesh.SetUVs(0, uv0s);
        mesh.SetUVs(1, uv1s);
        mesh.SetUVs(2, uv2s);
        mesh.SetUVs(3, uv3s);
        mesh.SetNormals(normals);
        mesh.SetTangents(tangents);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        // Drawing can be achieved by passing the mesh to be drawn to the CanvasRenderer
        canvasRenderer.SetMesh(mesh);
        canvasRenderer.materialCount = 1;
        canvasRenderer.SetMaterial(Canvas.GetDefaultCanvasMaterial(), 0);
        canvasRenderer.SetColor(Color.white);
    }



}
