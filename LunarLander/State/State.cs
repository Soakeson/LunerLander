using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

public abstract class State : IState
{
  protected SpriteBatch m_spriteBatch;
  protected GraphicsDevice m_graphicsDevice;
  protected int m_screenWidth;
  protected int m_screenHeight;
  protected KeyboardInput m_keyboard = new KeyboardInput();

  public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics) {
    m_screenWidth = graphics.PreferredBackBufferWidth;
    m_screenHeight = graphics.PreferredBackBufferHeight;
    m_spriteBatch = new SpriteBatch(graphicsDevice);
    m_graphicsDevice = graphicsDevice;
  }

  public abstract void loadContent(ContentManager contentManager);
  public abstract StateEnum processInput(GameTime gameTime);
  public abstract void render(GameTime gameTime);
  public abstract void update(GameTime gameTime);
}

