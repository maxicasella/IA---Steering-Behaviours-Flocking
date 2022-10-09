using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] GameObject _food;

    bool _spawn = true;

    private void Start()
    {
        foreach (var b in GameManager.instance.boids)
        {
            b.arriveTarget = this.transform;
        }
    }

    private void Update()
    {
        foreach (var b in GameManager.instance.boids)
        {
            Vector3 distance = b.transform.position - transform.position;
            if (distance.magnitude <= 0.5f) FoodSpawn(_food);
        }
    }

    public void FoodSpawn(GameObject food)
    {
        Vector3 randomVector = new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(- 8f, 8f));
        if (_spawn)
        {
            _spawn = false;
            Instantiate(food, randomVector, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
