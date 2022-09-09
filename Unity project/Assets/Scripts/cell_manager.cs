using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell_manager : MonoBehaviour
{
  [SerializeField] GameObject[] cellingss;
    // Start is called before the first frame update
  public void Spawncell() 
  {
    int w = Random.Range(0,cellingss.Length);
    GameObject celling = Instantiate(cellingss[w], transform);
    celling.transform.position = new Vector3(Random.Range(-3.43f, 4.15f),5.5f,0f);
  }
}
