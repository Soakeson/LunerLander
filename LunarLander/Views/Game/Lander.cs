using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public class Lander : GameObject
{
    private Texture2D m_sprite;
    public Vector2 m_velocity;
    private Vector2 m_position;
    private Vector2 m_original;
    private Vector2 m_gravity;
    public Vector2 m_angle;
    public float m_rotation;
    public float m_fuel;
    private TimeSpan m_lastTime;
    private Vector3 m_hitBox;
    private LinkedList<(Point, bool)> m_skyLine;
    private bool m_collided;
    

    public Lander(int x, int y, LinkedList<(Point, bool)> skyLine)
    {
        m_original = new Vector2(x, y);
        m_position = new Vector2(x, y);
        m_hitBox = new Vector3(x, y, 19f);
        m_gravity = new Vector2(0, 0.001152f);
        m_velocity = new Vector2(1, 0);
        m_angle = new Vector2(0, -1);
        m_rotation = 0f;
        m_fuel = 100f;
        m_skyLine = skyLine;
    }

    public override void Render()
    {
        m_spriteBatch.Begin();
        m_spriteBatch.Draw(
            texture: m_sprite,
            position: m_position,
            sourceRectangle: null,
            rotation: m_rotation,
            origin: new Vector2((float)(m_sprite.Width/2), (float)(m_sprite.Height/2)),
            scale: new Vector2(.1f, .1f),
            layerDepth: 0,
            effects: SpriteEffects.None,
            color: Color.White);

        m_spriteBatch.End();
    }

    public override void loadContent(ContentManager contentManager)
    {
        m_sprite = contentManager.Load<Texture2D>("Images/lander");
    }

    public override void Update(GameTime gameTime)
    {
        TimeSpan diff = gameTime.TotalGameTime - m_lastTime;

        // Update gravity
        if (!m_collided)
        {
            m_velocity += m_gravity * diff.Milliseconds; 
        }

        //Update position with the next velocity update
        m_position += m_velocity;

        // Update clock
        m_lastTime = gameTime.TotalGameTime;

        //Set Hitbox to follow lander
        m_hitBox.X = m_position.X;
        m_hitBox.Y = m_position.Y;

        // Set collision check if it is different and if true set velecoity to zero
        bool collision = CollisionDetection();
        if (collision != m_collided) 
        {
            m_collided = collision;
            if (collision) {m_velocity = new Vector2(0, 0);};
        }
    }

    public void Thrust(GameTime gameTime)
    {
        if (m_fuel > 0.0f)
        {
            TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
            m_velocity += m_angle * (.0027f * diff.Milliseconds);
            m_fuel -= .01f * diff.Milliseconds;
        }
    }

    public void RotateLeft(GameTime gameTime)
    {
        if (m_fuel > 0.0f)
        {
            TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
            m_rotation -= .02f;
            m_fuel -= .005f * diff.Milliseconds;
            m_angle = new Vector2((float)Math.Sin(m_rotation), -(float)Math.Cos(m_rotation));
        }
    }

    public void RotateRight(GameTime gameTime)
    {
        if (m_fuel > 0)
        {
            TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
            m_rotation += .02f;
            m_fuel -= .005f * diff.Milliseconds;
            m_angle = new Vector2((float)Math.Sin(m_rotation), -(float)Math.Cos(m_rotation));
        }
    }

    public void Reset(LinkedList<(Point, bool)> newSkyLine)
    {
        m_position = m_original;
        m_fuel = 1000f;
        m_rotation = 0f;
        m_angle = new Vector2(0, -1);
        m_velocity = new Vector2(1, 0);
        m_hitBox.X = m_original.X;
        m_hitBox.Y = m_original.Y;
        m_skyLine = newSkyLine;
    }

    public bool CollisionDetection()
    {
        for (LinkedListNode<(Point, bool)> p = m_skyLine.First; p != null; p = p.Next)
        {
            if (p.Next != null)
            {
                bool res = LineCircleIntersection(p.Value.Item1, p.Next.Value.Item1);
                if (res && p.Value.Item2 && p.Value.Item2)
                {
                    return true;
                }
                if (res)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool LineCircleIntersection(Point p1, Point p2) 
    {
        Point v1 = new Point(p2.X - p1.X, p2.Y - p1.Y);
        Point v2 = new Point((int)(p1.X - m_hitBox.X), (int)(p1.Y - m_hitBox.Y));
        int b = -2 * (v1.X * v2.X + v1.Y * v2.Y);
        int c = 2 * (v1.X * v1.X + v1.Y * v1.Y);
        double d = Math.Sqrt((b * b) - 2 * c * (v2.X * v2.X + v2.Y * v2.Y - (m_hitBox.Z * m_hitBox.Z)));

        if (d == double.NaN)
        {
            return false;
        }

        double u1 = (b - d) / c;
        double u2 = (b + d) / c;
        if (u1 <= 1 && u1 >= 0)
        {
            return true;
        }
        if (u2 <= 1 && u2 >= 0)
        {
            return true;
        }
        return false;
    }
}
