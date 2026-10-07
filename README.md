# Gochs.Nfgo — учёт личного состава НФГО

Учебный web-продукт для ведения структуры нештатных формирований по обеспечению выполнения мероприятий гражданской обороны (НФГО).

## Возможности

- учёт формирований НФГО;
- учёт подразделений внутри формирований;
- учёт личного состава;
- учёт техники и имущества;
- контроль состояния техники;
- история и регистрация оповещений;
- сводный Dashboard;
- карточка формирования с подразделениями, личным составом, техникой и оповещениями;
- фильтрация сотрудников и техники по формированию/подразделению;
- Swagger для проверки REST API;
- демонстрационные данные в Development.

## Технологии

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQLite
- Swagger / Swashbuckle
- Bootstrap 5
- Vanilla JavaScript

## Архитектура

Приложение реализовано одним ASP.NET Core проектом:

HTTP → Controllers → Services → Repositories → AppDbContext → SQLite

Основные каталоги:

- `Controllers` — HTTP endpoints;
- `Services` — сценарии приложения и бизнес-правила;
- `Repositories` — доступ к данным;
- `Entities` — EF Core сущности;
- `DTOs` — API-контракты;
- `Data/Configurations` — EF Core configurations;
- `Data/Seed` — демонстрационные данные;
- `wwwroot` — web-интерфейс.

## Запуск

Требуется .NET SDK 8.

```bash
dotnet restore
dotnet build
dotnet run
```

При запуске в Development приложение:

1. применяет EF Core migrations;
2. при пустой таблице Formations добавляет демонстрационные данные;
3. открывает REST API и web-интерфейс.

Адрес приложения выводится в консоли после `dotnet run`.

Web UI:

```text
http://localhost:<port>/
```

Swagger:

```text
http://localhost:<port>/swagger
```

## База данных и миграции

SQLite connection string хранится в `appsettings.json`.

Применить миграции вручную:

```bash
dotnet ef database update
```

Создать новую миграцию после изменения модели:

```bash
dotnet ef migrations add MigrationName
```

Файл локальной SQLite БД не должен добавляться в Git.

## Основные бизнес-правила

- табельный номер сотрудника уникален;
- инвентарный номер техники уникален, если указан;
- запись техники с индивидуальным инвентарным номером имеет количество 1;
- количество техники должно быть больше нуля;
- сотрудник может ссылаться только на существующее подразделение;
- подразделение может ссылаться только на существующее формирование;
- нельзя удалить формирование, пока в нём есть подразделения или история оповещений;
- нельзя удалить подразделение, пока в нём есть сотрудники или техника;
- оповещение создаётся в статусе `Created`;
- допустимые переходы оповещения: `Created → Sent` и `Created → Failed`;
- завершённое оповещение повторно изменить нельзя.

## Основные API endpoints

- `GET/POST /api/formations`
- `GET/PUT/DELETE /api/formations/{id}`
- `GET /api/formations/{id}/units`
- `GET /api/formations/{id}/employees`
- `GET /api/formations/{id}/equipment`
- `GET /api/formations/{id}/notifications`
- `GET/POST /api/units`
- `GET/PUT/DELETE /api/units/{id}`
- `GET /api/units/{id}/employees`
- `GET /api/units/{id}/equipment`
- `GET/POST /api/employees`
- `GET/PUT/DELETE /api/employees/{id}`
- `GET/POST /api/equipment`
- `GET/PUT/DELETE /api/equipment/{id}`
- `GET/POST /api/notifications`
- `PATCH /api/notifications/{id}/status`
- `GET /api/dashboard`

Enum в JSON передаются строками.

## Ограничения проекта

В рамках задания не реализуются реальные SMS/e-mail/Telegram-рассылки, авторизация, электронная подпись, кадровая система, сложный аудит и микросервисная инфраструктура. Оповещение в данном продукте означает регистрацию факта/состояния оповещения.
