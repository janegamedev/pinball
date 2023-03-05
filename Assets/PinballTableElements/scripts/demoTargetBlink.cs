using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoTargetBlink : MonoBehaviour
{
    // Start is called before the first frame update

    public MeshFilter target_mesh;
    public Mesh[] target_mesh_states;
    public float blinkspeed = 0.5f;
    public float max_cycle = 4;
    
    void Start()
    {
        StartCoroutine(blinkTarget());
    }

    
    int bid = 0;
    IEnumerator blinkTarget(){
          while(true) 
         { 
            yield return new WaitForSeconds(blinkspeed);
            target_mesh.mesh = target_mesh_states[bid];
            bid += 1;
            if(bid >= max_cycle)
            {
                bid = 0;
            }
        }
    }
}
