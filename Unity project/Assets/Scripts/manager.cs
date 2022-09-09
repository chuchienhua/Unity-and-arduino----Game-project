using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
  [SerializeField] GameObject[]  balls;
  void Start()
  {
    InvokeRepeating("Spawnballcelling", 1, 5);
  }
  
    // Start is called before the first frame update
  public void Spawnballcelling() 
  {
    int r = Random.Range(0,balls.Length);
    GameObject ball = Instantiate(balls[r], transform);
    ball.transform.position = new Vector3(Random.Range(-4.2f, 4.7f),6f,0f);
  }
  
}
