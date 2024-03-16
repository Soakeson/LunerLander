using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

class GameView : State
{

    private Terrain m_generator;
    private Dictionary<EntityEnum, GameObject> m_entites;

    public GameView()
    {
        m_entites = new Dictionary<EntityEnum, GameObject>();
        m_entites.Add(EntityEnum.Lander, new Lander(100, 100));

        m_keyboard.registerCommand(Keys.Space, false, new IInputDevice.CommandDelegate((GameTime gameTime, float value) => 
                    {
                        Lander l = (Lander)m_entites[EntityEnum.Lander];
                        l.Thrust(gameTime);
                    }));
    }

    override public void loadContent(ContentManager contentManager)
    {
        m_entites.Add(EntityEnum.Terrian, new Terrain(m_screenWidth, m_screenHeight));

        // Initialize all entities
        foreach (GameObject entity in m_entites.Values)
        {
            entity.initialize(m_graphicsDevice, m_graphics);
        }

        // Load content for each entity
        foreach (GameObject entity in m_entites.Values)
        {
            entity.loadContent(contentManager);
        }
    }

    override public StateEnum processInput(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            // Returns to main menu
            return StateEnum.MainMenu;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.R))
        {
            // Returns to main menu
            Terrain t = (Terrain)m_entites[EntityEnum.Terrian];
            t.Generate();
        }
        m_keyboard.Update(gameTime);
        return StateEnum.Game;
    }

    override public void render(GameTime gameTime)
    {
        m_entites[EntityEnum.Terrian].Render();
        m_entites[EntityEnum.Lander].Render();
    }

    override public void update(GameTime gameTime)
    {
        foreach (GameObject entity in m_entites.Values)
        {
            entity.Update(gameTime);
        }

    }
}
