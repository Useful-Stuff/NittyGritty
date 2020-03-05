﻿using NittyGritty.Platform.Payloads;
using NittyGritty.Uwp;
using NittyGritty.Uwp.Activation;
using NittyGritty.Uwp.Activation.Operations;
using NittyGritty.Uwp.Activation.Operations.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NittyGritty.Sample.Uwp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : NGApp
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            DefaultView = typeof(MainPage);
        }

        public override Frame GetNavigationContext()
        {
            return Window.Current.Content as Frame;
        }

        public override IEnumerable<IActivationHandler> GetActivationHandlers()
        {
            yield return new LaunchActivationHandler(
                new PrimaryLaunchOperation(typeof(MainPage)));

            yield return new ProtocolActivationHandler(
                new ProtocolOperation("nittygritty-sample",
                    new ProtocolConfiguration("*", new MultiViewConfiguration<ProtocolPayload>(typeof(ProtocolPage)))
                //new ProtocolConfiguration("newwindow", new MultiViewConfiguration<ProtocolPayload>(null)) // TODO: update nittygritty to use view selector
                )
            );
        }
    }
}
