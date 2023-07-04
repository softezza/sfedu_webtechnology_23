<?php

use App\Logic\Response;
use App\Views\View;

function urlProcessing(string $request_url)
{
    if (strpos($request_url, '?')) {
        $request_url = substr($request_url, 0, strpos($request_url, "?"));
    }

    $request_url = ltrim($request_url, '/');
    $request_url = rtrim($request_url, '/');
    $controller_url = explode('/', $request_url);

    return $controller_url;
}

function initController(array $controller_url)
{
    if (empty($controller_url[0])) {
        startController();
        return;
    }

    $controllerName = addressToArgument($controller_url[0]);
    $controllerArguments = [];

    foreach (array_slice($controller_url, 1) as $value) {
        $controllerArguments[] = addressToArgument($value);
    }

    startController($controllerName, $controllerArguments);

    return;
}

function startController(string $controllerName = "ControlPanel", array $arguments = [])
{
    new ('App\\Controllers\\' . $controllerName . 'Controller')($arguments);
    // try {
    //     new ('App\\Controllers\\' . $controllerName . 'Controller')($arguments);
    // } catch (\Throwable $th) {
    //     // TODO auto inflate
    //     $view = new View("error_request");
    //     $view->message = "{$th->getCode()}:{$th->getMessage}";
    //     include_once VIEWS . '/error_request.php';
    // }
}

function addressToArgument(string $address)
{
    if (is_numeric($address)) {
        return $address + 0;
    }

    $argument = "";
    foreach (explode('_', $address) as $namePart) {
        $argument .= ucfirst($namePart);
    }

    return $argument;
}

try {
    initController(urlProcessing($_SERVER['REQUEST_URI']));
} catch (\Throwable $th) {
    $response = new Response();
    $response->isSuccess = false;
    $response->message = "{$th->getCode()}:{$th->getMessage()}";
    exit(json_encode($response));
}
