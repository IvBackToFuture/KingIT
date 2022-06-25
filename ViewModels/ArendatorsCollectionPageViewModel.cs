using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using KingIT.Views;
using KingIT.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class ArendatorsCollectionPageViewModel : BaseViewModel
    {
        #region Поле динамического поиска

        private string _SearchString;
        public string SearchString
        {
            get => _SearchString;
            set
            {
                Set(ref _SearchString, value);
                TenantsCollection = new ObservableCollection<Tenants>(AllTenantsCollection.Where(x => x.tenantName.Contains(_SearchString)));
            }
        }

        #endregion

        #region Коллекция арендаторов

        private ObservableCollection<Tenants> AllTenantsCollection;

        private ObservableCollection<Tenants> _TenantsCollection;
        public ObservableCollection<Tenants> TenantsCollection 
        { 
            get => _TenantsCollection;
            set => Set(ref _TenantsCollection, value); 
        }

        #endregion

        #region Выбранный арендатор

        private Tenants _CurrentTenant;
        public Tenants CurrentTenant
        {
            get => _CurrentTenant;
            set => Set(ref _CurrentTenant, value);
        }

        #endregion

        public ArendatorsCollectionPageViewModel()
        {
            AddNewArendatorCommmand = new LambdaCommand(OnAddNewArendatorCommmandExecuted, CanAddNewArendatorCommmandExecute);
            UpdateArendatorCommand = new LambdaCommand(OnUpdateArendatorCommandExecuted, CanUpdateArendatorCommandExecute);
            AllTenantsCollection = new ObservableCollection<Tenants>(KingITEntities.GetContext().Tenants.ToList());
            TenantsCollection = AllTenantsCollection;
        }

        #region Команда Добавления нового арендатора

        public ICommand AddNewArendatorCommmand { get; }
        private bool CanAddNewArendatorCommmandExecute(object d) => true;
        private void OnAddNewArendatorCommmandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new ArendatorPage();
            (MainWindowViewModel._CurrentViewModel.CurrentPage.DataContext as ArendatorPageViewModel).SetArendator(null);
        }

        #endregion

        #region Команда изменения арендатора

        public ICommand UpdateArendatorCommand { get; }
        private bool CanUpdateArendatorCommandExecute(object d) => CurrentTenant != null;
        private void OnUpdateArendatorCommandExecuted(object d)
        {
            MainWindowViewModel._CurrentViewModel.CurrentPage = new ArendatorPage();
            (MainWindowViewModel._CurrentViewModel.CurrentPage.DataContext as ArendatorPageViewModel).SetArendator(CurrentTenant);
        }

        #endregion
    }
}
