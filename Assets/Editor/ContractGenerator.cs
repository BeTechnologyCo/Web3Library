using Nethereum.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public class ContractGenerator : EditorWindow
{
    const string FILENAME = "VERSION.txt";
    static string contractName = "Contract";
    static string abi = "";

    [MenuItem("Web3/Generate Contracts")]
    public static void GenerateContract()
    {
        //string outputPath = EditorUtility.SaveFilePanelInProject(
        //                title: "Save Location",
        //                defaultName: "Contract",
        //                extension: "cs",
        //                message: "Where do you want to save this script?");
        ContractGenerator window = CreateInstance<ContractGenerator>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowUtility();
    }

    void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Set a contract name", EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
        contractName = EditorGUILayout.TextField("Contract: ", contractName);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Paste your abi here", EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
        abi = EditorGUILayout.TextField("Abi: ", abi);
        GUILayout.Space(60);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            var result = String.Empty;
            //await JSRuntime.InvokeAsync<object>("Prism.highlightAll", null);
            var serviceNamespace = contractName;
            //Same, we are generating single file
            int language = 0;
            var cqsNamespace = contractName + ".ContractDefinition";
            var dtoNamespace = contractName + ".ContractDefinition";
            var contractAbi = new Nethereum.Generators.Net.GeneratorModelABIDeserialiser().DeserialiseABI(abi);
            var generator = new ContractProjectGenerator(contractAbi, contractName, null, "Web3Library", serviceNamespace, cqsNamespace, dtoNamespace, "", "/", (Nethereum.Generators.Core.CodeGenLanguage)language);
            generator.AddRootNamespaceOnVbProjectsToImportStatements = false;
            var files = generator.GenerateAllMessagesFileAndService();
            foreach (var item in files)
            {
                File.WriteAllText($"Assets/Scripts/Contracts/{item.FileName}", item.GeneratedCode);
            }

            Close();
        }
        if (GUILayout.Button("Cancel")) Close();
        EditorGUILayout.EndHorizontal();
    }
}
