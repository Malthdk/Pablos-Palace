using UnityEngine;
using System.Collections;
/// <summary
/// Particle generator.
/// 
/// The particle generator simply spawns particles with custom values. 
/// See the Dynamic particle script to know how each particle works..
/// 
/// Visit: www.codeartist.mx for more stuff. Thanks for checking out this example.
/// Credit: Rodrigo Fernandez Diaz
/// Contact: q_layer@hotmail.com
/// </summary>

public class ParticleGenerator : MonoBehaviour {	
	public GameObject prefab;	
	public float spawnInterval = 0.025f; // How much time until the next particle spawns
	float lastSpawnTime = float.MinValue; //The last spawn time
	public float particleLifetime = 3; //How much time will each particle live
	public float particleSize = 2.0f; // The size of the particle
	public bool fixedSize = false; // Should the particle have a fixed size?
	public float spawnDuration = 2; // Duration of spawn
	public Vector3 particleForce; //Is there a initial force particles should have?
	public bool randomXForce = false; // Should X force be random?
	public bool randomYForce = false; // Should X force be random?
	public DynamicParticle.STATES particlesState = DynamicParticle.STATES.BLUE; // The state of the particles spawned
	public Transform particlesParent; // Where will the spawned particles will be parented (To avoid covering the whole inspector with them)
	public bool randomSize = true; // Should it have random size
	public bool spawn; 
	public bool shooter = false; 
	public float shooterCooldown = 1; // Duration of spawn
	float startTime = 0; // Used for setting start time of duration
	float shooterTime = 0; // Used for setting start time of duration for Shooter

	private float tempXForce;
	private float tempYForce;

	void Start() 
	{ 	
		spawn = false;
		tempXForce = particleForce.x;
		tempYForce = particleForce.y;
	}

	public void Update() 
	{	
		/*if (spawn) {
			if (startTime < spawnDuration) {
				if (lastSpawnTime + spawnInterval < Time.time) {
					GameObject newLiquidParticle = (GameObject)Instantiate (Resources.Load ("LiquidPhysics/DynamicParticle")); //Spawn a particle
					if (randomXForce) {
						particleForce.x = Random.Range(tempXForce*-1, tempXForce);
					}
					if (randomYForce) {
						particleForce.y = Random.Range(tempYForce*-1, tempYForce);
					}
					newLiquidParticle.GetComponent<Rigidbody2D> ().AddForce (particleForce); //Add our custom force
					DynamicParticle particleScript = newLiquidParticle.GetComponent<DynamicParticle> (); // Get the particle script

					if (randomSize) {
						int temp = 0;
						if (temp == 0) {
							particleScript.SetSize (particleSize,particleSize);
							if (!fixedSize) {
								particleScript.SetRandomSize (-0.8f,0.8f);
							}
							temp++;
						}
					}
					
					particleScript.SetLifeTime (particleLifetime); //Set each particle lifetime
					particleScript.SetState (particlesState); //Set the particle State
					newLiquidParticle.transform.position = transform.position;// Relocate to the spawner position
					newLiquidParticle.transform.parent = particlesParent;// Add the particle to the parent container
					lastSpawnTime = Time.time; // Register the last spawnTime	
				}
				startTime += Time.deltaTime / spawnDuration;
			} else {
				spawn = false;
			}
			shooterTime = 0.0f;
		} else {
			startTime = 0.0f;
		}*/

		if (spawn) {
			if (startTime < spawnDuration) {
				if (lastSpawnTime + spawnInterval < Time.time) {
					if (randomXForce) {
						particleForce.x = Random.Range(tempXForce*-1, tempXForce);
					}
					if (randomYForce) {
						particleForce.y = Random.Range(tempYForce*-1, tempYForce);
					}
//					PoolManager.instance.ReuseObject (prefab, this.transform.position, Quaternion.identity, particleForce, particleLifetime, particlesState, particleSize); 
					lastSpawnTime = Time.time; // Register the last spawnTime	
				}
				startTime += Time.deltaTime / spawnDuration;
			} else {
				spawn = false;
			}
			shooterTime = 0.0f;
		} else {
			startTime = 0.0f;
		}



		if (shooter) {
			if (shooterTime < shooterCooldown) {
				shooterTime += Time.deltaTime / spawnDuration;
				//Debug.Log (shooterTime);
			} else {
				spawn = true;
			}
		}
	}
}

