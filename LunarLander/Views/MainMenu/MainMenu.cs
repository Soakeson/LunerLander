using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class MainMenu : State
{
  private SpriteFont m_menuFont;
  private StateEnum m_nextState = StateEnum.MainMenu;
  private MainMenuEnum m_currSelect;
  private Stack<MainMenuEnum> m_mainMenu;
  private Dictionary<MainMenuEnum, StateEnum> m_stateSelect;

  public MainMenu()
  {
    // Build the key value pairs that pair MainMenuEnum's to StateEnum's
    m_stateSelect = new Dictionary<MainMenuEnum, StateEnum>();
    m_stateSelect.Add(MainMenuEnum.Start, StateEnum.Game);
    m_stateSelect.Add(MainMenuEnum.Credits, StateEnum.Credits);
    m_stateSelect.Add(MainMenuEnum.HighScores, StateEnum.Scores);
    m_stateSelect.Add(MainMenuEnum.Exit, StateEnum.Exit);

    // Build the stack that will represent the main menu selection. Built backwards because of the stack structure
    m_mainMenu.Push(MainMenuEnum.Exit);
    m_mainMenu.Push(MainMenuEnum.Credits);
    m_mainMenu.Push(MainMenuEnum.HighScores);
    m_mainMenu.Push(MainMenuEnum.Start);
  }

  override public void loadContent(ContentManager contentManager)
  {
    m_menuFont = contentManager.Load<SpriteFont>("Fonts/Micro5");
  }

  override public StateEnum processInput(GameTime gameTime)
  {
      if (Keyboard.GetState().IsKeyDown(Keys.Enter))
      {
        return m_stateSelect[m_currSelect];
      }
      return StateEnum.MainMenu;
  }

  override public void render(GameTime gameTime)
  {
    m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState:SamplerState.PointClamp);
    m_spriteBatch.DrawString(m_menuFont, "MAIN MENU", new Vector2(m_screenWidth/2, m_screenHeight/2), Color.White);
    m_spriteBatch.End();
  }

  override public void update(GameTime gameTime)
  {

  }
}
