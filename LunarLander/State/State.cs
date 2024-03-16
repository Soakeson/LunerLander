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
  // protected ControlsManager m_controls = new ControlsManager();

  public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics) 
  {
    m_screenWidth = graphics.PreferredBackBufferWidth;
    m_screenHeight = graphics.PreferredBackBufferHeight;
    m_spriteBatch = new SpriteBatch(graphicsDevice);
    m_graphicsDevice = graphicsDevice;
  }

  protected static void drawOutlineText(SpriteBatch spriteBatch, SpriteFont font, string text, Color outlineColor, Color frontColor, int pixelOffset, Vector2 position, float scale)
  {
      // outline
      spriteBatch.DrawString(font, text, position - new Vector2(pixelOffset * scale, 0), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
      spriteBatch.DrawString(font, text, position + new Vector2(pixelOffset * scale, 0), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
      spriteBatch.DrawString(font, text, position - new Vector2(0, pixelOffset * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
      spriteBatch.DrawString(font, text, position + new Vector2(0, pixelOffset * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);

      // outline corners
      spriteBatch.DrawString(font, text, position - new Vector2(pixelOffset * scale, pixelOffset * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
      spriteBatch.DrawString(font, text, position + new Vector2(pixelOffset * scale, pixelOffset * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
      spriteBatch.DrawString(font, text, position - new Vector2(-(pixelOffset * scale), pixelOffset * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);
      spriteBatch.DrawString(font, text, position + new Vector2(-(pixelOffset * scale), pixelOffset * scale), outlineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);

      // inside
      spriteBatch.DrawString(font, text, position, frontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
  }

  public abstract void loadContent(ContentManager contentManager);
  public abstract StateEnum processInput(GameTime gameTime);
  public abstract void render(GameTime gameTime);
  public abstract void update(GameTime gameTime);
}

