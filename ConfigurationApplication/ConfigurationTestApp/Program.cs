using ConfigurationLibrary;

var configReader = new ConfigurationReader(
    "SERVICE-A", 
    "mongodb://mongo:27017", 
    10000 // 10 saniyede bir güncelle
);

string siteName = configReader.GetValue<string>("SiteName");

Console.WriteLine($"SiteName değeri: {siteName}");
