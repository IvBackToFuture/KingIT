using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using KingIT.Views.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class ShopCenterInterfacePageViewModel : BaseViewModel
    {
        #region Выбранный ТЦ

        private ShopCenters _CurrentShopCenter;
        public ShopCenters CurrentShopCenter
        {
            get => _CurrentShopCenter;
            set => Set(ref _CurrentShopCenter, value);
        }

        private bool IsNewShopCenter;

        #endregion

        #region Выбранное изображение ТЦ

        private byte[] _CurrentPicture;
        public byte[] CurrentPicture
        {
            get => _CurrentPicture;
            set => Set(ref _CurrentPicture, value);
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

        #region Список всех статусов

        private ObservableCollection<string> _StatusCollection;
        public ObservableCollection<string> StatusCollection
        {
            get => _StatusCollection;
            set => Set(ref _StatusCollection, value);
        }

        #endregion

        #region Статический номер текущего ТЦ

        private static int CurrentShopCenterID;

        #endregion

        #region Метод передачи/создания нового ТЦ

        public void SetCurrentShopCenter(ShopCenters shopCenters)
        {
            if (shopCenters != null)
            {
                CurrentShopCenter = shopCenters;
                IsNewShopCenter = false;
                CurrentPicture = CurrentShopCenter.photo;
            }
            else
            {
                CurrentShopCenter = new ShopCenters() { shopCenterNumber = KingITEntities.GetContext().ShopCenters.Max(x => x.shopCenterNumber) + 1 };
                IsNewShopCenter = true;
            }
            CurrentShopCenterID = CurrentShopCenter.shopCenterNumber;
        }

        #endregion

        public ShopCenterInterfacePageViewModel()
        {
            if (CurrentShopCenterID != 0)
            {
                CurrentShopCenter = KingITEntities.GetContext().ShopCenters.Find(CurrentShopCenterID);
                IsNewShopCenter = false;
                CurrentPicture = CurrentShopCenter.photo;
            }

            CityCollection = new ObservableCollection<string>(KingITEntities.GetContext().ShopCenters.Select(x => x.city).Distinct());
            StatusCollection = new ObservableCollection<string>(KingITEntities.GetContext().ShopCenters.Select(x => x.status).Distinct());

            SaveChangesCommand = new LambdaCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
            LoadPictureCommand = new LambdaCommand(OnLoadPictureCommandExecuted, CanLoadPictureCommandExecute);
            MoveOnPavilionsCollectionPageCommand = new LambdaCommand(OnMoveOnPavilionsCollectionPageCommandExecuted, CanMoveOnPavilionsCollectionPageCommandExecute);
        }

        #region Команда сохранения изменений

        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => CanSaveShopCenterChanges();
        private void OnSaveChangesCommandExecuted(object d)
        {
            SaveCahangesShopCenter();
        }

        #endregion

        #region Команда перехода к интерфейсу списка павильонов

        public ICommand MoveOnPavilionsCollectionPageCommand { get; }
        private bool CanMoveOnPavilionsCollectionPageCommandExecute(object d) => CanSaveShopCenterChanges();
        private void OnMoveOnPavilionsCollectionPageCommandExecuted(object d)
        {
            if (SaveCahangesShopCenter())
            {
                MainWindowViewModel._CurrentViewModel.CurrentPage = new PavilionsCollectionPage();
                (MainWindowViewModel._CurrentViewModel.CurrentPage.DataContext as PavilionsCollectionPageViewModel).SetCurrentShopCenter(CurrentShopCenter);
            }
        }

        #endregion

        #region Метод сохранения изменений ТЦ
        
        private bool SaveCahangesShopCenter()
        {
            if (IsNewShopCenter)
            {
                KingITEntities.GetContext().ShopCenters.Add(CurrentShopCenter);
                IsNewShopCenter = false;
            }
            CurrentShopCenter.photo = CurrentPicture;
            try
            {
                KingITEntities.GetContext().SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //KingITEntities.GetContext().ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Modified).ToList().ForEach(x => { x.CurrentValues.SetValues(x.OriginalValues); x.State = System.Data.Entity.EntityState.Unchanged; });
                KingITEntities.GetContext().Entry(CurrentShopCenter).Reload();
                return false;
            }
        }

        #endregion

        #region Метод проверки ограничения сохранений

        private bool CanSaveShopCenterChanges()
        {
            return CurrentPicture != null &&
            !string.IsNullOrWhiteSpace(CurrentShopCenter.shopCenterName) &&
            CurrentShopCenter.valueAddedFactor > 0.1 &&
            !string.IsNullOrWhiteSpace(CurrentShopCenter.status) &&
            CurrentShopCenter.price > 0 &&
            !string.IsNullOrWhiteSpace(CurrentShopCenter.city) &&
            CurrentShopCenter.numberOfStoreys > 0 &&
            CurrentShopCenter.countOfPavilions >= 0;
        }

        #endregion

        #region Команда загрузки картинки

        public ICommand LoadPictureCommand { get; }
        private bool CanLoadPictureCommandExecute(object d) => true;
        private void OnLoadPictureCommandExecuted(object d)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files | *.BMP;*.JPG;*.PNG";
            if (fileDialog.ShowDialog() == true)
            { 
                CurrentPicture = File.ReadAllBytes(fileDialog.FileName);
            }
        }

        #endregion
    }
}
