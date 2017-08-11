using UnityEngine;
using _scripts;

namespace _Scripts
{
    public class AudioTest : MonoBehaviour
    {
        public AudioClip agent;
        public AudioSource As;

        void Awake()
        {
          //  As = GetComponent<AudioSource>();
        }
        void Update()
        {
            if (Input.GetKey("g"))
            {
                // Debug.Log(As.clip);
                As.volume = 1.0f;
                As.PlayOneShot(agent, 1.0f);
            }
        }
    }
}