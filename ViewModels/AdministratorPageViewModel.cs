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
    class AdministratorPageViewModel : BaseViewModel
    {
        public AdministratorPageViewModel()
        {
            MoveOnAddTenantsDataCommand = new LambdaCommand(OnMoveOnAddTenantsDataCommandExecuted, CanMoveOnAddTenantsDataCommandExecute);
            MoveOnShowsTenantsDataCommand = new LambdaCommand(OnMoveOnShowsTenantsDataCommandExecuted, CanMoveOnShowsTenantsDataCommandExecute);
        }

        #region Команда перехода к интерфейсу просмотра данных Арендаторов

        public ICommand MoveOnShowsTenantsDataCommand { get; }
        private bool CanMoveOnShowsTenantsDataCommandExecute(object d) => true;
        private void OnMoveOnShowsTenantsDataCommandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new TenantsPage();
        }

        #endregion

        #region Команда перехода к интерфейсу добавления Арендаторов

        public ICommand MoveOnAddTenantsDataCommand { get; }
        private bool CanMoveOnAddTenantsDataCommandExecute(object d) => true;
        private void OnMoveOnAddTenantsDataCommandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new System.Windows.Controls.Page();
        }

        #endregion
    }
}
