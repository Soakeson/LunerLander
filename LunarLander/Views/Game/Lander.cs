using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public class Lander : GameObject
{
    private Texture2D m_sprite;
    private TimeSpan m_lastTime;
    private Vector3 m_hitBox;
    private LinkedList<(Point, bool)> m_skyLine;
    private Vector2 m_original;
    private ParticleSystem m_thrustParticleSystem;
    private ParticleSystem m_explosionParticleSystem;
    private ParticleSystemRenderer m_thrustRenderer;
    private ParticleSystemRenderer m_explosionRenderer;
    private int m_explosionDuration = 1000;
    private int m_thrustDuration = 100;
    public Vector2 m_position;
    public bool m_alive = true;
    public bool m_active = true;
    public double m_speed = 0;
    public Vector2 m_gravity = new Vector2(0, 0.001152f);
    public Vector2 m_velocity = new Vector2(2, 1);
    public Vector2 m_angle = new Vector2(0, -1);
    public float m_rotation = 0f;
    public float m_fuel = 100f;

    public Lander(int x, int y, LinkedList<(Point, bool)> skyLine)
    {
        m_original = new Vector2(x, y);
        m_position = new Vector2(x, y);
        m_hitBox = new Vector3(x, y, 19f);
        m_skyLine = skyLine;

        m_explosionParticleSystem = new ParticleSystem(
            m_position,
            10,
            4,
            0.25f,
            0.05f,
            400,
            100,
            null);

        m_thrustParticleSystem = new ParticleSystem(
            m_position,
            10,
            4,
            0.15f,
            0.05f,
            200,
            50,
            -m_angle);

        m_thrustRenderer = new ParticleSystemRenderer("Images/fire");
        m_explosionRenderer = new ParticleSystemRenderer("Images/fire");
    }

    public override void Render()
    {
        if (!m_alive && m_explosionDuration > 0)
        {
            m_explosionRenderer.draw(m_spriteBatch, m_explosionParticleSystem);
        }
        if (m_alive && m_thrustDuration > 0)
        {
            m_thrustRenderer.draw(m_spriteBatch, m_thrustParticleSystem);
        }
        if (m_alive)
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
    }

    public override void loadContent(ContentManager contentManager)
    {
        m_sprite = contentManager.Load<Texture2D>("Images/lander");
        m_explosionRenderer.LoadContent(contentManager);
        m_thrustRenderer.LoadContent(contentManager);
    }

    public override void Update(GameTime gameTime)
    {
        TimeSpan diff = gameTime.TotalGameTime - m_lastTime;

        // If dead update particle system with current position and gameTime
        if (!m_alive)
        {
            m_explosionParticleSystem.update(gameTime, m_position, null);
            m_explosionDuration -= diff.Milliseconds;
        }

        // Calculate meters per second
        m_speed = Math.Sqrt((int)Math.Abs(m_velocity.X*1000)^2 + (int)Math.Abs(m_velocity.Y*1000)^2);
        if (m_active)
        {
            // Update gravity
            m_velocity += m_gravity * diff.Milliseconds; 

            //Update position with the next velocity update
            m_position += m_velocity;

            // Update clock
            m_lastTime = gameTime.TotalGameTime;

            //Set Hitbox to follow lander
            m_hitBox.X = m_position.X;
            m_hitBox.Y = m_position.Y;
            
            // Update thrust particles
            m_thrustParticleSystem.update(gameTime, new Vector2(m_position.X, m_position.Y), -m_angle);
            m_thrustDuration -= diff.Milliseconds;

            // Set collision check if it is different and if true set velecoity to zero
            int collision = CollisionDetection();

            // If collision check if it was going to fast or at too steep of an angle then set alive status
            if (collision != 0) 
            {
                if (collision == 1 && (m_angle.Y > -.97f || m_speed > 25))
                {
                    m_alive = false;
                }
                if (collision == -1)
                {
                    m_alive = false;
                }
                m_active = false;
                m_velocity = new Vector2(0, 0);
            }
        }
    }

    public void Thrust(GameTime gameTime)
    {
        if (m_fuel > 0.0f && m_active)
        {
            TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
            m_velocity += m_angle * .0027f * diff.Milliseconds;
            m_fuel -= .01f * diff.Milliseconds;
            m_thrustDuration = 100;
        }
    }

    public void RotateLeft(GameTime gameTime)
    {
        if (m_fuel > 0.0f && m_active)
        {
            TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
            m_rotation -= .02f;
            m_fuel -= .005f * diff.Milliseconds;
            m_angle = new Vector2((float)Math.Sin(m_rotation), -(float)Math.Cos(m_rotation));
        }
    }

    public void RotateRight(GameTime gameTime)
    {
        if (m_fuel > 0.0f && m_active)
        {
            TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
            m_rotation += .02f;
            m_fuel -= .005f * diff.Milliseconds;
            m_angle = new Vector2((float)Math.Sin(m_rotation), -(float)Math.Cos(m_rotation));
        }
    }

    public void Reset(LinkedList<(Point, bool)> newSkyLine, GameTime gameTime)
    {
        m_lastTime = gameTime.TotalGameTime;
        m_position = m_original;
        m_fuel = 100f;
        m_rotation = 0f;
        m_angle = new Vector2(0, -1);
        m_velocity = new Vector2(2, 1);
        m_hitBox.X = m_original.X;
        m_hitBox.Y = m_original.Y;
        m_skyLine = newSkyLine;
        m_alive = true;
        m_active = true;
        m_explosionDuration = 1000;
        m_explosionParticleSystem = new ParticleSystem(
            m_position,
            10,
            4,
            0.25f,
            0.05f,
            400,
            100,
            null);
    }

    public int CollisionDetection()
    {
        for (LinkedListNode<(Point, bool)> p = m_skyLine.First; p != null; p = p.Next)
        {
            if (p.Next != null)
            {
                bool res = LineCircleIntersection(p.Value.Item1, p.Next.Value.Item1);

                // Safe zone
                if (res && p.Value.Item2 && p.Value.Item2 && m_active)
                {
                    return 1;
                }

                // Unsafe zone
                if (res && m_active)
                {
                    return -1;
                }
            }
        }

        // No hit detected
        return 0;
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
