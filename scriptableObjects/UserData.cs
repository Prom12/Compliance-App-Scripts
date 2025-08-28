using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


[CreateAssetMenu(fileName="GeneralScriptableObj", menuName="ScriptableObject/UserData")]
public class UserData : ScriptableObject
{
    [SerializeField] public GeneralScriptableObj GeneralScriptableObj;
    public int Usernumber = 0;
    public string userID;
    [SerializeField] public ExperimentType experimentCurrentType;
    [SerializeField] public Movie shortMovieClip;
    public List<ExerciseType> exerciseTypeOrder;
    [SerializeField] public int exercisePoint = 0;
    [SerializeField] public bool resetMasterControl = false;
    [SerializeField] public string UserExerciseMode = GeneralScriptableObj.UserExerciseMode.AUDIO.ToString();
    [SerializeField] public int initialExerciseNode = 0;
    // [SerializeField] public string totalExerciseNode = 0;
    string fileName ="";   
    string header = "";

    private void getCSVFilePath() {
        bool isOculus = new DeviceType().isOculus();
        if(isOculus) {
            fileName = "/sdcard/Download/userData.csv";
        }else{
            fileName = Application.persistentDataPath + "/userData.csv";
        }
    }

    public void Create_Csv_File(){

        getCSVFilePath();

        header = "User No,User ID,Experiment Group, Selected Video Category, Selected Video Title, Selected Exercises, Total Exercise Point Obtained, UserExerciseMode";
        
        CreateOrAppendFileWithHeader(fileName, header);
    }

    public void CreateUser(){
        Usernumber++;
        userID = (Usernumber).ToString() + "__" + Guid.NewGuid().ToString();
    }


    public List<string[]> ReadCSV()
    {
        getCSVFilePath();

        List<string[]> data = new List<string[]>();

        try
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    data.Add(line.Trim().Split(','));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }

        return data;
    }

    public void DisplayDataInCSV() {
        List<string[]> userData = ReadCSV();
        for (int i = 1; i < userData.Count; i++) {
            string[] rowData = userData[i];

            // Assuming the structure of your CSV matches the order in which data is saved
            //Usernumber = int.Parse(rowData[0]);
            //userID = rowData[1];
            // Assuming the rest of the data corresponds to other fields in UserData
            // Modify this according to your CSV structure
            //experimentCurrentType = new ExperimentType(); // Initialize or fetch experiment type
            //experimentCurrentType.experimentTitle = rowData[2];
            //shortMovieClip = new Movie(); // Initialize or fetch movie type
            //shortMovieClip.videoTitle = rowData[3];
            //shortMovieClip.category = rowData[4];
            //exerciseTypeOrder = new List<ExerciseType>(); // Initialize or fetch exercise types
            //string[] exercises = rowData[5].Split(';');
            //foreach (string exerciseTitle in exercises)
            //{
            //    ExerciseType exerciseType = new ExerciseType(); // Initialize or fetch exercise type
            //    exerciseType.activityTitle = exerciseTitle;
            //    exerciseTypeOrder.Add(exerciseType);
            //}
            //exercisePoint = int.Parse(rowData[6]);

            // Print the data
            //Debug.Log("Usernumber: " + rowData[0]);
            //Debug.Log("userID: " + rowData[1]);
            //Debug.Log("Experiment Type: " + rowData[2]);
            //Debug.Log("Video Title: " + rowData[3]);
            //Debug.Log("Video Category: " + rowData[4]);
            //Debug.Log("Exercises: " + rowData[5]);
            //Debug.Log("Exercise Point: " + rowData[6]);

            // Concatenate the data elements from each row into a single string
            string rowDataString = string.Format("Usernumber: {0}, userID: {1}, Experiment Type: {2}, Video Title: {3}, Video Category: {4}, Exercises: {5}, Exercise Point: {6}",
                rowData[0], rowData[1], rowData[2], rowData[3], rowData[4], rowData[5], rowData[6]);

            // Print the concatenated string
            Debug.Log(rowDataString);
        }
    }
    public void GetStatsData()
    {
        int[] counts = new int[5]; // Initialize with size 5 for counts from 0 to 4

        List<string[]> userData = ReadCSV();

        int totalPeople = userData.Count - 1; // Subtract 1 to exclude the header row

        for (int i = 1; i < userData.Count; i++) // Start from index 1 to skip the header
        {
            string[] rowData = userData[i];
            if (!string.IsNullOrEmpty(rowData[5]))
            {
                int itemCount = rowData[5].Split(';').Length;
                counts[itemCount]++; // Increment count for the corresponding number of items
            }
        }

        // Output results
        for (int i = 0; i < counts.Length; i++)
        {
            double percentage = (counts[i] / (double)totalPeople) * 100;
            Debug.Log("Percentage of people choosing " + i + " items: " + percentage.ToString("0.00") + "%");
        }
    }

    public void ClearUserData(){
        // Convert your data to string array
        string[] contentArray = {
            Usernumber.ToString(),
            userID,
            experimentCurrentType.experimentTitle.ToString(),
            shortMovieClip.videoTitle.ToString(),
            shortMovieClip.category.ToString(),
            string.Join(";", exerciseTypeOrder.Select(et => et.activityTitle.ToString())),
            exercisePoint.ToString(),
            UserExerciseMode
        };
        SaveDataToExcel(fileName, contentArray);
        userID = "";
        shortMovieClip = null;
        exerciseTypeOrder = new List<ExerciseType>();
        exercisePoint = 0;
    }

    void CreateOrAppendFileWithHeader(string fileName, string header)
    {
        bool fileExists = File.Exists(fileName);
        string existingContent = fileExists ? File.ReadAllText(fileName) : "";

        if (!fileExists || !existingContent.Contains(header))
        {
            File.AppendAllText(fileName, header + Environment.NewLine);
        }
        else
        {
            Debug.Log("Header already exists in the file: " + fileName);
        }
    }

    void SaveDataToExcel(string fileName, string[] contentArray)
    {
        string content = string.Join(",", contentArray);

        File.AppendAllText(fileName, content + Environment.NewLine);
        // Debug.Log("Content appended to the file: " + fileName);
    }

    public void ResetUserDataAndGeneralScript(){
        Usernumber = 0;
        userID = "";
        shortMovieClip = null;
        exerciseTypeOrder = new List<ExerciseType>();
        exercisePoint = 0;
        GeneralScriptableObj.resetScriptable();
        GeneralScriptableObj.reloadUI = true;
    }
    public void ResetUserData(){
        exerciseTypeOrder = new List<ExerciseType>();
        exercisePoint = 0;
    }

}
