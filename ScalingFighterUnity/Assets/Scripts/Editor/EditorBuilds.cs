using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Build;

// Place inside an Editor/ folder
//https://docs.unity3d.com/Manual/BuildPlayerPipeline.html
public class EditorBuilds
{
    static string game_name = "ScalableFighter";

    static string build_subfolder = "Builds";

    static string web_build_folder = "ScalableFighter_Web";
    static string windows_build_folder = "ScalableFighter_Windows";

    public const string batch_folder_path = "Uploads";    // No slashes




    // Returns TRUE if succeeded
    public static bool BuildGame(BuildTarget build_target, bool build_game=true, bool run_game_afterwards = false, bool is_development_build = false)
    {
        string default_build_folder = ExtensionMethods.GetEditorFolderPath(build_subfolder);
        // Get filename
        string default_path = web_build_folder;
        string file_extension = ".html";
        string relative_data_folder_path = game_name + "_Data/";    // Used for windows and linux (not mac)

        switch (build_target)
        {
            default:
            case BuildTarget.WebGL:
                default_path = web_build_folder;
                file_extension = ".html";
                relative_data_folder_path = game_name + "_Data/";
                break;
            case BuildTarget.StandaloneWindows64:
                default_path = windows_build_folder;
                file_extension = ".exe";
                relative_data_folder_path = game_name + "_Data/";
                break;
                /*
            case BuildTarget.StandaloneLinux64:
                default_path = linux_server_build_folder;
                file_extension = ".x64";
                break;
            case BuildTarget.Android:
                default_path = android_build_folder;
                file_extension = ".apk";
                break;
                */
        }
        string folder_to_create_if_not_present = ExtensionMethods.GetEditorFolderPath(build_subfolder + "\\" + default_path);
        UnityEngine.Debug.Log($"Ensuring directory {folder_to_create_if_not_present} exists");
        string path = folder_to_create_if_not_present;
        string full_exe_path = game_name + file_extension;
        string location_path_name = path;//path + "/" + full_exe_path;

        if (build_game)
        {
            if (!Directory.Exists(folder_to_create_if_not_present))
            {
                Directory.CreateDirectory(folder_to_create_if_not_present);
            }
            else
            {
                // If there are files in that directory, remove them.
                Directory.Delete(folder_to_create_if_not_present, true);
                Directory.CreateDirectory(folder_to_create_if_not_present);
            }

            UnityEngine.Debug.Log("Building " + build_target.ToString() + " in " + path);

            // Do nothing if this is cancelled
            if (string.IsNullOrEmpty(path))
                return false;

            OnPreprocessBuild();

            // Record current time for buikd
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (build_target == BuildTarget.StandaloneWindows64 || build_target == BuildTarget.StandaloneLinux64)
            {
                // Windows has error here for some reason. Delete folder for it to work?
                //if (Directory.Exists(folder_to_create_if_not_present))
                //{
                //    Directory.Delete(folder_to_create_if_not_present);
                //}
                location_path_name += "/" + full_exe_path;
            }

            /////////////////////////////////// CHANGE BUILD OPTIONS HERE
            BuildPlayerOptions build_options = new BuildPlayerOptions();
            //build_options.scenes = GetLevelsInBuild(server_mode == ServerMode.Not).ToArray();
            // Put scenes in build? Weird it's not working at all
            List<string> scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                    scenes.Add(scene.path);
            }
            build_options.scenes = scenes.ToArray();
            build_options.locationPathName = location_path_name;
            build_options.target = build_target;

            /////////// SET OPTIONS HERE
            // build_options.options = BuildOptions.ForceEnableAssertions; //| 
            build_options.options = BuildOptions.None;

            if (is_development_build)
            {
                // We already have BETTER in-game debug console asset
                //build_options.options |= BuildOptions.Development;
            }

            ////////////////////////////
            BuildReport report = BuildPipeline.BuildPlayer(build_options);
            ////////////////////////////////////////////////////////////////////


            // OLD WAY
            //BuildReport report = BuildPipeline.BuildPlayer(GetLevelsInBuild().ToArray(), path + "/" + full_exe_path, build_target, BuildOptions.None);

            // Don't continue if this fails
            if (report.summary.result != BuildResult.Succeeded)
                return false;

            // Report how long it took
            var build_elapsed_seconds = watch.ElapsedMilliseconds / 1000f;
            watch.Stop();

            UnityEngine.Debug.Log("Finished build " + build_target + " took: " + build_elapsed_seconds + " seconds or " + (build_elapsed_seconds / 60f) + " minutes\n" + location_path_name);



            /*
            var copy_elapsed_seconds = watch.ElapsedMilliseconds / 1000f;
            var copy_only_seconds = copy_elapsed_seconds - build_elapsed_seconds;
            UnityEngine.Debug.Log("Copying files over took: " + copy_only_seconds + " seconds or " + (copy_only_seconds / 60f) + " minutes");
            */

            ///////////////////////// MOVE FOLDERS OVER
            //CopyTextFolderToBuildFolder(path, relative_data_folder_path);
        }



        // Run the game (Process class from System.Diagnostics).
        // Only do this if we're in windows
        // AND not doing a BUIDALL
        if (run_game_afterwards && build_target == BuildTarget.StandaloneWindows64)
        {
            string full_path = path + "/" + full_exe_path; ;
            UnityEngine.Debug.Log("Trying to run process " + full_path);
            Process proc = new Process();
            proc.StartInfo.FileName = full_path;
            proc.Start();
        }

        return true;
    }


    public static void CopyTextFolderToBuildFolder(string path, string relative_data_folder_path="Geomancers_Data")
    {
        // Copy a file from the project folder to the build folder, alongside the built game.
        // Place text folder inside _Data/
        string destination_folder = ExtensionMethods.PrepareFilePath(new string[] { path, relative_data_folder_path, ExtensionMethods.TextFolderName });
        string ingame_path_to_copyable_folder = ExtensionMethods.GetTextFilepath("");
        ExtensionMethods.CopyFolderTo(ingame_path_to_copyable_folder, destination_folder);
        return;
    }


    #region build targets
    [MenuItem("Build/Web Build AND Itch Upload)")]
    public static void WebBuildAndItchUpload()
    {
        if (BuildGame(BuildTarget.WebGL, is_development_build: true))
            UploadWeb();
    }
    [MenuItem("Build/Web Build (NO upload)")]
    public static void BuildWeb()
    {
        BuildGame(BuildTarget.WebGL, run_game_afterwards: true, is_development_build: true);
    }
    [MenuItem("Build/Itch Upload (no build))")]
    public static void UploadWeb()
    {
        RunBatchFile("ScalableFighter_Web_Itch_Upload.bat", ExtensionMethods.GetEditorFolderPath(batch_folder_path));
    }
    #endregion

    public static void RunBatchFile(string batchfile_name, string folder_containing_batch)//, string path_to_batch_file=ScriptBuild.batch_folder_path)
    {
        string working_dir = folder_containing_batch; //path_to_batch_file;
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo(batchfile_name);
            psi.WorkingDirectory = working_dir;
            Process.Start(psi);
            UnityEngine.Debug.Log("Launched process " + batchfile_name + " path " + working_dir);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("RunBatchFile ERROR " + batchfile_name + " path " + working_dir + "\n" + e.Message);
        }
    }

    public static void RunCmd(string executeString)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo(executeString);
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.UseShellExecute = false;
        Process process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        process.WaitForExit();
        if (process.ExitCode == -1)
            throw new Exception(process.StandardOutput.ReadToEnd());
    }


    /// <summary>
    /// Doesn't get called on its own. Call MANUALLY when building game
    /// </summary>
    /// <param name="report"></param>
    public static void OnPreprocessBuild()
    {
        File.WriteAllText("Assets/Resources/BuildDate.txt", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        UnityEngine.Debug.Log("OnPreprocessBuild");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
}
