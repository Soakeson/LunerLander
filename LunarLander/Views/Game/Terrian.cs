using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class Terrain 
{
    public VertexPositionColor[] m_vertInfo;
    public int[] m_vertIndex;
    public LinkedList<(Point, bool)> m_skyLine;
    private int m_width;
    private int m_height;

    public Terrain(LinkedList<(Point, bool)> skyLine, int width, int height)
    {
        m_skyLine = skyLine;
        m_vertInfo = new VertexPositionColor[m_skyLine.Count];
        m_vertIndex = new int[m_skyLine.Count];
        m_width = width;
        m_height = height;

        //generate vertexpostions from skyline
        int i = 0;
        for (LinkedListNode<(Point, bool)> p = m_skyLine.First; p != null; p = p.Next)
        {
            // m_vertInfo[i].Position = new Vector3(p.Value.X, p.Value.Y, 0);
            // m_vertInfo[i].Color = Color.White;
            // m_vertIndex[i] = i;
            //
            // m_vertInfo[i+1].Position = new Vector3(p.Value.X, m_height, 0);
            // m_vertInfo[i+1].Color = Color.White;
            // m_vertIndex[i+1] = i+1;
            //
            //
            // if (p.Next != null)
            // {
            //     m_vertInfo[i+2].Position = new Vector3(p.Next.Value.X, p.Next.Value.Y, 0);
            //     m_vertInfo[i+2].Color = Color.White;
            //     m_vertIndex[i+2] = i+2;
            //
            //     m_vertInfo[i+3].Position = new Vector3(p.Next.Value.X, m_height, 0);
            //     m_vertInfo[i+3].Color = Color.White;
            //     m_vertIndex[i+3] = i+3;
            // }
            //
            // i += 4;
            m_vertInfo[i].Position = new Vector3(p.Value.Item1.X, p.Value.Item1.Y, 0);
            m_vertInfo[i].Color = Color.White;
            m_vertIndex[i] = i;
            i++;
        }
    }

    public void Render()
    {
        // render the terrain
    }
}
