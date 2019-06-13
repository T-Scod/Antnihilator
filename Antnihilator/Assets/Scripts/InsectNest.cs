using PathCreation;
using UnityEngine;

public class InsectNest : MonoBehaviour
{
    public enum InsectType
    {
        ant,
        rollyPolly,
        beetle,
        random
    }

    [System.Serializable]
    public struct InsectInfo
    {
        readonly InsectType insectType;
        readonly int pathIndex;
    }

    public bool loop = false;
    public bool randomiseAllTypes = false;
    public bool randomiseAllPaths = false;
    public InsectInfo[] insectOrder;
    public PathCreator[] pathCreators;
    public float minSpawnDuration = 2.0f;
    public float maxSpawnDuration = 4.0f;

    private float m_spawnTimer = 0.0f;
    private int m_insectIndex = 0;

    private void Update()
    {
        if (m_spawnTimer <= 0.0f)
        {
            m_spawnTimer = Random.Range(minSpawnDuration, maxSpawnDuration);
            if (randomiseAllPaths && randomiseAllTypes)
            {

            }
        }
    }
}