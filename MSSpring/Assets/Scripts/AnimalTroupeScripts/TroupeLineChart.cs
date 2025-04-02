using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroupeLineChart : Graphic
{
    public List<Vector2> points = new List<Vector2>();
    float lineWidth = 5f;
    Color lineColor = Color.yellow;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Count < 2) return;

        var rect = rectTransform.rect;
        Vector2 prev = new Vector2(points[0].x * rect.width, points[0].y * rect.height);

        for (int i = 1; i < points.Count; i++)
        {
            Vector2 current = new Vector2(points[i].x * rect.width, points[i].y * rect.height);
            DrawLine(vh, prev, current, lineColor, lineWidth);
            prev = current;
        }
    }

    void DrawLine(VertexHelper vh, Vector2 p1, Vector2 p2, Color color, float width)
    {
        Vector2 direction = (p2 - p1).normalized;
        Vector2 normal = Vector2.Perpendicular(direction) * width * 0.5f;

        UIVertex[] verts = new UIVertex[4];
        verts[0] = UIVertex.simpleVert; verts[0].color = color; verts[0].position = p1 - normal;
        verts[1] = UIVertex.simpleVert; verts[1].color = color; verts[1].position = p1 + normal;
        verts[2] = UIVertex.simpleVert; verts[2].color = color; verts[2].position = p2 + normal;
        verts[3] = UIVertex.simpleVert; verts[3].color = color; verts[3].position = p2 - normal;

        int startIndex = vh.currentVertCount;
        for (int i = 0; i < 4; i++)
            vh.AddVert(verts[i]);

        vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        vh.AddTriangle(startIndex, startIndex + 2, startIndex + 3);
    }
}

