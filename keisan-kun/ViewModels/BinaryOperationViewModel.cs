using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keisan_kun.ViewModels
{
    public enum BinaryOperationType
    {
        /// <summary>
        /// +
        /// </summary>
        plus,
        /// <summary>
        /// -
        /// </summary>
        minus,
        /// <summary>
        /// *
        /// </summary>
        multiply,
        /// <summary>
        /// /
        /// </summary>
        division,
    }
    class BinaryOperationViewModel : BindableBase, INavigationAware
    {
        private BinaryOperationType _BinaryOperationType;
        private RegionManager _RegionManager;

        #region NotifiablePropery

        #endregion

        public BinaryOperationViewModel(RegionManager regionManager)
        {
            _RegionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var opeType = navigationContext.Parameters["operatorType"];
            if(opeType != null) 
            { 
                _BinaryOperationType =  (BinaryOperationType)opeType;
            }
        }
    }
}
