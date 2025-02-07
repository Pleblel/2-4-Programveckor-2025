using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (collision.collider.CompareTag("Enemy"))
            Destroy(collision.gameObject);
    }
}
