using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using KingIT.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace KingIT.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Текущая страница

        private Page _CurrentPage;
        public Page CurrentPage
        {
            get => _CurrentPage;
            set
            {
                if (_CurrentPage != null && IsBackMove == false)
                {
                    Pages.Push(_CurrentPage.GetType().FullName);
                }
                IsBackMove = false;
                Set(ref _CurrentPage, value);
            }
        }

        #endregion

        #region Текущий контекст

        public static MainWindowViewModel _CurrentViewModel;

        #endregion

        #region Страничный стек

        private Stack<string> Pages;
        private bool IsBackMove;

        #endregion

        public MainWindowViewModel()
        {
            IsBackMove = false;
            _CurrentViewModel = this;
            Pages = new Stack<string>();
            CurrentPage = new AutorizationPage();

            ReturnOnPreviousPageCommand = new LambdaCommand(OnReturnOnPreviousPageCommandExecuted, CanReturnOnPreviousPageCommandExecute);
        }

        #region Команда возврата на предыдущую страницу //пердыдущую

        public ICommand ReturnOnPreviousPageCommand { get; }
        private bool CanReturnOnPreviousPageCommandExecute(object d) => Pages.Count() > 0;
        private void OnReturnOnPreviousPageCommandExecuted(object d)
        {
            KingITEntities.GetContext().ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Modified).ToList().ForEach(x => { x.CurrentValues.SetValues(x.OriginalValues); x.State = System.Data.Entity.EntityState.Unchanged; });
            IsBackMove = true;
            string s = Pages.Pop();
            Type type = Type.GetType(s);
            var constructor = type.GetConstructor(new Type[] { });
            CurrentPage = (Page)constructor.Invoke(new object[] { });
        }

        #endregion
    }
}
