using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class GameView : State
{

    private BasicEffect m_effect;
    private Terrain m_terrain;
    private TerrainGenerator m_generator;

    public GameView()
    {
    }

    override public void loadContent(ContentManager contentManager)
    {

        m_generator = new TerrainGenerator(m_screenWidth, m_screenHeight);
        m_terrain = m_generator.Generate();
        m_effect = new BasicEffect(m_graphicsDevice)
        {
            VertexColorEnabled = true,
            View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up),

            Projection = Matrix.CreateOrthographicOffCenter(
                left: 0,
                right: m_graphicsDevice.Viewport.Width,
                bottom: m_graphicsDevice.Viewport.Height,
                top: 0,   // doing this to get it to match the default of upper left of (0, 0)
                zNearPlane: 0.1f,
                zFarPlane: 2)
        };

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
            m_terrain = m_generator.Generate();
        }
        return StateEnum.Game;
    }

    override public void render(GameTime gameTime)
    {
        foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            m_graphicsDevice.DrawUserIndexedPrimitives(
              primitiveType: PrimitiveType.LineStrip,
              vertexData: m_terrain.m_vertInfo,
              vertexOffset: 0,
              numVertices:  m_terrain.m_vertInfo.Length,
              indexData: m_terrain.m_vertIndex,
              indexOffset: 0,
              primitiveCount: m_terrain.m_vertIndex.Length-1
            );
        }
    }

    override public void update(GameTime gameTime)
    {

    }
}
