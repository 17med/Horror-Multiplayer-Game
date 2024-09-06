using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookat : MonoBehaviour
{private Quaternion initialRotation;
    public Vector3 ang;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] GB= GameObject.FindGameObjectsWithTag("Player");
        if (GB.Length > 0)
        {
            GameObject min= GB[0];
            for (int i = 1; i < GB.Length; i++)
            {
                if (Vector3.Distance(transform.position, GB[i].transform.position) <
                    Vector3.Distance(transform.position, min.transform.position))
                {
                    min = GB[i];
                }
            }

            Transform target = min.transform;
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Flatten the direction vector to avoid tilting

            // Create a rotation that looks in the direction of the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Extract the Y rotation from the target rotation
            float targetYRotation = targetRotation.eulerAngles.y;

            // Combine the initial rotation with the Y-axis rotation to get the final rotation
            Quaternion finalRotation = Quaternion.Euler(initialRotation.eulerAngles.x, targetYRotation, initialRotation.eulerAngles.z);

            // Apply the final rotation to the object
            transform.rotation = finalRotation;
            transform.Rotate(ang);
        }
    }
}
