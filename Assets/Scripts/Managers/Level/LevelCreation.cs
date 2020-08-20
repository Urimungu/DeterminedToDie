using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class LevelCreation : EditorWindow{
    //Creates the Objectives
    private readonly string[] TypeOptions = new string[] { "Nothing", "Pick Up", "Destination", "Wait", "Kill Enemies", "End Game"};
    private readonly string[] ObjectOptions = new string[] { "NO", "Start", "End"};
    private Vector2 _scrollPos;

    //References
    public LevelReciever levelReciever;

    [MenuItem("Window/Level Creation")]
    static void Init() {
        LevelCreation window = (LevelCreation)GetWindow(typeof(LevelCreation));
        window.Show();
    }

    //Main Display
    private void OnGUI(){
        GUILayout.Label("Level Settings");
        levelReciever = (LevelReciever)EditorGUILayout.ObjectField("Level: ", levelReciever, typeof(LevelReciever), true);

        //Exits if it doesn't have a level Reciever
        if (levelReciever == null) return;

        //Calls the General Options that are at the top
        GeneralOptions();

        //Displays the Objectives that already exist
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(position.width - 5));
        for (int i = 0; i < levelReciever.Objectives.Count; i++){
            //Selects or Diselects
            var originalFont = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            levelReciever.SelectedItems[i] = EditorGUILayout.Toggle("Objective " + i + ": ", levelReciever.SelectedItems[i], EditorStyles.toggle);
            EditorStyles.label.fontStyle = originalFont;

            //Displays the Content
            DisplayObjective(i);

            //Creates a page break
            GUILayout.Label("------ --- -- - ");
        }
        EditorGUILayout.EndScrollView();
    }

    //General Options
    private void GeneralOptions() {
        EditorGUILayout.BeginHorizontal();

        //Creates a new Objective
        if (GUILayout.Button("Create New Objective"))
            CreateObjective();

        EditorGUILayout.BeginVertical();
        //Removes the Selected Objectives
        if (GUILayout.Button("Remove Selected"))
            RemoveObjective();

        //Clears all of the Current Objectives
        if (GUILayout.Button("Clear Current List"))
            ClearReferences();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
    private void ClearReferences() {
        levelReciever.Objectives.Clear();
        levelReciever.ObjectiveDetails.Clear();
        levelReciever.SelectedItems.Clear();
    }
    private void CreateObjective() {
        levelReciever.Objectives.Add(ObjectiveStruct.ObjectiveType.Nothing);
        levelReciever.ObjectiveDetails.Add(new ObjectiveStruct());
        levelReciever.SelectedItems.Add(false);
    }
    private void RemoveObjective() {
        //Initializes Variables
        int count = levelReciever.SelectedItems.Count;

        //Removes all Selected Items
        for (int i = count; i-- > 0;) {
            if (levelReciever.SelectedItems[i] == false) continue;

            //Removes the items that are selected
            levelReciever.Objectives.RemoveAt(i);
            levelReciever.ObjectiveDetails.RemoveAt(i);
            levelReciever.SelectedItems.RemoveAt(i);
        }
    }

    //Displays the Items
    private void DisplayObjective(int ID) {

        //Creates the Objective Title
        if (levelReciever.Objectives[ID] != ObjectiveStruct.ObjectiveType.Nothing)
            levelReciever.ObjectiveDetails[ID].Objective = EditorGUILayout.TextField("Objective Title: ", levelReciever.ObjectiveDetails[ID].Objective);

        //Can Change the Objective Type
        levelReciever.Objectives[ID] = (ObjectiveStruct.ObjectiveType)EditorGUILayout.Popup("Objective Type: ", (int)levelReciever.Objectives[ID], TypeOptions);

        //Displays the Proper Settings
        switch (levelReciever.Objectives[ID]) {
                //Displays the specific variables required to fullfill every objective-type
            case ObjectiveStruct.ObjectiveType.PickUp:      DisplayPickUp(ID);          break;
            case ObjectiveStruct.ObjectiveType.Destination: DisplayDestination(ID);     break;
            case ObjectiveStruct.ObjectiveType.Wait:        DisplayWait(ID);            break;
            case ObjectiveStruct.ObjectiveType.Kill:        DisplayKillEnemies(ID);     break;
            case ObjectiveStruct.ObjectiveType.ExitGame:    DisplayExitGame(ID);        break;

                //Displays Nothing
            case ObjectiveStruct.ObjectiveType.Nothing:
            default: break;
        }

        if (levelReciever.Objectives[ID] == ObjectiveStruct.ObjectiveType.ExitGame) return;

        //If the Level Needs to Remove an Object
        levelReciever.ObjectiveDetails[ID].RemoveObject = (ObjectiveStruct.ObjectActionType)EditorGUILayout.Popup("Remove an Object: ", (int)levelReciever.ObjectiveDetails[ID].RemoveObject, ObjectOptions);

        if (levelReciever.ObjectiveDetails[ID].RemoveObject != ObjectiveStruct.ObjectActionType.Nothing)
            levelReciever.ObjectiveDetails[ID].Remove = (GameObject)EditorGUILayout.ObjectField("Object to Remove: ", levelReciever.ObjectiveDetails[ID].Remove, typeof(GameObject), true);

        //If the Level Needs to Return an Object
        levelReciever.ObjectiveDetails[ID].ReturnObject = (ObjectiveStruct.ObjectActionType)EditorGUILayout.Popup("Return an Object: ", (int)levelReciever.ObjectiveDetails[ID].ReturnObject, ObjectOptions);

        if (levelReciever.ObjectiveDetails[ID].ReturnObject != ObjectiveStruct.ObjectActionType.Nothing)
            levelReciever.ObjectiveDetails[ID].Return = (GameObject)EditorGUILayout.ObjectField("Object to Return: ", levelReciever.ObjectiveDetails[ID].Return, typeof(GameObject), true);

    }

    //Displays Specific Item Requirments
    private void DisplayPickUp(int ID) {
        levelReciever.ObjectiveDetails[ID].PickUp = (Transform)EditorGUILayout.ObjectField("Pick Up Target: ", levelReciever.ObjectiveDetails[ID].PickUp, typeof(Transform), true);
    }
    private void DisplayDestination(int ID) {
        levelReciever.ObjectiveDetails[ID].Destination = (Transform)EditorGUILayout.ObjectField("Location: ", levelReciever.ObjectiveDetails[ID].Destination, typeof(Transform), true);
        levelReciever.ObjectiveDetails[ID].DestinationDistance = EditorGUILayout.FloatField("Proximity to Advance: ", levelReciever.ObjectiveDetails[ID].DestinationDistance);
    }
    private void DisplayWait(int ID) {
        levelReciever.ObjectiveDetails[ID].WaitTime = EditorGUILayout.FloatField("Wait Time: ", levelReciever.ObjectiveDetails[ID].WaitTime);
    }
    private void DisplayKillEnemies(int ID) {
        levelReciever.ObjectiveDetails[ID].EnemiesToKill = EditorGUILayout.IntField("Enemy Kills Needed: ", levelReciever.ObjectiveDetails[ID].EnemiesToKill);
    }
    private void DisplayExitGame(int ID) {
        levelReciever.ObjectiveDetails[ID].LeveltoLoad = EditorGUILayout.TextField("Next Level Name: ", levelReciever.ObjectiveDetails[ID].LeveltoLoad);
        levelReciever.ObjectiveDetails[ID].ExitTime = EditorGUILayout.FloatField("Leave Time: ", levelReciever.ObjectiveDetails[ID].ExitTime);
    }
}

#endif
