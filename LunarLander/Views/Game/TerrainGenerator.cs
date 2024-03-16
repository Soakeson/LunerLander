using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CS5410;

class TerrainGenerator
{
    int m_width;
    int m_height;
    LinkedList<(Point, bool)> m_skyLine;
    MyRandom m_rand = new MyRandom();

    public TerrainGenerator(int width, int height)
    {
        m_width = width;
        m_height = height;
    }

    private void Generate(LinkedListNode<(Point, bool)> start, LinkedListNode<(Point, bool)> end, int lr, int curl)
    {
        // If refinement depth has been reached return
        if (curl == lr)
        {
            return;
        }
        double s = 1.5;
        double r = s * m_rand.nextGaussian(0.0, 1.0) * Math.Abs(end.Value.Item1.X - start.Value.Item1.X);
        r = r > m_height / 4 ? m_height / 45 : r;
        r = r < -m_height / 4 ? m_height / 55 : r;

        int y = (int)((start.Value.Item1.Y + end.Value.Item1.Y) / 2 + r);
        int x = (start.Value.Item1.X + end.Value.Item1.X) / 2;

        m_skyLine.AddAfter(start, new LinkedListNode<(Point, bool)>((new Point(x, y), false)));

        Generate(start, start.Next, lr, curl + 1);
        Generate(end.Previous, end, lr, curl + 1);

        return;
    }

    // Initialization public facing function for the recursive one.
    public Terrain Generate()
    {
        int startY = m_rand.Next((int)(m_height * .35), (int)(m_height * .75));
        int safeX = m_rand.Next((int)(m_width * .15), (int)(m_width * .75));

        // Add Start, End and Start and End of safe zone.
        m_skyLine = new LinkedList<(Point, bool)>();
        m_skyLine.AddFirst((new Point(0, startY), false));
        m_skyLine.AddLast((new Point(m_width, startY), false));
        m_skyLine.AddAfter(m_skyLine.First, (new Point(safeX, startY), true));
        m_skyLine.AddAfter(m_skyLine.First.Next, (new Point(safeX + (int)(m_width * .15), startY), true));

        // Generate in two chunks; Before and After the safe zone.
        Generate(m_skyLine.First, m_skyLine.First.Next, 6, 0);
        Generate(m_skyLine.Last.Previous, m_skyLine.Last, 6, 0);

        // Pass of to the Terrian object to make it renderable
        return new Terrain(m_skyLine, m_width, m_height);
    }
}
