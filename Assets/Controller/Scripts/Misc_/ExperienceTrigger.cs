using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceTrigger : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var config = PlayerConfigManager.Instance.Config;
        if (collision.gameObject.tag == "Player")
        {
            config.addRunExperience(100);
            Destroy(this.gameObject);
        }
    }
}
