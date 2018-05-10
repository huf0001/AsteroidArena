using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpawnAsteroidScript : MonoBehaviour 
{
    public GameObject controller;
    public Vector3 asteroidOffset;
	public GameObject smallAsteroid;
    public GameObject mediumAsteroid;
    public GameObject largeAsteroid;
	public Transform headDirection;
    //public Vector3 objectRotation;
    public GameObject spawner;
	public float shootForce = 30f;
    public float timerLimit;
    private float period = 0.0f;

    public AudioClip shootSound;
	private AudioSource asteroidSource;
	public GameObject asteroidParticle;
	private ParticleSystem asteroidSystem;

	void Awake()
	{
		asteroidSource = this.gameObject.AddComponent<AudioSource>();
		asteroidSource.loop = false;
		asteroidSource.playOnAwake = false;
        if (shootSound != null)
        {
            asteroidSource.clip = shootSound;
        }

    }

	void Update () 
	{
        if (period > timerLimit)
        {
            //Randomise asteroid size
            int option = Random.Range(1, 4);
            GameObject asteroid = smallAsteroid;

            switch (option)
            {
                case 2:
                    asteroid = mediumAsteroid;
                    break;
                case 3:
                    asteroid = largeAsteroid;
                    break;
            }

            //Reset period
            period = 0;

            //transforms the instantiate position into world space based on the head rotation
            Vector3 origin = headDirection.TransformDirection(asteroidOffset);

            //Randomise rotation of asteroid spawner on the Y axis
            float yRotation = Random.Range(0, 360);
            spawner.gameObject.transform.Rotate(Vector3.up, yRotation);

            //instatiates the asteroidPrefab, sets its position/rotation and stores its rigidbody
            GameObject projectile = (GameObject)Instantiate(asteroid, transform.position + origin + new Vector3(0, 2, 0), new Quaternion(0, 0, 0, 0));
            projectile.GetComponent<EnemyScript>().SetGameplayController(controller);

            //adds force to the object
            if (projectile.GetComponent<Rigidbody>() != null)
            {
                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                Vector3 direction = new Vector3(Random.RandomRange(-2, 2), 0, Random.RandomRange(-2, 2));
                projectileRigidbody.AddForce(direction * shootForce, ForceMode.Impulse);

                if (asteroidParticle != null)
                {
                    asteroidSystem = ((GameObject)Instantiate(asteroidParticle, transform.position + origin + new Vector3(0, 2, 0), headDirection.rotation)).GetComponent<ParticleSystem>();
                    Destroy((GameObject)asteroidSystem.gameObject, asteroidSystem.main.duration);
                }

                if (shootSound != null)
                {
                    asteroidSource.Play();
                }
            }
            else
            {
                Debug.LogError("The gameobject you are trying to use does not have a rigidbody");
            }

            //Randomise location of asteroid spawner before next spawn
            float x = 0;
            float z = 0;
            x = Random.Range(12.5f, 47f);
            z = Random.Range(12.5f, 47f);

            if (Random.Range(0, 2) == 0)
            {
                x = -x;
            }

            if (Random.Range(0, 2) == 0)
            {
                z = -z;
            }

            spawner.transform.position = new Vector3(x, -0.6f, z);
        }

        period += Time.deltaTime;
    }
}
