using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

class OptionsView : State
{

  public OptionsView()
  {

  }

  override public void loadContent(ContentManager contentManager)
  {

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

  }

  override public void update(GameTime gameTime)
  {

  }
}
