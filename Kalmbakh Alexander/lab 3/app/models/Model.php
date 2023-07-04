<?php

namespace App\Models;

use App\System\DBConnector;
use Exception;

abstract class Model implements \JsonSerializable 
{
    const TABLE = '';

    public $id;

    use \App\Traits\MagicFuncs;

    public function jsonSerialize() {
        return $this->data;
    }

    /**
     * Func for getting all objects in db by class table
     * 
     * @return array<object>|null
     */
    public static function findAll()
    {
        $database = DBConnector::instance();
        $sqlQuery = 'SELECT * FROM `' . static::TABLE . '`';
        $result = $database->getObjects(
            $sqlQuery,
            [],
            static::class
        );
        if (!empty($result)) {
            return $result;
        }
        return null;
    }

    /**
     * Func of getting object in db by id
     * 
     * @param string $id id by which it will search
     * 
     * @return array<object>|null return an object if it exists in the database, otherwise return NULL
     */
    public static function findById($id)
    {
        $database = DBConnector::instance();
        $sqlQuery = 'SELECT * FROM `' . static::TABLE . '`' . ' WHERE id=:id';
        $result = $database->getObjects(
            $sqlQuery,
            [':id' => $id],
            static::class
        );

        if (!empty($result) && is_array($result)) {
            return $result[0];
        }
        return null;
    }

    /**
     * Func for deleting by id record
     * 
     * @param int $id record id
     */
    public static function deleteById($id)
    {
        $database = DBConnector::instance();
        $sqlQuery = 'DELETE FROM `' . static::TABLE . '`' . ' WHERE id=:id';

        $result = $database->execute(
            $sqlQuery,
            [':id' => $id]
        );
    }

    /**
     * Func of finding in db by fields
     * 
     * @param array<string,string> $fields fields by which it will search
     * 
     * @return array<object>|null return an object if it exists in the database, otherwise return NULL
     */
    public static function findByFields($fields)
    {
        $database = DBConnector::instance();
        $sqlQuery = 'SELECT * FROM `' . static::TABLE . '`' . ' WHERE ' . DBConnector::createQueryCondition($fields);

        $result = $database->getObjects(
            $sqlQuery,
            DBConnector::createCondition($fields),
            static::class
        );
        if (!empty($result) && is_array($result)) {
            return $result;
        }
        return null;
    }

    /**
     * Func for deleting records in db by fields
     * 
     * @param array $fields fields for find records in db
     * 
     * @return bool result
     */
    public static function deleteByFields($fields)
    {
        $database = DBConnector::instance();
        $sqlQuery = 'DELETE FROM `' . static::TABLE . '`' . ' WHERE ' . DBConnector::createQueryCondition($fields);

        return $database->execute(
            $sqlQuery,
            DBConnector::createCondition($fields)
        );
    }

    /**
     * Func for getting object field names without 'id'
     * 
     * @return array field names
     */
    public function getFieldNames()
    {
        $fieldNames = [];
        foreach ($this->data as $key => $value) {
            if ('id' === $key) {
                continue;
            }
            $fieldNames[] = $key;
        }

        return $fieldNames;
    }

    /**
     * Property for check inserted object
     * 
     * @return int object id into db
     */
    public function isNew()
    {
        return empty($this->id);
    }

    /**
     * Func for inserting class data like record in db
     * 
     * @return int record id in db 
     */
    public function insert()
    {
        $fieldNames = $this->getFieldNames();
        $values = DBConnector::createCondition($this->data);

        if (empty($fieldNames) || empty($values)) {
            // TODO REFACTOR (will add error notification)
            return "empty fields or values";
        }

        $queryFields = '`' . implode('`, `', $fieldNames) . '`';
        $queryValues = implode(', ', array_keys($values));

        $sqlQuery = 'INSERT INTO `' . static::TABLE . '` '
            . '(' . $queryFields . ')'
            . ' VALUES '
            . '(' . $queryValues . ')';

        $database = DBConnector::instance();
        $database->execute($sqlQuery, $values);
        $this->id = $database->getLastInsertId();

        return $this->id;
    }

    /**
     * Func for updating record into db
     * 
     *  @return int record id in db 
     */
    public function update()
    {
        if ($this->isNew()) {
            return false;
        }

        $fields = [];
        foreach ($this->data as $key => $_) {
            $fields[] = $key . '=:' . $key;
        }
        $queryFields = implode(', ', $fields);
        $values = DBConnector::createCondition($this->data);

        if (empty($queryFields) || empty($values)) {
            // TODO REFACTOR (will add error notification)
            return false;
        }

        $sqlQuery = 'UPDATE `' . static::TABLE . '` '
            . ' SET ' . $queryFields  
            . ' WHERE id=:id ';

        $values[':id'] = $this->id;

        $database = DBConnector::instance();
        $database->execute($sqlQuery, $values);

        return true;
    }

    /**
     * Func for saving class data once 
     * 
     * @return int record id in db 
     */
    public function save()
    {
        if (!$this->isNew()) {
            return $this->id;
        }

        return $this->insert();
    }

    /**
     * Func for saving class data once without duplicate into db
     * 
     * @return int|null|string record id in db or null if duplicate or error message if catch exception
     */
    public function saveWithoutDuplicate($fields)
    {
        try{
            if (!$this->isNew()) {
                return $this->id;
            }
    
            if (null === $this->findByFields($fields)) {
                return $this->insert();
            }
    
            return null;
        }catch(Exception $exception){
            return $exception;
        }
    }

    /**
     * Func for delete record into db
     * 
     * @return bool result
     */
    public function delete()
    {
        if ($this->isNew()) {
            return false;
        }
        return static::deleteById($this->id);;
    }
}
