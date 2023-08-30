using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeShop : MonoBehaviour
{
    public GameObject character;
    public GameObject cannon;
    public GameObject storage;
    public TextMeshProUGUI moveSpeedDiplay;
    public TextMeshProUGUI miningSpeedDisplay;
    public TextMeshProUGUI carryWeightDisplay;
    public TextMeshProUGUI cannonDamageDisplay;
    public TextMeshProUGUI cannonRotSpeedDisplay;
    public TextMeshProUGUI cannonFireRateDisplay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveSpeedDiplay.text = "Move Speed: " + character.GetComponent<CharacterMovement>().speed;
        miningSpeedDisplay.text = "Mining Speed: " + character.GetComponent<CharacterMovement>().Damage;
        carryWeightDisplay.text = "Carry Weight: " + Mathf.Ceil((character.GetComponent<Rigidbody2D>().mass-1)/0.3f+1f);
        cannonDamageDisplay.text = "Cannon Damage: " + cannon.GetComponent<CannonController>().BulletDamage;
        cannonRotSpeedDisplay.text = "Cannon Rotation Speed: " + cannon.GetComponent<CannonController>().rotationSpeed;
        cannonFireRateDisplay.text = "Cannon Fire Rate: " + Mathf.Round(100 / cannon.GetComponent<CannonController>().attackCooldown)/100;

    }
    public void upgradeMoveSpeed()
    {
        if(storage.GetComponent<Storage>().iron>=1)
        {
            character.GetComponent<CharacterMovement>().speed+=0.5f;
            character.GetComponent<OreAttachment>().oreWeight += 0.4f;
            character.GetComponent<OreAttachment>().spring += 0.1f;
            storage.GetComponent<Storage>().iron -= 1;
        }
    }
    public void upgradeMiningSpeed()
    {
        if (storage.GetComponent<Storage>().iron >= 2)
        {
            character.GetComponent<CharacterMovement>().Damage++;
            storage.GetComponent<Storage>().iron -= 2;
        }
    }
    public void upgradeCarryWeight()
    {
        if (storage.GetComponent<Storage>().iron >= 3)
        {
            character.GetComponent<Rigidbody2D>().mass += 0.3f;
            character.GetComponent<CharacterMovement>().pushbackForce++;
            storage.GetComponent<Storage>().iron -= 3;
        }
    }

    public void upgradeCannonDamage()
    {
        if (storage.GetComponent<Storage>().copper >= 2)
        {
            cannon.GetComponent<CannonController>().BulletDamage += 10;
            storage.GetComponent<Storage>().copper -= 2;
        }
 
    }

    public void upgradeCannonRotSpeed()
    {
        if (storage.GetComponent<Storage>().copper >= 1)
        {
            cannon.GetComponent<CannonController>().rotationSpeed += 20;
            storage.GetComponent<Storage>().copper -= 1;
        }
    }

    public void upgradeCannonFireRate()
    {
        if (storage.GetComponent<Storage>().copper >= 3)
        {
            cannon.GetComponent<CannonController>().attackCooldown *= 0.8f;
            storage.GetComponent<Storage>().copper -= 3;
        }
    }

}
