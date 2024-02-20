using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public interface IState
{
    public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics);
    public void loadContent(ContentManager contentManager);
    public StateEnum processInput(GameTime gameTime);
    public void update(GameTime gameTime);
    public void render(GameTime gameTime);
}
