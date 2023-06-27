<?php

namespace App\Spa;

use App\System;
use App\Models;

class CreateEvent extends Spa
{
    public function __construct()
    {
        parent::__construct();
        if (empty($_POST)){
            $this->getContents(); 
        }

        $this->processingData($_POST);
    }

    private function getContents()
    {
        $this->response->data["js"] = "/res/js/control_panel/form-dynamic_handler.js";
        $this->response->data["card"] = file_get_contents(VIEWS . '/spa/create_event.php');
        $this->response->isSuccess = true;
        exit(json_encode($this->response));
    }

    private function processingData($postData)
    {
        $this->response->data["data"] = $postData;
        $this->response->isSuccess = true;
        $this->response->message = "data taked done";

        $database = System\DBConnector::instance();
        $database->beginTransaction();

        $event = new Models\Event();
        $event->event_name = System\DataFilter::toString($postData['event-name']);
        if(false === is_numeric($messageOrId = $event->create())){
            $this->response->isSuccess = false;
            $this->response->message = $messageOrId;
            $database->rollBackTransaction();
            exit(json_encode($this->response));
        }
        
        if(count($postData['competition']) > 0){
            $competition = null;
            foreach ($postData['competition'] as $competitionData) {
                $competition = new Models\Competition();
                $competition->event_id = $messageOrId;
                $competition->maximum_score = System\DataFilter::toInt($competitionData['max-score']);
                $competition->competition_name = System\DataFilter::toString($competitionData['name']);

                if (true !== ($message = $competition->create())) {
                    $this->response->isSuccess = false;
                    $this->response->message = $message;
                    $database->rollBackTransaction();
                    exit(json_encode($this->response));
                }
            }
        }
        
        $database->commitTransaction();
        exit(json_encode($this->response));
    }
}
