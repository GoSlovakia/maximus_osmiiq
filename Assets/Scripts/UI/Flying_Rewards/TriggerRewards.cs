using UnityEngine;

public class TriggerRewards : MonoBehaviour
{
    public ParticleMagnet magnet;
    public Contador cont1;
    private ParticleSystem ps;
    [SerializeField] private ParticleSystemForceField ff;
    public int numParticles;
    [SerializeField] private Sprite currentSprite;
    //Speed
    private float maxSpeed;
    [SerializeField]private float speedIncr = 1f;
    
    private bool emitp = false;

    private void Start()
    {
        ps = magnet.particles;
        maxSpeed = magnet.attractionForce;
    }

    private void Update()
    {
        if(emitp && magnet.attractionForce < maxSpeed)
        { 
            float speedRate = Time.deltaTime * speedIncr;
            magnet.attractionForce += speedRate; 
        }
    }

    public void TriggerParticle()
    {
        Debug.Log("Triggering " + numParticles + " particles");
        magnet.particles.textureSheetAnimation.SetSprite(0,currentSprite);
        var emission = ps.emission;
        if (30 <= numParticles)
        {
            numParticles = 30;
        }
        emission.rateOverTime = numParticles;
        DoEmit();
        Invoke("sS", 1f);
        Invoke("StopEmit", 1.1f);
    }
    void DoEmit()
    {
        emitp = false;
        magnet.attractionForce = 0f;
        magnet.particles.Play();
    }

    void StopEmit()
    {
        magnet.particles.Stop();
    }

    void sS()
    {
        emitp = true;
    }
}
