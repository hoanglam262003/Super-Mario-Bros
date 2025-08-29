using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType 
    { Coin, 
      ExtraLife, 
      MagicMushroom,
      StarPower 
    }

    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case PowerUpType.Coin:
                GameManager.Instance.AddCoin();
                Debug.Log("Coin collected!");
                break;
            case PowerUpType.ExtraLife:
                GameManager.Instance.AddLife();
                Debug.Log("Extra life gained!");
                break;
            case PowerUpType.MagicMushroom:
                player.GetComponent<Player>().Grow();
                Debug.Log("Magic Mushroom collected! Player grows!");
                break;
            case PowerUpType.StarPower:
                player.GetComponent<Player>().StarPower();
                Debug.Log("Star Power collected! Player is invincible!");
                break;
        }
        Destroy(gameObject);
    }
}
