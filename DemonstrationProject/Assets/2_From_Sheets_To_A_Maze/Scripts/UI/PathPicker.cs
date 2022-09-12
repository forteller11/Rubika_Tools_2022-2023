using System;
using System.Collections.Generic;
using System.IO;
using Charly.SheetsToMaze.Utils;
using UnityEditor;
using UnityEngine.UIElements;

namespace Charly.SheetsToMaze
{
    public class PathPicker : VisualElement
    {
        public Button OpenFileExplorer;
        public TextField FilePath;
        public string Path => FilePath?.value ?? String.Empty;
        public bool IsPathValid { get; private set; }
        
        public bool IsFolderPathValid => IsPathValid && PickerType == PathPickerType.Directory;
        public bool IsFilePathValid => IsPathValid && PickerType == PathPickerType.File;

        private void OnClicked()
        {
            string initialPath = string.Empty;
            if (!String.IsNullOrWhiteSpace(FilePath.value))
            {
                initialPath = Directory.GetParent(FilePath.value)?.FullName ?? string.Empty;
            }

            string path = null;
            switch (PickerType)
            {
                case PathPickerType.File:
                    path = EditorUtility.OpenFilePanelWithFilters(PickerTitle, initialPath, new[] { PickerFileType, PickerExtensions });
                    break;
                case PathPickerType.Directory:
                {
                    path = EditorUtility.OpenFolderPanel(PickerTitle, initialPath, String.Empty);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!String.IsNullOrWhiteSpace(path))
            {
                FilePath.value = path;
            }
        }

        private void FilePathChanged()
        {
            IsPathValid = PickerType switch
            {
                PathPickerType.File => File.Exists(FilePath.value),
                PathPickerType.Directory => Directory.Exists(FilePath.value),
                _ => false
            };

            if (IsPathValid) 
                UIUtils.RemoveUnderline(FilePath);
            else 
                UIUtils.AddErrorUnderline(FilePath);
        }

        public PathPickerType PickerType { get; set; }
        public string DefaultPath {get; set;}
        public string PathLabel {get; set;}
        public string PickerLabel {get; set;}
        public string PickerTitle {get; set;}
        public string PickerFileType {get; set;}
        public string PickerExtensions {get; set;}

        public enum PathPickerType
        {
            File,
            Directory
        }

        public new class UxmlFactory : UxmlFactory<PathPicker, UxmlTraits>{}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlEnumAttributeDescription<PathPickerType> _pathPickerType = new() { name =  UIUtils.CamelToDash(nameof(PickerType)), defaultValue = PathPickerType.File };
            private UxmlStringAttributeDescription _filePathLabel = new() { name = UIUtils.CamelToDash(nameof(PathLabel)), defaultValue = "Path" };
            private UxmlStringAttributeDescription _defaultFilePath = new() { name = UIUtils.CamelToDash(nameof(DefaultPath)), defaultValue = string.Empty };
            private UxmlStringAttributeDescription _filePickerLabel = new() { name =  UIUtils.CamelToDash(nameof(PickerLabel)), defaultValue = "Open Explorer" };
            private UxmlStringAttributeDescription _filePickerTitle = new() { name =  UIUtils.CamelToDash(nameof(PickerTitle)), defaultValue = "Choose a File" };
            private UxmlStringAttributeDescription _filePickerFileType = new() { name =  UIUtils.CamelToDash(nameof(PickerFileType)), defaultValue = "all" };
            private UxmlStringAttributeDescription _filePickerFileExtensions = new() { name =  UIUtils.CamelToDash(nameof(PickerExtensions)), defaultValue = "*" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var root = (PathPicker) ve;
                root.Clear();

                root.PathLabel = _filePathLabel.GetValueFromBag(bag, cc);
                root.DefaultPath = _defaultFilePath.GetValueFromBag(bag, cc);
                root.PickerLabel = _filePickerLabel.GetValueFromBag(bag, cc);
                root.PickerTitle = _filePickerTitle.GetValueFromBag(bag, cc);
                root.PickerFileType = _filePickerFileType.GetValueFromBag(bag, cc);
                root.PickerExtensions = _filePickerFileExtensions.GetValueFromBag(bag, cc);
                root.PickerType = _pathPickerType.GetValueFromBag(bag, cc);
                
                var path = new TextField()
                {
                    label = root.PathLabel,
                    value = root.DefaultPath,
                    viewDataKey = "file-picker-file-path"
                };
                var explorer = new Button()
                {
                    text = root.PickerLabel,
                    style = {
                        marginRight = 8
                    }
                };
                
                var pickerRow = new VisualElement {
                    style = {
                        flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row)
                        
                    }
                };
                var pickerSpacer = new VisualElement {
                    style = {
                        flexGrow = 10f 
                    }
                };

                root.Add(path);
                root.Add(pickerRow);
                    pickerRow.Add(pickerSpacer);
                    pickerRow.Add(explorer);

                root.FilePath = path;
                root.OpenFileExplorer = explorer;
                
                root.OpenFileExplorer.clickable = new Clickable(root.OnClicked);
                root.FilePath.RegisterValueChangedCallback(_ => root.FilePathChanged());
                root.FilePathChanged();
            }
        }
    }
}