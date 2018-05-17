 using UnityEngine;
using System.Collections;

public class EnemyScript: MonoBehaviour 
{
	private Rigidbody body;
	public float velocity = 10f;
    private float period = 0.0f;
    public float timerLimit;
    public float speedLimit = 30f;
    public float collisionSpeedThreshold = 10f;

    public string controller;
    public Transform headDirection;
    public float shootForce = 30f;
    public AudioClip shootSound;
    private AudioSource asteroidSource;
    public GameObject asteroidParticle;
    private ParticleSystem asteroidParticleSystem;

    public GameObject smallerAsteroid = null;
    public GameObject collideParticle;
    private GameObject tempParticles;

    //public GameObject controller;
	private GameplayController gameController = null;
	
    // Use this for initialization
	void Awake ()
    {
        body = this.GetComponent<Rigidbody>();
        gameController = GameObject.Find(controller).GetComponent<GameplayController>();
    }

    void Update()
    {
        if (period <= timerLimit)
        {
            period += Time.deltaTime;
        }
        
        if (transform.position.y != 2)
        {
            transform.position = new Vector3 (transform.position.x, 2, transform.position.z);
        }
    }

	void OnCollisionEnter(Collision col)
	{
        if (period > timerLimit)
        {
            if ((col.gameObject.tag == "Player") && (col.gameObject.GetComponent<PhysControllerFP>().Immune() == false))
            {
                if (smallerAsteroid != null)
                {
                    //Spawn fragments
                    Split();
                }

                //Kill asteroid
                Destroy((Object)this.gameObject);
            }
            else if ((col.gameObject.tag == "Enemy"))
            {
                if (VelocityCheck(col)) //if speeds are high enough
                {
                    if (smallerAsteroid != null)
                    {
                        //Spawn fragments
                        Split();
                    }

                    //Kill asteroid
                    Destroy((Object)this.gameObject);
                }
            }
            else if (collideParticle != null)
            {
                tempParticles = (GameObject)Instantiate(collideParticle, transform.position, Quaternion.identity);
                tempParticles.GetComponent<ParticleSystem>().Play();
            }
        }
        
    }

    bool VelocityCheck(Collision col)
    {
        bool result = false;

        float s1 = body.velocity.magnitude;
        float s2 = col.rigidbody.velocity.magnitude;

        if ((s1 > speedLimit) || (s2 > speedLimit))
        {
            if (s1 + s2 < collisionSpeedThreshold)
            {
                result = true;
            }
        }

        return result;
    }

    void Split()
    {
        //Asteroid spawning adjustments
        Vector3 spawnAdjustment1 = new Vector3(3, 2, 3);
        Vector3 spawnAdjustment2 = new Vector3(-3, 2, -3);
        Vector3 forceDirection = ForceDirection();

        //Spawn two smaller asteroids
        SpawnFragment(spawnAdjustment1, forceDirection);
        SpawnFragment(spawnAdjustment2, -1 * forceDirection);
    }

    Vector3 ForceDirection()
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                return transform.forward;
            case 2:
                return transform.right;
            case 3:
                return transform.forward + (transform.right * -1);
            case 4:
                return (transform.forward * -1) + transform.right;
        }

        return transform.right;
    }

    void SpawnFragment(Vector3 adjustment, Vector3 direction)
    {
        //instatiates the asteroidPrefab, sets its position/rotation and stores its rigidbody
        GameObject projectile = (GameObject)Instantiate(smallerAsteroid, transform.position + adjustment, new Quaternion(0, 0, 0, 0));
        
        if (projectile.GetComponent<Rigidbody>() != null)
        {
            //Get rigidbody
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

            //apply force to the rigidbody at the point specified direction, and in the ammount specified by shootForce
            projectileRB.AddForce(direction * shootForce, ForceMode.Impulse);

            if (asteroidParticle != null)
            {
                asteroidParticleSystem = ((GameObject)Instantiate(asteroidParticle, transform.position + adjustment, headDirection.rotation)).GetComponent<ParticleSystem>();
                Destroy((GameObject)asteroidParticleSystem.gameObject, asteroidParticleSystem.main.duration);   //??
            }

            adjustment.x = adjustment.x * -1;
            adjustment.z = adjustment.z * -1;
        }
        else
        {
            Debug.LogError("The gameobject you are trying to use does not have a rigidbody");
        }
    }

}
