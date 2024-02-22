using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class LunarLanderGame : Game
{
    private GraphicsDeviceManager m_graphics;
    private Dictionary<StateEnum, State> m_stateList;
    private State m_currState;
    private StateEnum m_nextState;

    public LunarLanderGame()
    {
        m_graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        m_stateList = new Dictionary<StateEnum, State>();
        m_stateList.Add(StateEnum.MainMenu, new MainMenu());
        m_currState = m_stateList[StateEnum.MainMenu];
        m_currState.initialize(GraphicsDevice, m_graphics);
        
        // TODO: Initialize all states!
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        m_currState.loadContent(this.Content);
    }

    protected override void Update(GameTime gameTime)
    {
        m_nextState = m_currState.processInput(gameTime);
        if (m_nextState == StateEnum.Exit)
        {
          Exit();
        }

        m_currState.update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        m_currState.render(gameTime);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
