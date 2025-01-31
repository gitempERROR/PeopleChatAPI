# PeopleChat API
![image](https://github.com/user-attachments/assets/d00e9834-e48c-4f56-91b3-4391b9646548)

## Описание

Проект представляет собой серверную часть для проекта [**PeopleChat**](https://github.com/gitempERROR/PeopleChat).
Серверная часть включает в себя логики авторизации, регистрации, управления пользователи и работы с базой данных.
![image](https://github.com/user-attachments/assets/c4105b26-03c8-44ed-993d-e329ab4ae445)

## Демо

- **Авторизация и Регистрация:**

Эндпоинт авторизации

![image](https://github.com/user-attachments/assets/a75e0797-5a00-44f0-b2d6-690a1fd4e84b)

Пример запроса регистрации:

![image](https://github.com/user-attachments/assets/87321592-bb62-4f13-93ed-26965494fe81)

Ответ сервера при регистрации:

![image](https://github.com/user-attachments/assets/f862333f-92f0-4b38-ab95-b992d567f4ca)

- **Работа с сообщениями:**

Получение всех сообщений между двумя пользователями

![image](https://github.com/user-attachments/assets/0c5e585e-3c14-4daf-a84b-a28b2de55fbe)

Отправка сообщения 

![image](https://github.com/user-attachments/assets/31294096-a8cb-4cc9-b4c4-055c6bd09f59)

- **Работа с пользователями:**

Получение списка всех пользователей

![image](https://github.com/user-attachments/assets/bb77d87d-0575-4a84-bd8e-f72a4e59a868)

Обновление данных пользователя

![image](https://github.com/user-attachments/assets/8d89e589-6e73-4031-ac56-9e97029d5819)

## Технологии в проекте

### Основная информация:

- Язык программирования - **C#**

- Среда разработки - **Visual Studio Community 2022**

- Фреймворк разработки - **ASP.NET Core**

### Библиотеки:

- **Microsoft.AspNetCore.Authentication.JwtBearer** - Авторизация через JWT-токен
- **Microsoft.AspNetCore.SignalR.Common** и **Microsoft.AspNetCore.SignalR.Protocols.Json** - Получение уведомлений клиентами со стороны сервера
- **Microsoft.EntityFrameworkCore.Tools** и **Npgsql.EntityFrameworkCore.PostgreSQL** - Работа с БД PostgreSQL
- **Microsoft.VisualStudio.Web.CodeGeneration.Design** - Автоматическая генерация API контроллеров на основе моделей EntityFramework
- **Swashbuckle.AspNetCore** - Swagger

## Особенности проекта

**Список особенностей (при работе с проектом [PeopleChat](https://github.com/gitempERROR/PeopleChat)):**
- Шифрование пароля пользователей
- Авторизация через JWT-токен
- Уведомление клиентской программы со стороны сервера, если пользователю было прислано сообщение
- Отслеживание количества непрочитанных сообщений
- Звуковое уведомление при приходе сообщения
- Фильтрация отображаемых пользователей
- Выбор изображения пользователя

## Техническое описание проекта

### Установка

**Процесс установки:**

1. Запустить Visual Studio Community 2022.

2. Средствами Visual Studio склонировать проект. Ссылка для клонирования - https://github.com/gitempERROR/PeopleChatAPI.git

3. Установить сервер PostgreSQL и создать базу данных. Ссылка на скрипт - https://share.note.sx/r7kyv6io#sIb/W40zstiosoIEnqPyrpNJV07LU4T7ij6EoLU1DtM

### Настройка

**Процесс настройки:**

1. В Visual Studio нажать правой кнопкой мыши по проекту, выбрать "Управление секретами пользователей". Ввести туда следующие данные:
``` json
{
  "AppSettings": {
    "AuthOptions": {
      "Issuer": "PeopleChatAPI",
      "Audience": "PeopleChat",
      "EncryptionKey": "ВАШ_ТОКЕН"
    }
  }
}
```

2. Выбрать в настроке запуска http, либо установить сертификат безопасности для использования https.

3. Запустить проект из Visual Studio.

### Планируемые дополнения

- Система добавления в друзья;

- Возможность передачи файлов и изображений;

- Возможность записи голосовых сообщений;

- Расширенные настройки пользовательских профилей.
