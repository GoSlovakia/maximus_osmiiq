#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleMagnet : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystemForceField attractor;

    public Vector2 targetPosition;
    public float attractionForce;
    public float colliderRadius;

    public int maxNumberOfParticles;
    public float startSpeed;

    [SerializeField]
    private CircleCollider2D collider;

    // Start is called before the first frame update
    void Awake()
    {
        attractor.transform.position = targetPosition;
        attractor.gravity = attractionForce;
        collider.radius = colliderRadius;

        ParticleSystem.MainModule main = particles.main;
        main.maxParticles = maxNumberOfParticles;
        main.startSpeed = startSpeed;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.rotation = new Vector3(0, Vector3.Angle(attractor.transform.position - transform.position, Vector3.up));
    }

    public void TriggerParticles(int totalParticles)
    {
        particles.Emit(totalParticles);
    }

    private void FixedUpdate()
    {
        attractor.gravity = attractionForce;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (gameObject == Selection.activeGameObject)
        {
            attractor.transform.position = targetPosition;
            collider.radius = colliderRadius;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, targetPosition - (Vector2)transform.position);
        }
    }
#endif
    private void OnValidate()
    {
        attractor.transform.position = targetPosition;
        attractor.gravity = attractionForce;
        collider.radius = colliderRadius;

        ParticleSystem.MainModule main = particles.main;
        main.maxParticles = maxNumberOfParticles;
        main.startSpeed = startSpeed;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.rotation = new Vector3(0, Vector3.Angle(attractor.transform.position - transform.position, Vector3.up));
    }
}
