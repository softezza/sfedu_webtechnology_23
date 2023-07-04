<?php

namespace App\Spa;

use App\Logic\Response;

class Spa
{
    public $response;
    public function __construct()
    {
        $this->response = new Response();
    }
}