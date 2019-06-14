using UnityEngine;
using PathCreation;

public class Insect : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5.0f;
    public int health = 3;
    public int attackAmount = 1;

    private float m_distanceTravelled;
    private bool m_isPaused = false;

    private void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    private void Update()
    {
        if (pathCreator != null && !m_isPaused)
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

    public void SetPause(bool pause)
    {
        m_isPaused = pause;
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}