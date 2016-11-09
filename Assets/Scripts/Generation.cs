using UnityEngine;
using System.Collections;
using System;

public class Generation : MonoBehaviour {

    #region UnityEditor;

    // Prefabs
    public GameObject corridor;
    public GameObject[] possibleRooms;

    // UI
    public UnityEngine.UI.Button generate;
    public UnityEngine.UI.Slider nbRooms;
    public UnityEngine.UI.Text nbRoomsTxt;
    public UnityEngine.UI.Text loading;
    public UnityEngine.UI.InputField seedInput;
    public UnityEngine.UI.Toggle seedToggle;

    // Containers
    public GameObject roomsContainer;
    public GameObject corridorsContainer;

    #endregion UnityEditor;

    // GameObjects
    private ArrayList rooms = new ArrayList();
    private ArrayList corridors = new ArrayList();

    // GameManager
    private GameManager gameManager;

    #region Accessors;

    public GameManager GameManager
    {
        get
        {
            return gameManager;
        }

        set
        {
            gameManager = value;
        }
    }

    #endregion Accessors;

    void Start ()
    {
        nbRoomsTxt.text = nbRooms.value.ToString(); // starting rooms number

        // Listeners initialization
        generate.onClick.AddListener(() => Generate());
        seedToggle.onValueChanged.AddListener(SeedToggleChanged);
        nbRooms.onValueChanged.AddListener(NbRoomsChanged);
	}

    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Q)) Application.Quit();
    }

    #region Listeners;

    // change the possibility to specify a seed
    private void SeedToggleChanged(bool toggle)
    {
        if (toggle) seedInput.interactable = false;
        else seedInput.interactable = true;
    }

    // update the rooms number
    private void NbRoomsChanged(float value)
    {
        nbRoomsTxt.text = value.ToString();
    }

    #endregion Listeners;

    #region Generation;

    private void Generate ()
    {
        Clean();

        // initialize with random or custom seed
        gameManager.InitializeSeed((seedToggle.isOn)? null : seedInput.text);

        // if random seed, show it
        if (seedToggle.isOn) seedInput.text = gameManager.Seed;

        // generate nbRooms rooms
        for (int i = 0; i < nbRooms.value; i++) GenerateNewRoom();

        // generate additional corridors to avoid a "tree" dungeon
        GenerateNewCorridors();

        // make everything isometric
        Iso();
    }

    private void Clean ()
    {
        // rooms clean up
        foreach (GameObject room in rooms)
            Destroy(room);
        rooms.Clear();

        // corridors clean up
        foreach (GameObject corridor in corridors)
            Destroy(corridor);
        corridors.Clear();
    }

    private void GenerateNewRoom ()
    {
        // we select a random room type among the possible rooms
        GameObject roomType = possibleRooms[gameManager.Rand.Next(0, possibleRooms.Length)];

        // instantiation of the room
        GameObject room = InstantiateRoom(roomType);

        // if it is not the first room, we will have to seclude it and generate a corridor
        if (rooms.Count == 0) rooms.Add(room);
        else
        {
            ArrayList pool = rooms.Clone() as ArrayList;
            GameObject from = null;
            /* each new room start is generated from
             * the position of a previous one
             * while we don't find a suitable origin room,
             * we keep searching */
            do
            {
                if (from != null) pool.Remove(from); // non suitable origins are removed from the pool
                from = rooms[gameManager.Rand.Next(0, rooms.Count)] as GameObject;
            } while (!SecludeRoom(room, from, Values.DistanceBetweenRooms));

            // once the new room have a suitable position, we generate
            // a corridor to the closest room (not always its origin)
            CorridorToClosest(room, rooms.Clone() as ArrayList);
            rooms.Add(room);
        }
    }
    
    private GameObject InstantiateRoom (GameObject roomType)
    {
        GameObject instance = Instantiate(roomType, roomsContainer.transform) as GameObject;
        (instance.GetComponent("Room") as Room).Initialize(gameManager.Rand); // room initialization
        return instance;
    }

    private bool SecludeRoom (GameObject room, GameObject from, int distance)
    {
        // we move the new room to the potential origin
        room.transform.position = new Vector2(from.transform.position.x, from.transform.position.y);

        float x2 = from.transform.position.x,
            y2 = from.transform.position.y,
            w2 = (from.GetComponent("Room") as Room).Width,
            h2 = (from.GetComponent("Room") as Room).Height,
            w1 = (room.GetComponent("Room") as Room).Width,
            h1 = (room.GetComponent("Room") as Room).Height;

        /* in order to have various distances between rooms
         * xAxis = horizontal moving factor
         * yAxis = vertical moving factor
         * yOrX = in order to generate a direct corridor,
         * we have to keep a direct access between the rooms (on axis y or x) */
        int xAxis = gameManager.Rand.Next(-distance, distance),
            yAxis = gameManager.Rand.Next(0, distance);
        if (xAxis == 0 && yAxis == 0) yAxis = 1;
        bool yOrX = yAxis >= Mathf.Abs(xAxis);

        int i = 0;
        while(!IsSecluded(room, distance))
        {
            // if the seclusion is too long, we move on
            if (i == 10) return false;
            float x1 = room.transform.position.x,
                   y1 = room.transform.position.y;

            // if yOrX, we try to keep a direct access to
            // the origin room (for a potential corridor)
            if (yOrX && y1 + yAxis > y2 + h2 - Values.MinimumCorridorWidth)
            {
                yAxis = 0;
                if (xAxis == 0) xAxis = 1;
            }

            // same if !yOrX but on the other axis
            if (!yOrX && (xAxis > 0 && x1 + xAxis > x2 + w2 - Values.MinimumCorridorWidth || xAxis < 0 && x1 + w1 + xAxis < x2 + Values.MinimumCorridorWidth))
            {
                xAxis = 0;
                if (yAxis == 0) yAxis = 1;
            }

            // translation
            room.transform.position = new Vector2(room.transform.position.x + xAxis, room.transform.position.y + yAxis);
            i++;
        }
        // if the room can be secluded with this origin
        return true;
    }

    // for each existing room and corridor,
    // we check if there is no intersection with the room
    private bool IsSecluded (GameObject room, int distance)
    {
        float x1 = room.transform.position.x,
            y1 = room.transform.position.y,
            w1 = (room.GetComponent("Room") as Room).Width,
            h1 = (room.GetComponent("Room") as Room).Height;
        foreach (GameObject room2 in rooms)
        {
            float x2 = room2.transform.position.x,
                y2 = room2.transform.position.y,
                w2 = (room2.GetComponent("Room") as Room).Width,
                h2 = (room2.GetComponent("Room") as Room).Height;
            if ((x1 <= x2 + w2 + distance && x1 + distance >= x2 || x1 + w1 + distance >= x2 && x1 + w1 <= x2 + w2 + distance) &&
                (y1 <= y2 + h2 + distance && y1 + distance >= y2 || y1 + h1 + distance >= h2 && y1 + h1 <= y2 + h2 + distance))
                return false;
        }
        foreach (GameObject corridor in corridors)
        {
            float x2 = corridor.transform.position.x,
                y2 = corridor.transform.position.y,
                w2 = (corridor.GetComponent("Room") as Room).Width,
                h2 = (corridor.GetComponent("Room") as Room).Height;
            if ((x1 <= x2 + w2 + distance && x1 + distance >= x2 || x1 + w1 + distance >= x2 && x1 + w1 <= x2 + w2 + distance) &&
                (y1 <= y2 + h2 + distance && y1 + distance >= y2 || y1 + h1 + distance >= h2 && y1 + h1 <= y2 + h2 + distance))
                return false;
        }
        return true;
    }

    private void CorridorToClosest (GameObject room, ArrayList pool)
    {
        bool done = false;
        while (!done)
        {
            if (pool.Count == 0) return;
            GameObject closest = null;
            float min = 9999, temp;

            // we look for the closest room
            foreach (GameObject room2 in pool)
            {
                temp = Distance(room, room2);
                if (temp < min)
                {
                    closest = room2;
                    min = temp;
                }
            }
            // is there is no room in the pool or we there is
            // already a corridor between the two rooms, we stop
            if (closest == null || (room.GetComponent("Room") as Room).CorridorsTo.Contains(closest)) return;
            GameObject instance = Instantiate(corridor);

            // if the creation of the corridor is a success
            done = (instance.GetComponent("Corridor") as Corridor).Initialize(closest, room, gameManager.Rand);
            if (done)
            {
                // the corridor is added
                (room.GetComponent("Room") as Room).CorridorsTo.Add(closest);
                (closest.GetComponent("Room") as Room).CorridorsTo.Add(room);
                corridors.Add(instance);
                instance.transform.SetParent(corridorsContainer.transform);
            }
            else
            {
                // the room is removed from the pool
                Destroy(instance);
                pool.Remove(closest);
            }
        }
    }

    // we try to generate new corridors
    // (to avoid a tree shape dungeon)
    private void GenerateNewCorridors()
    {
        foreach (GameObject room1 in rooms)
            foreach (GameObject room2 in rooms)
                if (room1 != room2 && Distance(room1, room2) < Values.DistanceAdditionalCorridors) 
                {
                    ArrayList temp = new ArrayList();
                    temp.Add(room2);
                    CorridorToClosest(room1, temp);
                }
    }

    #endregion Generation;

    // we transform every object
    // into isometric ones
    private void Iso()
    {
        foreach (GameObject room in rooms)
            (room.GetComponent("Room") as Room).Iso();
        foreach (GameObject corridor in corridors)
            (corridor.GetComponent("Room") as Room).Iso();
    }

    // distance between two gameObjects
    private float Distance (GameObject room1, GameObject room2)
    {
        float x1 = room1.transform.position.x,
            y1 = room1.transform.position.y,
            x2 = room2.transform.position.x,
            y2 = room2.transform.position.y,
            w1 = (room1.GetComponent("Room") as Room).Width,
            h1 = (room1.GetComponent("Room") as Room).Height,
            w2 = (room2.GetComponent("Room") as Room).Width,
            h2 = (room2.GetComponent("Room") as Room).Height;
        return Mathf.Sqrt(Mathf.Pow((x1 + w1)/2 - (x2 + w2)/2, 2) + Mathf.Pow((y1 + h1)/2 - (y2 + h2)/2, 2));
    }
}
