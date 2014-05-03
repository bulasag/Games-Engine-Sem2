using UnityEngine;
using System.Collections;

namespace BGE
{
    public class Rotate : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.Rotate(0, Time.smoothDeltaTime * 10.0f, 0);
        }
    }
}