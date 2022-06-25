using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class ArendatorPageViewModel : BaseViewModel
    {
        #region Выбранный арендатор

        private Tenants _CurrentTenant;
        public Tenants CurrentTenant
        {
            get => _CurrentTenant;
            set => Set(ref _CurrentTenant, value);
        }

        bool IsNewTenant;

        #endregion

        #region Метод передачи арендатора

        public void SetArendator(Tenants tenants)
        {
            if (tenants != null)
            {
                CurrentTenant = tenants;
                IsNewTenant = false;
            }
            else
            {
                CurrentTenant = new Tenants();
                IsNewTenant = true;
            }
        }

        #endregion

        public ArendatorPageViewModel()
        {
            SaveChangesCommand = new LambdaCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        }

        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => !string.IsNullOrWhiteSpace(CurrentTenant?.tenantName) &&
                !string.IsNullOrWhiteSpace(CurrentTenant?.adress) &&
                !string.IsNullOrWhiteSpace(CurrentTenant?.phoneNumber);
        private void OnSaveChangesCommandExecuted(object d)
        {
            if (IsNewTenant)
            {
                CurrentTenant.tenantNumber = KingITEntities.GetContext().Tenants.Max(x => x.tenantNumber) + 1;
                KingITEntities.GetContext().Tenants.Add(CurrentTenant);
                IsNewTenant = false;
            }
            KingITEntities.GetContext().SaveChanges();
            MessageBox.Show("Сохранено");
        }

    }
}
