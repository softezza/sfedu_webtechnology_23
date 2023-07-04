<?php

namespace App\Models;

class Event extends Model
{
    const TABLE = 'events';

    /**
     * The function to create event and return record id.
     * 
     * @return string|int if success return id else return error message
     */
    public function create()
    {
        if(false === empty($this->event_name)){
            $this->event_name_translite = $this->translit($this->event_name);

            if(is_numeric($inserted_id = $this->saveWithoutDuplicate(["event_name_translite" => $this->event_name_translite]))){
                return $inserted_id;
            }else{
                return "error: fail save event - $inserted_id";
            }

        }else{
            return "error: event name not set or empty";
        }
    }

    private function translit($s) {
        $s = (string) $s; // преобразуем в строковое значение
        $s = trim($s); // убираем пробелы в начале и конце строки
        $s = function_exists('mb_strtolower') ? mb_strtolower($s) : strtolower($s); // переводим строку в нижний регистр (иногда надо задать локаль)
        $s = strtr($s, array('а'=>'a','б'=>'b','в'=>'v','г'=>'g','д'=>'d','е'=>'e','ё'=>'e','ж'=>'j','з'=>'z','и'=>'i','й'=>'y','к'=>'k','л'=>'l','м'=>'m','н'=>'n','о'=>'o','п'=>'p','р'=>'r','с'=>'s','т'=>'t','у'=>'u','ф'=>'f','х'=>'h','ц'=>'c','ч'=>'ch','ш'=>'sh','щ'=>'shch','ы'=>'y','э'=>'e','ю'=>'yu','я'=>'ya','ъ'=>'','ь'=>''));
        return $s; // возвращаем результат
    }
}
