using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;
using WMPLib;
using System.Speech.Synthesis;
using keisan_kun.Model;
using Newtonsoft.Json;
using System.IO;
using Prism.Regions;

namespace keisan_kun.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        #region Commands
        public DelegateCommand GoToHomeCommand { get; private set; }
        #endregion

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.Home));

            CreateCommands();
        }

        private void CreateCommands()
        {
            GoToHomeCommand = new DelegateCommand(GoToHome);
        }

        private void GoToHome()
        {
            _regionManager.RequestNavigate("ContentRegion", "Home");
        }
    }
}
