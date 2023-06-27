<?php

namespace App\Controllers;

use App\Views;
use App\Models;

class TestController extends BaseController
{
    public function __construct()
    {        
        static::$view = new Views\View();
        static::$view->showView('test');
    }
}