<?php

namespace App\Traits;

/**
 * Trait implements magic functions by defualt.
 */
trait MagicFuncs
{
    protected $data = [];

    public function __get($key)
    {
        if (isset($this->data[$key])) {
            return $this->data[$key];
        }else{ 
            return null; 
        }
    }

    public function __set($key, $value)
    {
        $this->data[$key] = $value;
    }

    public function __isset($key): bool
    {
        return isset($this->data[$key]);
    }

    public function __unset($key)
    {
        unset($this->data[$key]);
    }

    public function __serialize(): array
    {
        return $this->data;
    }
    
    // public function __unserialize(array $data): void
    // {
    //     foreach ($data as $key => $value) {
    //         $this->$$key = $value;
    //     }
    // }
}
