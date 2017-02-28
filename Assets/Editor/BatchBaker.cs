
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections;
using UnityEditor;
 
public class BatchBaker : EditorWindow
{
    private int levelIndex;
    private bool startedBaking = false;
    private string fileName;
   
    [MenuItem ("Window/BatchBaker")]
    static void ShowWindow ()
    {
        EditorWindow.GetWindow (typeof(BatchBaker));
    }
   
    private int fieldCount = 3;
    private string[] levelNames = new string[] {
               // "LevelTutorial",
                "Level1",
               // "Level2",
              //  "Level3_1",                
                "Level4_1",  
                "Level13_1",  
              //  "Level5",
                //"Level10",
             //   "Level7",
                 
                //"Level4",
              //  "Level6",
               // "Level22",
           //     "Level11",
                "Level18_1", 
               // "Level23",                
               // "Level16",

              //  "Level17",
                "Level19",
                
                "Level3",
                "Level9",
                "Level14",
                "Level21",

                "Level18",
                "Level12",
                "Level13",
                "Level15",
                "Level8",     
};
   
    void OnGUI()
    {
        fieldCount = levelNames.Length;
        fieldCount = EditorGUILayout.IntField("Field Count:  ",fieldCount);
        if (levelNames == null || levelNames.Length != fieldCount)
            levelNames = new string[fieldCount];
       
        for (int i = 0; i < levelNames.Length; i++)
        {
            if (levelNames[i] == null)
                levelNames[i] = "";
           
            levelNames[i] = EditorGUILayout.TextField("Level: ",levelNames[i]);    
        }
       
        if(GUILayout.Button("Begin Batch Bake"))
        {
            levelIndex = 0;
            OpenBakeAndSave();
        }
    }
 
    void Update()
    {
        if( startedBaking && Lightmapping.isRunning==false)
        {
            startedBaking = false;
            GoToNext();
        }
    }
 
    void GoToNext()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        levelIndex++;
        if(levelIndex < levelNames.Length)
        {
            OpenBakeAndSave();
        }
    }
 
    void OpenBakeAndSave()
    {
        fileName = "Assets/Scenes/Levels/"+ levelNames[levelIndex] +".unity";

        EditorSceneManager.OpenScene(fileName);
               
        Lightmapping.Clear();
       
      //  LightmapEditorSettings.maxAtlasHeight = 4096;
      //  LightmapEditorSettings.maxAtlasWidth = 4096;       
       
        Lightmapping.BakeAsync();    
 
        startedBaking = true;
 
    }
 
   
 
}