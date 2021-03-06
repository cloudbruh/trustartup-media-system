<h1 align="center"> Trustartup Media System </h1>

<h3 align="center">
  Микросервис для проекта Trustartup.
</h3>

## Содержание

-   [Описание](#описание)
-   [Технологии](#технологии)
-   [Использование](#использование)
-   [API](#api)

## Описание

Отвечает на все запросы связанные с загрузкой и обработкой медиа(картинок, документов, видео).

**_Не отвечает за отдачу медиа_**

## Технологии

-   ASP.NET (C#)
-   PostgreSQL
-   Docker

## Использование

Микросервис компилируется и запускается в докере.

### Docker

-   [Docker](https://www.docker.com/get-docker)

Сначала постройте образ:

```bash
docker-compose build
```

Запустите микросервис:

```bash
docker-compose up -d
```

По умолчанию сервис запустится на `8083` порте

## API

Полная api-документация в формате OpenAPI3.0 будет находиться по адресу `/swagger/v1/swagger.json`
