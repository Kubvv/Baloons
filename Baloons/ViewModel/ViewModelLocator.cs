using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;

namespace Baloons.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<MainViewModel>()
                .BuildServiceProvider());
        }

        public MainViewModel MainInstance => Ioc.Default.GetService<MainViewModel>() ??
            throw new Exception(string.Format(Consts.ServiceNotFound, nameof(MainViewModel)));

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}