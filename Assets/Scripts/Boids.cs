using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxForce;
    
    [SerializeField] float _separationWeight;
    [SerializeField] float _cohesionWeight;
    [SerializeField] float _alignmentWeight;
    [SerializeField] float _viewRadius;
    [SerializeField] float _arriveRadius;
    [SerializeField] float _arriveWeight;
    [SerializeField] float _separationRadius;
    [SerializeField] float _evadeRadius;
    [SerializeField] float _evadeWeight;
    [SerializeField] Transform _evadiusTarget;
    [SerializeField] Npc _npc;

    Vector3 _velocity;
    public bool isEvade;

    public Transform arriveTarget;
 
    void Start()
    {
        GameManager.instance.AddBoids(this);

        Vector3 randomVector = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * _maxSpeed;
        AddForce(randomVector);
    }

    void Update()
    {        
        BehavioursForce(Arrive(arriveTarget.position) * _arriveWeight);

        if (Vector3.Distance(transform.position, _npc.transform.position) <= _evadeRadius)
        {
            ColorBoid(Color.black);
            BehavioursForce(Evade(_evadiusTarget.position) * _evadeWeight);
        }

        AddForce(Alignment() * _alignmentWeight);
        AddForce(Cohesion() * _cohesionWeight);
        AddForce(Separation() * _separationWeight);
  

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
   
        CollisionBounds();
    }

    private void OnDrawGizmos()
    {
        //Rangos de Vision y Separacion
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _separationRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _arriveRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _evadeRadius);

    }

    void BehavioursForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(new Vector3(_velocity.x,0f,_velocity.z)+ force, _maxSpeed);
    }

    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    Vector3 Steering(Vector3 desired)
    {
        return Vector3.ClampMagnitude((desired.normalized * _maxSpeed) - _velocity, _maxForce);
    }

    void CollisionBounds()
    {
        transform.position = GameManager.instance.Bounds(transform.position);
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in GameManager.instance.boids)
        {
            Vector3 distance = item.transform.position - transform.position;
            if (distance.magnitude <= _viewRadius) desired += distance;
        }

        if (desired == Vector3.zero) return desired;

        desired = -desired;
        desired.Normalize();
        desired *= _maxSpeed;

        return Steering(desired);
    }

    Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in GameManager.instance.boids)
        {
            if (item == this) continue;
            if(Vector3.Distance(item.transform.position, transform.position) <= _viewRadius)
            {
                desired += item._velocity;
                count++;
            }
        }

        if (count == 0) return desired;
        desired /= count;

        return Steering(desired);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in GameManager.instance.boids)
        {
            if (item == this) continue;
            if (Vector3.Distance(transform.position, item.transform.position) <= _viewRadius)
            {
                desired += item.transform.position;
                count++;
            }
        }

        if (count == 0) return desired;
        desired /= count;
        desired -= transform.position;

        return Steering(desired);
    }

    Vector3 Arrive(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        float distance = desired.magnitude;
        desired.Normalize();

        if (distance <= _arriveRadius)
            desired *= _maxSpeed * (distance / _arriveRadius);
        else 
            desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);
        
        ColorBoid(Color.yellow);

        return steering;
    }

    Vector3 Evade(Vector3 target)
    {
        Vector3 futurePos = target + _npc.velocity - transform.position;
        Vector3 desired = (futurePos - transform.position);
        desired.Normalize();
        desired *= -1;
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

         return steering;
    }

    public Vector3 Velocity()
    {
        return _velocity;
    }

    public void ColorBoid(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
