using UnityEngine;

public class Finish : MonoBehaviour
{
    private float timer;
    private bool inRadius = false;
    private Manager manager;

    private void Start()
    {
        manager = GameObject.Find("Canvas").GetComponent<Manager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inRadius && other.CompareTag("Spieler"))
        {
            inRadius = true;
            timer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (inRadius && other.CompareTag("Spieler"))
        {
            inRadius = false;
            timer = 0f;
        }
    }

    private void Update()
    {
        if (!inRadius)
            return;

        timer += Time.deltaTime;

        if (timer >= 2f)
        {
            manager.Finish();
            inRadius = false;
            timer = 0f;
        }
    }
}
