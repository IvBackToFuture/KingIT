using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using KingIT.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class ManagerCPageViewModel : BaseViewModel
    {
        #region Список всех ТЦ

        private ObservableCollection<ShopCenters> _ShopCentersCollection;
        public ObservableCollection<ShopCenters> ShopCentersCollection 
        {
            get => _ShopCentersCollection;
            set => Set(ref _ShopCentersCollection, value);
        }

        public void SetShopCentersCollection()
        {
            if (CurrentStatus != "Все" && CurrentCity != "Все")
                ShopCentersCollection = new ObservableCollection<ShopCenters>(KingITEntities.GetContext().ShopCenters.Where(x => x.status == CurrentStatus && x.city == CurrentCity));
            else if (CurrentStatus != "Все")
                ShopCentersCollection = new ObservableCollection<ShopCenters>(KingITEntities.GetContext().ShopCenters.Where(x => x.status == CurrentStatus).OrderBy(x => x.city));
            else if (CurrentCity != "Все")
                ShopCentersCollection = new ObservableCollection<ShopCenters>(KingITEntities.GetContext().ShopCenters.Where(x => x.city == CurrentCity).OrderBy(x => x.status));
            else
                ShopCentersCollection = new ObservableCollection<ShopCenters>(KingITEntities.GetContext().ShopCenters.Where(x => x.status != "Удален").OrderBy(x => x.city).ThenBy(x => x.status));
        }

        #endregion

        #region Выбранный ТЦ

        private ShopCenters _CurrentShopCenters;
        public ShopCenters CurrentShopCenters
        {
            get => _CurrentShopCenters;
            set => Set(ref _CurrentShopCenters, value);
        }

        #endregion

        #region Список всех статусов

        private ObservableCollection<string> _StatusCollection;
        public ObservableCollection<string> StatusCollection
        { 
            get=>_StatusCollection;
            set=>Set(ref _StatusCollection, value);
        }

        #endregion

        #region Выбранный статус

        private string _CurrentStatus;
        public string CurrentStatus
        {
            get => _CurrentStatus;
            set
            {
                Set(ref _CurrentStatus, value);
                SetShopCentersCollection();
            }
        }

        #endregion

        #region Список всех городов

        private ObservableCollection<string> _CityCollection;
        public ObservableCollection<string> CityCollection
        { 
            get => _CityCollection;
            set => Set(ref _CityCollection, value); 
        }

        #endregion

        #region Выбранный город

        private string _CurrentCity;
        public string CurrentCity
        {
            get => _CurrentCity;
            set
            {
                Set(ref _CurrentCity, value);
                SetShopCentersCollection();
            }
        }

        #endregion

        public ManagerCPageViewModel()
        {
            ShopCentersCollection = new ObservableCollection<ShopCenters>(KingITEntities.GetContext().ShopCenters.Where(x => x.status != "Удален").OrderBy(y => y.city).ThenBy(x => x.status).ToList());
            StatusCollection = new ObservableCollection<string>(ShopCentersCollection.Select(x => x.status).Distinct());
            CityCollection = new ObservableCollection<string>(ShopCentersCollection.Select(x => x.city).Distinct());
            StatusCollection.Insert(0, "Все");
            CityCollection.Insert(0, "Все");
            
            DeleteShopCenterCommand = new LambdaCommand(OnDeleteShopCenterCommandExecuted, CanDeleteShopCenterCommandExecute);
            MoveOnSCInterfaceCommand = new LambdaCommand(OnMoveOnSCInterfaceCommandExecuted, CanMoveOnSCInterfaceCommandExecute);
            MoveOnSCInterfaceWithCreateNewCommand = new LambdaCommand(OnMoveOnSCInterfaceWithCreateNewCommandExecuted);
        }

        #region Команда удаления ТЦ

        public ICommand DeleteShopCenterCommand { get; }
        private bool CanDeleteShopCenterCommandExecute(object d) => CurrentShopCenters != null;
        private void OnDeleteShopCenterCommandExecuted(object d)
        {
            CurrentShopCenters.status = "Удален";
            KingITEntities.GetContext().SaveChanges();
            ShopCentersCollection.Remove(CurrentShopCenters);
        }

        #endregion

        #region Команда перехода к интерфейсу ТЦ

        public ICommand MoveOnSCInterfaceCommand { get; }
        private bool CanMoveOnSCInterfaceCommandExecute(object d) => CurrentShopCenters != null;
        private void OnMoveOnSCInterfaceCommandExecuted(object d)
        {
            //Переход к интерфейсу выбранного ТЦ
            MainWindowViewModel._CurrentViewModel.CurrentPage = new ShopCenterInterfacePage();
            (MainWindowViewModel._CurrentViewModel.CurrentPage.DataContext as ShopCenterInterfacePageViewModel).SetCurrentShopCenter(CurrentShopCenters);
        }

        public ICommand MoveOnSCInterfaceWithCreateNewCommand { get; }
        private void OnMoveOnSCInterfaceWithCreateNewCommandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new ShopCenterInterfacePage();
            (MainWindowViewModel._CurrentViewModel.CurrentPage.DataContext as ShopCenterInterfacePageViewModel).SetCurrentShopCenter(null);
        }

        #endregion
    }
}
