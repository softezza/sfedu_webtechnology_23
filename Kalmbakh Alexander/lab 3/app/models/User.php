<?php

namespace App\Models;

use App\System;

class User extends Model
{
    const TABLE = 'users';

    /**
     * The function to get the user by login, if it exists.
     * 
     * @param string $login login of user
     * 
     * @return User|false
     */
    protected static function getUserByLogin($login): User|false
    {
        $userArray = static::findByFields(['login' => $login]);

        if (empty($userArray)) {
            return false;
        } else {
            if (1 === count($userArray)) {
                return $userArray[0];
            } else {
                // TODO Write Log
            }
        }
    }

    /**
     * The function to validate the user by login and password, and return if it exist.
     * 
     * @param string $login login of user
     * @param string $password user password
     * 
     * @return User|false
     */
    public static function validateUser($login, $password): User|false
    {
        if ($user = static::getUserByLogin(System\DataFilter::toLogin($login))) {
            if (false === empty($user->password)) {
                if (password_verify($password, $user->password)) {
                    return (0 == $user->is_removed) ? $user : false;
                }
            } else {
                die('<br>user password is empty!');
            }
        }

        return false;
    }

    /**
     * The function to the get user from the session,
     *  if it exist, and check if it has been removed. 
     * 
     * @return User|false
     */
    public static function getUserFromSession(): User|false
    {
        $sessionHandler = System\Session::instance();
        if (isset($sessionHandler->user)) {
            if ($user = User::getUserByLogin($sessionHandler->user->login)) {
                return (0 == $user->is_removed) ? $user : false; 
            }
        }
        return false;
    }

    /**
     * User session registration function
     * 
     * @return bool result registtration
     */
    public function sessionRegistration(): bool
    {
        if (false === (isset($this->login) && isset($this->surname) && isset($this->name) && isset($this->role))) {
            return false;
        }
        
        $sessionHandler = System\Session::instance();
        $sessionHandler->user = $this;
        return true;
    }
}
