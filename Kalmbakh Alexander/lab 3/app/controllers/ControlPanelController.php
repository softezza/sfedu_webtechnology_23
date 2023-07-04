<?php

namespace App\Controllers;

use App\Views;
use App\Models;
use App\System;

class ControlPanelController extends BaseController
{
    public function __construct($args)
    {
        $user = $this->Auth();
        $this->defaultView($user);
        exit();
    }

    private function defaultView($user)
    {
        static::$view = new Views\View();
        static::$view->fio = "$user->surname $user->name" . (isset($user->patronymic) ? (' ' . $user->patronymic) : '');
        static::$view->user_role = $user->role;
        static::$view->event_list = $this->getEventByUser($user);
        static::$view->showView('control_panel');
    }
}
