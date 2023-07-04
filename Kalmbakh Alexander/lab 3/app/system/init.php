<?php

require str_replace('/', '\\', $_SERVER['DOCUMENT_ROOT']) . '/app/system/autoload.php';

define('ROOT', $_SERVER['DOCUMENT_ROOT']);
define('APP', ROOT . '/app');
define('SESSIONS_FOLDER', APP . '/sessions');
define('SYSTEM', APP . '/system');
define('TRAITS', APP . '/traits');
define('MODELS', APP . '/models');
define('VIEWS', APP . '/views');
define('CONTROLLERS', APP . '/controllers');
