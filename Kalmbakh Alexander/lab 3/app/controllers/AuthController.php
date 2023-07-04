<?php

namespace App\Controllers;

use App\Views;
use App\Models;
use App\System;

class AuthController extends BaseController
{
    public function __construct($args)
    {
        if(Models\User::getUserFromSession()){
            new System\Header("control_panel");
        }

        static::$view = new Views\View();

        if (empty($_POST)) {
            $this->showAuthorizationView();
        }

        static::$view->default_login = $_POST['login'];
        
        if (empty($_POST['login']) || empty($_POST['password'])) {
            //TODO refactor error handler
            static::$view->error_message = 'Заполните поля логина и пароля для входа в систему';
        }else{

            if($user = Models\User::validateUser($_POST['login'], $_POST['password'])){
                if ($user->sessionRegistration()) {
                    new System\Header("control_panel");
                }
            }else{
                static::$view->error_message = 'Неверный логин или пароль';
            }
        }

        $this->showAuthorizationView();
    }

    public function showAuthorizationView()
    {
        static::$view->showView('auth');
        exit();
    }
}
