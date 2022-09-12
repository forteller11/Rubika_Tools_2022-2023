using System;
using System.IO;
using System.Net;
using System.Text;
using Charly.SheetsToMaze.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Charly.SheetsToMaze
{
    public class ImportMazeEditor : EditorWindow
    {
        [SerializeField] MazeFileDeserialized _mazeDeserialized;
        [SerializeField] ImportFrom _importFrom;

        //todo
        public event Action<bool> IsUpToDate;

        private AutoImportNameToAssetsEditor _importNameToAssetsEditor;

        private EnumField _importFromEl;
        private PathPicker _mazePathPickerEl;
        private TextField _urlEl;
        private Button _importEl;
        private ObjectField _nameToAssetsEl;
        private Button _generateEl;
        private ScrollView _mazeDeserializedRootEl;

        [MenuItem("Windows/Charly/Import Maze Editor")]
        static void Create()
        {
            var window = GetWindow<ImportMazeEditor>();
            window.titleContent = new GUIContent("Maze Importer");
            window.Show();
        }

        private void CreateGUI()
        {
            Debug.Log(nameof(CreateGUI));
            rootVisualElement.Clear();
            Init();
        }

        private void OnFocus()
        {
            Debug.Log("on refocus");
            _importNameToAssetsEditor.OnFocus();
        }

        private void Init()
        {
            //todo make these paths come from an SO so they're not hard coded.
            const string baseUIAssetsPath = @"Assets/2_From_Sheets_To_A_Maze/UI";
            var uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(baseUIAssetsPath + "/UXML/GenerateMazeEditor.uxml");
            var uiDefaultStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(baseUIAssetsPath + "/USS/default.uss");
            DebugUtils.AreNotNotNull(uiAsset, uiDefaultStyle);
            
            var uiTemplate = uiAsset.CloneTree();
            uiTemplate.contentContainer.styleSheets.Add(uiDefaultStyle);
            rootVisualElement.Add(uiTemplate);

            _importFromEl = rootVisualElement.Q<EnumField>();
            _mazePathPickerEl = rootVisualElement.Q<PathPicker>("maze-file-picker");
            _urlEl = rootVisualElement.Q<TextField>("url");
            _importEl = rootVisualElement.Q<Button>("import-maze");
            _generateEl = rootVisualElement.Q<Button>("generate");
            _nameToAssetsEl = rootVisualElement.Q<ObjectField>("name-to-assets");
            _mazeDeserializedRootEl = rootVisualElement.Q<ScrollView>("maze-deserialized-container");

            var mazeVisualsContainer = rootVisualElement.Q<VisualElement>("import-maze-visuals");
            _importNameToAssetsEditor = new AutoImportNameToAssetsEditor();
            _importNameToAssetsEditor.Init(mazeVisualsContainer);
            
            DebugUtils.AreNotNotNull(
                _importFromEl, 
                _mazePathPickerEl,
                _urlEl,
                _importEl,
                _generateEl,
                _nameToAssetsEl,
                _mazeDeserializedRootEl);

            _importFromEl.value = _importFrom;
            //todo make this value persistent
            _importFromEl.bindingPath = nameof(_importFrom);
            _importFromEl.Bind(new SerializedObject(this));

            _nameToAssetsEl.objectType = typeof(SymbolAssetLink);
            
            OnImportValueChanged();
            _importFromEl.RegisterValueChangedCallback( _ => OnImportValueChanged());

            _importEl.clickable = new Clickable(OnImportButtonClick);
            _generateEl.clickable = new Clickable(OnGenerateButtonClick);

            _urlEl.RegisterValueChangedCallback(_ => URIChanged());
        }

        private void OnGenerateButtonClick()
        {
            var nameToAssets = _nameToAssetsEl.value as SymbolAssetLink;
            if (nameToAssets == null)
            {
                Debug.LogError($"{_nameToAssetsEl.name} must have a valid {typeof(SymbolAssetLink)} assigned.");
                return;
            }
            
            if (_mazeDeserialized == null)
            {
                Debug.LogError($"{_mazeDeserialized} must have a valid {typeof(MazeFileDeserialized)} imported.");
                return;
            }
            
            MazeGenerator.Generate(_nameToAssetsEl.value as SymbolAssetLink, _mazeDeserialized);
        }

        private void URIChanged()
        {
            bool isWellFormatted = Uri.IsWellFormedUriString(_urlEl.value, UriKind.Absolute);
            if (isWellFormatted)
                UIUtils.RemoveUnderline(_urlEl);
            else
                UIUtils.AddErrorUnderline(_urlEl);
        }

        private async void OnImportButtonClick()
        {
            string textBody = null;
            switch (_importFrom)
            {
                case ImportFrom.FileSystem:
                {
                    string path = _mazePathPickerEl.FilePath.value;
                    var task = System.IO.File.ReadAllTextAsync(path, Encoding.Default);
                    textBody = await task;
                    break;
                }
                case ImportFrom.Cloud:
                    if (!Uri.IsWellFormedUriString(_urlEl.value, UriKind.Absolute))
                    {
                        Debug.LogError(_urlEl.value + " is not a well formated URI!", this);
                        return;
                    }
                    HttpWebRequest request = WebRequest.CreateHttp(_urlEl.value);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    
                    var response = (HttpWebResponse) await request.GetResponseAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        Debug.LogWarning(response.StatusCode);

                    //"await using" explanation: https://stackoverflow.com/questions/58791938/c-sharp-8-understanding-await-using-syntax... basically the disposal itself can happen on another thread
                    await using ( var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                        textBody = await reader.ReadToEndAsync();
                    break;
            }
            
            var parser = new MazeFileParser(textBody);
            var errors = parser.Parse(out var mazeFile);
            
            if (errors != null)
            {
                bool openFile = EditorUtility.DisplayDialog("Bad Maze File", $"{errors}", "Take me there", "I'll deal with this later");
                if (openFile)
                {
                    switch (_importFrom)
                    {
                        case ImportFrom.FileSystem:
                            System.Diagnostics.Process.Start(_mazePathPickerEl.FilePath.value);
                            break;
                        case ImportFrom.Cloud:
                            Application.OpenURL(_urlEl.value);
                            break;
                    }
                }
                return;
            }
                
            _mazeDeserialized = mazeFile;
            _mazeDeserializedRootEl.Clear();
            _mazeDeserializedRootEl.Add(new MazeFileDeserializedEl(mazeFile));
        }

        void OnImportValueChanged()
        {
            var importFrom = _importFrom;
            switch (importFrom)
            {
                case ImportFrom.FileSystem:
                    _mazePathPickerEl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    _urlEl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    break;
                case ImportFrom.Cloud:
                    _mazePathPickerEl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    _urlEl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    URIChanged();
                    break;
            }
        }
    }

    public enum ImportFrom
    {
        FileSystem,
        Cloud
    }
}