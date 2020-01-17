﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NittyGritty.Platform.Files
{
    public class NGFolder : IFolder
    {
        private readonly IStorageFolder folder;

        public NGFolder(IStorageFolder folder)
        {
            this.folder = folder;
        }

        public string Name { get { return folder.Name; } }

        public string Path { get { return folder.Path; } }

        public async Task<IFile> GetFile(string name)
        {
            var file = await folder.GetFileAsync(name);
        }

        public async Task<IReadOnlyList<IFile>> GetFiles()
        {
            var ngFiles = new List<IFile>();
        }

        public async Task<IFolder> GetFolder(string name)
        {
            var f = await folder.GetFolderAsync(name);
        }

        public async Task<IReadOnlyList<IFolder>> GetFolders()
        {
            var ngFolders = new List<IFolder>();
        }
    }
}