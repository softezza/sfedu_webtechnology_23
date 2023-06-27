<?php

namespace App\Spa;

class FormSettings extends Spa
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
        $this->response->data["js"] = "";
        $this->response->data["card"] = file_get_contents(VIEWS . '/spa/form_settings.php');
        $this->response->isSuccess = true;
        exit(json_encode($this->response));
    }
}