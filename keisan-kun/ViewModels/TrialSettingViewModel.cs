using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keisan_kun.ViewModels
{
    class TrialSettingViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;

        #region NotifiableProperty

        private string _Mode;
        public string m_Mode
        {
            get { return _Mode; }
            set { this.SetProperty(ref this._Mode, value); }
        }

        #endregion

        #region Commands
        public DelegateCommand GoToTrialCommand { get; private set; }
        private bool CanGoToTrial()
        {
            // ToDo: 設定項目が設定されているかどうかの判定が必要
            return true;
        }
        #endregion

        public TrialSettingViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            CreateCommands();
        }

        private void CreateCommands()
        {
            GoToTrialCommand = new DelegateCommand(GoToTrial, CanGoToTrial);
        }

        private void GoToTrial()
        {
            //_regionManager.RequestNavigate("ContentRegion", "Trial");
            _regionManager.RequestNavigate("ContentRegion", "MultiCalc");
        }


        /// <summary>
        /// 画面遷移時にインスタンスを使い回すかどうか
        /// true: 使い回す（コンストラクタ呼ばれない）
        /// false: 使い回さない（コンストラクタ呼ばれる）
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            m_Mode = navigationContext.Parameters["mode"] as string;
        }
    }
}
