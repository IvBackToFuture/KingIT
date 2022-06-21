using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class PavilionsCollectionPageViewModel : BaseViewModel
    {
        #region Выбранный ТЦ

        private ShopCenters _CurrentShopCenter;
        public ShopCenters CurrentShopCenter
        {
            get => _CurrentShopCenter;
            set => Set(ref _CurrentShopCenter, value);
        }

        #endregion

        #region Список павильонов

        private ObservableCollection<Pavilions> _PavilionsCollection;
        public ObservableCollection<Pavilions> PavilionsCollection
        {
            get => _PavilionsCollection;
            set => Set(ref _PavilionsCollection, value);
        }

        #endregion

        #region Выбранный павильон

        private Pavilions _CurrentPavilions;
        public Pavilions CurrentPavilions
        {
            get => _CurrentPavilions;
            set => Set(ref _CurrentPavilions, value);
        }

        #endregion

        #region Список этажей

        private ObservableCollection<string> _StagesList;
        public ObservableCollection<string> StagesList
        {
            get => _StagesList;
            set => Set(ref _StagesList, value);
        }

        #endregion

        #region Выбранный этаж

        private string _CurrentStages;
        public string CurrentStages
        {
            get => _CurrentStages;
            set
            {
                Set(ref _CurrentStages, value);
                SearchCollecton();
            }
        }

        #endregion

        #region Список статусов

        private ObservableCollection<string> _StatusList;
        public ObservableCollection<string> StatusList
        {
            get => _StatusList;
            set => Set(ref _StatusList, value);
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
                SearchCollecton();
            }
        }

        #endregion

        #region Значения площади "от" и "до"

        private double _FromArea;
        public double FromArea
        {
            get => _FromArea;
            set
            {
                Set(ref _FromArea, value);
                SearchCollecton();
            }
        }

        private double _ToArea;
        public double ToArea
        {
            get => _ToArea;
            set
            {
                Set(ref _ToArea, value);
                SearchCollecton();
            }
        }

        #endregion

        #region Статический номер ТЦ

        private static int ShopCenterNumber;

        #endregion

        #region Метод передачи ТЦ

        public void SetCurrentShopCenter(ShopCenters shopCenter)
        {
            CurrentShopCenter = shopCenter;
            ShopCenterNumber = CurrentShopCenter.shopCenterNumber;
            var listOfPavilions = CurrentShopCenter.Pavilions.Where(x => x.status != "Удален");
            var stages = listOfPavilions.Select(x => x.stage).Distinct().OrderBy(x => x).Select(x => x.ToString()).ToList();
            var status = listOfPavilions.Select(x => x.status).Distinct().OrderBy(x => x).ToList();
            stages.Insert(0, "Все");
            status.Insert(0, "Все");
            StagesList = new ObservableCollection<string>(stages);
            StatusList = new ObservableCollection<string>(status);
            if (listOfPavilions.Count() > 0)
            {
                FromArea = listOfPavilions.Min(x => x.area);
                ToArea = listOfPavilions.Max(x => x.area);
            }
        }

        #endregion

        #region Метод поиска

        private void SearchCollecton()
        {
            List<Pavilions> result = CurrentShopCenter.Pavilions.Where(x => x.area >= FromArea && x.area <= ToArea && x.status != "Удален").ToList();
            List<Pavilions> stagesPav = null;
            List<Pavilions> statusPav = null;

            if (CurrentStages != "Все" && !string.IsNullOrWhiteSpace(CurrentStages))
            {
                int stage = int.Parse(CurrentStages);
                stagesPav = result.Where(x => x.stage == stage).ToList();
            }
            if (CurrentStatus != "Все" && !string.IsNullOrWhiteSpace(CurrentStatus))
                statusPav = result.Where(x => x.status == CurrentStatus).ToList();
            
            if (stagesPav != null)
                result = result.Join(stagesPav, x => x.area, x => x.area, (x, y) => x).ToList();
            if (statusPav != null)
                result = result.Join(statusPav, x => x.area, x => x.area, (x, y) => x).ToList();

            PavilionsCollection = new ObservableCollection<Pavilions>(result);
        }

        #endregion

        public PavilionsCollectionPageViewModel()
        {
            if (ShopCenterNumber != 0)
                SetCurrentShopCenter(KingITEntities.GetContext().ShopCenters.Find(ShopCenterNumber));

            DeletePavilionsCommand = new LambdaCommand(OnDeletePavilionsCommandExecuted, CanDeletePavilionsCommandExecute);
            MoveOnPavilionInterfaceCommand = new LambdaCommand(OnMoveOnPavilionInterfaceCommandExecuted, CanMoveOnPavilionInterfaceCommandExecute);
            MoveOnPavilionInterfaceWithCreateNewCommand = new LambdaCommand(OnMoveOnPavilionInterfaceCommandExecuted);
        }

        #region Команда удаления павильона

        public ICommand DeletePavilionsCommand { get; }
        private bool CanDeletePavilionsCommandExecute(object d) => CurrentPavilions != null;
        private void OnDeletePavilionsCommandExecuted(object d)
        {
            CurrentPavilions.status = "Удален";
            KingITEntities.GetContext().SaveChanges();
            PavilionsCollection.Remove(CurrentPavilions);
        }

        #endregion

        #region Команда перехода к интерфейсу павильона

        public ICommand MoveOnPavilionInterfaceCommand { get; }
        private bool CanMoveOnPavilionInterfaceCommandExecute(object d) => CurrentPavilions != null;
        private void OnMoveOnPavilionInterfaceCommandExecuted(object d)
        {
            //Переход к интерфейсу выбранного ТЦ
            //MainWindowViewModel._CurrentViewModel.CurrentPage = new ShopCenterInterfacePage();
            //(MainWindowViewModel._CurrentViewModel.CurrentPage.DataContext as ShopCenterInterfacePageViewModel).SetCurrentShopCenter(CurrentShopCenters);
        }

        public ICommand MoveOnPavilionInterfaceWithCreateNewCommand { get; }

        #endregion
    }
}
