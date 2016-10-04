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

public class ParticleGeneratorStill : MonoBehaviour {	

	public GameObject prefab;

	public float particleSize = 4.0f; // The size of the particle
	public int particleNumber = 60; 
	public float particleLifetime = 1000; //How much time will each particle live
	public float xMinSize = -10.0f, xMaxSize = 10.0f;
	public DynamicParticle.STATES particlesState = DynamicParticle.STATES.BLUE; // The state of the particles spawned
	public Transform particlesParent; // Where will the spawned particles will be parented (To avoid covering the whole inspector with them)
	public bool spawn; 
	float startTime = 0; // Used for setting start time of duration

	GameObject[] newLiquidParticle;
	DynamicParticle[] particleScript;
	
	void Start() 
	{ 	
		newLiquidParticle = new GameObject[particleNumber];
		particleScript = new DynamicParticle[particleNumber];
		//PoolManager.instance.CreatePool (prefab, poolSize);
	}
	
	public void Update() 
	{	
		/*if (spawn) {
			for (int i = 0; i < particleNumber; i++) {
				float rnd = Random.Range (xMinSize, xMaxSize);
				Vector2 force = new Vector2 (0, 0);
				force.x = rnd;
				//newLiquidParticle[i] = (GameObject)Instantiate (Resources.Load ("LiquidPhysics/DynamicParticle")); //Spawn a particle
				PoolManager.instance.ReuseObject (prefab, this.transform.position, Quaternion.identity); 
				newLiquidParticle[i].GetComponent<Rigidbody2D> ().AddForce (force); //Add our custom force
				particleScript[i] = newLiquidParticle[i].GetComponent<DynamicParticle> (); // Get the particle script
				particleScript[i].SetRandomSize (-3.0f,3.0f);
				particleScript[i].SetLifeTime (particleLifetime); //Set each particle lifetime
			
				particleScript[i].SetState (particlesState); //Set the particle State
				newLiquidParticle[i].transform.position = transform.position;// Relocate to the spawner position
				newLiquidParticle[i].transform.parent = particlesParent;// Add the particle to the parent container	
		
			}
			spawn = false;
			Destroy (gameObject);
		}*/

		if (spawn) {
			for (int i = 0; i < particleNumber; i++) {
				Vector2 force = new Vector2 (Random.Range (xMinSize, xMaxSize), 0);
				PoolManager.instance.ReuseObject (prefab, this.transform.position, Quaternion.identity, force, particleLifetime, particlesState, particleSize); 
			}
			spawn = false;
			Destroy (gameObject);
		}
	}
		
}
