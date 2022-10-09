using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Boids> boids = new List<Boids>();

    [SerializeField] float _boundHeight;
    [SerializeField] float _boundWidth;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddBoids(Boids boid)
    {
        if (boids.Contains(boid)) return;
        else boids.Add(boid);
    }

    public Vector3 Bounds(Vector3 objectPosition)
    {
        if (objectPosition.x > _boundWidth / 2) objectPosition.x = -_boundWidth / 2;
        if (objectPosition.x < -_boundWidth / 2) objectPosition.x = _boundWidth / 2;
        if (objectPosition.z < -_boundHeight / 2) objectPosition.z = _boundHeight / 2;
        if (objectPosition.z > _boundHeight / 2) objectPosition.z = -_boundHeight / 2;

        return objectPosition;
    }
}
