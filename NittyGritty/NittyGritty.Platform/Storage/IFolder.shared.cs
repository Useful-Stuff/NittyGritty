﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NittyGritty.Platform.Storage
{
    public interface IFolder : IStorageItem
    {
        Task<IFile> GetFile(string name);        Task<IReadOnlyList<IFile>> GetFiles();
        Task<IFolder> GetFolder(string name);        Task<IReadOnlyList<IFolder>> GetFolders();
    }
}
