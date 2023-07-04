<?php

namespace App\Traits;

use FFI\Exception;

/**
 *  Default realisation of  singlton pattern.
 */
trait Singleton
{
    protected static $instance = null;
    /**
     * Constructor pseudo-static class.
     */
    protected function __construct()
    {
    }

    public function __clone()
    {
        // TODO exception
    }

    /**
     *  Function for get instance of class.
     * @return instance instance of class.
     */
    public static function instance()
    {
        if (null === static::$instance) {
            if (true === method_exists(static::class, 'init')) {
                static::init();
            }
            static::$instance = new static;
        }
        return static::$instance;
    }
}
