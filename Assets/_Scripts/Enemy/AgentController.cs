using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _scripts
{
	public class AgentController : MonoBehaviour
	{
		public readonly float distance;
		public float speed;
		public float absDistanceToGo;
		public float spawnChance;
		public Vector2 direction; //move direction for enemy
		private Rigidbody2D rb;
		private Vector2 spawnpoint;
		private Animator animator;

		// Use this for initialization
		void Start()
		{
			float random = Random.Range(0f, 1f);
			if (random > spawnChance)
			{
				Destroy(this.gameObject);
				return;
			}
			
			direction = randomDirection();
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();
			
			rb.velocity = direction * speed;
			spawnpoint = transform.position;
			
			//freze the position in which dimesnion the agent isnt moving
			if (direction.y == 0)
				rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;	
			
			if (direction.x == 0)
				rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;	
			
			UpdateAnimation();
		}
		
		//get a random normalized Vector for the enemy's moving direction where x and y are distinct
		private Vector2 randomDirection() {
			var sign = Mathf.Sign(Random.Range(-2, 2));
			var axis= Random.Range(0, 2); //0 = x axis, 1 = y axis

			return axis == 0 ? new Vector2(sign, 0) : new Vector2(0, sign); 
//			
//			var randX = Random.Range(-1, 2);
//			
//			if (randX != 0)
//				return new Vector2(randX, 0);
//			
//			var signX = Random.Range(0, 2);
//			return signX == 0 ? new Vector2(randX, -1) : new Vector2(randX, 1);
		}

		//inverst the direction only if the player isnt moving away from its spawnoint 
		private void InvertDirectionToSpawnpoint()
		{
			if (direction.x != 0)
			{
				if ((spawnpoint.x - transform.position.x) / direction.x < 0)
				{
					//moves away from spawnpoint -> invert direction
					direction = direction * -1;
					rb.AddForce(Vector2.zero);
					rb.velocity = direction * speed * 2;
					UpdateAnimation();
				}
				else
				{
					//Todo fehler ausgeben
					//print(direction.x + " X direction");
				}
			}
			else if (direction.y != 0)
			{
				if ((spawnpoint.y - transform.position.y) / direction.y < 0)
				{
					//moves away from spawnpoint -> invert direction
					direction = direction * -1;
					rb.AddForce(Vector2.zero);
					rb.velocity = direction * speed * 2;
					UpdateAnimation();
				}
				else
				{
					//print(direction.y + " Y direction");
				}
			}
			
		}
		private void UpdateAnimation()
		{
			animator.SetBool("right", false);
			animator.SetBool("left", false);
			animator.SetBool("backward", false);
			animator.SetBool("forward", false);
			
			if (direction.x > 0)
			{
				animator.SetBool("right", true);
			} else if (direction.x < 0)
			{
				animator.SetBool("left", true);
			} else if (direction.y > 0)
			{
				animator.SetBool("forward", true);
			} else if (direction.y < 0)
			{
				animator.SetBool("backward", true);
			}
		}

		private void InvertDirectionOnCollision()
		{
			direction *= -1;
			rb.velocity = Vector2.zero;
			rb.velocity = direction * speed;
			UpdateAnimation();
		}

		//fixed update für collision
		private void FixedUpdate()
		{
			rb.velocity = direction * speed;
			//prevent the the agent from moving in two directions on collision since freezePosition apparently isnt working
			//correctly
			if (direction.y == 0)
			{
				transform.position = new Vector3(transform.position.x, spawnpoint.y, 0);
			}
			else
			{
				transform.position = new Vector3(spawnpoint.x, transform.position.y, 0);
			}
			var vector2dtemp = new Vector2(transform.position.x, transform.position.y);
			if ((vector2dtemp - spawnpoint).magnitude >= absDistanceToGo)
			{
				InvertDirectionToSpawnpoint();	
			}
		}

		//collision handling
		private void OnCollisionEnter2D(Collision2D other)
		{
			//the view field should not collide the agent
			if (other.gameObject.CompareTag("ViewField"))
				return;
			if (CollisionIsInWay(other))
			{
				InvertDirectionOnCollision();
			}
		}

		private bool CollisionIsInWay(Collision2D other)
		{
			//just checks wether the colliding object is directly in front of the agent
			return Mathf.Abs(Mathf.Acos(Vector2.Dot(other.transform.position - this.transform.position, direction) / ((other.transform.position - this.transform.position).magnitude * direction.magnitude)) * Mathf.Rad2Deg) <= 45f;
		}
	}
}
