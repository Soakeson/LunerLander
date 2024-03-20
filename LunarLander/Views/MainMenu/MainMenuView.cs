using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

class MainMenuView : State
{
    private SpriteFont m_titleFont;
    private SpriteFont m_itemFont;
    private Vector2 m_titleSize;
    private Vector2 m_itemSize;
    private LinkedListNode<KeyValuePair<MainMenuEnum, StateEnum>> m_currSelect;
    private LinkedList<KeyValuePair<MainMenuEnum, StateEnum>> m_mainMenu;
    private StateEnum m_nextState = StateEnum.MainMenu;

    public MainMenuView()
    {
        // Register all key:(menu items) value:(state changes) into the linked list
        m_mainMenu = new LinkedList<KeyValuePair<MainMenuEnum, StateEnum>>();
        m_mainMenu.AddFirst(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Start, value: StateEnum.Game));
        m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Options, value: StateEnum.Options));
        m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.HighScores, value: StateEnum.Scores));
        m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Credits, value: StateEnum.Credits));
        m_mainMenu.AddLast(new KeyValuePair<MainMenuEnum, StateEnum>(key: MainMenuEnum.Exit, value: StateEnum.Exit));

        // Initilaize first value in the linked list as the start
        m_currSelect = m_mainMenu.First;

    }

    override public void loadContent(ContentManager contentManager)
    {
        m_titleFont = contentManager.Load<SpriteFont>("Fonts/Micro5-100");
        m_titleSize = m_titleFont.MeasureString("LUNAR LANDER");
        m_itemFont = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
        m_itemSize = m_itemFont.MeasureString("Y");


        m_keyboard.registerCommand(Keys.W, true, new IInputDevice.CommandDelegate((GameTime gameTime, float value) =>
        {
            m_currSelect = m_currSelect.Previous is not null ? m_currSelect.Previous : m_mainMenu.Last;
        }));
        m_keyboard.registerCommand(Keys.S, true, new IInputDevice.CommandDelegate((GameTime GameTime, float value) =>
        {
            m_currSelect = m_currSelect.Next is not null ? m_currSelect.Next : m_currSelect = m_mainMenu.First;
        }));
        m_keyboard.registerCommand(Keys.Enter, true, new IInputDevice.CommandDelegate((GameTime GameTime, float value) =>
        {
            m_nextState = m_currSelect.Value.Value;
        }));
    }

    override public StateEnum processInput(GameTime gameTime)
    {
        m_keyboard.Update(gameTime);
        StateEnum nextState = m_nextState;
        m_nextState = StateEnum.MainMenu;
        return nextState;
    }

    override public void render(GameTime gameTime)
    {
        m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
        drawOutlineText(
            spriteBatch: m_spriteBatch,
            font: m_titleFont,
            text: "LUNAR LANDER",
            outlineColor: Color.White,
            frontColor: Color.Black,
            pixelOffset: 8,
            position: new Vector2((m_screenWidth - m_titleSize.X) / 2, 30),
            scale: 1.0f
            );

        int idx = 0;
        foreach (MainMenuEnum e in MainMenuEnum.GetValues(typeof(MainMenuEnum)))
        {
            // If item has been selected render differently
            if (m_currSelect.Value.Key == e)
            {
                drawOutlineText(
                    spriteBatch: m_spriteBatch,
                    font: m_itemFont,
                    text: "[" + e.ToString() + "]",
                    frontColor: Color.Black,
                    outlineColor: Color.White,
                    pixelOffset: 4,
                    position: new Vector2(30, (idx * m_itemSize.Y) + m_screenHeight/3),
                    scale: 1
                    );
            }
            else
            {
                drawOutlineText(
                    spriteBatch: m_spriteBatch,
                    font: m_itemFont,
                    text: e.ToString(),
                    frontColor: Color.Black,
                    outlineColor: Color.DarkOrange,
                    pixelOffset: 4,
                    position: new Vector2(30, (idx * m_itemSize.Y) + m_screenHeight/3),
                    scale: 1
                    );
            }
            idx++;
        }
        m_spriteBatch.End();
    }

    override public void update(GameTime gameTime)
    {
    }
}
