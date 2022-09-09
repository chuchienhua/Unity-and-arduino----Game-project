using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <-5f)
        {
            Destroy(gameObject);
            transform.parent.GetComponent<cell_manager>().Spawncell();
        }
    }
}
