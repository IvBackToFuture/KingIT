using KingIT.Models;
using KingIT.ViewModels.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingIT.ViewModels
{
    class TenantsPageViewModel : BaseViewModel
    {
        #region Список арендаторов

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

        public TenantsPageViewModel()
        {
            TenantsCollection = new ObservableCollection<Tenants>(KingITEntities.GetContext().Tenants);
            CurrentTenant = TenantsCollection[0];
            Printer(CurrentTenant.JSON);
            //System.Windows.MessageBox.Show("Hello");
        }

        private void Printer(JObject jobj)
        {
            foreach (JProperty k in jobj.Properties())
            {
                Trace.WriteLine(k.Name.ToString());
                Trace.WriteLine(k.Value.ToString());
            }
        }
    }
}