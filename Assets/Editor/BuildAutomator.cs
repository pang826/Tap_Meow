using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildAutomator
{
    [MenuItem("Build/Window Build")]
    public static void Build()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();

        options.locationPathName = "C:\\Builds\\Game.exe";
        options.scenes = new string[] { "Assets\\1.Worker\\WGH\\Scenes\\WGH_TestScene.unity" };
        options.target = BuildTarget.StandaloneWindows;
        options.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(options);
    }
}
