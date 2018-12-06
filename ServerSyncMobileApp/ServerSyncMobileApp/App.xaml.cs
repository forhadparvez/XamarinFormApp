using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Plugin.Connectivity;
using ServerSyncMobileApp.Persistence;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ServerSyncMobileApp
{
    public partial class App : Application
    {
        // sqlite
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Product> _products;

        // api
        private const string Url = "http://192.168.0.111/api/products";
        private HttpClient _client = new HttpClient();

        public App()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            try
            {
               var isConnected= CrossConnectivity.Current.IsConnected;
                if (isConnected)
                {
                    await SqLiteToServerSync();
                }
            }
            catch (Exception ex)
            {
                
            }

            // connectivity change
            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                try
                {
                    if (args.IsConnected)
                    {
                        await SqLiteToServerSync();
                    }
                }
                catch (Exception ex)
                {
                    
                }
            };

        }

        private async Task SqLiteToServerSync()
        {
            // get form SqLite
            await _connection.CreateTableAsync<Product>();
            var products = await _connection.Table<Product>().ToListAsync();
            _products = new ObservableCollection<Product>(products);

            foreach (var product in _products)
            {
                // api insert
                var context = JsonConvert.SerializeObject(product,
                    new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
                var stringContent = new StringContent(context, Encoding.UTF8, "application/json");
                await _client.PostAsync(Url, stringContent);

                // delete SqLite
                await _connection.DeleteAsync(product);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
