<?php

namespace App\Spa;

class Users extends Spa
{
    public function __construct()
    {
        parent::__construct();
        if (empty($_POST)){
            $this->getContents(); 
        }
    }

    private function getContents()
    {
        $this->response->data["js"] = "/res/js/control_panel/form-dynamic_handler.js";
        $this->response->data["card"] = file_get_contents(VIEWS . '/spa/users.php');
        $this->response->isSuccess = true;
        exit(json_encode($this->response));
    }
}