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
    private GameStateEnum m_state;
    private Dictionary<EntityEnum, GameObject> m_entites;
    private SpriteFont m_itemFont;
    private int m_timer = 3000;
    private int m_score;
    private int m_level = 0;
    private TimeSpan m_lastTime;

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

        m_state = GameStateEnum.Waiting;
    }

    override public void loadContent(ContentManager contentManager)
    {
        Terrain t = new Terrain(m_screenWidth, m_screenHeight, 2);
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
            ResetGame(false, gameTime);
        }

        if (m_state == GameStateEnum.Gameplay)
        {
            m_keyboard.Update(gameTime);
        }

        if (m_state == GameStateEnum.Lost && m_timer < 0)
        {
            m_state = GameStateEnum.Waiting;
            ResetGame(false, gameTime);
            return StateEnum.MainMenu;
        }

        if (m_level == 1 && m_state == GameStateEnum.Won && m_timer < 0)
        {
            m_state = GameStateEnum.Waiting;
            ResetGame(false, gameTime);
            return StateEnum.Scores;
        }

        return StateEnum.Game;
    }

    private void ResetGame(bool next, GameTime gameTime)
    {
        if (next) m_level++;
        else {m_level = 0; m_score = 0;};
        Terrain t = (Terrain)m_entites[EntityEnum.Terrian];
        if (m_level == 0) {t.Generate(2);};
        if (m_level == 1) {t.Generate(1);};
        Lander l = (Lander)m_entites[EntityEnum.Lander];
        l.Reset(t.m_skyLine, gameTime);
        m_timer = 3000;
        m_state = GameStateEnum.Waiting;
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
                text: $"Fuel: {(int)l.m_fuel}" ,
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 0),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"Dir: ({l.m_angle.X}, {l.m_angle.Y})",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 25),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: "Pos: (" + ((int)l.m_position.X).ToString() + " , " + ((int)l.m_position.Y).ToString() + ")",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 50),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"Speed: {(int)l.m_speed}",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 75),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"Level: {m_level+1}",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 100),
                scale: .5f 
                );
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"Score: {m_score}",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 125),
                scale: .5f 
                );
        if (m_state == GameStateEnum.Won)
        {
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"WINNER",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 150),
                scale: .5f 
                );
        }
        if (m_state == GameStateEnum.Lost)
        {
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"LOSER",
                frontColor: Color.Black,
                outlineColor: Color.White,
                pixelOffset: 4,
                position: new Vector2(10, 150),
                scale: .5f 
                );
        }
        if (m_state == GameStateEnum.Waiting)
        {
            drawOutlineText(
                    spriteBatch: m_spriteBatch,
                    font: m_itemFont,
                    text: $"{m_timer / 1000}",
                    frontColor: Color.Red,
                    outlineColor: Color.White,
                    pixelOffset: 4,
                    position: new Vector2(m_screenWidth/2, m_screenHeight/2),
                    scale: 1f 
                    );
        }
        m_spriteBatch.End();
    }

    override public void update(GameTime gameTime)
    {
        Terrain t = (Terrain)m_entites[EntityEnum.Terrian];
        Lander l = (Lander)m_entites[EntityEnum.Lander];
        TimeSpan diff = gameTime.TotalGameTime - m_lastTime;
        m_lastTime = gameTime.TotalGameTime;
        m_timer -= diff.Milliseconds;

        if (m_state == GameStateEnum.Waiting && m_timer < 0)
        {
            m_state = GameStateEnum.Gameplay;
        }

        if (m_state == GameStateEnum.Gameplay) 
        {
            l.Update(gameTime);
            if (!l.m_active && l.m_alive) {m_timer = 3000; m_state = GameStateEnum.Won;};
            if (!l.m_active && !l.m_alive) {m_timer = 3000; m_state = GameStateEnum.Lost;};
        }

        if (m_state == GameStateEnum.Won)
        {
            l.Update(gameTime);
            m_score += (int)l.m_fuel;
            if (m_timer < 0 && m_level < 1)
            {
                ResetGame(true, gameTime);
            }
        }
        if (m_state == GameStateEnum.Lost)
        {
            l.Update(gameTime);
        }
    }
}
