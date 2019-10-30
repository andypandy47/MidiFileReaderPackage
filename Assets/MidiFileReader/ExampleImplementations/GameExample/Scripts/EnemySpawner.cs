using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiFileReader;

public class EnemySpawner : MonoBehaviour
{

    public static Vector3[] spawnPositions;
    private static List<int> numbers;
    private List<GameObject> enemyPool;
    public GameObject enemyPrefab;
    public int poolSize;

    private void Start()
    {
        //Creat and set spawn positions
        spawnPositions = new Vector3[4];
        Vector3 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        spawnPositions[0] = new Vector3(-screenDimensions.x, screenDimensions.y, 0); //top left
        spawnPositions[1] = new Vector3(screenDimensions.x, screenDimensions.y, 0); //top right
        spawnPositions[2] = new Vector3(-screenDimensions.x, -screenDimensions.y, 0); //bottom left
        spawnPositions[3] = new Vector3(screenDimensions.x, -screenDimensions.y, 0); //botton right

        //create a list of numbers that can provide an integer when needed for random.
        //this integer will then be removed from the list to ensure no random duplicates
        numbers = new List<int>();
        FillNumbers();

        //Create enemypool
        enemyPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            enemyPool.Add(obj);
            obj.SetActive(false);
        }
    }

    public void SpawnEnemyAtRandomSpawnLocation()
    {
        
        GameObject enemyToSpawn;
        for(int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                enemyToSpawn = enemyPool[i];
                enemyToSpawn.SetActive(true);
                break;
            }
            else
                continue;
        }
    }

    public void DepsawnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (spawnPositions != null)
        {
            for(int i = 0; i < spawnPositions.Length; i++)
            {
                if (spawnPositions[i] == null)
                    break;

                Gizmos.DrawCube(spawnPositions[i], Vector3.one / 3);
            }
        }
        
    }*/

    public void OnNoteEvent(NoteEventData data)
    {
        SpawnEnemyAtRandomSpawnLocation();
    }

    public static int RandNumber()
    {
        if (numbers.Count == 0)
        {
            numbers = new List<int>();
            FillNumbers();
        }

        int result = numbers[Random.Range(0, numbers.Count)];
        numbers.Remove(result);
        return result;
    }

    private static void FillNumbers()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
            numbers.Add(i);
    }

    public void OnNoteOnEvent(NoteEventData noteData)
    {
        SpawnEnemyAtRandomSpawnLocation();
    }

    public void OnNoteOffEvent(NoteEventData noteData)
    {
        
    }
}
