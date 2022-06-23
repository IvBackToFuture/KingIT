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
    class PavilionPageViewModel : BaseViewModel
    {
        #region Выбранный павильон

        private Pavilions _CurrentPavilion;
        public Pavilions CurrentPavilion
        {
            get => _CurrentPavilion;
            set => Set(ref _CurrentPavilion, value);
        }

        #endregion

        #region Булево значение новый павильон

        private bool _IsNewPavilion;
        public bool IsNewPavilion
        {
            get => _IsNewPavilion;
            set => Set(ref _IsNewPavilion, value);
        }

        #endregion

        #region Список доступных этажей

        private ObservableCollection<int> _StagesCollection;
        public ObservableCollection<int> StagesCollection
        { 
            get => _StagesCollection;
            set => Set(ref _StagesCollection, value);
        }

        #endregion

        #region Список доступных статусов

        private ObservableCollection<string> _StatusCollection;
        public ObservableCollection<string> StatusCollection
        {
            get => _StatusCollection;
            set => Set(ref _StatusCollection, value);
        }

        #endregion

        #region Метод передачи/создания нового павильона

        public void SetCurrentPavilion(Pavilions pavilions, ShopCenters shopCenter)
        {
            if (pavilions != null)
            {
                CurrentPavilion = pavilions;
                IsNewPavilion = false;
            }
            else
            {
                CurrentPavilion = new Pavilions();
                IsNewPavilion = true;
                CurrentPavilion.shopCenterNumber = shopCenter.shopCenterNumber;
            }
            StagesCollection = new ObservableCollection<int>(Enumerable.Range(1, shopCenter.numberOfStoreys));
            StatusCollection = new ObservableCollection<string>(KingITEntities.GetContext().Pavilions.Select(x => x.status).Distinct());
        }

        #endregion

        public PavilionPageViewModel()
        {
            SaveChangesCommand = new LambdaCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        }

        #region Команда сохранения изменений

        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => CurrentPavilion?.costPerSquareMeter > 0 &&
            CurrentPavilion?.area > 0 && !string.IsNullOrWhiteSpace(CurrentPavilion?.pavilionNumber) &&
            !string.IsNullOrWhiteSpace(CurrentPavilion?.status) && CurrentPavilion?.stage != 0 && CurrentPavilion?.valueAddedFactor > 0.1;
        private void OnSaveChangesCommandExecuted(object d)
        {
            try
            {

                if (IsNewPavilion)
                {
                     if (KingITEntities.GetContext().Pavilions.Find(CurrentPavilion.pavilionNumber, CurrentPavilion.shopCenterNumber) == null){

                        KingITEntities.GetContext().Pavilions.Add(CurrentPavilion);
                        IsNewPavilion = false;
                    }
                    else
                        MessageBox.Show("Не удалось добавить новый павильон");
                }

                KingITEntities.GetContext().SaveChanges();
                MessageBox.Show("Сохранено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //KingITEntities.GetContext().Entry(CurrentPavilion).Reload();
            }
        }

        #endregion
    }
}
