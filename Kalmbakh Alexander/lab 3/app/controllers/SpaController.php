<?php

namespace App\Controllers;

use App\Logic\Response;
use App\System;
use App\Spa;

use App\Views;

class SpaController extends BaseController
{
    public $response;

    public function __construct($args)
    {
        $this->response = new Response();

        // if (false == ($user = $this->checkUser())) {
        //     // TODO 401 error
        //     new System\Header("auth");
        //     exit("401");
        // }
        $user = $this->Auth();

        if (empty($args) || $args[0] == "ControlPanel") {
            $this->response->data["card"] = file_get_contents(VIEWS . '/spa/default.php');
            $this->response->isSuccess = true;
            exit(json_encode($this->response));
        }

        // TODO js file load
        // TODO first load html page with args 
        switch ($args[0]) {
            case "CreateEvent": {
                    new Spa\CreateEvent();
                    break;
                }
            case "FormSettings": {
                    new Spa\FormSettings();
                    break;
                }
            case "Users": {
                    new Spa\Users();
                    break;
                }
            case "Competitions": {
                    new Spa\Competitions();
                    break;
                }
            case "Works": {
                    new Spa\Works();
                    break;
                }
            case "Results": {
                    new Spa\Results();
                    break;
                }
            case "UpdateSelector": {
                    $this->response->isSuccess = true;
                    $this->response->data = $this->getEventByUser($user);
                    $defaultOptions = (object) ['event_name' => 'Выберите событие', 'event_name_translite' => ''];
                    if (count($this->response->data) == 0) {
                        $this->response->data[] = $defaultOptions;
                    } else {
                        array_unshift($this->response->data, $defaultOptions);
                    }
                    exit(json_encode($this->response));
                    break;
                }
            default: {
                    exit("404");
                }
        }
    }
}
