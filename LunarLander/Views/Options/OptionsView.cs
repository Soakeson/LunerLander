using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

class OptionsView : State
{

  private float m_fontScaling = .5f;
  private SpriteFont m_font;
  public OptionsView()
  {
  }

  override public void loadContent(ContentManager contentManager)
  {
    m_font = contentManager.Load<SpriteFont>("Fonts/Micro5");
  }

  override public StateEnum processInput(GameTime gameTime)
  {
    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
    {
      // Returns to main menu
      return StateEnum.MainMenu;
    }
    return StateEnum.Options;
  }

  override public void render(GameTime gameTime)
  {
    m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState:SamplerState.PointClamp);
    drawOutlineText(
        spriteBatch: m_spriteBatch,
        font: m_font,
        text: "Controls",
        frontColor: Color.Black,
        outlineColor: Color.White,
        pixelOffset: 4,
        position: new Vector2(m_screenWidth/2, m_screenHeight/2),
        scale: m_fontScaling
        );
    m_spriteBatch.End();
  }

  override public void update(GameTime gameTime)
  {

  }
}
