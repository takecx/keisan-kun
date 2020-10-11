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
