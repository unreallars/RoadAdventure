using UnityEngine;

public class Cargo : MonoBehaviour
{
    private Manager manager;

    private void Start()
    {
        manager = GameObject.Find("Canvas").GetComponent<Manager>();
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Boden"))
        {
            TriggerGameOver("Du hast deine Ladung verloren!", "You lost your load!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killzone"))
        {
            TriggerGameOver("Du darfst nicht ins Gebäude fahren!", "You are not allowed to drive into the building!");
        }
    }

    private void TriggerGameOver(string germanMessage, string englishMessage)
    {
        manager.GameOver(PlayerPrefs.GetInt("Language") == 1 ? germanMessage : englishMessage);
    }
}
