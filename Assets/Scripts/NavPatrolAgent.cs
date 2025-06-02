using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavPatrolAgent : MonoBehaviour
{
    public float Speed;
    public List<Vector3> Targets;
    Rigidbody rb;
    
    const float DISTANCETOTURN = 1f;
    public int currentIndex = 0;
    
    int nextIndex //to give the next target
    {
        get
        {
            if (currentIndex == Targets.Count - 1)
            {
                return 0;
            }
            return currentIndex+1;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
                
    }

    private void FixedUpdate()
    {
        Vector3 direction = Targets[nextIndex] - Targets[currentIndex];
        rb.MovePosition(rb.position+(direction.normalized*Speed*Time.fixedDeltaTime));
        //if reaching the target, then go to next and turn around
        if (Vector3.Distance(rb.position, Targets[nextIndex]) < DISTANCETOTURN)
        {
            //turn around
            Vector3 currentEuler = rb.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(currentEuler.x, currentEuler.y+180f, currentEuler.z);
            rb.MoveRotation(targetRotation);
            //rb.rotation = Quaternion.Euler(currentEuler.x, 360f, currentEuler.z);
            //calculate the index
            if (currentIndex == Targets.Count - 1)
            {
                currentIndex = 0;
            }
            else
                currentIndex++;
        }
    }


}
