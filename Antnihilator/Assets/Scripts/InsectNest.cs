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
        public InsectType insectType;
        public int pathIndex;
    }

    public bool loop = false;
    public bool randomiseAllTypes = false;
    public bool randomiseAllPaths = false;
    public InsectInfo[] insectOrder;
    public Insect[] insects;
    public PathCreator[] pathCreators;
    public float minSpawnDuration = 2.0f;
    public float maxSpawnDuration = 4.0f;

    private float m_spawnTimer = 0.0f;
    private int m_insectIndex = 0;

    private void Update()
    {
        if (!loop && !randomiseAllPaths && !randomiseAllTypes && m_insectIndex >= insectOrder.Length)
        {
            return;
        }

        m_spawnTimer -= Time.deltaTime;
        if (m_spawnTimer <= 0.0f)
        {
            m_spawnTimer = Random.Range(minSpawnDuration, maxSpawnDuration);
            if (randomiseAllPaths && randomiseAllTypes)
            {
                int insectIndex = Random.Range(0, insects.Length);
                int pathIndex = Random.Range(0, pathCreators.Length);
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
            }
            else if (randomiseAllTypes)
            {
                int insectIndex = Random.Range(0, insects.Length);
                int pathIndex = insectOrder[m_insectIndex].pathIndex;
                if (pathIndex == pathCreators.Length)
                {
                    pathIndex = Random.Range(0, pathCreators.Length);
                }
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                m_insectIndex++;
            }
            else if (randomiseAllPaths)
            {
                int insectIndex = (int)insectOrder[m_insectIndex].insectType;
                if (insectIndex == (int)InsectType.random)
                {
                    insectIndex = Random.Range(0, insects.Length);
                }
                int pathIndex = Random.Range(0, pathCreators.Length);
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                m_insectIndex++;
            }
            else
            {
                int insectIndex = (int)insectOrder[m_insectIndex].insectType;
                if (insectIndex == (int)InsectType.random)
                {
                    insectIndex = Random.Range(0, insects.Length);
                }
                int pathIndex = insectOrder[m_insectIndex].pathIndex;
                if (pathIndex == pathCreators.Length)
                {
                    pathIndex = Random.Range(0, pathCreators.Length);
                }
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                m_insectIndex++;
            }

            if (loop && m_insectIndex >= insectOrder.Length)
            {
                m_insectIndex = 0;
            }
        }
    }
}