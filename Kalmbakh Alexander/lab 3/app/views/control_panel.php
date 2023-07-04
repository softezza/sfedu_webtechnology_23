<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link rel="stylesheet" href="/res/css/control_panel.css">

    <script src="/res/js/control_panel/help_functions.js" defer></script>
    <script src="/res/js/control_panel/routing.js" defer></script>
    <script src="/res/js/control_panel/form-send.js" defer></script>
    <script src="/res/js/control_panel/form-dynamic_handler.js" defer></script>
    
    <script src="/res/js/control_panel.js" defer></script>
    <script src="/res/js/control_panel/main_create_event.js" defer></script>

    <title>Панель управления</title>
</head>

<body>
    <!-- main container - wrapper -->
    <div class="wrapper">
        <div class="container" id="container">
        </div>
        <nav>
            <div class="user-details">
                <a href="" class="user-details__full-name">
                    <?= isset($this->fio) ? $this->fio : "unknow" ?>
                </a>
            </div>

            <?php if (count($this->event_list) > 0) : ?>
                <ul class="navbar-menu">
                    <!-- TODO foreach menu -->
                    <!-- TODO DELETE plug -->
                    <li><a href="/control_panel/form_settings" class="navbar-menu__action">Настройки формы</a></li>
                    <li><a href="/control_panel/users" class="navbar-menu__action">Пользователи</a></li>
                    <li><a href="/control_panel/competitions" class="navbar-menu__action">Этапы/Номинации</a></li>
                    <li><a href="/control_panel/works" class="navbar-menu__action">Работы</a></li>
                    <li><a href="/control_panel/results" class="navbar-menu__action">Результаты</a></li>
                </ul>
            <?php endif ?>
        </nav>
        <header>
            <div class="event-menu">
                <form action="" class="select-dropdown">
                    <?php if (count($this->event_list) > 0) : ?>
                        <select name="event" id="event-select" class="select-dropdown__select">
                            <option value=""><span>Выберите событие</span></option>
                            <?php 
                                $event_list = $this->event_list;
                                $event_first_element = array_shift($event_list); 
                            ?>
                            <option selected value="<?= $event_first_element->event_name_translite; ?>"><span><?= $event_first_element->event_name; ?></span></option>
                            <?php foreach ($event_list as $event_item) : ?>
                                <option value="<?= $event_item->event_name_translite; ?>"><span><?= $event_item->event_name; ?></span></option>
                            <?php endforeach ?>
                        </select>
                    <?php endif ?>
                </form>
                <?php if ($this->user_role == "admin") : ?>
                    <div class="click-create-event"><a href="" onclick="route('/control_panel/create_event'); return false;">Создать событие</a></div>
                <?php endif ?>
            </div>
        </header>
    </div>
</body>

</html>