# .Net MAUI | Todo App | With Sqlite database 

## Install Packages

	sqlite-net-pcl
	SQLiteNetExtensions.Async
	SQLitePCLRaw.bundle_green
	SQLitePCLRaw.core
	SQLitePCLRaw.provider.dynamic_cdecl
	SQLitePCLRaw.provider.sqlite3

## Add this code to App.xaml.cs
   
    MainPage = new NavigationPage(new TodoListPage()) => BarTextColor = Color.FromRgb(255, 255, 255);
  
## Create Folder Models Add TodoItem.cs

    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }

## Create Folder Data Add TodoItemDatabase.cs

    public class TodoItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<TodoItemDatabase> Instance =
            new AsyncLazy<TodoItemDatabase>(async () =>
            {
                var instance = new TodoItemDatabase();

                try
                {
                    CreateTableResult result = await Database.CreateTableAsync<TodoItem>();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return instance;
            });

        public TodoItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabaseFileName, Constants.Flags);
        }


        public Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return Database.Table<TodoItem>().ToListAsync();
        }

        public Task<List<TodoItem>> GetTodoItemsNotDoneAsync()
        {
            return Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TodoItem item)
        {
            return Database.DeleteAsync(item);
        }

    }
    
    
## Create forlder Services Add AsyncLazy.cs
    public class AsyncLazy<T>
    {

        public AsyncLazy() 
        {
            
        }
        private readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }

        public void Start()
        {
            var unused = instance.Value;
        }
    }
    
 ## Add into folder Services this class Constants.cs
  
     public static class Constants
    {
        public const string DatabaseFileName = "TodoList.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;


        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFileName);
            }
        }
    }

## Create folder Views Add TodoItemPage.xaml

    <ContentPage.Content>
        <StackLayout Margin="20" Spacing="10" VerticalOptions="StartAndExpand">
            <Label Text="Name"/>
            <Entry Text="{Binding Name}"/>

            <Label Text="Notes"/>
            <Entry Text="{Binding Notes}"/>

            <StackLayout Orientation="Horizontal">
                <Label Text="Done" 
                       Margin="0,10" 
                       HorizontalOptions="StartAndExpand"/>
            </StackLayout>

            <Button x:Name="btnOnSave" Clicked="btnOnSave_Clicked" Text="Save"/>
            <Button x:Name="btnOnDelete" Clicked="btnOnDelete_Clicked" Text="Delete"/>
            <Button x:Name="btnOnCancel" Clicked="btnOnCancel_Clicked" Text="Cancel"/>

        </StackLayout>
    </ContentPage.Content>
    
 ## Add this code into TodoItemPage.xaml.cs
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
 
## Create folder Views Add TodoListPage.xaml
     <ContentPage.ToolbarItems>
        <ToolbarItem x:Name="OnItemAdd" Clicked="OnItemAdd_Clicked" Text="+">
            <ToolbarItem.IconImageSource>
                <OnPlatform x:TypeArguments="ImageSource">
                    <On Platform="Android,UWP" Value="add.png" />
                </OnPlatform>
            </ToolbarItem.IconImageSource>
        </ToolbarItem>
    </ContentPage.ToolbarItems>

    <ListView x:Name="lisView" Margin="20" ItemSelected="OnListItemSelected">
        <ListView.ItemTemplate>
            <DataTemplate>
                <ViewCell>
                    <StackLayout Margin="20,0,0,0" HorizontalOptions="FillAndExpand" Orientation="Horizontal">
                        <Label HorizontalOptions="StartAndExpand" Text="{Binding Name}"/>
                        <Image HorizontalOptions="End" 
                               IsVisible="{Binding Done}"
                               Source="check.png"/>
                    </StackLayout>
                </ViewCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
    
 ## Add this code into TodoListPage.xaml.cs
 
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
