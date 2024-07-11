using UnityEngine;

// Dies if pair is not there. Solution for 
// having object messing with outline of 
// parent

public class DestroyOnPair : MonoBehaviour
{
    [SerializeField] GameObject pair;

    // Update is called once per frame
    void Update()
    {
        if (pair == null)
            Destroy(this.gameObject);
    }
}
