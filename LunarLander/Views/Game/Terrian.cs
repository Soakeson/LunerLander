using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410;

//TODO add variable amounts of safe zones
public class Terrain : GameObject
{
    private BasicEffect m_effect;
    private VertexPositionColor[] m_outlineVertInfo;
    private int[] m_outlineVertIndex;
    private VertexPositionColor[] m_vertInfo;
    private int[] m_vertIndex;
    public LinkedList<(Point, bool)> m_skyLine;
    private int m_width;
    private int m_height;
    MyRandom m_rand = new MyRandom();

    public Terrain(int width, int height, int safeZones)
    {
        m_width = width;
        m_height = height;

        Generate(safeZones);
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

            m_graphicsDevice.DrawUserIndexedPrimitives(
              primitiveType: PrimitiveType.LineStrip,
              vertexData: m_outlineVertInfo,
              vertexOffset: 0,
              numVertices: m_outlineVertInfo.Length - 1,
              indexData: m_outlineVertIndex,
              indexOffset: 0,
              primitiveCount: m_outlineVertIndex.Length - 1
            );
        }
    }

    public override void loadContent(ContentManager contentManager)
    {
        m_graphics.GraphicsDevice.RasterizerState = new RasterizerState
        {
            FillMode = FillMode.Solid,
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

    public override void Update(GameTime gameTime)
    {
    }

    private void Generate(LinkedListNode<(Point, bool)> start, LinkedListNode<(Point, bool)> end, int lr, int curl)
    {
        // If refinement depth has been reached return
        if (curl == lr)
        {
            return;
        }
        double s = .7;
        double r = s * m_rand.nextGaussian(0.0, 1.0) * Math.Abs(end.Value.Item1.X - start.Value.Item1.X);
        r = r > m_height / 3 ? m_height / 100 : r;
        r = r < -m_height / 3 ? m_height / 100 : r;

        int y = (int)((start.Value.Item1.Y + end.Value.Item1.Y) / 2 + r);
        y = y > m_height ? m_height - (int)Math.Abs(r) : y;
        int x = (start.Value.Item1.X + end.Value.Item1.X) / 2;

        m_skyLine.AddAfter(start, new LinkedListNode<(Point, bool)>((new Point(x, y), false)));

        Generate(start, start.Next, lr, curl + 1);
        Generate(end.Previous, end, lr, curl + 1);

        return;
    }

    // Initialization public facing function for the recursive one.
    public void Generate(int safeZones)
    {
        int startY = m_rand.Next((int)(m_height * .55), (int)(m_height * .85));

        m_skyLine = new LinkedList<(Point, bool)>();
        // Add Start, End and Start and End of safe zone.
        m_skyLine.AddFirst((new Point(0, startY), false));
        m_skyLine.AddLast((new Point(m_width, startY), false));
        LinkedListNode<(Point, bool)> currNode = m_skyLine.First;

        // Generate safe zone points
        for (int i = 0; i < safeZones; i++)
        {
            // Mid point zone
            int safeX = m_rand.Next((int)(m_width * ((i/(float)safeZones))), (int)(m_width * ((i+1)/(float)safeZones)));

            // Zone at the start
            if (i == 0)
            {
                safeX = m_rand.Next((int)(m_width * ((i/(float)safeZones) + .15f)), (int)(m_width * (((i+1)/(float)safeZones)-.1f)));
            }

            // Zone on the end
            if (i == safeZones - 1)
            {
                safeX = m_rand.Next((int)(m_width * ((i/(float)safeZones)+.1f)), (int)(m_width * (((i+1)/(float)safeZones) - .15f)));
            }

            int safeY = m_rand.Next((int)(m_height * .55), (int)(m_height * .85));
            m_skyLine.AddAfter(currNode, (new Point(safeX, safeY), true));
            m_skyLine.AddAfter(currNode.Next, (new Point(safeX + (int)(m_width * .1), safeY), true));
            currNode = currNode.Next.Next;
        }

        // Pair up zones that need to be generated
        List<(LinkedListNode<(Point, bool)>, LinkedListNode<(Point, bool)>)> pairs = new List<(LinkedListNode<(Point, bool)>, LinkedListNode<(Point, bool)>)>();
        for (LinkedListNode<(Point, bool)> p = m_skyLine.First; p != null; p = p.Next.Next)
        {
            pairs.Add((p, p.Next));
        }

        // Generate each pair
        foreach ((LinkedListNode<(Point, bool)>, LinkedListNode<(Point, bool)>) pair in pairs)
        {
            Generate(pair.Item1, pair.Item2, 7, 0);
        }

        m_vertInfo = new VertexPositionColor[m_skyLine.Count * 3];
        m_vertIndex = new int[m_skyLine.Count * 3];
        m_outlineVertInfo = new VertexPositionColor[m_skyLine.Count];
        m_outlineVertIndex = new int[m_skyLine.Count];

        TranslateToTriStrip();
    }

    private void TranslateToTriStrip()
    {
        //generate vertexpostions from skyline
        int i = 0;
        int j = 0;
        for (LinkedListNode<(Point, bool)> p = m_skyLine.First; p != null; p = p.Next)
        {
            m_outlineVertInfo[j].Position = new Vector3(p.Value.Item1.X, p.Value.Item1.Y, 0);
            m_outlineVertInfo[j].Color = Color.White;
            m_outlineVertIndex[j] = j;

            // Low point
            m_vertInfo[i].Position = new Vector3(p.Value.Item1.X, m_height, 0);
            m_vertInfo[i].Color = Color.Gray;
            m_vertIndex[i] = i;

            // Point on skyline
            m_vertInfo[i + 1].Position = new Vector3(p.Value.Item1.X, p.Value.Item1.Y, 0);
            m_vertInfo[i + 1].Color = Color.Gray;
            m_vertIndex[i + 1] = i + 1;

            // Next connector point
            if (p.Next != null)
            {
                m_vertInfo[i + 2].Position = new Vector3(p.Next.Value.Item1.X, p.Next.Value.Item1.Y, 0);
                m_vertInfo[i + 2].Color = Color.Gray;
                m_vertIndex[i + 2] = i + 2;
            }
            else
            {
                m_vertInfo[m_vertInfo.Length - 1].Position = new Vector3(m_width, m_height, 0);
                m_vertInfo[m_vertInfo.Length - 1].Color = Color.Gray;
                m_vertIndex[m_vertInfo.Length - 1] = m_vertInfo.Length - 1;
            }
            j++;
            i += 2;
        }
    }
}
