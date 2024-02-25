using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;
using System;

class Credits : State 
{
  private KeyboardInput m_keyboard;
  private SpriteFont m_creditFont;
  private TimeSpan? m_momentOfCreation;

  public Credits()
  {
    m_keyboard = new KeyboardInput();
  }

  override public void loadContent(ContentManager contentManager)
  {
    m_keyboard = new KeyboardInput();
    m_creditFont = contentManager.Load<SpriteFont>("Fonts/Micro5");
  }

  override public StateEnum processInput(GameTime gameTime)
  {
      m_keyboard.Update(gameTime);
      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        // Returns to main menu
        return StateEnum.MainMenu;
      }
      return StateEnum.Credits;
  }

  override public void render(GameTime gameTime)
  {
    m_spriteBatch.Begin(SpriteSortMode.Deferred);
    m_spriteBatch.DrawString(m_creditFont, $"Creator: Skyler Oakeson \n", new Vector2(m_screenWidth/2, m_screenHeight/2), Color.White);
    m_spriteBatch.End();
  }

  override public void update(GameTime gameTime)
  {
    if (!m_momentOfCreation.HasValue)
    {
      m_momentOfCreation = gameTime.TotalGameTime;
    }
  }
}
