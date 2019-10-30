using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    public float attackForceMod;
    private Vector3 attackForceDir; //attack force direction
    private Rigidbody2D rb;
    private static List<int> numbers; //keeps track of which spawns have been used

    private void Awake()
    {
        numbers = new List<int>();
        for (int i = 0; i < 4; i++)
            numbers.Add(i);
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        int randNumber = EnemySpawner.RandNumber();
        StartCoroutine(Init(EnemySpawner.spawnPositions[randNumber], (Direction)randNumber));
    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0.0f;
    }
    

	private IEnumerator Init(Vector3 spawnPosition, Direction attackDir)
    {
        transform.parent.position = spawnPosition;

        attackForceDir = (PlayerController.instance.transform.position - transform.parent.position).normalized;
        rb.AddForce(attackForceDir * attackForceMod);
        yield return null;

    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    private int RandNumber()
    {
        if (numbers == null)
        {
            numbers = new List<int>();
        }
            
        int result = numbers[Random.Range(0, numbers.Count)];
        numbers.Remove(result);
        return result;
    }
}
