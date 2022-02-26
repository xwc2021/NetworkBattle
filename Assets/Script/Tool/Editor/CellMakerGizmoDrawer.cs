﻿using UnityEngine;
using UnityEditor;
using DiyAStar;
public class CellMakerGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoFor(CellMaker target, GizmoType gizmoType)
    {
        //測試2線段交叉
        Gizmos.DrawLine(target.P0.position, target.P1.position);
        Gizmos.DrawLine(target.P2.position, target.P3.position);

        var colliderRects = target.colliderRects;
        if (colliderRects != null)
        {
            foreach (var rect in colliderRects)
                DrawRect(rect, colliderColor);
        }

        //outer
        var OuterQuadRect = target.GetOuterQuadRect();
        foreach (var rect in OuterQuadRect)
            DrawRect(rect, quadTreeOuter);

        //innner
        var InnerQuadRect = target.GetInnerQuadRect();
        foreach (var rect in InnerQuadRect)
            DrawRect(rect, quadTreeInner);


        var nodes = target.GetRawPath();
        DrawPathList(nodes, target.nodeSize);

        var points = target.GetModifyPath();
        DrawPathList(points, target.modifyNodeSize);

        if (target.showNodeLink)
        {
            var HorizontalLinks = QuadTreeConnectedNode.GetHorizontalLinks();

            foreach (var link in HorizontalLinks)
                DrawQuadTreeLink(link.from, link.to, quadTreeHorizontalLink);

            var VerticaLinks = QuadTreeConnectedNode.GetVerticaLinks();

            foreach (var link in VerticaLinks)
                DrawQuadTreeLink(link.from, link.to, quadTreeVerticalLink);
        }
    }

    static Color nodeColor = Color.green;
    static Color modifyNodeColor = Color.blue;
    static Color colliderColor = Color.green;
    static Color quadTreeOuter = Color.yellow;
    static Color quadTreeInner = Color.red;
    static Color quadTreeHorizontalLink = new Color(0, 0.99609375f, 0.99609375f);
    static Color quadTreeVerticalLink = new Color(0.9375f, 0.5f, 0.5f);
    static void DrawRect(IRect rect, Color color)
    {
        var point = rect.GetRectInfo();
        if (point == null)
            return;

        Gizmos.color = color;
        for (var i = 0; i <= 2; ++i)
            Gizmos.DrawLine(point[i], point[i + 1]);
        Gizmos.DrawLine(point[3], point[0]);
    }

    public static void DrawQuadTreeLink(QuadTreeConnectedNode from, QuadTreeConnectedNode to, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(from.GetCenter(), to.GetCenter());
    }

    public static void DrawPathList(IGraphNode[] nodes, float nodeSize)
    {
        Gizmos.color = nodeColor;
        foreach (var node in nodes)
        {
            Gizmos.DrawSphere(node.GetPosition(), nodeSize);
        }
    }

    public static void DrawPathList(Vector3[] points, float nodeSize)
    {
        Gizmos.color = modifyNodeColor;
        foreach (var p in points)
            Gizmos.DrawSphere(p, nodeSize);
    }
}