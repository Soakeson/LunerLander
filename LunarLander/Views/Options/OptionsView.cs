using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CS5410.Input;

class OptionsView : State
{
    private SpriteFont m_titleFont;
    private SpriteFont m_itemFont;
    private Vector2 m_titleSize;
    private Vector2 m_itemSize;

    private OptionStateEnum m_currState;
    private LinkedListNode<ControlsEnum> m_currSelect;
    private LinkedList<ControlsEnum> m_optionsMenu;

    private enum OptionStateEnum
    {
        Selection,
        Listening 
    }

    public OptionsView()
    {
        m_currState = OptionStateEnum.Selection;

        m_optionsMenu = new LinkedList<ControlsEnum>();
        m_optionsMenu.AddFirst(ControlsEnum.RotateRight);
        m_optionsMenu.AddLast(ControlsEnum.RotateLeft);
        m_optionsMenu.AddLast(ControlsEnum.Thrust);

        m_currSelect = m_optionsMenu.First;

        m_keyboard.registerCommand(Keys.W, true, new IInputDevice.CommandDelegate((GameTime gameTime, float value) =>
        {
            m_currSelect = m_currSelect.Previous is not null ? m_currSelect.Previous : m_optionsMenu.Last;
        }));

        m_keyboard.registerCommand(Keys.S, true, new IInputDevice.CommandDelegate((GameTime GameTime, float value) =>
        {
            m_currSelect = m_currSelect.Next is not null ? m_currSelect.Next : m_currSelect = m_optionsMenu.First;
        }));
    }

    override public void loadContent(ContentManager contentManager)
    {
        m_titleFont = contentManager.Load<SpriteFont>("Fonts/Micro5-100");
        m_titleSize = m_titleFont.MeasureString("OPTIONS");
        m_itemFont = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
        m_itemSize = m_itemFont.MeasureString("Y");
    }

    override public StateEnum processInput(GameTime gameTime)
    {
        m_keyboard.Update(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            m_currState = OptionStateEnum.Listening;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            // Returns to main menu
            return StateEnum.MainMenu;
        }
        return StateEnum.Options;
    }

    override public void render(GameTime gameTime)
    {
        m_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
        drawOutlineText(
            spriteBatch: m_spriteBatch,
            font: m_titleFont,
            text: "OPTIONS",
            outlineColor: Color.White,
            frontColor: Color.Black,
            pixelOffset: 8,
            position: new Vector2((m_screenWidth - m_titleSize.X) / 2, 30),
            scale: 1
            );

        int idx = 0;
        foreach (ControlsEnum option in m_optionsMenu)
        {
            // If item has been selected render differently
            if (m_currSelect.Value == option && m_currState == OptionStateEnum.Selection)
            {
                drawOutlineText(
                    spriteBatch: m_spriteBatch,
                    font: m_itemFont,
                    text: "[" + option.ToString() + " : " + "]",
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
                    text: option.ToString() + " : " ,
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
        // if (m_currState == OptionStateEnum.Listening)
        // {
        //     m_controls.SetKey(ControlsEnum.MenuUp, Keys.Up);
        //     m_currState = OptionStateEnum.Selection;
        // }
    }
}
