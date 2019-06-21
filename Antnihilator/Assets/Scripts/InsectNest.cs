using PathCreation;
using UnityEngine;

/// <summary>
/// Spawns insects on predetermined paths.
/// </summary>
public class InsectNest : MonoBehaviour
{
    /// <summary>
    /// Used to identify the type of insect.
    /// </summary>
    public enum InsectType
    {
        ant, // standard enemy
        rollyPolly, // tank enemy
        beetle, // fast enemy
        blindingAnt, // ant with mirror on back
        fireAnt, // exploding ant
        boss, // boss enemy
        random // used to spawn a random enemy type
    }

    /// <summary>
    /// The insect that is going to be spawnned.
    /// </summary>
    [System.Serializable]
    public struct InsectInfo
    {
        /// <summary>
        /// The type of enemy.
        /// </summary>
        [Tooltip("The type of enemy.")]
        public InsectType insectType;
        /// <summary>
        /// The index of the path in the array of paths that will be moved along.
        /// </summary>
        [Tooltip("The index of the path in the array of paths that will be moved along.")]
        public int pathIndex;
    }

    /// <summary>
    /// Determines if the insect order will loop once the end is reached.
    /// </summary>
    [Tooltip("Determines if the insect order will loop once the end is reached.")]
    public bool loop = false;
    /// <summary>
    /// Determines if all insect types should be random.
    /// </summary>
    [Tooltip("Determines if all insect types should be random.")]
    public bool randomiseAllTypes = false;
    /// <summary>
    /// Determines if all insect paths should be random.
    /// </summary>
    [Tooltip("Determines if all insect paths should be random.")]
    public bool randomiseAllPaths = false;
    /// <summary>
    /// Array of the order of insects that will be spawnned.
    /// </summary>
    [Tooltip("Array of the order of insects that will be spawnned.")]
    public InsectInfo[] insectOrder;
    /// <summary>
    /// Collection of the different types of insect prefabs.
    /// </summary>
    [Tooltip("Collection of the different types of insect prefabs. Must be in same order as insect types.")]
    public Insect[] insects;
    /// <summary>
    /// Collection of the different predetermined paths.
    /// </summary>
    [Tooltip("Collection of the different predetermined paths.")]
    public PathCreator[] pathCreators;
    /// <summary>
    /// The minimum duration between enemy spawnning.
    /// </summary>
    [Tooltip("The minimum duration between enemy spawnning.")]
    public float minSpawnDuration = 2.0f;
    /// <summary>
    /// The maximum duration between enemy spawnning.
    /// </summary>
    [Tooltip("The maximum duration between enemy spawnning.")]
    public float maxSpawnDuration = 4.0f;

    /// <summary>
    /// Used to time the time passed since last enemy spawnning.
    /// </summary>
    private float m_spawnTimer = 0.0f;
    /// <summary>
    /// The index of the current insect being spawnned in the insect order array.
    /// </summary>
    private int m_insectIndex = 0;
    /// <summary>
    /// Used to check if the game is paused.
    /// </summary>
    private bool m_isPaused = false;

    /// <summary>
    /// Sets the active status of all insects.
    /// </summary>
    /// <param name="pause">Determines if the game is paused.</param>
    public void SetPause(bool pause)
    {
        m_isPaused = pause;
        Insect[] children = GetComponentsInChildren<Insect>();
        // checks if there are any spawnned insects
        if (children.Length > 0)
        {
            // sets all insects' activeness to the pause status
            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(!m_isPaused);
            }
        }
    }

    /// <summary>
    /// Spawns insects and updates timers.
    /// </summary>
    private void Update()
    {
        // checks if no insects should be spawnned
        if (m_isPaused || (!loop && !randomiseAllPaths && !randomiseAllTypes && m_insectIndex >= insectOrder.Length))
        {
            return;
        }

        // decrements the timer
        m_spawnTimer -= Time.deltaTime;
        // checks if the timer has run out
        if (m_spawnTimer <= 0.0f)
        {
            // gets a new random timer duration between the min and max spawn durations
            m_spawnTimer = Random.Range(minSpawnDuration, maxSpawnDuration);

            // checks if the insect being spawnned should have a random type and path
            if (randomiseAllPaths && randomiseAllTypes)
            {
                // gets a random insect index from the collection of insect prefabs
                int insectIndex = Random.Range(0, insects.Length);
                // gest a random path index from the collection of paths in the scene
                int pathIndex = Random.Range(0, pathCreators.Length);
                // creates a insect at on the path
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                insect.transform.parent = gameObject.transform;
                // checks if the enemy spawnned is a fire ant
                if (insectIndex == (int)InsectType.fireAnt)
                {
                    insect.fireAnt = true;
                }
            }
            // checks if only the insect type is random
            else if (randomiseAllTypes)
            {
                // gets a random insect index from the collection of insect prefabs
                int insectIndex = Random.Range(0, insects.Length);
                // gets the path index from the insect order
                int pathIndex = insectOrder[m_insectIndex].pathIndex;
                // if the index provided is out of range then randomise the path
                if (pathIndex >= pathCreators.Length || pathIndex < 0)
                {
                    pathIndex = Random.Range(0, pathCreators.Length);
                }
                // creates a insect at on the path
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                insect.transform.parent = gameObject.transform;
                // checks if the enemy spawnned is a fire ant
                if (insectIndex == (int)InsectType.fireAnt)
                {
                    insect.fireAnt = true;
                }
                // increments the current index in the insect order
                m_insectIndex++;
            }
            else if (randomiseAllPaths)
            {
                // gets the insect index from the insect order
                int insectIndex = (int)insectOrder[m_insectIndex].insectType;
                // if the index provided is of random type then randomise the insect type
                if (insectIndex == (int)InsectType.random)
                {
                    insectIndex = Random.Range(0, insects.Length);
                }
                int pathIndex = Random.Range(0, pathCreators.Length);
                // creates a insect at on the path
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                insect.transform.parent = gameObject.transform;
                // checks if the enemy spawnned is a fire ant
                if (insectIndex == (int)InsectType.fireAnt)
                {
                    insect.fireAnt = true;
                }
                // increments the current index in the insect order
                m_insectIndex++;
            }
            else
            {
                // gets the insect index from the insect order
                int insectIndex = (int)insectOrder[m_insectIndex].insectType;
                // if the index provided is of random type then randomise the insect type
                if (insectIndex == (int)InsectType.random)
                {
                    insectIndex = Random.Range(0, insects.Length);
                }
                // gets the path index from the insect order
                int pathIndex = insectOrder[m_insectIndex].pathIndex;
                // if the index provided is out of range then randomise the path
                if (pathIndex == pathCreators.Length)
                {
                    pathIndex = Random.Range(0, pathCreators.Length);
                }
                // creates a insect at on the path
                Insect insect = Instantiate(insects[insectIndex], pathCreators[pathIndex].path.vertices[0], Quaternion.identity);
                insect.pathCreator = pathCreators[pathIndex];
                insect.transform.parent = gameObject.transform;
                // checks if the enemy spawnned is a fire ant
                if (insectIndex == (int)InsectType.fireAnt)
                {
                    insect.fireAnt = true;
                }
                // increments the current index in the insect order
                m_insectIndex++;
            }

            // if the order should loop and the index is out of range the wrap the index
            if (loop && m_insectIndex >= insectOrder.Length)
            {
                m_insectIndex = 0;
            }
        }
    }
}