using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class GameView : State
{

    private TerrainGenerator m_generator;
    private List<GameObject> m_entites;

    public GameView()
    {
    }

    override public void loadContent(ContentManager contentManager)
    {
        m_generator = new TerrainGenerator(m_screenWidth, m_screenHeight);
        m_entites = new List<GameObject>();
        m_entites.Add(m_generator.Generate());

        // Initialize all entities
        foreach (GameObject entity in m_entites)
        {
            entity.initialize(m_graphicsDevice, m_graphics);
        }

        // Load content for each entity
        foreach (GameObject entity in m_entites)
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
        return StateEnum.Game;
    }

    override public void render(GameTime gameTime)
    {
        foreach (GameObject entity in m_entites)
        {
            entity.Render();
        }
    }

    override public void update(GameTime gameTime)
    {

    }
}
