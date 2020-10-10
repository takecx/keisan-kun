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

        private bool _IsCheckedTrial60sec = false;
        public bool m_IsCheckedTrial60sec
        {
            get { return _IsCheckedTrial60sec; }
            set { 
                this.SetProperty(ref this._IsCheckedTrial60sec, value);
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedTrial120sec = false;
        public bool m_IsCheckedTrial120sec
        {
            get { return _IsCheckedTrial120sec; }
            set { 
                this.SetProperty(ref this._IsCheckedTrial120sec, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedTrialInfinity = false;
        public bool m_IsCheckedTrialInfinity
        {
            get { return _IsCheckedTrialInfinity; }
            set { 
                this.SetProperty(ref this._IsCheckedTrialInfinity, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedPlusOperator = false;
        public bool m_IsCheckedPlusOperator
        {
            get { return _IsCheckedPlusOperator; }
            set { 
                this.SetProperty(ref this._IsCheckedPlusOperator, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedMinusOperator = false;
        public bool m_IsCheckedMinusOperator
        {
            get { return _IsCheckedMinusOperator; }
            set { 
                this.SetProperty(ref this._IsCheckedMinusOperator, value);
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedMultiplyOperator = false;
        public bool m_IsCheckedMultiplyOperator
        {
            get { return _IsCheckedMultiplyOperator; }
            set { 
                this.SetProperty(ref this._IsCheckedMultiplyOperator, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedDivisionOperator = false;
        public bool m_IsCheckedDivisionOperator
        {
            get { return _IsCheckedDivisionOperator; }
            set { 
                this.SetProperty(ref this._IsCheckedDivisionOperator, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands
        public DelegateCommand GoToTrialCommand { get; private set; }
        private bool CanGoToTrial()
        {
            return CheckIsCheckedMode() && CheckIsCheckedOperator();
        }

        private bool CheckIsCheckedMode()
        {
            return m_IsCheckedTrial60sec || m_IsCheckedTrial120sec || m_IsCheckedTrialInfinity;
        }

        private bool CheckIsCheckedOperator()
        {
            return m_IsCheckedPlusOperator || m_IsCheckedMinusOperator || m_IsCheckedMultiplyOperator || m_IsCheckedDivisionOperator;
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
        }
    }
}
