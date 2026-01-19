## 📷 Screenshots
<img width="1347" height="742" alt="Screenshot 2026-01-20 032400" src="https://github.com/user-attachments/assets/2dd75b8a-13b7-4957-bb97-30ee642860ef" />
<img width="1237" height="845" alt="Screenshot 2026-01-20 032406" src="https://github.com/user-attachments/assets/93fa5cee-e6eb-4705-9e19-dc502ca4d562" />
<img width="1332" height="702" alt="Screenshot 2026-01-20 031506" src="https://github.com/user-attachments/assets/ebf829f3-06e8-4a5f-b9e0-4ad2d13086ed" />
<img width="1338" height="766" alt="Screenshot 2026-01-20 031457" src="https://github.com/user-attachments/assets/274e8a50-ba75-47a4-91d0-824bd8e80e77" />

# 📝 TodoWeb — ASP.NET Core Task Management System
**TodoWeb** — bu foydalanuvchilarga o‘z vazifalarini boshqarish, adminlarga esa barcha foydalanuvchilar va ularning topshiriqlari ustidan to‘liq nazoratni amalga oshirish imkonini beruvchi zamonaviy **To-Do Management System**.
---
## 🚀 Imkoniyatlar
### 👤 Foydalanuvchilar uchun
- 🔐 Ro‘yxatdan o‘tish va tizimga kirish (xavfsiz autentifikatsiya)
- ✅ Vazifa yaratish
- 🗑️ Vazifani o‘chirish
- ✔️ Vazifani bajarilgan deb belgilash
- 👁️ Faqat o‘z vazifalarini ko‘rish
---
### 🛡️ Admin Panel
- 📊 Dashboard:
  - Jami foydalanuvchilar soni
  - Jami vazifalar soni
  - Bajarilgan vazifalar statistikasi
- 👥 Foydalanuvchilar ro‘yxati:
  - Har bir foydalanuvchining aktiv va bajarilgan vazifalari
- 🧠 To‘liq boshqaruv:
  - Foydalanuvchilarga vazifa biriktirish
  - Vazifalarni o‘chirish
  - Vazifa holatini o‘zgartirish (Aktiv / Bajarilgan)
- ❌ Foydalanuvchini tizimdan o‘chirish
---
## 🛠️ Texnologiyalar

- **Framework:** ASP.NET Core 8.0 / 9.0 (MVC)
- **ORM:** Entity Framework Core
- **Database:** MS SQL Server
- **Xavfsizlik:** ASP.NET Core Identity (Role-based Authorization)
- **Frontend:** Bootstrap 5, Razor Views, HTML5, CSS3
---
## 🔌 Database sozlamasi
`appsettings.json` faylida quyidagi connection string mavjud bo‘lishi kerak:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoWebDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
````
---
## 🧩 Migratsiyalarni ishga tushirish
**Package Manager Console** yoki **PowerShell** orqali:
```powershell
Add-Migration InitialCreate
Update-Database
```
---
## ▶️ Loyihani ishga tushirish
Loyiha birinchi marta ishga tushirilganda **admin foydalanuvchi avtomatik yaratiladi**.
### 🔑 Default Admin Account
* **Login:** `admin`
* **Password:** `Admin123!`
---
## 📌 Eslatma
* Loyihada **Role-based authorization** qo‘llanilgan
* Admin va User rollari mavjud
* Real loyihalarda ishlatish uchun mos struktura
