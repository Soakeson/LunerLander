using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class GameView : State
{

  private BasicEffect m_effect;
  private VertexPositionColor[] m_terrain;
  private int[] m_indexTerrian;
  private VertexPositionColor[] m_vertsTris;
  private VertexPositionColor[] m_vertsTriStrip;
  private int[] m_indexTris;
  private int[] m_indexTriStrip;

  public GameView()
  {
  }

  override public void loadContent(ContentManager contentManager)
  {

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

    // Define the data for 3 triangles in a triangle strip
    m_vertsTriStrip = new VertexPositionColor[5];
    m_vertsTriStrip[0].Position = new Vector3(0, 100, 0);
    m_vertsTriStrip[0].Color = Color.Red;
    m_vertsTriStrip[1].Position = new Vector3(200, 100, 0);
    m_vertsTriStrip[1].Color = Color.Blue;
    m_vertsTriStrip[2].Position = new Vector3(400, 100, 0);
    m_vertsTriStrip[2].Color = Color.Yellow;
    m_vertsTriStrip[3].Position = new Vector3(900, 100, 0);
    m_vertsTriStrip[3].Color = Color.Orange;
    m_vertsTriStrip[4].Position = new Vector3(m_screenWidth, 100, 0);
    m_vertsTriStrip[4].Color = Color.White;

    m_indexTriStrip = new int[6];
    m_indexTriStrip[0] = 0;
    m_indexTriStrip[1] = 1;
    m_indexTriStrip[2] = 2;
    m_indexTriStrip[3] = 3;
    m_indexTriStrip[4] = 4;
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
    foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
    {
      pass.Apply();
      m_graphicsDevice.DrawUserIndexedPrimitives(
        primitiveType: PrimitiveType.LineStrip,
        vertexData: m_vertsTriStrip,
        vertexOffset: 0,
        numVertices: m_vertsTriStrip.Length,
        indexData: m_indexTriStrip,
        indexOffset: 0,
        primitiveCount: 3
      );
    }
  }

  override public void update(GameTime gameTime)
  {

  }
}
