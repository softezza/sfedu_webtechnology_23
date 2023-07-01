# Запуск проекта

* Запустить контейнеры (на Windows): **docker-compose up -d --build**
* Остановить контейнеры: **docker-compose down**

Используются 4 контейнера: 
1) бд MySql 
2) adminer для админки
3) wait-for-db для проверки бд
4) и само web-приложение (серверная часть)

Доступ к adminer: http://localhost:8080/ user: root, password: 654321
Доступ к swagger: http://localhost:8000/docs