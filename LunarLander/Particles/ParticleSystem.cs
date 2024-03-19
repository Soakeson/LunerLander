using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class ParticleSystem
{
    private Dictionary<long, Particle> m_particles = new Dictionary<long, Particle>();
    public Dictionary<long, Particle>.ValueCollection particles { get { return m_particles.Values; } }
    private MyRandom m_random = new MyRandom();

    private Vector2 m_center;
    private int m_sizeMean; // pixels
    private int m_sizeStdDev;   // pixels
    private float m_speedMean;  // pixels per millisecond
    private float m_speedStDev; // pixles per millisecond
    private float m_lifetimeMean; // milliseconds
    private float m_lifetimeStdDev; // milliseconds
    private Vector2? m_direction = null;

    public ParticleSystem(Vector2 center, int sizeMean, int sizeStdDev, float speedMean, float speedStdDev, int lifetimeMean, int lifetimeStdDev, Vector2? dir)
    {
        m_center = center;
        m_sizeMean = sizeMean;
        m_sizeStdDev = sizeStdDev;
        m_speedMean = speedMean;
        m_speedStDev = speedStdDev;
        m_lifetimeMean = lifetimeMean;
        m_lifetimeStdDev = lifetimeStdDev;
        m_direction = dir;
    }

    private Particle create()
    {
        float size = (float)m_random.nextGaussian(m_sizeMean, m_sizeStdDev);
        Vector2 dir = m_direction != null ? (Vector2)m_direction : m_random.nextCircleVector();
        var p = new Particle(
                m_center,
                dir,
                (float)m_random.nextGaussian(m_speedMean, m_speedStDev),
                new Vector2(size, size),
                new System.TimeSpan(0, 0, 0, 0, (int)(m_random.nextGaussian(m_lifetimeMean, m_lifetimeStdDev)))); ;

        return p;
    }

    public void update(GameTime gameTime, Vector2? newPos, Vector2? newDir)
    {
        // Update existing particles
        m_center = newPos != null ? (Vector2)newPos : m_center;
        m_direction = newDir != null ? (Vector2)newDir : m_direction;
        List<long> removeMe = new List<long>();
        foreach (Particle p in m_particles.Values)
        {
            if (!p.update(gameTime))
            {
                removeMe.Add(p.name);
            }
        }

        // Remove dead particles
        foreach (long key in removeMe)
        {
            m_particles.Remove(key);
        }

        // Generate some new particles
        for (int i = 0; i < 8; i++)
        {
            var particle = create();
            m_particles.Add(particle.name, particle);
        }
    }
}

