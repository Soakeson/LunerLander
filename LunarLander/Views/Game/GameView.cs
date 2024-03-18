using System;
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
    private SpriteFont m_itemFont;

    public GameView()
    {
        m_entites = new Dictionary<EntityEnum, GameObject>();

        m_keyboard.registerCommand(Keys.Space, false, new IInputDevice.CommandDelegate((GameTime gameTime, float value) => 
                    {
                        Lander l = (Lander)m_entites[EntityEnum.Lander];
                        l.Thrust(gameTime);
                    }));

        m_keyboard.registerCommand(Keys.D, false, new IInputDevice.CommandDelegate((GameTime gameTime, float value) => 
                    {
                        Lander l = (Lander)m_entites[EntityEnum.Lander];
                        l.RotateRight(gameTime);
                    }));

        m_keyboard.registerCommand(Keys.A, false, new IInputDevice.CommandDelegate((GameTime gameTime, float value) => 
                    {
                        Lander l = (Lander)m_entites[EntityEnum.Lander];
                        l.RotateLeft(gameTime);
                    }));
    }

    override public void loadContent(ContentManager contentManager)
    {
        Terrain t = new Terrain(m_screenWidth, m_screenHeight);
        m_entites.Add(EntityEnum.Terrian, t);
        m_entites.Add(EntityEnum.Lander, new Lander(100, 100, t.m_skyLine));
        m_itemFont = contentManager.Load<SpriteFont>("Fonts/Micro5-50");

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
            Lander l = (Lander)m_entites[EntityEnum.Lander];
            l.Reset(t.m_skyLine);
        }
        m_keyboard.Update(gameTime);
        return StateEnum.Game;
    }

    override public void render(GameTime gameTime)
    {
        m_entites[EntityEnum.Terrian].Render();
        Lander l = (Lander)m_entites[EntityEnum.Lander];
        l.Render();

        // Render Readout
        m_spriteBatch.Begin();
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: "Fuel: " + ((int)l.m_fuel).ToString(),
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 0),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: "X-Dir: " + (l.m_angle.X).ToString(),
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 25),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: "Y-Dir: " + (l.m_angle.Y).ToString(),
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 50),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: "Pos: (" + ((int)l.m_position.X).ToString() + " , " + ((int)l.m_position.Y).ToString() + ")",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 75),
                scale: .5f 
                );
        m_spriteBatch.End();
    }

    override public void update(GameTime gameTime)
    {
        Terrain t = (Terrain)m_entites[EntityEnum.Terrian];
        Lander l = (Lander)m_entites[EntityEnum.Lander];
        l.Update(gameTime);

        // Check if win or dead
        if (!l.m_alive)
        {
            if (l.m_angle.Y < -.95f && l.m_touchSafeZone)
            {
                Console.WriteLine("WINNER");
            }
            else
            {
                Console.WriteLine("LOSER");
            }

        }
    }
}
