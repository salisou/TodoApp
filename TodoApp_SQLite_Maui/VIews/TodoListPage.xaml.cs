using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp_SQLite_Maui.Data;
using TodoApp_SQLite_Maui.Models;

namespace TodoApp_SQLite_Maui.VIews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage()
        {
            InitializeComponent();
        }

        protected override  async void OnAppearing()
        {
            base.OnAppearing();
            TodoItemDatabase database = await TodoItemDatabase.Instance;
            lisView.ItemsSource = await database.GetTodoItemsAsync();
        }

        private async void OnItemAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage { BindingContext = new TodoItem() });
        }

        private async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null) await Navigation.PushAsync(new TodoItemPage { BindingContext = e.SelectedItem  as TodoItem});
        }
    }
}