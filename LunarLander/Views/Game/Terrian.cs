using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class Terrain : GameObject
{
    private BasicEffect m_effect;
    private VertexPositionColor[] m_vertInfo;
    private int[] m_vertIndex;
    private LinkedList<(Point, bool)> m_skyLine;
    private int m_width;
    private int m_height;

    public Terrain(LinkedList<(Point, bool)> skyLine, int width, int height)
    {
        m_skyLine = skyLine;
        m_vertInfo = new VertexPositionColor[m_skyLine.Count * 3];
        m_vertIndex = new int[m_skyLine.Count * 3];
        m_width = width;
        m_height = height;


        //generate vertexpostions from skyline
        int i = 0;
        for (LinkedListNode<(Point, bool)> p = m_skyLine.First; p != null; p = p.Next)
        {

            // Low point
            m_vertInfo[i].Position = new Vector3(p.Value.Item1.X, m_height, 0);
            m_vertInfo[i].Color = Color.White;
            m_vertIndex[i] = i;

            // Point on skyline
            m_vertInfo[i+1].Position = new Vector3(p.Value.Item1.X, p.Value.Item1.Y, 0);
            m_vertInfo[i+1].Color = Color.White;
            m_vertIndex[i+1] = i+1;

            // Next connector point
            if (p.Next != null)
            {
                m_vertInfo[i + 2].Position = new Vector3(p.Next.Value.Item1.X, p.Next.Value.Item1.Y, 0);
                m_vertInfo[i + 2].Color = Color.White;
                m_vertIndex[i + 2] = i + 2;
            }
            else 
            {
                m_vertInfo[m_vertInfo.Length-1].Position = new Vector3(m_width, m_height, 0);
                m_vertInfo[m_vertInfo.Length-1].Color = Color.White;
                m_vertIndex[m_vertInfo.Length-1] = m_vertInfo.Length-1;
            }
            i += 2;

            // m_vertInfo[i].Position = new Vector3(p.Value.Item1.X, p.Value.Item1.Y, 0);
            // m_vertInfo[i].Color = Color.White;
            // m_vertIndex[i] = i;
            // i++;
        }
    }

    public override void loadContent(ContentManager contentManager)
    {
        m_graphics.GraphicsDevice.RasterizerState = new RasterizerState
        {
            FillMode = FillMode.WireFrame,
            CullMode = CullMode.CullClockwiseFace, // remove counter-clockwise faces
            MultiSampleAntiAlias = true,
        };

        m_effect = new BasicEffect(m_graphicsDevice)
        {
            VertexColorEnabled = true,
            View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up),

            Projection = Matrix.CreateOrthographicOffCenter(
                left: 0,
                right: m_graphicsDevice.Viewport.Width,
                bottom: m_graphicsDevice.Viewport.Height,
                top: 0,   // doing this to get it to match the default of upper left of (0, 0)
                zNearPlane: 0.1f,
                zFarPlane: 2)
        };
    }

    public override void Render()
    {
        foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            m_graphicsDevice.DrawUserIndexedPrimitives(
              primitiveType: PrimitiveType.TriangleStrip,
              vertexData: m_vertInfo,
              vertexOffset: 0,
              numVertices: m_vertInfo.Length - 2,
              indexData: m_vertIndex,
              indexOffset: 0,
              primitiveCount: m_vertIndex.Length - 2
            );
        }
    }

    public override void Update()
    {
    }

}
