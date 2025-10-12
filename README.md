Configuration Application (Konfigürasyon Uygulaması)

Bu proje, bir merkezi konfigürasyon (yapılandırma) servisidir. Farklı uygulamaların (örneğin SERVICE-A, SERVICE-B) çalışma zamanı ayarlarını (örneğin özellik anahtarları, site adları, sayı limitleri) depolamak, yönetmek ve dinamik olarak sunmak için kullanılır.

Veri depolama katmanı olarak MongoDB kullanılmakta olup, uygulamaya ASP.NET Core API üzerinden erişilir.
🏗️ Proje Yapısı

Proje, Docker Compose ile yönetilen üç ana bileşenden oluşur:

    mongo: Tüm konfigürasyon verilerini depolayan MongoDB veritabanı konteyneri.

    configuration-api: Konfigürasyon verilerini okumak, yazmak ve yönetmek için kullanılan bir ASP.NET Core Web API.

    configuration-webui (Opsiyonel): Konfigürasyon verilerini görsel olarak yönetmek için kullanılan kullanıcı arayüzü (Front-end).

⚙️ Gereksinimler

Bu projeyi yerel ortamınızda çalıştırmak için aşağıdaki yazılımların kurulu olması gerekir:

    Docker

    Docker Compose (Genellikle Docker Desktop ile birlikte gelir.)

🚀 Kurulum ve Çalıştırma Talimatları (Docker Compose)

Projenin tamamını tek bir komutla başlatabilirsiniz. Bu adımlar, API kodundaki en son değişiklikleri içerir ve MongoDB bağlantı sorunlarını çözmek için yeniden derlemeyi garanti eder.
Adım 1: Proje Klasörüne Konumlanma

Terminalinizde, docker-compose.yml dosyasının bulunduğu ana dizine gidin.

cd /yol/proje-klasörünüze

Adım 2: Konteynerleri İnşa Etme ve Başlatma

Aşağıdaki komut, tüm servisleri (API dahil) yeniden inşa eder (--build) ve arka planda (-d) başlatır:

docker-compose up --build -d

    Not: Eğer daha önce başarısız olmuş konteynerleriniz varsa, temiz bir başlangıç için başlatmadan önce docker-compose down komutunu çalıştırabilirsiniz.

Adım 3: Sağlık Kontrolü

Tüm servislerin sağlıklı bir şekilde çalıştığından emin olun. Özellikle mongo konteynerinin (healthy) durumuna geçtiğini kontrol edin.

docker ps

Adım 4: Kullanıma Başlama

Servisler başlatıldıktan sonra, uygulamaya erişmek için aşağıdaki linkleri kullanabilirsiniz:

    Web Yönetim Arayüzü: http://localhost:8080

    API Testi (Swagger): http://localhost:5000/swagger/index.html

🔗 Bağlantı ve Konfigürasyon Detayları

Servis Adı
	

Adres
	

Açıklama

Konfigürasyon API
	

http://localhost:5000
	

Uygulamaların konfigürasyon verilerini çektiği ana servis.

MongoDB Bağlantısı
	

mongodb://localhost:27018
	

Yerel makineden bir MongoDB istemcisi (örneğin Compass) ile veritabanına erişim.
Container-to-Container Bağlantısı

API, Docker ağı içinde MongoDB'ye bağlanırken bu dizeyi kullanır: mongodb://mongo:27017
🧪 Veritabanı Kontrolü

MongoDB'ye bağlanıp kayıtları manuel olarak kontrol etmek için:

docker exec -it mongo mongosh

Komut istemcisinde:

use ConfigurationDB
db.Configurations.find().pretty()
