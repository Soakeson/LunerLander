using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CS5410.Input;

public class ControlsManager
{
    private Dictionary<ControlsEnum, (Keys, bool, IInputDevice.CommandDelegate)> m_controlList = new Dictionary<ControlsEnum, (Keys, bool, IInputDevice.CommandDelegate)>();
    private KeyboardInput m_keyboard = new KeyboardInput();

    public void Update(GameTime gameTime) 
    {
        m_keyboard.Update(gameTime);
    }

    public void Register(ControlsEnum control, Keys key, bool keyPressOnly, IInputDevice.CommandDelegate @delegate)
    {
        m_controlList.Add(control, (key, keyPressOnly, @delegate));
        m_keyboard.registerCommand(key, keyPressOnly, @delegate);
    }

    public void SetKey(ControlsEnum control, Keys key)
    {
        (Keys, bool, IInputDevice.CommandDelegate) oldPair = m_controlList[control];
        m_controlList.Remove(control);
        m_controlList.Add(control, (key, oldPair.Item2, oldPair.Item3));
        m_keyboard = new KeyboardInput();
        foreach ((Keys, bool, IInputDevice.CommandDelegate) commands in m_controlList.Values)
        {
            m_keyboard.registerCommand(commands.Item1, commands.Item2, commands.Item3);
        }
    }

    public Keys GetKey(ControlsEnum control)
    {
        return m_controlList[control].Item1;
    }
}
