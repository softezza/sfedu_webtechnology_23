<?php

namespace App\System;

use FFI\Exception;

class Session{

    private const SESSION_LIFE_TIME = 7200; // секунд (2 часа)

    use \App\Traits\Singleton;
    
    // Magic functions to manipulate the session values by key 
    protected static $namesOfObjectsAndArray = [];
    /**
     * Property to get value by key. 
     * If the class is included, you can work with the object.
     * 
     * @param string $key value name
     * 
     * @return mixed
     */
    public function __get($key)
    {
        if (isset($_SESSION[$key]) && 'namesOfObjectsAndArray' != $key) {
            if (in_array($key, static::$namesOfObjectsAndArray)) {
                return unserialize($_SESSION[$key]);
            }else{
                return $_SESSION[$key];
            }
        }
    }
    /**
     * Helper function to get an array if __get doesn't work.
     * 
     * @param string $key value name
     * 
     * @return array|NULL
     */
    public function getArray($key): array|NULL
    {
        if (isset($_SESSION[$key])) {
            $arrayByField = unserialize($_SESSION[$key]);
            if (is_array($arrayByField)) {
                return $arrayByField;
            }
        }
        return NULL;
    }
    /**
     * Helper function to get an object if __get doesn't work.
     * 
     * @param string $key value name
     * 
     * @return object|NULL
     */
    public function getObject($key, object $object): object|NULL
    {
        if (isset($_SESSION[$key])) {
            $objectFields = unserialize($_SESSION[$key]);
            if (is_object($objectFields)) {
                foreach ($objectFields as $key => $value) {
                    if ('id' == $key) {
                        continue;
                    }
                    $object->$$key = $value;
                }
                return $object;
            }
        }
        return NULL;
    }
    /**
     * The property to set the value to the session. It can work with an object
     * 
     * @param string $key
     * @param mixed $value
     */
    public function __set($key, $value)
    {
        if (is_object($value) || is_array($value)) {
            static::$namesOfObjectsAndArray[] = $key;
            $_SESSION[$key] = serialize($value);
        }else{
            $_SESSION[$key] = $value;
        }
    }
    public function __isset($key): bool
    {
        return isset($_SESSION[$key]);
    }
    public function __unset($key)
    {
        unset($_SESSION[$key]);
    }

    /**
     * The property to check session state
     * 
     * @return bool
     */
    public static function isCreated(): bool
    {
        return PHP_SESSION_ACTIVE === session_status();
    }
    
    /**
     * Function to create an initial session
     */
    protected static function init()
    {
        if (false === static::isCreated()) {
            session_set_cookie_params(self::SESSION_LIFE_TIME);
            ini_set('session.gc_maxlifetime', self::SESSION_LIFE_TIME);
            ini_set('session.cookie_lifetime', self::SESSION_LIFE_TIME);
            ini_set('session.gc_probability', '15');
            ini_set('session.save_path', SESSIONS_FOLDER);

            session_start();
        }

        if (isset($_SESSION['namesOfObjectsAndArray'])) {
            static::$namesOfObjectsAndArray = unserialize($_SESSION['namesOfObjectsAndArray']);
        }
        
        register_shutdown_function(function(){
            if (!empty(static::$namesOfObjectsAndArray)) {
                $_SESSION['namesOfObjectsAndArray'] = serialize(static::$namesOfObjectsAndArray);
            }
        });
    }

    /**
     * Function to clear a session data
     * 
     * @return bool
     */
    public static function clear(): bool
    {
        return session_unset();
    }

    /**
     * Session restart function
     */
    public static function restart()
    {
        self::clear();
        self::init();
    }
}

