using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] float knockbackStrength = 10f;
    [SerializeField] float knockbackDuration = 0.5f;


    public void ApplyKnockback(Transform source, Rigidbody target)
    {
        Vector3 direction = (target.position - source.position).normalized;
        Vector3 force = direction * knockbackStrength;


    }

    private IEnumerator ApplyKnockCoroutine(Rigidbody target, Vector3 force)
    {
        float elapsedTime = 0f;

        while(elapsedTime < knockbackDuration)
        {
            target.velocity = force;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.velocity = Vector3.zero;
    }
}
