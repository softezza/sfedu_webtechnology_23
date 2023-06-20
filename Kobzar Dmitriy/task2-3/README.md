# Запуск

* Запуск приложения: **sudo docker-compose up -d --build**
* Остановка: **sudo docker-compose down**

Запускаются 4 контейнера: бд MySQL, adminer, wait-for-db и само web-приложение.
Доступ к adminer: http://localhost:8080/ user: root, password: 12345
