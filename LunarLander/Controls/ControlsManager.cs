using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CS5410.Input;

public class ControlsManager
{
    private Dictionary<ControlsEnum, Keys> m_controlList;
    private KeyboardInput m_keyboard = new KeyboardInput();
    private bool m_reload = false;

    public ControlsManager()
    {
        m_controlList = new Dictionary<ControlsEnum, Keys>();
        Default();
    }

    public void Default()
    {
        m_controlList.Add(ControlsEnum.MenuUp, Keys.W);
        m_controlList.Add(ControlsEnum.MenuDown, Keys.S);
        m_controlList.Add(ControlsEnum.Thrust, Keys.Space);
        m_controlList.Add(ControlsEnum.RotateLeft, Keys.A);
        m_controlList.Add(ControlsEnum.RotateRight, Keys.D);
    }

    public void Update(GameTime gameTime) 
    {
        if (m_reload) 
        {

        }

        m_keyboard.Update(gameTime);
    }

    public void Register(IInputDevice.CommandDelegate @delegate, Keys key)
    {
        m_keyboard.registerCommand(key, true, @delegate);
    }

    public void SetKey(ControlsEnum control, Keys key)
    {
        m_controlList[control] = key;
        m_reload = true;
    }

    public Keys MenuDownKey()
    {

        return m_controlList[ControlsEnum.MenuDown];
    }

    public Keys MenuUpKey()
    {

        return m_controlList[ControlsEnum.MenuUp];
    }

    public Keys ThrustKey()
    {
        return m_controlList[ControlsEnum.Thrust];
    }

    public Keys RotateLeftKey()
    {

        return m_controlList[ControlsEnum.RotateLeft];
    }

    public Keys RotateRightKey()
    {

        return m_controlList[ControlsEnum.RotateRight];
    }
}
