<?php

namespace App\Views;

class View
{
    use \App\Traits\MagicFuncs;

    public function showView($viewPath)
    {
        $viewFullPath = VIEWS . '/' . $viewPath . '.php';

        if (empty($viewPath) || !file_exists($viewFullPath)) {
            // TODO REFACTOR (will add error notification)
        }

        foreach ($this->data as $key => $value) {
            ${$key} = $value;
        }
        
        // TODO header

        include_once $viewFullPath;

        // TODO footer
    }
}
