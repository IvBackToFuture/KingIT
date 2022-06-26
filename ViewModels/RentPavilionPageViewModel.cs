using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    public class RentPavilionPageViewModel : BaseViewModel
    {
        #region Даты начала и конца

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get => _StartDate;
            set => Set(ref _StartDate, value);
        }

        private DateTime _StopDate;
        public DateTime StopDate
        {
            get => _StopDate;
            set => Set(ref _StopDate, value);
        }

        #endregion

        #region Список всех Арендаторов

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

        #region Выбранный павильон

        private Pavilions Pavilion;

        #endregion

        #region Метод передачи павильона

        public void SetPavilion(Pavilions pavilions)
        {
            Pavilion = pavilions;
        }

        #endregion

        public RentPavilionPageViewModel()
        {
            StartDate = DateTime.Today;
            StopDate = DateTime.Today;
            TenantsCollection = new ObservableCollection<Tenants>(KingITEntities.GetContext().Tenants);
            RentBronePavilionCommand = new LambdaCommand(OnRentBronePavilionCommandExecuted, CanRentBronePavilionCommandExecute);
        }

        #region Команда Аренды/Бронирования павильона

        public ICommand RentBronePavilionCommand { get; }
        private bool CanRentBronePavilionCommandExecute(object d) => StopDate >= StartDate && StartDate >= DateTime.Today;
        private void OnRentBronePavilionCommandExecuted(object d)
        {
            bool statusAction = StartDate > DateTime.Today;
            try
            {
                KingITEntities.GetContext().RentOrBookPavilionInMall(statusAction, Pavilion.pavilionNumber, Pavilion.shopCenterNumber, StartDate, StopDate, CurrentTenant.tenantNumber, MainWindowViewModel.InsertedEmployee);
                MessageBox.Show(statusAction ? "Забронировано" : "Арендовано");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        #endregion

        #region Метод для тестирования

        public static string CheckCanRentBronePavilion(DateTime StartDate, DateTime StopDate)
        {
            if (!(StopDate >= StartDate && StartDate >= DateTime.Today))
                return "Неверный формат ввода даты";
            return StartDate > DateTime.Today ? "Забронировано" : "Арендовано";
        }

        #endregion
    }
}
