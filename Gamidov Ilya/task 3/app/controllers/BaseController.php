<?php

namespace App\Controllers;

use App\Views;
use App\Models;
use App\System;

abstract class BaseController
{
    protected static Views\View $view;

    public function __construct()
    {
        // TODO DELETE DEBUG
        echo '<br>' . 'abstract class construct';
    }

    public function checkUser()
    {
        return Models\User::getUserFromSession();
    }

    public function Auth()
    {
        if (false === ($user = Models\User::getUserFromSession())) {
            new System\Header("auth");
        }

        return $user;
    }

    // TODO refactor
    protected function getEventByUser($user)
    {
        return Models\Event::findAll();
        switch($user->role){

            case "admin":
                return Models\Event::findAll();

            case "jury":
                // TODO refactor
                return Models\Event::findAll();
        }
    }
}
