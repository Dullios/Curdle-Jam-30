using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedOverride : MonoBehaviour
{
    public Animator animator;
    public float time;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (time == 0) 
        { 
            time = Random.Range(0f, 1f); 
        }

        StartCoroutine(starting());
    }

    IEnumerator starting()
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("start", true);
    }
}
