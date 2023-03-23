using TodoApp_SQLite_Maui.Data;
using TodoApp_SQLite_Maui.VIews;

namespace TodoApp_SQLite_Maui;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new TodoListPage())
		{
			BarTextColor = Color.FromRgb(255, 255, 255)
		};
	}

}
