using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Unity;
using Prism;
using Prism.Ioc;
using System.Windows;
using keisan_kun.Views;

namespace keisan_kun
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Home>();
            containerRegistry.RegisterForNavigation<Views.MultiCalc>();
        }
    }
}
