using KingIT.Models;
using KingIT.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace KingIT.ViewModels
{
    class StatOfShopCenterPageViewModel : BaseViewModel
    {
        #region Список торговых центров

        private ObservableCollection<ShopCenters> _ShopCentersCollection;
        public ObservableCollection<ShopCenters> ShopCentersCollection
        {
            get => _ShopCentersCollection;
            set => Set(ref _ShopCentersCollection, value);
        }

        #endregion

        #region Выбранный торговый центр

        private ShopCenters _CurrentShopCenter;
        public ShopCenters CurrentShopCenter
        {
            get => _CurrentShopCenter;
            set
            {
                Set(ref _CurrentShopCenter, value); 
                StatOfTTs = KingITEntities.GetContext().GetStatOfTTS(_CurrentShopCenter.shopCenterNumber).FirstOrDefault();
            }
        }

        #endregion

        #region Статистика

        private StatOfTTS_Result _StatOfTTs;
        public StatOfTTS_Result StatOfTTs
        {
            get => _StatOfTTs;
            set => Set(ref _StatOfTTs, value);
        }

        #endregion

        public StatOfShopCenterPageViewModel()
        {
            ShopCentersCollection = new ObservableCollection<ShopCenters>(KingITEntities.GetContext().ShopCenters);
        }
    }
}
