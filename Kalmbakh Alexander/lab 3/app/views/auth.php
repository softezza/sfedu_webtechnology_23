<!DOCTYPE html>
<html lang="ru">

<head>
  <meta charset="UTF-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Авторизация</title>
  <link rel="stylesheet" href="/res/css/auth.css">
  <!-- <script src="res/js/auth.js" defer></script> -->
</head>

<body>

  <!-- Контейнер -->
  <article class="container">

    <!-- Внутрений блок -->
    <!-- <div class="block">

      <section class="block__item block-item">
        <h2 class="block-item__title">У вас уже есть аккаунт?</h2>
        <button class="block-item__btn signin-btn">Войти</button>
      </section>

      <section class="block__item block-item">
        <h2 class="block-item__title">У вас нет аккаунта?</h2>
        <button class="block-item__btn signup-btn">Зарегистрироваться</button>
      </section>

    </div> -->

    <!-- Блок формы -->
    <div class="form-box">

      <!-- Форма входа -->
      <form action="/auth" class="form form-signin" method="post">

        <h3 class="form__title">Вход</h3>

        <p class="form__error">
          <!-- TODO REFUCTOR -->
          <?=(empty($error_message)) ? '' : '* ' . $error_message;?>
        </p>

        <p>
          <!-- TODO add alert picture -->
          <input type="text" class="form__input" placeholder="Логин" name="login" value="<?=(empty($default_login) ? '' : $default_login);?>">
        </p>
        <p>
          <!-- TODO add alert picture -->
          <input type="password" class="form__input" placeholder="Пароль" name="password">
        </p>
        <p>
          <button class="form__btn">Войти</button>
        </p>
        <!-- <p>
          <a href="#" class="form__forgot">Восстановить пароль</a>
        </p> -->

      </form>
      

      <!-- Форма регистрации -->
      <!-- <form action="#" class="form form-signup">

        <h3 class="form__title">Регистрация</h3>

        <p>
          <input type="text" class="form__input" placeholder="Логин">
        </p>
        <p>
          <input type="email" class="form__input" placeholder="Email">
        </p>
        <p>
          <input type="password" class="form__input" placeholder="Пароль">
        </p>
        <p>
        <p>
          <input type="password" class="form__input" placeholder="Повторите пароль">
        </p>
        <p>
          <button class="form__btn form__btn-signup">Зарегистрироваться</button>
        </p>

        <p class="form__error"></p>

      </form> -->

    </div>

  </article>

</body>

</html>