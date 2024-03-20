using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;


class ScoresView : State
{
    private SortedList<string, int> m_highScores = new SortedList<string, int>();
    private SpriteFont m_titleFont;
    private SpriteFont m_itemFont;
    private Vector2 m_titleSize;

    override public void loadContent(ContentManager contentManager)
    {
        m_titleFont = contentManager.Load<SpriteFont>("Fonts/Micro5-100");
        m_itemFont = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
        m_titleSize = m_titleFont.MeasureString("HIGH SCORES");
        m_highScores.Add("Sky", 12000);
        m_highScores.Add("Dea", 2000);
        m_highScores.Add("Jes", 15000);
        m_highScores.Add("Pre", 15000);
        m_highScores.Add("Ben", 15000);
    }

    override public StateEnum processInput(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            // Returns to main menu
            return StateEnum.MainMenu;
        }
        return StateEnum.Scores;
    }

    override public void render(GameTime gameTime)
    {
        m_spriteBatch.Begin();
        drawOutlineText(
            spriteBatch: m_spriteBatch,
            font: m_titleFont,
            text: "HIGH SCORES",
            outlineColor: Color.White,
            frontColor: Color.Black,
            pixelOffset: 8,
            position: new Vector2((m_screenWidth - m_titleSize.X) / 2, 30),
            scale: 1.0f
            );

        int offset = m_screenHeight/2;
        foreach((string name, int score) in m_highScores)
        {
            drawOutlineText(
                spriteBatch: m_spriteBatch,
                font: m_itemFont,
                text: $"{name}: {score}" ,
                frontColor: Color.Black,
                outlineColor: Color.Orange,
                pixelOffset: 4,
                position: new Vector2(30, offset),
                scale: 1f 
                );
            offset += 50;
        }
        m_spriteBatch.End();
    }

    override public void update(GameTime gameTime)
    {

    }

    public void SaveScore(string name, int score)
    {
        m_highScores.Add(name, score);
    }
}
