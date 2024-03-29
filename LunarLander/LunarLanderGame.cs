﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class LunarLanderGame : Game
{
    private GraphicsDeviceManager m_graphics;
    private Dictionary<StateEnum, State> m_stateList;
    private State m_currState;
    private StateEnum m_nextState;
    private SpriteBatch m_spriteBatch;
    private Texture2D m_background;
    protected ControlsManager m_controls = new ControlsManager();

    public LunarLanderGame()
    {
        m_graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        m_graphics.PreferredBackBufferWidth = 1920;
        m_graphics.PreferredBackBufferHeight = 1080;

        m_graphics.ApplyChanges();

        m_stateList = new Dictionary<StateEnum, State>();
        m_stateList.Add(StateEnum.MainMenu, new MainMenuView());
        m_stateList.Add(StateEnum.Game, new GameView());
        m_stateList.Add(StateEnum.Options, new OptionsView());
        m_stateList.Add(StateEnum.Scores, new ScoresView());
        m_stateList.Add(StateEnum.Credits, new CreditsView());
        m_currState = m_stateList[StateEnum.MainMenu];
        m_spriteBatch = new SpriteBatch(m_graphics.GraphicsDevice);

        // Initialize all states in the list
        foreach (StateEnum e in StateEnum.GetValues(typeof(StateEnum)))
        {
            if (e is not StateEnum.Exit)
                m_stateList[e].initialize(GraphicsDevice, m_graphics, m_controls);
        }
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Load content for all states
        m_background = this.Content.Load<Texture2D>("Images/background");
        foreach (StateEnum e in StateEnum.GetValues(typeof(StateEnum)))
        {
            if (e is not StateEnum.Exit)
                m_stateList[e].loadContent(this.Content);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        m_nextState = m_currState.processInput(gameTime);
        if (m_nextState == StateEnum.Exit)
        {
            Exit();
        }
        else
        {
            m_currState = m_stateList[m_nextState];
            m_currState.update(gameTime);
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        m_spriteBatch.Begin();
        m_spriteBatch.Draw(
                m_background,
                new Rectangle(
                    x: 0,
                    y: 0,
                    width: m_graphics.PreferredBackBufferWidth,
                    height: m_graphics.PreferredBackBufferHeight 
                    ),
                Color.White
                );
        m_spriteBatch.End();

        base.Draw(gameTime);
        m_currState.render(gameTime);
    }
}
