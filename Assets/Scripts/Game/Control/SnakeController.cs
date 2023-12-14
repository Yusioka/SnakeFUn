using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snakefun.Game.Control
{
    public class SnakeController : MonoBehaviour
    {
        public float GameTime { get; private set; }
        public int Points { get; private set; }
        public bool IsDead { get; private set; }

        public event Action OnEat;
        public event Action OnDie;

        [SerializeField] private List<Transform> tails;
        [SerializeField] private GameObject firstbonePrefab;
        [SerializeField] private GameObject secondbonePrefab;

        [Range(0f, 4f)]
        [SerializeField] private float bonesDistance;

        [Range(0f, 4f)]
        [SerializeField] private float speed;

        private void Update()
        {
            if (!IsDead)
            {
                GameTime += Time.deltaTime;

                MoveSnake(transform.position + transform.forward * speed);

                float angle = Input.GetAxis("Horizontal");
                transform.Rotate(0, angle, 0);
            }
        }

        private void MoveSnake(Vector3 newPosition)
        {
            var sqrDistance = bonesDistance * bonesDistance;
            var previousPosition = transform.position;

            foreach (var bone in tails)
            {
                if ((bone.position - previousPosition).sqrMagnitude > sqrDistance)
                {
                    var temp = bone.position;
                    bone.position = previousPosition;
                    previousPosition = temp;
                }

                else
                {
                    break;
                }
            }

            transform.position = newPosition;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Food"))
            {
                Destroy(collision.gameObject);

                Transform lastTail = tails[tails.Count - 1];
                GameObject bone;

                if (tails.Count % 2 == 0)
                {
                    bone = Instantiate(secondbonePrefab, lastTail.position, Quaternion.identity);
                }

                else
                {
                    bone = Instantiate(firstbonePrefab, lastTail.position, Quaternion.identity);
                }

                tails.Add(bone.transform);

                if (OnEat != null)
                {
                    OnEat.Invoke();
                    Points += 5;
                }
            }

            else if (collision.gameObject.CompareTag("Block"))
            {
                OnDie?.Invoke();

                StartCoroutine(DeleteAllBones());
                IsDead = true;
            }

            else if (collision.gameObject.CompareTag("Bone"))
            {
                OnDie?.Invoke();

                StartCoroutine(DeleteAllBones());
                IsDead = true;
            }
        }

        private IEnumerator DeleteAllBones()
        {
            tails.Reverse();

            foreach (var bone in tails)
            {
                Destroy(bone.gameObject);
                yield return new WaitForSeconds(0.1f);
            }

            Destroy(gameObject);
        }
    }
}