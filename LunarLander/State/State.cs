using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public abstract class State : IState
{
  protected GraphicsDeviceManager m_graphics;
  protected SpriteBatch m_spriteBatch;
  protected GraphicsDevice m_graphicsDevice;

  public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics) {
    m_graphics = graphics;
    m_spriteBatch = new SpriteBatch(graphicsDevice);
    m_graphicsDevice = graphicsDevice;
  }

  public abstract void loadContent(ContentManager contentManager);
  public abstract StateEnum processInput(GameTime gameTime);
  public abstract void render(GameTime gameTime);
  public abstract void update(GameTime gameTime);
}

