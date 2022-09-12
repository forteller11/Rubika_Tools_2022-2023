using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Charly.SheetsToMaze.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Charly.SheetsToMaze
{
    public class AutoImportNameToAssetsEditor
    {
        public VisualElement Root;
        public PathPicker SourceDirEl;
        public PathPicker DestDirEl;
        public ProgressBar ProgressBar;
        public Button BeginImport;
        public Label UpToDateLabel;
        
        public static string [] SOURCE_MESH_EXTENSIONS = {"fbx", "obj"};

        private List<string> _filesToReimportCache;
        private int _totalValidSrcFiles;

        public void Init(VisualElement root)
        {
            Root = root;
            
            SourceDirEl = Root.Q<PathPicker>("picker-src");
            DestDirEl = Root.Q<PathPicker>("picker-dst");
            BeginImport = Root.Q<Button>("import-visuals");
            ProgressBar = Root.Q<ProgressBar>();
            UpToDateLabel = Root.Q<Label>("is-up-to-date");

            DebugUtils.AreNotNotNull(
                SourceDirEl, 
                DestDirEl, 
                BeginImport,
                ProgressBar,
                UpToDateLabel);

            RefreshFilesToReimportAndUI();
            
            BeginImport.clickable = new Clickable(ImportFiles);
        }

        public async void RefreshFilesToReimportAndUI(bool recalculate = true)
        {
            ProgressBar.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            BeginImport.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            UpToDateLabel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

            if (recalculate)
                (_filesToReimportCache, _totalValidSrcFiles) = await GetPathsOfFilesToImport();
            BeginImport.text = $"Files To Import: {_filesToReimportCache.Count}/{_totalValidSrcFiles}";

            if (_filesToReimportCache?.Count > 0)
            {
                BeginImport.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                UpToDateLabel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                UpToDateLabel.text = $"All {_totalValidSrcFiles} files are up to date.";
            }
            
            ProgressBar.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        public async void OnFocus()
        {
            RefreshFilesToReimportAndUI();
        }
        
        public async Task<(List<string> pathsToReimport, int Length)> GetPathsOfFilesToImport()
        {
            if (!SourceDirEl.IsFolderPathValid)
            {
                Debug.LogError($"{SourceDirEl.Path} is not a valid folder path");
                return (null, 0);
            }

            if (!DestDirEl.IsFolderPathValid)
            {
                Debug.LogError($"{DestDirEl.Path} is not a valid folder path");
                return (null, 0);
            }
            
            var srcPaths = Directory.GetFiles(SourceDirEl.Path);
            var pathsToReimport = new List<string>(srcPaths.Length);

            for (var i = 0; i < srcPaths.Length; i++)
            {
                var srcPath = srcPaths[i];
                string fileName = Path.GetFileName(srcPath);

                ProgressBar.value = (float)i / srcPaths.Length;
                ProgressBar.title = $"Checking hashes: {fileName}";

                #region is file extension valid
                string ext = Path.GetExtension(srcPath).Remove(0, 1);
                bool isValidExt = false;
                foreach (string validExt in SOURCE_MESH_EXTENSIONS)
                {
                    if (ext == validExt)
                    {
                        isValidExt = true;
                        break;
                    }
                }

                if (!isValidExt)
                    continue;

                #endregion

                //check if already exists in dest, if not, then add it
                string dstPath = Path.Combine(DestDirEl.Path, fileName);
                if (!File.Exists(dstPath))
                {
                    Debug.Log(
                        $"Destination file doesn't exist for {srcPath}, so it's going to be imported for the first time");
                    pathsToReimport.Add(srcPath);
                    continue;
                }

                #region compare hashes
                int bufferLength = IOUtils.ByteToMB * 25;
                var buffer1 = ArrayPool<byte>.Shared.Rent(bufferLength);
                var srcHashTask = IOUtils.GetFileHash(srcPath, buffer1);

                var buffer2 = ArrayPool<byte>.Shared.Rent(bufferLength);
                var dstHashTask = IOUtils.GetFileHash(dstPath, buffer2);

                await Task.WhenAll(new Task[] { srcHashTask, dstHashTask });

                ArrayPool<byte>.Shared.Return(buffer1);
                ArrayPool<byte>.Shared.Return(buffer2);

                if (!srcHashTask.IsCompletedSuccessfully || !dstHashTask.IsCompletedSuccessfully)
                {
                    Debug.Log($"Something went wrong when trying to hash the files and one or Tasks timed out. Just going to reimport {srcPath} to be safe");
                    Debug.Log(srcHashTask.Exception);
                    Debug.Log(dstHashTask.Exception);
                    pathsToReimport.Add(srcPath);
                    continue;
                }
                else if (srcHashTask?.Result != dstHashTask?.Result)
                {
                    Debug.Log($"Modification detected since last import at {srcPath}");
                    pathsToReimport.Add(srcPath);
                    continue;
                }
                Debug.Log($"No changes detected at {srcPath}");
                #endregion
            }
            
            return (pathsToReimport, srcPaths.Length);
        }
        public async void ImportFiles()
        {
            if (_filesToReimportCache == null || _filesToReimportCache.Count == 0)
            {
                return;
            }

            try
            {
                int i = 0;
                foreach (var srcPath in _filesToReimportCache)
                {
                    string nameWithExt = Path.GetFileName(srcPath);
                    string dstPath = Path.Combine(DestDirEl.Path, nameWithExt);
                    string dstPathRel = AssetDBUtils.AbsoluteToRelativePath(dstPath);
                    
                    EditorUtility.DisplayProgressBar("Copying", $"{srcPath} to {dstPath}", (float) i / _filesToReimportCache.Count);
                    i++;

                    if (File.Exists(dstPath))
                        File.Delete(dstPath);
                    //todo make async
                    FileUtil.CopyFileOrDirectory(srcPath, dstPath);
                    //todo make parralal
                    AssetDatabase.ImportAsset(AssetDBUtils.AbsoluteToRelativePath(dstPathRel));
                    
                    AssetImporter dstImporter = AssetImporter.GetAtPath(dstPathRel);
                    dstImporter.SaveAndReimport();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                //todo wait for reimport to be finished or at least flush FileUtil.CopyFileOrDirectory before calling this
                RefreshFilesToReimportAndUI(false);
            }

        }

    }
}