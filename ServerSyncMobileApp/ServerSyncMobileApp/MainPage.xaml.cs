using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServerSyncMobileApp.Persistence;
using SQLite;
using Xamarin.Forms;

namespace ServerSyncMobileApp
{
    public partial class MainPage : ContentPage
    {
        // sqlite
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Product> _products;

        // api
        private const string Url = "http://192.168.0.111/api/products";
        private HttpClient _client = new HttpClient();
        private ObservableCollection<Product> _posts;
        public MainPage()
        {
            InitializeComponent();

            _connection=DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            try
            {
                // sqlite
                await _connection.CreateTableAsync<Product>();
                var products = await _connection.Table<Product>().ToListAsync();
                _products = new ObservableCollection<Product>(products);
                //productListView.ItemsSource = _products;

                // api
                var context = await _client.GetStringAsync(Url);
                var posts = JsonConvert.DeserializeObject<List<Product>>(context);
                _posts = new ObservableCollection<Product>(posts);
                productListView.ItemsSource = _posts;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Server Error";
            }

            base.OnAppearing();
        }


        private async void OnAdd(object sender, EventArgs e)
        {
            // sqlite
            var product = new Product(){Name = name.Text};
            await _connection.InsertAsync(product);
            _products.Add(product);
            //productListView.ItemsSource = _products;

            try
            {
                // api
                var dto = new ProductDto() {Id = 0, Name = name.Text};
                var context = JsonConvert.SerializeObject(dto, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                var stringContent = new StringContent(context, Encoding.UTF8, "application/json");
                await _client.PostAsync(Url, stringContent);
                //productListView.ItemsSource = _posts;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Server Error Save Fail";
            }
        }
    }

    [Table("Products")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
