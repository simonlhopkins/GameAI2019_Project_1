using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PhaseManager is the place to keep a succession of events or "phases" when building 
/// a multi-step AI demo. This is essentially a state variable for the map (aka level)
/// itself, not the state of any particular NPC.
/// 
/// Map state changes could happen for one of two reasons:
///     when the user has pressed a number key 0..9, desiring a new phase
///     when something happening in the game forces a transition to the next phase
/// 
/// One use will be for AI demos that are switched up based on keyboard input. For that, 
/// the number keys 0..9 will be used to dial in whichever phase the user wants to see.
/// </summary>

public class PhaseManager : MonoBehaviour {
    // Set prefabs
    public GameObject PlayerPrefab;     // You, the player
    public GameObject HunterPrefab;     // Agent doing chasing
    public GameObject WolfPrefab;       // Agent getting chased
    public GameObject RedPrefab;     // reserved for future use
    // public GameObject BluePrefab;    // reserved for future use

    public NPCController house;         // THis goes away
    bool autoPlay;

    // Set up to use spawn points. Can add more here, and also add them to the 
    // Unity project. This won't be a good idea later on when you want to spawn
    // a lot of agents dynamically, as with Flocking and Formation movement.

    public GameObject spawner1;
    public Text SpawnText1;
    public GameObject spawner2;
    public Text SpawnText2;
    public GameObject spawner3;
    public Text SpawnText3;
 
    private List<GameObject> spawnedNPCs;   // When you need to iterate over a number of agents.
    
    private int currentMapState = 0;           // This stores which state the map or level is in.
    private int previousMapState = 0;          // The map state we were just in

    public int MapState => currentMapState;

    LineRenderer line;                  // GOING AWAY

    public GameObject[] Path;


    public Text narrator;                   // 

    // Use this for initialization. Create any initial NPCs here and store them in the 
    // spawnedNPCs list. You can always add/remove NPCs later on.

    void Start() {
        spawnedNPCs = new List<GameObject>();
        Number0();
        autoPlay = true;
    }

    /// <summary>
    /// This is where you put the code that places the level in a particular state.
    /// Unhide or spawn NPCs (agents) as needed, and give them things (like movements)
    /// to do. For each case you may well have more than one thing to do.
    /// </summary>
    private void Update() {

        string inputstring = Input.inputString;
        int num;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoPlay = true;
        }

        // Look for a number key click
        if (inputstring.Length > 0)
        {
            Debug.Log(inputstring);

            if (Int32.TryParse(inputstring, out num))
            {
                if (num != currentMapState)
                {
                    previousMapState = currentMapState;
                    currentMapState = num;
                }
            }
        }
        
        // Check if a game event had caused a change of state in the level.
        if (currentMapState == previousMapState)
            return;

       // If we get here, we've been given a new map state, from either source
       switch (currentMapState) {
           case 0:
                autoPlay = false;
                EnterMapStateZero();
                previousMapState = currentMapState;
               break;

           case 1:
                autoPlay = false;
                EnterMapStateOne();
                previousMapState = currentMapState;
                break;

           case 2:
                autoPlay = false;
                EnterMapStateTwo();
                previousMapState = currentMapState;
                break;

           case 3:
                autoPlay = false;
                EnterMapStateThree();
                previousMapState = currentMapState;
                break;
           case 4:
                autoPlay = false;
                EnterMapStateFour();
                previousMapState = currentMapState;
                break;
           case 5:
                autoPlay = false;
                EnterMapStateFive();
                previousMapState = currentMapState;
                break;
           case 6:
                autoPlay = false;
                EnterMapStateSix();
                previousMapState = currentMapState;
                break;
           case 7:
                autoPlay = false;
                EnterMapStateSeven();
                previousMapState = currentMapState;
                break;
           case 8:
                autoPlay = false;
                EnterMapStateEight();
                previousMapState = currentMapState;
                break;
           case 9:
                autoPlay = false;
                EnterMapStateNine();
                previousMapState = currentMapState;
                break;
        }
    }

    void Refresh()
    {
        for(int i = spawnedNPCs.Count - 1;i>=0; i--)
        {
            GameObject character = spawnedNPCs[i];
            character.GetComponent<NPCController>().label.enabled = false;
            character.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
    }

    private void EnterMapStateZero()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character = spawnedNPCs[i];
            character.GetComponent<NPCController>().label.enabled = false;
            character.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        Number0();
    }
    private void EnterMapStateOne() {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number1();
        Debug.Log("Doing it");
    }
    private void EnterMapStateTwo()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        character = SpawnItem(spawner2, WolfPrefab, null, SpawnText2, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number2();
    }
    private void EnterMapStateThree()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number3();
    }
    private void EnterMapStateFour()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number4();
    }
    private void EnterMapStateFive()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        character = SpawnItem(spawner3, RedPrefab, null, SpawnText3, 7);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number5();
    }
    private void EnterMapStateSix()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 1);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        character = SpawnItem(spawner3, RedPrefab, null, SpawnText3, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number6();
    }
    private void EnterMapStateSeven()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        character = SpawnItem(spawner3, RedPrefab, null, SpawnText3, 4);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number7();
    }
    private void EnterMapStateEight()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        character = SpawnItem(spawner3, RedPrefab, null, SpawnText3, 4);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number8();
    }
    private void EnterMapStateNine()
    {
        for (int i = spawnedNPCs.Count - 1; i >= 0; i--)
        {
            GameObject character1 = spawnedNPCs[i];
            character1.GetComponent<NPCController>().label.enabled = false;
            character1.SetActive(false);
            Debug.Log("Deleted");
        }
        spawnedNPCs.Clear();
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        character = SpawnItem(spawner3, RedPrefab, null, SpawnText3, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        Number9();
    }


    // ... Etc. Etc.

    /// <summary>
    /// SpawnItem placess an NPC of the desired type into the game and sets up the neighboring 
    /// floating text items nearby (diegetic UI elements), which will follow the movement of the NPC.
    /// </summary>
    /// <param name="spawner"></param>
    /// <param name="spawnPrefab"></param>
    /// <param name="target"></param>
    /// <param name="spawnText"></param>
    /// <param name="mapState"></param>
    /// <returns></returns>
    private GameObject SpawnItem(GameObject spawner, GameObject spawnPrefab, NPCController target, Text spawnText, int mapState)
    {
        Vector3 size = spawner.transform.localScale;
        Vector3 position = spawner.transform.position + new Vector3(UnityEngine.Random.Range(-size.x / 2, size.x / 2), 0, UnityEngine.Random.Range(-size.z / 2, size.z / 2));
        GameObject temp = Instantiate(spawnPrefab, position, Quaternion.identity);
        if (target)
        {
            temp.GetComponent<SteeringBehavior>().target = target;
        }
        temp.GetComponent<NPCController>().label = spawnText;
        temp.GetComponent<NPCController>().mapState = mapState;         // This is separate from the NPC's internal state
        Camera.main.GetComponent<CameraController>().player = temp;
        return temp;
    }


    // These next two methods show spawning an agent might look.
    // You make them happen when you want to by using the Invoke() method.
    // These aren't needed for the first assignment.

    private void Number0()
    {
        narrator.text = "We see the hunter, waiting for its prey to appear";
        GameObject character = SpawnItem(spawner1, HunterPrefab, null, SpawnText1, 0);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        if (autoPlay)
        {
            Invoke("Number1", 5);
        }
        
    }

    private void Number1()
    {
        narrator.text = "The hunter's prey appears, simply grazing and wandering across the field, unaware of the danger it is about to face.";
        GameObject character = SpawnItem(spawner2, WolfPrefab, null, SpawnText2, 7);
        spawnedNPCs.Add(character);
        character.GetComponent<NPCController>().label.enabled = true;
        if (autoPlay)
        {
            Invoke("Number2", 10);
        }
    }
    private void Number2 ()
    {
        narrator.text = "The hunter begins chasing its prey. The prey flees, but is ultimately trapped by the hunter.";
        spawnedNPCs[0].GetComponent<NPCController>().NewTarget(spawnedNPCs[1].GetComponent<NPCController>());
        //sets the map state of the hunter to 1, which is seeking the target
        spawnedNPCs[0].GetComponent<NPCController>().mapState = 3;
        //sets the target of the wolf to the hunter
        spawnedNPCs[1].GetComponent<NPCController>().NewTarget(spawnedNPCs[0].GetComponent<NPCController>());
        //sets the map state of the wolf to flee from the hunter
        spawnedNPCs[1].GetComponent<NPCController>().mapState = 2;
        if (autoPlay)
        {
            Invoke("Number3", 40);
        }
    }
    private void Number3()
    {
        narrator.text = "After its feast, the hunter dozes in the field, intent on waking up if more prey appears.";
        spawnedNPCs[0].GetComponent<NPCController>().mapState = 0;
        GameObject wolf = spawnedNPCs[1];
        spawnedNPCs.RemoveAt(1);
        wolf.SetActive(false);
        if (autoPlay)
        {
            Invoke("Number4", 5);
        }
    }

    private void Number4()
    {
        narrator.text = "While the hunter sleeps, more prey appears, and begins grazing acorss the fields.";
        spawnedNPCs.Add(SpawnItem(spawner3, RedPrefab, null, SpawnText3, 7));
        spawnedNPCs[1].GetComponent<NPCController>().NewTarget(spawnedNPCs[0].GetComponent<NPCController>());
        if (autoPlay)
        {
            Invoke("Number5", 10);
        }
    }

    private void Number5()
    {
        narrator.text = "Seeing more prey, the hunter awakens and begins its chase again.";
        spawnedNPCs[0].GetComponent<NPCController>().NewTarget(spawnedNPCs[1].GetComponent<NPCController>());
        spawnedNPCs[0].GetComponent<NPCController>().mapState = 1;
        if (autoPlay)
        {
            Invoke("Number6", 7);
        }
    }

    private void Number6()
    {
        narrator.text = "This prey, however, has evolved, and is able to evade its attacker.";
        spawnedNPCs[1].GetComponent<NPCController>().mapState = 4; // this should the evade algorithm
        if (autoPlay)
        {
            Invoke("Number7", 7);
        }
    }

    private void Number7()
    {
        narrator.text = "Seeing this, the hunter adjusts its tactics, hoping to catch the prey by surprise.";
        spawnedNPCs[0].GetComponent<NPCController>().mapState = 3; // this should be the pursue arrive algorithm
        if (autoPlay)
        {
            Invoke("Number8", 5);
        }
    }

    private void Number8()
    {
        // ALIGN
        narrator.text = "While the prey cowers in fear, the hunter aligns himself to hunt down the prey";
        spawnedNPCs[0].GetComponent<NPCController>().mapState = 5;
        if (autoPlay)
        {
            Invoke("Number9", 5);
        }
    }

    private void Number9()
    {
        // FACE
        narrator.text = "Finally, the prey has had enough, and decides to hunt the hunter, facing it head on.";
        spawnedNPCs[1].GetComponent<NPCController>().mapState = 6;
        spawnedNPCs[0].GetComponent<NPCController>().mapState = 4;
        if (autoPlay)
        {
            Invoke("EnterMapStateZero", 5);
        }
    }

    // Here is an example of a method you might want for when an arrival actually happens.
    private void SetArrive(GameObject character) {

        character.GetComponent<NPCController>().mapState = 3; // Whatever the new map state is after arrival
        character.GetComponent<NPCController>().DrawConcentricCircle(character.GetComponent<SteeringBehavior>().slowRadiusL);
    }

    // Vestigial. Maybe you'll find it useful.
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(spawner1.transform.position, spawner1.transform.localScale);
        Gizmos.DrawCube(spawner2.transform.position, spawner2.transform.localScale);
        Gizmos.DrawCube(spawner3.transform.position, spawner3.transform.localScale);
    }
}
