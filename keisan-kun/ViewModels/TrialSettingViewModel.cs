using keisan_kun.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace keisan_kun.ViewModels
{
    class TrialSettingViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;

        #region NotifiableProperty

        private bool _IsCheckedTrial1m = false;
        public bool m_IsCheckedTrial1m
        {
            get { return _IsCheckedTrial1m; }
            set { 
                this.SetProperty(ref this._IsCheckedTrial1m, value);
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedTrial3m = false;
        public bool m_IsCheckedTrial3m
        {
            get { return _IsCheckedTrial3m; }
            set { 
                this.SetProperty(ref this._IsCheckedTrial3m, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedTrial5m = false;
        public bool m_IsCheckedTrial5m
        {
            get { return _IsCheckedTrial5m; }
            set { 
                this.SetProperty(ref this._IsCheckedTrial5m, value); 
                GoToTrialCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _IsCheckedTrial10m = false;
        public bool m_IsCheckedTrial10m
        {
            get { return _IsCheckedTrial10m; }
            set { 
                this.SetProperty(ref this._IsCheckedTrial10m, value); 
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

        private bool _IsCarryUpChecked = false;
        public bool m_IsCarryUpChecked
        {
            get { return _IsCarryUpChecked; }
            set { this.SetProperty(ref this._IsCarryUpChecked, value); }
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
            return m_IsCheckedTrial1m || m_IsCheckedTrial3m || m_IsCheckedTrial5m || m_IsCheckedTrial10m || m_IsCheckedTrialInfinity;
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
            var navigationParams = new NavigationParameters();
            navigationParams.Add("OperatorType", GenBinaryOperatorType(m_IsCheckedPlusOperator,m_IsCheckedMinusOperator,m_IsCheckedMultiplyOperator,m_IsCheckedDivisionOperator));
            navigationParams.Add("LimitationTime", GenLimitationTime(m_IsCheckedTrial1m, m_IsCheckedTrial3m, m_IsCheckedTrial5m, m_IsCheckedTrial10m, m_IsCheckedTrialInfinity));
            navigationParams.Add("QuestionType", GenQuestionType());
            _regionManager.RequestNavigate("ContentRegion", "BinaryOperation",navigationParams);
        }

        private QuestionType GenQuestionType()
        {
            if (m_IsCarryUpChecked == false && m_IsCheckedPlusOperator)
            {
                return QuestionType.add_single_no_carryup;
            }
            else if (m_IsCarryUpChecked == true && m_IsCheckedPlusOperator)
            {
                return QuestionType.add_single_carryup;
            }
            else if (m_IsCarryUpChecked == false && m_IsCheckedMinusOperator)
            {
                return QuestionType.sub_single_no_carrydown;
            }
            else if (m_IsCarryUpChecked == true && m_IsCheckedMinusOperator)
            {
                return QuestionType.sub_single_carrydown;
            }
            else
            {
                throw new Exception();
            }
        }

        private int GenLimitationTime(bool m_IsCheckedTrial1m, bool m_IsCheckedTrial3m, bool m_IsCheckedTrial5m, bool m_IsCheckedTrial10m, bool m_IsCheckedTrialInfinity)
        {
            if (m_IsCheckedTrial1m) return 60;
            else if (m_IsCheckedTrial3m) return 180;
            else if (m_IsCheckedTrial5m) return 300;
            else if (m_IsCheckedTrial10m) return 600;
            else if (m_IsCheckedTrialInfinity) return -1;
            else throw new ArgumentException();
        }

        private BinaryOperationType GenBinaryOperatorType(bool isCheckedPlusOperator, bool isCheckedMinusOperator, bool isCheckedMultiplyOperator, bool isCheckedDivisionOperator)
        {
            if (isCheckedPlusOperator) return BinaryOperationType.plus;
            else if (isCheckedMinusOperator) return BinaryOperationType.minus;
            else if (isCheckedMultiplyOperator) return BinaryOperationType.multiply;
            else if (isCheckedDivisionOperator) return BinaryOperationType.division;
            else throw new ArgumentException();
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
