using KingIT.Infrastructure;
using KingIT.ViewModels.Base;
using KingIT.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class ManagerAChoosenPageViewModel : BaseViewModel
    {
        public ManagerAChoosenPageViewModel()
        {
            MoveOnArendatorsPageCommand = new LambdaCommand(OnMoveOnArendatorsPageCommandExecuted, CanMoveOnArendatorsPageCommandExecute);
            MoveOnStatOfShopCenterPageCommand = new LambdaCommand(OnMoveOnStatOfShopCenterPageCommandExecuted, CanMoveOnStatOfShopCenterPageCommandExecute);
        }

        #region Команда перехода к интерфейсу арендаторов

        public ICommand MoveOnArendatorsPageCommand { get; }
        private bool CanMoveOnArendatorsPageCommandExecute(object d) => true;
        private void OnMoveOnArendatorsPageCommandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new ArendatorsCollectionPage();
        }

        #endregion

        #region Команда перехода к интерфейсу статистики ТЦ

        public ICommand MoveOnStatOfShopCenterPageCommand { get; }
        private bool CanMoveOnStatOfShopCenterPageCommandExecute(object d) => true;
        private void OnMoveOnStatOfShopCenterPageCommandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new StatOfShopCenterPage();
        }

        #endregion
    }
}
