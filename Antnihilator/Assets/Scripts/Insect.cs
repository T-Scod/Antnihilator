using UnityEngine;
using PathCreation;

public class Insect : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5.0f;
    public int health = 3;
    public int attackAmount = 1;
    public float deathDelay = 1.0f;
    public float particleDecreaseSpeed = 10.0f;
    public Transform particles;

    private float m_distanceTravelled;
    private bool m_audioPlaying = false;
    private bool m_dead = false;

    private void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    private void Update()
    {
        if (!m_audioPlaying && particles.transform.localScale.y > 0.0f)
        {
            particles.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * particleDecreaseSpeed;
            if (particles.transform.localScale.y < 0.0f)
            {
                particles.transform.localScale = Vector3.zero;
            }
        }
        else if (m_audioPlaying && particles.transform.localScale.y < 1.0f)
        {
            particles.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * particleDecreaseSpeed;
            if (particles.transform.localScale.y > 1.0f)
            {
                particles.transform.localScale = Vector3.one;
            }
        }

        if (pathCreator != null && !m_dead)
        {
            m_distanceTravelled += speed * Time.deltaTime;
            float t = m_distanceTravelled / pathCreator.path.length;
            t = Mathf.Clamp01(t);
            if (t != 1)
            {
                transform.position = pathCreator.path.GetPointAtDistance(m_distanceTravelled, EndOfPathInstruction.Stop);
                transform.rotation = pathCreator.path.GetRotationAtDistance(m_distanceTravelled, EndOfPathInstruction.Stop);
            }
            else
            {
                UnityEngine.UI.Text healthText = FindObjectOfType<UnityEngine.UI.Text>();
                int playerHealth = int.Parse(healthText.text) - attackAmount;
                healthText.text = playerHealth.ToString();
                Destroy(gameObject);
            }
        }
    }

    private void OnPathChanged()
    {
        m_distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0)
        {
            m_dead = true;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSources[0].Stop();
            audioSources[1].Play();
            Destroy(gameObject, deathDelay);
        }
    }

    public void StartAudio()
    {
        if (!m_audioPlaying)
        {
            m_audioPlaying = true;
            GetComponents<AudioSource>()[0].Play();
        }
    }

    public void StopAudio()
    {
        if (m_audioPlaying)
        {
            m_audioPlaying = false;
            GetComponents<AudioSource>()[0].Stop();
        }
    }
}