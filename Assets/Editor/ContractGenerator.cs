using Nethereum.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public class ContractGenerator : EditorWindow
{
    static string path = "Assets/Scripts/Contracts/";
    static string contractName = "ExempleContract";
    static string abi = "";

    [MenuItem("Web3/Generate Contracts Classes")]
    public static void GenerateContract()
    {
        //string outputPath = EditorUtility.SaveFilePanelInProject(
        //                title: "Save Location",
        //                defaultName: "Contract",
        //                extension: "cs",
        //                message: "Where do you want to save this script?");
        ContractGenerator window = CreateInstance<ContractGenerator>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 640, 300);
        window.ShowUtility();
    }

    void OnGUI()
    {
        GUILayout.Space(10);
        contractName = EditorGUILayout.TextField("Set a class name : ", contractName);
        GUILayout.Space(10);
        abi = EditorGUILayout.TextField("Paste your abi here : ", abi);
        GUILayout.Space(60);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            var result = String.Empty;
            //await JSRuntime.InvokeAsync<object>("Prism.highlightAll", null);
            var serviceNamespace = contractName;
            //Same, we are generating single file
            int language = 0;
            var cqsNamespace = contractName;
            var dtoNamespace = contractName;
            var contractAbi = new Nethereum.Generators.Net.GeneratorModelABIDeserialiser().DeserialiseABI(abi);
            var generator = new ContractProjectGenerator(contractAbi, contractName, null, null, serviceNamespace, cqsNamespace, dtoNamespace, "", "/", (Nethereum.Generators.Core.CodeGenLanguage)language);
            generator.AddRootNamespaceOnVbProjectsToImportStatements = false;
            var files = generator.GenerateAllMessagesFileAndService();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var item in files)
            {
                var filePath = $"{Path.Combine(path, item.FileName)}";
                File.WriteAllText(filePath, item.GeneratedCode);
                Debug.Log($"File generated : {filePath}");
            }


            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}
