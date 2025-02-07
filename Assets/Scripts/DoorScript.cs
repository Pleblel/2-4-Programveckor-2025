using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    public GameObject destinationDoor;
    public CanvasGroup canvasGroup;
    public float exitOffset = 2f;
    public float freezeDuration = 1f;
    public float fadeDuration = 1f;
    private bool fadingIn = false;
    private bool fadingOut = false;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = FindObjectOfType<CanvasGroup>();
        }
    }

    private void Start()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enabled == true)
        {
            StartCoroutine(TransitionPlayer(other.transform));
        }
    }


    private IEnumerator TransitionPlayer(Transform player)
    {
        yield return StartCoroutine(FadeIn());


      
        player.position = destinationDoor.transform.position; 

        // Freeze the game
        Time.timeScale = 0f;

        // Wait for the specified duration (using unscaled time)
        yield return new WaitForSecondsRealtime(freezeDuration);

        // Unfreeze the game
        Time.timeScale = 1f;

        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime * 2f;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime * 2f;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
