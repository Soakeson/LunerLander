using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

class CreditsView : State 
{
  private SpriteFont m_creditFont;
  private TimeSpan? m_momentOfCreation;
  private float m_fontScaling = .5f;

  override public void loadContent(ContentManager contentManager)
  {
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
    m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState:SamplerState.PointClamp);
    Vector2 stringSize = m_creditFont.MeasureString("Creator: Skyler Oakeson");
    int yPos = ((int)(gameTime.TotalGameTime.TotalMilliseconds - m_momentOfCreation.Value.TotalMilliseconds));
    m_spriteBatch.DrawString(
        spriteFont: m_creditFont,
        text: "Created by:\n Skyler Oakeson", 
        position: new Vector2(20, yPos < m_screenHeight-stringSize.Y ? yPos : m_screenHeight-stringSize.Y), 
        color: Color.Silver,
        rotation: 0f,
        origin: new Vector2(0,0),
        effects: SpriteEffects.None,
        scale: m_fontScaling,
        layerDepth: 0
        );
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
