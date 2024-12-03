using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myeongjin
{
	public class CFrontWheel : MonoBehaviour
	{
		private const float CARWEIGHT = 2200.0f;
		private const float MAXSPEED = 30.0f;

		private Rigidbody rigidBody;

		private Vector3 direction;
		private Vector3 worldBottomPosition;

		private float radius;
		private float velocity = 0;

		private void Start()
		{
            rigidBody = GetComponent<Rigidbody>();
			radius = GetComponent<CapsuleCollider>().radius * transform.localScale.x;
        }
		private void FixedUpdate()
		{
			RotateWheel();

			Friction();
		}

		private void Update()
		{
			PushAcceleration();
		}
        private void OnCollisionStay(Collision collision)
        {
            SetDirection(collision.contacts[0]);

            Move();
        }
		private void PushAcceleration()
		{

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && velocity < MAXSPEED)
                velocity += Time.fixedDeltaTime * 2;
            else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && velocity > -(MAXSPEED / 2))
                velocity -= Time.fixedDeltaTime * 2;
            else
            {
                if (velocity > 0.01f)
                    velocity -= Time.fixedDeltaTime;
                else if (velocity < -0.01f)
                    velocity += Time.fixedDeltaTime * 1.5f;
                else
                    velocity = 0;
            }
        }
        private void RotateWheel()
		{
			transform.Rotate(new Vector3(0, -velocity, 0));
		}
		private void Move()
		{
		}
		private void Friction()
		{

		}
		private void SetDirection(ContactPoint contact)
		{
			Vector3 collisionVector3;
			Debug.Log("TransformUp : " + transform.position);
			Debug.Log("ContactPoint : " + contact.point);

            worldBottomPosition = transform.position;
			worldBottomPosition.y = -radius;

			collisionVector3 = contact.point - transform.position;
			
        }
    }
}