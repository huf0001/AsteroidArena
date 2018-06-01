 using UnityEngine;
using System.Collections;

public class EnemyScript: MonoBehaviour 
{
	//private Rigidbody body;
	public float velocity = 10f;
    private float spawnPeriod = 0.0f;
    public float spawnTimerLimit;
    private float destructiblePeriod = 0.0f;
    public float destructibleTimeLimit = 3f;
    //public float speedLimit = 30f;
    //public float collisionSpeedThreshold = 10f;
    private bool destructible = false;
    public Color normalColor;
    public Color destructibleColor;
    public float normalIntensity = 10f;
    public float destructibleIntensity = 10f;
    private Light asteroidLight;
    
    public string spawner;
    private SpawnAsteroidScript spawnPoint;
    public Transform headDirection;
    public float shootForce = 30f;

    public GameObject asteroidParticle;
    private ParticleSystem asteroidParticleSystem;

    public GameObject smallerAsteroid = null;
    public GameObject collideParticle;
    private GameObject tempParticles;

    public string controller;
    private GameplayController gameController = null;
    private ArenaAudioScript audioScript = null;

    private TrailRenderer tailRenderer;
    public Material normalMaterial;
    public Material destructibleMaterial;

    public int lives = 100;
	
    // Use this for initialization
	void Awake ()
    {
        //body = this.GetComponent<Rigidbody>();
        gameController = GameObject.Find(controller).GetComponent<GameplayController>();
        audioScript = GameObject.Find(controller).GetComponent<ArenaAudioScript>();

        spawnPoint = GameObject.Find(spawner).GetComponent<SpawnAsteroidScript>();
        spawnPoint.AddAsteroid();

        asteroidLight = this.gameObject.GetComponentInChildren<Light>();
        asteroidLight.color = normalColor;
        asteroidLight.intensity = normalIntensity;

        tailRenderer = this.gameObject.GetComponent<TrailRenderer>();
        tailRenderer.material = normalMaterial;
    }

    public bool Destructible
    {
        get
        {
            return destructible;
        }

        set
        {
            destructible = value;

            if (destructible)
            {
                destructiblePeriod = 0f;
                asteroidLight.color = destructibleColor;
                asteroidLight.intensity = destructibleIntensity;
                tailRenderer.material = destructibleMaterial;
            }
        }
    }

    void Update()
    {
        if (spawnPeriod <= spawnTimerLimit)
        {
            spawnPeriod += Time.deltaTime;
        }

        if (destructiblePeriod <= destructibleTimeLimit)
        {
            destructiblePeriod += Time.deltaTime;
        }
        else
        {
            destructible = false;
            asteroidLight.color = normalColor;
            asteroidLight.intensity = normalIntensity;
            tailRenderer.material = normalMaterial;
        }
        
        if (transform.position.y != 2)
        {
            transform.position = new Vector3 (transform.position.x, 2f, transform.position.z);
        }

        CheckInBounds();
    }

    private void CheckInBounds()
    {
        if ((transform.position.x > 120) || (transform.position.x < -120) || (transform.position.z > 120) || (transform.position.z < -120))
        {
            Die();
        }
    }

	void OnCollisionEnter(Collision col)
	{
        if (spawnPeriod > spawnTimerLimit)
        {
            lives -= 1;

            if ((col.gameObject.tag == "Player") && (col.gameObject.GetComponent<PhysControllerFP>().Immune() == false))
            {
                if (smallerAsteroid != null)
                {
                    //Spawn fragments
                    Split();
                }

                LoudDie();
            }
            else if ((col.gameObject.tag == "Enemy"))
            {
                if ((destructible) || (col.gameObject.GetComponent<EnemyScript>().Destructible))
                {
                    gameController.UpdateProgress();

                    if (smallerAsteroid != null)
                    {
                        //Spawn fragments
                        Split();
                    }

                    LoudDie();
                }
                else
                {
                    //Play break sound
                    audioScript.PlayCollisionSFX("thud");
                }
            }
            else if (collideParticle != null)
            {
                //Play break sound
                audioScript.PlayCollisionSFX("thud");

                tempParticles = (GameObject)Instantiate(collideParticle, transform.position, Quaternion.identity);
                tempParticles.GetComponent<ParticleSystem>().Play();
            }

            if (lives == 0)
            {
                if (smallerAsteroid != null)
                {
                    //Spawn fragments
                    Split();
                }

                LoudDie();
            }
        }
    }

    /*bool VelocityCheck(Collision col)
    {
        bool result = false;

        float s1 = body.velocity.magnitude;
        float s2 = col.rigidbody.velocity.magnitude;

        if ((s1 > speedLimit) || (s2 > speedLimit))
        {
            if ((s1 + s2 <= collisionSpeedThreshold) && ((s1 + s2) * -1 <= collisionSpeedThreshold))
            {
                result = true;
            }
        }

        return result;
    }*/

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

    void LoudDie()
    {
        //Play break sound
        audioScript.PlayCollisionSFX("break");

        Die();
    }

    void Die()
    {
        //Decrement asteroid count
        spawnPoint.DeleteAsteroid();

        //Remove asteroid from attractor
        GetComponent<FauxGravityBody>().Die();

        //Kill asteroid
        Destroy((Object)this.gameObject);
    }
}
