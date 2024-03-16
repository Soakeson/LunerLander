using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public class Lander : GameObject
{
    private Vector2 m_position;
    private Texture2D m_sprite;
    public Vector2 m_velocity;
    private Vector2 m_gravity;
    public Vector2 m_rotation;
    private TimeSpan m_lastTime;

    public Lander(int x, int y)
    {
        m_position = new Vector2(x, y);
        m_gravity = new Vector2(0, 0.001652f);
    }

    public override void Render()
    {
        m_spriteBatch.Begin();
        m_spriteBatch.Draw(
            m_sprite,
            new Rectangle(
                x: (int)m_position.X,
                y: (int)m_position.Y,
                width: 50,
                height: 50 
                ),
            Color.White);
        m_spriteBatch.End();
    }

    public override void loadContent(ContentManager contentManager)
    {
        m_sprite = contentManager.Load<Texture2D>("Images/lander");
    }

    public override void Update(GameTime gameTime)
    {
        TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
        m_velocity += m_gravity * diff.Milliseconds; 
        m_position += m_velocity;
        m_lastTime = gameTime.TotalGameTime;
    }

    public void Thrust(GameTime gameTime)
    {
        TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
        m_velocity += new Vector2(0, .0037f) * -diff.Milliseconds;
    }

    public void RotateLeft(GameTime gameTime)
    {
    }

    public void RotateRight(GameTime gameTime)
    {
    }

    public bool CheckCollision(GameTime gameTime)
    {
        return false;
    }
}
