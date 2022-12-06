using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager manager;
    private void Awake() {
        if(manager!=null)
            Destroy(gameObject);
        else
            manager = this;
    }
    
    public List<Transform> spawnPoints;
}
