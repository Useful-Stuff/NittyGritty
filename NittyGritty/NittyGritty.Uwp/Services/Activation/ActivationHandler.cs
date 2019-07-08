﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace NittyGritty.Uwp.Services.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }

    public class ActivationHandler<T> : IActivationHandler
        where T : class
    {
        public virtual async Task HandleAsync(T args)
        {
            await Task.CompletedTask;
        }

        public virtual bool CanHandle(T args)
        {
            return args != null;
        }

        bool IActivationHandler.CanHandle(object args)
        {
            return CanHandle(args as T);
        }

        async Task IActivationHandler.HandleAsync(object args)
        {
            await HandleAsync(args as T);
        }
    }
}
