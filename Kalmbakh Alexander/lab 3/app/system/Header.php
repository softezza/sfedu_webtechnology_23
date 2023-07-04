<?php

namespace App\System;

class Header{
    public function __construct(string $url)
    {
        $host  = $_SERVER['HTTP_HOST'];
        header("Location: http://$host/$url");
        exit;
    }
}