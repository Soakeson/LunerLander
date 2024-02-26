using Microsoft.Xna.Framework.Graphics;

class TerrainGenerator
{
  int m_height;
  int m_width;

  public TerrainGenerator(int height, int width)
  {
    m_height = height;
    m_width = width;
  }

  public (int[] indexInfo, VertexPositionColor[] vertInfo) Generate()
  {
    return (new int[]{}, new VertexPositionColor[]{});
  }
}
