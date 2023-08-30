using System.Collections.Generic;
using UnityEngine;

public class OreAttachment : MonoBehaviour
{
    // The player's rigidbody
    public Rigidbody2D playerRb;

    // The delay between each ore attachment
    public float delay = 0.5f;

    // The maximum distance between the player and the ore
    public float maxDistance = 5f;

    public float lineLenght = 2f;

    // The timer for the delay
    private float timer;

    // The flag for holding space
    private bool holdingSpace;

    // The flag for holding E
    private bool holdingE;

    // The list of attached ores
    private List<GameObject> attachedOres;

    // The spring strength for the joint
    public float spring = 10f;

    // The damper for the joint
    public float damper = 1f;

    public float oreWeight=1f;

    void Start()
    {
        // Initialize the timer
        timer = delay;

        // Initialize the list of attached ores
        attachedOres = new List<GameObject>();
    }

    void Update()
    {
        // Check if the player is pressing space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            holdingSpace = true;
        }

        // Check if the player is releasing space
        if (Input.GetKeyUp(KeyCode.Space))
        {
            holdingSpace = false;
        }

        // Check if the player is pressing E
        if (Input.GetKeyDown(KeyCode.E))
        {
            holdingE = true;
        }

        // Check if the player is releasing E
        if (Input.GetKeyUp(KeyCode.E))
        {
            holdingE = false;
        }
    }

    void FixedUpdate()
    {
        // If the player is holding space and the timer is up
        if (holdingSpace && timer >= delay)
        {
            // Reset the timer
            timer = 0f;

            // Find the closest ore that is not yet attached
            GameObject closestOre = FindClosestOre();

            // If there is a closest ore
            if (closestOre != null)
            {
                // Attach it to the player with a spring joint 2d
                AttachOre(closestOre);
            }
        }

        // If the player is holding E and the timer is up and there are attached ores
        if (holdingE && timer >= delay && attachedOres.Count > 0)
        {
            // Reset the timer
            timer = 0f;

            // Find the farthest ore that is attached
            GameObject farthestOre = FindFarthestOre();

            // If there is a farthest ore
            if (farthestOre != null)
            {
                // Detach it from the player and remove it from the list of attached ores
                DetachOre(farthestOre);
                attachedOres.Remove(farthestOre);
            }
        }

        // Update the timer
        timer += Time.fixedDeltaTime;

        // Update the line renderers for each attached ore
        UpdateLines();
    }

    // Find the closest ore that is not yet attached
    GameObject FindClosestOre()
    {
        // Initialize the closest ore and distance
        GameObject closestOre = null;
        float closestDistance = Mathf.Infinity;

        // Find all the ores in the scene with the tag OreBlock
        GameObject[] ores = GameObject.FindGameObjectsWithTag("OreBlock");

        // Loop through all the ores
        foreach (GameObject ore in ores)
        {
            // Get the spring joint 2d component of the ore
            SpringJoint2D sj = ore.GetComponent<SpringJoint2D>();

            // If the spring joint 2d is disabled, meaning the ore is not attached yet
            if (!sj.enabled)
            {
                // Get the distance between the player and the ore
                float distance = Vector2.Distance(playerRb.position, ore.transform.position);

                // If the distance is less than the maximum distance and less than the current closest distance
                if (distance < maxDistance && distance < closestDistance)
                {
                    // Update the closest ore and distance
                    closestOre = ore;
                    closestDistance = distance;
                }
            }
        }

        // Return the closest ore or null if none found
        return closestOre;
    }

    // Find the farthest ore that is attached
    GameObject FindFarthestOre()
    {
        // Initialize the farthest ore and distance
        GameObject farthestOre = null;
        float farthestDistance = 0f;

        // Loop through all the attached ores
        foreach (GameObject ore in attachedOres)
        {
            if(ore != null)
            {
                // Get the distance between the player and the ore
                float distance = Vector2.Distance(playerRb.position, ore.transform.position);

                // If the distance is greater than the current farthest distance
                if (distance > farthestDistance)
                {
                    // Update the farthest ore and distance
                    farthestOre = ore;
                    farthestDistance = distance;
                }
            }

        }

        // Return the farthest ore or null if none found
        return farthestOre;
    }

    // Attach an ore to the player with a spring joint 2d
    void AttachOre(GameObject ore)
    {
        // Get the spring joint 2d component of the ore
        SpringJoint2D sj = ore.GetComponent<SpringJoint2D>();
        if (ore.GetComponent<oreName>().OreName!="Victorium")
        {
            ore.GetComponent<Rigidbody2D>().mass = oreWeight;
        }
        

        // Set the connected rigidbody to be the player's rigidbody
        sj.connectedBody = playerRb;

        // Set the auto configure distance to false and set the distance manually based on current distance
        sj.autoConfigureDistance = false;
        sj.distance = lineLenght;

        // Set the spring strength and damper values
        sj.frequency = spring;
        sj.dampingRatio = damper;

        // Enable the spring joint 2d component
        sj.enabled = true;

        // Draw a line between the player and the ore using a line renderer component on the ore
        DrawLine(ore);

        // Add the ore to the list of attached ores
        attachedOres.Add(ore);
    }

    // Detach an ore from the player and remove its line renderer component
    public void DetachOre(GameObject ore)
    {
        // Get the spring joint 2d component of the ore
        SpringJoint2D sj = ore.GetComponent<SpringJoint2D>();

        // Disable the spring joint 2d component
        sj.enabled = false;

        // Disable the line renderer component from the ore
        ore.GetComponent<LineRenderer>().enabled = false;
    }

    // Draw a line between an ore and its connected body using a line renderer component on the ore
    void DrawLine(GameObject ore)
    {
        // Get or add a line renderer component on the ore
        LineRenderer lr = ore.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = ore.AddComponent<LineRenderer>();
        }
        else
        {
            lr.enabled = true;
        }

        // Set the line width and color
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;

        // Set the line position count to 2
        lr.positionCount = 2;

        // Set the line positions to be the ore and its connected body
        lr.SetPosition(0, ore.transform.position);
        lr.SetPosition(1, ore.GetComponent<SpringJoint2D>().connectedBody.position);
    }

    // Update the line renderers for each attached ore
    void UpdateLines()
    {
        // Loop through all the attached ores
        foreach (GameObject ore in attachedOres)
        {
            // Get or add a line renderer component on the ore
            if (ore != null)
            {


                LineRenderer lr = ore.GetComponent<LineRenderer>();
                if (lr == null)
                {
                    lr = ore.AddComponent<LineRenderer>();
                }

                // Update the line positions to be the ore and its connected body
                lr.SetPosition(0, ore.transform.position);
                lr.SetPosition(1, ore.GetComponent<SpringJoint2D>().connectedBody.position);
            }
        }
    }
}
