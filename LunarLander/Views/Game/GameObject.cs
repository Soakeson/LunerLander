using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public abstract class GameObject
{
    protected SpriteBatch m_spriteBatch;
    protected GraphicsDevice m_graphicsDevice;
    protected GraphicsDeviceManager m_graphics;

    public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
    {
        m_spriteBatch = new SpriteBatch(graphicsDevice);
        m_graphicsDevice = graphicsDevice;
        m_graphics = graphics;
    }

    public abstract void Render();
    public abstract void Update();
    public abstract void loadContent(ContentManager contentManager);
}
