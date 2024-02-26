using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class GameView : State
{

  private BasicEffect m_effect;
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
            0, m_graphicsDevice.Viewport.Width,
            m_graphicsDevice.Viewport.Height, 0,   // doing this to get it to match the default of upper left of (0, 0)
            0.1f, 2)
    };
    // Create an array for each of the 6 triangle vertices
    m_vertsTris = new VertexPositionColor[6];

    // Define the position and color for each vertex - Triangle 1
    m_vertsTris[0].Position = new Vector3(100, 300, 0);
    m_vertsTris[0].Color = Color.Red;
    m_vertsTris[1].Position = new Vector3(200, 100, 0);
    m_vertsTris[1].Color = Color.Green;
    m_vertsTris[2].Position = new Vector3(300, 300, 0);
    m_vertsTris[2].Color = Color.Blue;

    // Define the position and color for each vertex - Triangle 2
    m_vertsTris[3].Position = new Vector3(500, 300, 0);
    m_vertsTris[3].Color = Color.Blue;
    m_vertsTris[4].Position = new Vector3(600, 100, 0);
    m_vertsTris[4].Color = Color.Green;
    m_vertsTris[5].Position = new Vector3(700, 300, 0);
    m_vertsTris[5].Color = Color.Red;

    // Create an array that holds the 'index' of each vertex
    // for each triangle, in groups of 3
    m_indexTris = new int[6];

    // Triangle 1 - Indexes
    m_indexTris[0] = 0;
    m_indexTris[1] = 1;
    m_indexTris[2] = 2;

    // Triangle 2 - Indexes
    m_indexTris[3] = 3;
    m_indexTris[4] = 4;
    m_indexTris[5] = 5;

    //
    // Define the data for 3 triangles in a triangle strip
    m_vertsTriStrip = new VertexPositionColor[5];
    m_vertsTriStrip[0].Position = new Vector3(200, 600, 0);
    m_vertsTriStrip[0].Color = Color.Red;
    m_vertsTriStrip[1].Position = new Vector3(300, 400, 0);
    m_vertsTriStrip[1].Color = Color.Green;
    m_vertsTriStrip[2].Position = new Vector3(400, 600, 0);
    m_vertsTriStrip[2].Color = Color.Blue;
    m_vertsTriStrip[3].Position = new Vector3(500, 400, 0);
    m_vertsTriStrip[3].Color = Color.Red;
    m_vertsTriStrip[4].Position = new Vector3(600, 600, 0);
    m_vertsTriStrip[4].Color = Color.Green;

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
            primitiveType: PrimitiveType.TriangleList, 
            vertexData: m_vertsTris, 
            vertexOffset: 0, 
            numVertices: m_vertsTris.Length, 
            indexData: m_indexTris, 
            indexOffset: 0, 
            primitiveCount: m_indexTris.Length / 3);

        m_graphicsDevice.DrawUserIndexedPrimitives(
            PrimitiveType.TriangleStrip,
            m_vertsTriStrip, 0, m_vertsTriStrip.Length,
            m_indexTriStrip, 0, 3);
    }
  }

  override public void update(GameTime gameTime)
  {

  }
}
