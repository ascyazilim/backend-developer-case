# Backend Developer Case Study - Microservices Architecture

Bu proje, modern .NET teknolojileri, SOLID prensipleri ve 12 Faktör Uygulama metodolojisi kullanılarak geliştirilmiş kapsamlı bir mikroservis vaka çalışmasıdır.

## 🏗 Mimari ve Tasarım Desenleri
* **Onion Architecture:** Product API içerisinde Domain, Application, Infrastructure ve API katmanları birbirinden izole edilerek Dependency Inversion kuralı tam anlamıyla uygulanmıştır.
* **CQRS & MediatR:** Okuma (Query) ve yazma (Command) işlemleri birbirinden ayrıştırılarak performansı artırılmış ve sorumluluklar bölünmüştür.
* **Event-Driven Architecture (Koreografi Tabanlı SAGA):** Servisler arası asenkron iletişim için RabbitMQ kullanılmıştır. Product servisi işlemi tamamladığında bir event fırlatır, Log servisi bunu dinleyerek kendi veritabanına kaydeder.
* **12-Factor App:** Konfigürasyonlar (Connection string, portlar) ortam değişkenleriyle (Environment Variables) yönetilmiş, stateless mimari benimsenmiş ve uygulamalar arası port bağımsızlığı sağlanmıştır.

## 🚀 Kullanılan Teknolojiler
* **Framework:** .NET 8 (C#)
* **Veritabanları:** MS SQL Server (İlişkisel), MongoDB (NoSQL Document), Redis (In-Memory Cache)
* **Mesaj Kuyruğu:** RabbitMQ
* **Loglama:** Serilog & Seq (Structured ve Merkezi Loglama)
* **API Gateway:** YARP (Yet Another Reverse Proxy) - Rate Limiting destekli
* **Güvenlik:** Microsoft Identity, JWT & Refresh Token, Role-Based Authorization
* **Konteynerizasyon:** Docker & Docker Compose

## 📦 Mikroservis Bileşenleri
* **Auth.API:** Merkezi kimlik doğrulama, kullanıcı/rol yönetimi (Admin/User), Access Token ve Refresh Token üretimi.
* **Product.API:** Ürün yönetimi, Redis üzerinden cache'li listeleme (Cache Invalidation stratejisiyle), RabbitMQ'ya asenkron event fırlatma işlemleri.
* **Log.API:** RabbitMQ'yu dinleyerek asenkron hata ve business loglarını MongoDB'ye ve merkezi Seq sunucusuna işleme.
* **ApiGateway:** Dış dünyadan gelen istekleri tek noktada karşılama, yönlendirme ve IP tabanlı Rate Limiting uygulama.

## ⚙️ Kurulum ve Çalıştırma
Proje bağımlılıkları ve altyapılarıyla birlikte tamamen Dockerize edilmiştir. Bilgisayarınızda sadece **Docker Desktop** kurulu olması yeterlidir.

1. Depoyu bilgisayarınıza klonlayın:
`git clone [GITHUB_LINKINIZI_BURAYA_YAZIN]`

2. Projenin ana dizinine (docker-compose.yml dosyasının bulunduğu yer) gidin:
`cd backend-developer-case`

3. Docker Compose komutu ile tüm altyapıyı ve servisleri tek tuşla ayağa kaldırın:
`docker compose up -d --build`

*Not: Sistem ilk kez ayağa kalkarken EF Core Auto-Migration özelliği devreye girecek ve SQL Server içerisindeki ilgili veritabanları ile tabloları (AuthDb, ProductDb) otomatik olarak oluşturacaktır.*

## 🧪 Erişim Adresleri ve Test Portları
Uygulama başarıyla çalıştıktan sonra dışarıya açılan portlar üzerinden testlerinizi gerçekleştirebilirsiniz:

* **API Gateway (Ana Giriş):** `http://localhost:5000`
* **Auth API Swagger:** `http://localhost:5001/swagger`
* **Product API Swagger:** `http://localhost:5002/swagger`
* **Log API Swagger:** `http://localhost:5003/swagger`
* **RabbitMQ Management:** `http://localhost:15672` (Kullanıcı: guest / Şifre: guest)
* **Seq Merkezi Log Paneli:** `http://localhost:5341` (Kullanıcı: admin / Şifre: Admin12345!)

## 🔗 Kod Deposu Bağlantısı
(https://github.com/ascyazilim/backend-developer-case)