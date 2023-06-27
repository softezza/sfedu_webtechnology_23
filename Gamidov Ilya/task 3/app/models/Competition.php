<?php

namespace App\Models;

class Competition extends Model
{
    const TABLE = 'competitions';

    /**
     * The function to create competition and return record id.
     * 
     * @return string|int if success return true else return error message
     */
    public function create()
    {
        if(false === (empty($this->event_id) || empty($this->competition_name) || empty($this->maximum_score))){
            if(false === is_numeric($this->maximum_score)){
                return "error: max score is not numeric";
            }

            if(is_numeric($errorMessageOrId = $this->saveWithoutDuplicate(["event_id" => $this->event_id, "competition_name" => $this->competition_name]))){
                return true;
            }else{
                return "error: fail save competition - $errorMessageOrId";
            }
            
        }else{
            return "error: competition name not set or empty";
        }
    }
}
