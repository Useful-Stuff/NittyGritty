﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NittyGritty.Platform.Files
{
    public interface IFolder
    {
        string Name { get; }

        string Path { get; }

        Task<IFile> GetFile(string name);
        Task<IFolder> GetFolder(string name);
    }
}