using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

class MainMenu : State
{
  private SpriteFont m_menuFont;
  private LinkedListNode<KeyValuePair<MainMenuEnum, StateEnum>> m_currSelect;
  private LinkedList<KeyValuePair<MainMenuEnum, StateEnum>> m_mainMenu;
  private KeyboardInput m_keyboard;
 
  public MainMenu()
  {
    // register all key:(menu items) value:(state changes) into the linked list
    m_mainMenu = new LinkedList<KeyValuePair<MainMenuEnum, StateEnum>>();
    m_mainMenu.AddFirst(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Start, value: StateEnum.Game));
    m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.HighScores, value: StateEnum.Scores));
    m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Credits, value: StateEnum.Credits));
    m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Exit, value: StateEnum.Exit));

    // initilaize first value in the linked list as the start
    m_currSelect = m_mainMenu.First;

    m_keyboard = new KeyboardInput();
    m_keyboard.registerCommand(Keys.W, true, new IInputDevice.CommandDelegate(onSelectPrev));
    m_keyboard.registerCommand(Keys.S, true, new IInputDevice.CommandDelegate(onSelectNext));
  }

  override public void loadContent(ContentManager contentManager)
  {
    m_menuFont = contentManager.Load<SpriteFont>("Fonts/Micro5");
  }

  override public StateEnum processInput(GameTime gameTime)
  {
      m_keyboard.Update(gameTime);
      if (Keyboard.GetState().IsKeyDown(Keys.Enter))
      {
        return m_currSelect.Value.Value;
      }
      return StateEnum.MainMenu;
  }

  override public void render(GameTime gameTime)
  {
    m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState:SamplerState.PointClamp);
    int offset = 0;
    foreach(MainMenuEnum e in MainMenuEnum.GetValues(typeof(MainMenuEnum)))
    {
      if (m_currSelect.Value.Key == e)
      {
        m_spriteBatch.DrawString(m_menuFont, "{" + e.ToString() + "}", new Vector2(30, m_screenHeight*3/4 + offset), Color.DarkOrange);
      }
      else
      {
        m_spriteBatch.DrawString(m_menuFont, e.ToString(), new Vector2(30, m_screenHeight*3/4 + offset), Color.White);
      }
      offset += 24;
    }
    m_spriteBatch.End();
  }

  override public void update(GameTime gameTime)
  {
  }

  protected void onSelectNext(GameTime gameTime, float value) {
    if (m_currSelect.Next != null)
    {
      m_currSelect = m_currSelect.Next;
    }
    else
    {
      m_currSelect = m_mainMenu.First;
    }
  }

  protected void onSelectPrev(GameTime gameTime, float value) {
    if (m_currSelect.Previous != null)
    {
      m_currSelect = m_currSelect.Previous;
    }
    else
    {
      m_currSelect = m_mainMenu.Last;
    }
  }
}
