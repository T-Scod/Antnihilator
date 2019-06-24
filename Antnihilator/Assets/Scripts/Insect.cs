using UnityEngine;
using PathCreation;

/// <summary>
/// Enemy that moves along a path.
/// </summary>
public class Insect : MonoBehaviour
{
    /// <summary>
    /// The path to follow.
    /// </summary>
    [Tooltip("The path to follow.")]
    public PathCreator pathCreator;
    /// <summary>
    /// The movement speed of the enemy.
    /// </summary>
    [Tooltip("The movement speed of the enemy.")]
    public float speed = 5.0f;
    /// <summary>
    /// The amount of damage the enemy can take before dying.
    /// </summary>
    [Tooltip("The amount of damage the enemy can take before dying.")]
    public int health = 3;
    /// <summary>
    /// Amount of damage that can be dealt to the player.
    /// </summary>
    [Tooltip("Amount of damage that can be dealt to the player.")]
    public int attackAmount = 1;
    /// <summary>
    /// The delay before an enemy dies. Gives a chance for audio to play.
    /// </summary>
    [Tooltip("The delay before an enemy dies. Gives a chance for audio to play.")]
    public float deathDelay = 1.0f;
    /// <summary>
    /// The speed at which particles decrease when transitioning between taking damage and not taking damage.
    /// </summary>
    [Tooltip("The speed at which particles decrease when transitioning between taking damage and not taking damage.")]
    public float particleDecreaseSpeed = 10.0f;
    /// <summary>
    /// Reference to the particle system to be displayed.
    /// </summary>
    [Tooltip("Reference to the particle system to be displayed.")]
    public Transform particles;
    /// <summary>
    /// Reference to the particle systen to be displayed when fire ants explode.
    /// </summary>
    [Tooltip("Reference to the particle systen to be displayed when fire ants explode.")]
    public Transform explosionParticles;
    /// <summary>
    /// The radius of the explosion.
    /// </summary>
    [Tooltip("The radius of the explosion.")]
    public float explosionRadius = 10.0f;
    /// <summary>
    /// The amount of damage the eplosion causes to surrounding enemies.
    /// </summary>
    [Tooltip("The amount of damage the eplosion causes to surrounding enemies.")]
    public int explosionDamage = 1;

    /// <summary>
    /// Determines if the enemy is a fire ant.
    /// </summary>
    [HideInInspector]
    public bool fireAnt = false;

    /// <summary>
    /// The amount of distance the object has travelled.
    /// </summary>
    private float m_distanceTravelled;
    /// <summary>
    /// Determines if audio is playing.
    /// </summary>
    private bool m_audioPlaying = false;
    /// <summary>
    /// Determines if the enemy is dead.
    /// </summary>
    private bool m_dead = false;

    /// <summary>
    /// Subscribes the OnPathChanged method to the path creator.
    /// </summary>
    private void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    /// <summary>
    /// Moves the enemy along the path and decreases / increases particles as necessary.
    /// </summary>
    private void Update()
    {
        // checks if the enemy is not being attacked and if the particles are still visible
        if (!m_audioPlaying && particles.transform.localScale.y > 0.0f)
        {
            // reduces the particles
            particles.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * particleDecreaseSpeed;
            // sets the particle scale to 0 if they were decremented too far
            if (particles.transform.localScale.y < 0.0f)
            {
                if (particles.gameObject.activeSelf)
                {
                    particles.gameObject.SetActive(false);
                }
                particles.transform.localScale = Vector3.zero;
            }
        }
        // checks if the enemy is being attacked and if the particles are not completely visible
        else if (m_audioPlaying && particles.transform.localScale.y < 1.0f)
        {
            if (!particles.gameObject.activeSelf)
            {
                particles.gameObject.SetActive(true);
            }
            // increases the particles
            particles.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * particleDecreaseSpeed;
            // sets the particle scale to 1 if they were incremented too far
            if (particles.transform.localScale.y > 1.0f)
            {
                particles.transform.localScale = Vector3.one;
            }
        }

        // checks if the enemy is not dead
        if (pathCreator != null && !m_dead)
        {
            // increases the count of distance travelled
            m_distanceTravelled += speed * Time.deltaTime;
            // the enemy's time along the path
            float t = m_distanceTravelled / pathCreator.path.length;
            t = Mathf.Clamp01(t);
            // hasn't reached the end yet
            if (t != 1)
            {
                // moves the enemy along the path
                transform.position = pathCreator.path.GetPointAtDistance(m_distanceTravelled, EndOfPathInstruction.Stop);
                transform.rotation = pathCreator.path.GetRotationAtDistance(m_distanceTravelled, EndOfPathInstruction.Stop);
            }
            else
            {
                // damages the player
                FindObjectOfType<UIManager>().TakeDamage(attackAmount);
                // destroys the ememy
                Destroy(gameObject);
            }
        }
        // checks if the ant exploded
        else if (m_dead && fireAnt)
        {
            if (!explosionParticles.gameObject.activeSelf)
            {
                explosionParticles.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Corrects the distance travelled if the path is changed.
    /// </summary>
    private void OnPathChanged()
    {
        m_distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    /// <summary>
    /// Reduces the remaining health of the enemy.
    /// </summary>
    public void TakeDamage()
    {
        // decrements the remaining health
        health--;
        // checks if the enemy died
        if (health == 0)
        {
            // changes the audio source
            m_dead = true;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSources[0].Stop();
            audioSources[1].Play();
            // destroys the enemy after the delay
            Destroy(gameObject, deathDelay);

            // checks if the enemy is a fire ant
            if (fireAnt)
            {
                // gets all surrounding colliders
                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius * 0.5f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    // checks if the collider is an enemy
                    if (colliders[i].tag == "Enemy")
                    {
                        // damages the enemy
                        colliders[i].GetComponent<Insect>().TakeDamage();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Plays the audio for when the enemy is being damaged.
    /// </summary>
    public void StartAudio()
    {
        if (!m_audioPlaying)
        {
            m_audioPlaying = true;
            GetComponents<AudioSource>()[0].Play();
        }
    }

    /// <summary>
    /// Stops the audio when the enemy is not being damaged.
    /// </summary>
    public void StopAudio()
    {
        if (m_audioPlaying)
        {
            m_audioPlaying = false;
            GetComponents<AudioSource>()[0].Stop();
        }
    }
}