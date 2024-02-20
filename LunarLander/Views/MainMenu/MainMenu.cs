using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class MainMenu : State
{
  SpriteFont m_menuFont;

  override public void loadContent(ContentManager contentManager)
  {
    m_menuFont = contentManager.Load<SpriteFont>("Fonts/Micro5");
  }

  override public StateEnum processInput(GameTime gameTime)
  {
      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        return StateEnum.Exit;
      }
      return StateEnum.MainMenu;
  }

  override public void render(GameTime gameTime)
  {
    m_graphicsDevice.Clear(Color.Black);
    m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState:SamplerState.PointClamp);
    m_spriteBatch.DrawString(m_menuFont, "THIS IS THE MAIN MENU", new Vector2(100, 100), Color.White);
    m_spriteBatch.End();
  }

  override public void update(GameTime gameTime)
  {

  }
}
