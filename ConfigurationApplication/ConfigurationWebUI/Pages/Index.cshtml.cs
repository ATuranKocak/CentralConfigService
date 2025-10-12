using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Net.Http; 
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ConfigurationWebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ConfigurationItem>? Configurations { get; set; }
        
        // Form verilerini otomatik olarak bağlar
        [BindProperty] 
        public ConfigurationItem EditItem { get; set; } = new();
        
        public bool IsEditMode { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Listeleme (GET)
        public async Task OnGetAsync()
        {
            await LoadConfigurationsAsync();
        }

        public async Task<IActionResult> OnPostAdd()
        {
            var client = _httpClientFactory.CreateClient("configApi");

            //Mevcut listedeki en büyük ID'yi bulup +1 ver
            await LoadConfigurationsAsync(); // mevcut kayıtları getir
            if (EditItem.Id == 0)
            {
                var maxId = Configurations != null && Configurations.Any() ? Configurations.Max(x => x.Id): 0;
                EditItem.Id = maxId + 1;
            }

            var response = await client.PostAsJsonAsync("configurations", EditItem);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            await LoadConfigurationsAsync();
            return Page();
        }
        
        //  Silme (DELETE)
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("configApi");
            var response = await client.DeleteAsync($"configurations/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                 //Formu temizlemek için
                 return RedirectToPage();
            }

            await LoadConfigurationsAsync();
            return Page();
        }

        // Düzenleme Formunu Doldur (Edit butonu)
        // Dönüş tipi Page() döndürmek için Task<IActionResult> olmalı
        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            await LoadConfigurationsAsync();
            EditItem = Configurations?.FirstOrDefault(x => x.Id == id) ?? new ConfigurationItem();
            IsEditMode = true;
            
            return Page();
        }

        //  Güncelleme (PUT)
        public async Task<IActionResult> OnPostUpdate()
        {
            var client = _httpClientFactory.CreateClient("configApi");

            var response = await client.PutAsJsonAsync($"configurations/{EditItem.Id}", EditItem);
            
            if (response.IsSuccessStatusCode)
            {
                //Formu temizlemek için 
                return RedirectToPage(); 
            }
            
            await LoadConfigurationsAsync();
            return Page();
        }

        public IActionResult OnPostCancelEdit()
        {
            return RedirectToPage();
        }

        private async Task LoadConfigurationsAsync()
        {
            var client = _httpClientFactory.CreateClient("configApi");

            try
            {
                var taskA = client.GetFromJsonAsync<List<ConfigurationItem>>("configurations/SERVICE-A");
                var taskB = client.GetFromJsonAsync<List<ConfigurationItem>>("configurations/SERVICE-B");

                await Task.WhenAll(taskA, taskB);

                Configurations = new List<ConfigurationItem>();
                if (taskA.Result != null)
                    Configurations.AddRange(taskA.Result);

                if (taskB.Result != null)
                    Configurations.AddRange(taskB.Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("API bağlantı hatası: " + ex.Message);
                Configurations = new List<ConfigurationItem>();
            }
        }

    }

    public class ConfigurationItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Value { get; set; } = "";
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; } = "";
    }
}