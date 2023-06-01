## Backend and frontend for Smart Goal setter
This application is build with FastApi and Vue, the tortoise library used as ORM
### How to build
```
docker compose up -d --build
```
### To init database
```
docker compose exec backend aerich init -t src.database.config.TORTOISE_ORM
docker compose exec backend aerich init-db
```
