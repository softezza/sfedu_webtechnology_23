<?php

namespace App\System;

use PDO;
use Exception;

class DBConnector
{
    use \App\traits\Singleton;

    private const STR_CONNECT = 'mysql:host=127.0.0.1;dbname=event_organizer';
    private const DB_LOGIN = 'root';
    private const DB_PASSWORD = 'JgmtfPmmed10';

    private $databaseHandle = null;

    protected function __construct()
    {
        $this->databaseHandle = new PDO(static::STR_CONNECT, static::DB_LOGIN, static::DB_PASSWORD);
    }

    /**
     * Func for creating conditions for db query.
     * 
     * @param array<string, mixed> $fields array where [field_name => field_value]
     * 
     * @return array for pdo query
     */
    public static function createCondition($fields)
    {
        $arrayNew = [];
        foreach ($fields as $key => $value) {
            $arrayNew[':' . $key] = $value;
        }
        return $arrayNew;
    }

    /**
     * Func for creating paramets for db query.
     * 
     * @param array<string, mixed> $fields array where [field_name => field_value]
     * 
     * @return string for pdo paramets
     */
    public static function createQueryCondition($fields)
    {
        $arrayNew = [];
        foreach ($fields as $key => $_) {
            $arrayNew[] = $key . '=:' . $key;
        }
        return implode(' and ', $arrayNew);
    }

    /**
     * Func for executing sql query.
     * 
     * @param string $sql sql query
     * @param array $params array of value for sql query
     * 
     * @return bool
     */
    public function execute($sql, $params = []): bool
    {
        $statementHandle = $this->databaseHandle->prepare($sql);
        return $statementHandle->execute($params);
    }

    /**
     * Func for get last insert id
     * 
     * @return string|null id last insert row
     */
    public function getLastInsertId()
    {
        return $this->databaseHandle->lastInsertId();
    }

    /**
     * Func for get objects by type class.
     * 
     * @param string $sql sql query
     * @param array $params - array of value for sql query
     * @param string $className - type of class
     * 
     * @return array|[] object array or empty array
     */
    public function getObjects($sql, $params, $className)
    {
        $statementHandle = $this->databaseHandle->prepare($sql);
        $result = $statementHandle->execute($params);
        if (false !== $result) {
            return $statementHandle->fetchAll(PDO::FETCH_CLASS, $className);
        }
        return [];
    }

    public function beginTransaction()
    {
        $this->databaseHandle->beginTransaction();
    }

    public function commitTransaction()
    {
        $this->databaseHandle->commit();
    }

    public function rollBackTransaction()
    {
        $this->databaseHandle->rollBack();
    }
}
