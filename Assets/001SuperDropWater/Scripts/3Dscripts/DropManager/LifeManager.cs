using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public GameObject player;
    public Renderer playerRenderer;
    public float maxLife;
    public float currentLife;
    public float respawnTime;
    public float invincibilityTime;
    public float flashTime = 0.1f;
    private float flashCounter;
    private float invincibilityCounter;
    private Vector3 respawnPoint;
    private CharacterController controller;
    private bool isRespawning;

    public Image HealthBar;

    void Start()
    {
        respawnPoint = player.transform.position;
        controller = player.GetComponent<CharacterController>();
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = currentLife / maxLife;

        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            if(flashCounter <= 0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashTime;
            }
        }else if (invincibilityCounter <= 0)
        {
            playerRenderer.enabled = true;
        }
    }

    public void damagePlayer(int damage, Vector3 direction)
    {
        if (invincibilityCounter <= 0)
        {
            currentLife -= damage;
            if (currentLife <= 0)
            {
                player.transform.position = respawnPoint;
                Respawn();
            }
            else
            {
                player.GetComponent<DropController>().Knockback(direction);
                invincibilityCounter = invincibilityTime;
                playerRenderer.enabled = false;
                flashCounter = flashTime;
            }
        }
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo()
    {
        isRespawning = true;
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        isRespawning = false;
        player.gameObject.SetActive(true);
       // player.transform.position = respawnPoint;
        currentLife = maxLife;
        //invincibilityCounter = invincibilityTime;
        playerRenderer.enabled = false;
        //flashCounter = flashTime;
    }

     public void SetSpawnPoint(Vector3 newPosition)
     {
         respawnPoint = newPosition;
     }

    /* public void HealPlayer(int healAmount)
     {
         currentHealth += healAmount;
         if (currentHealth > maxHealth)
         {
             currentHealth = maxHealth;
         }
     }*/


}
