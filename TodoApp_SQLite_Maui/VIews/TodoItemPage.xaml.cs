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
    public partial class TodoItemPage : ContentPage
    {
        public TodoItemPage()
        {
            InitializeComponent();
        }

        private async void btnOnSave_Clicked(object sender, EventArgs e)
        {
            var todoSvae = (TodoItem)BindingContext;
            TodoItemDatabase database = await TodoItemDatabase.Instance;
            await database.SaveItemAsync(todoSvae);
            await Navigation.PopAsync();
        }


        private async void btnOnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnOnDelete_Clicked(object sender, EventArgs e)
        {
            var todoDelet = (TodoItem)BindingContext;
            TodoItemDatabase database = await TodoItemDatabase.Instance;
            await database.SaveItemAsync(todoDelet);
            await Navigation.PopAsync();
        }
    }
}