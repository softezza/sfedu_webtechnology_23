<article class="card">
    <div class="card__header">
        <span class="card__title">
            Редактирование формы заявки
        </span>
    </div>
    <div class="card__body">
        <form action="" method="post" class="form" id="form-create-form">
            <div class="form-group text">
                <label for="form-name" class="form-label">Название формы:</label>
                <input type="text" id="form-name" name="form-name" class="form-input" placeholder="Название формы">
            </div>
            <div class="form-group textarea">
                <label for="form-name" class="form-label">Описание:</label>
                <textarea id="form-name" name="form-name" class="form-textarea" placeholder="Описание формы"></textarea>
            </div>
            <fieldset class="form-dynamic">
                <legend class="form-dynamic__title">Добавление полей формы:</legend>
                <div class="form-dynamic__body">
                    <div class="form-dynamic__part">
                        <div class="form-group text-short">
                            <label class="form-short-label" for="competition[0][name]-0">Название поля</label>
                            <input class="form-input" type="text" name="competition[0][name]" id="competition[0][name]-0">
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Выбор типа поля</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Множественный выбор</span></option>
                                <option value="single-select"><span>Одиночный выбор</span></option>
                                <option value="text"><span>Текст</span></option>
                                <option value="mail"><span>Почта</span></option>
                                <option value="phone"><span>Телефон</span></option>
                            </select>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                    <div class="form-dynamic__part">
                        <div class="form-group text-short">
                            <label class="form-short-label" for="competition[0][name]-0">Название поля</label>
                            <input class="form-input" type="text" name="competition[0][name]" id="competition[0][name]-0">
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Выбор типа поля</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Множественный выбор</span></option>
                                <option value="single-select"><span>Одиночный выбор</span></option>
                                <option value="text"><span>Текст</span></option>
                                <option value="mail"><span>Почта</span></option>
                                <option value="phone"><span>Телефон</span></option>
                            </select>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                    <div class="form-dynamic__part">
                        <div class="form-group text-short">
                            <label class="form-short-label" for="competition[0][name]-0">Название поля</label>
                            <input class="form-input" type="text" name="competition[0][name]" id="competition[0][name]-0">
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Выбор типа поля</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Множественный выбор</span></option>
                                <option value="single-select"><span>Одиночный выбор</span></option>
                                <option value="text"><span>Текст</span></option>
                                <option value="mail"><span>Почта</span></option>
                                <option value="phone"><span>Телефон</span></option>
                            </select>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                    <div class="form-dynamic__part">
                        <div class="form-group text-short">
                            <label class="form-short-label" for="competition[0][name]-0">Название поля</label>
                            <input class="form-input" type="text" name="competition[0][name]" id="competition[0][name]-0">
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Выбор типа поля</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Множественный выбор</span></option>
                                <option value="single-select"><span>Одиночный выбор</span></option>
                                <option value="text"><span>Текст</span></option>
                                <option value="mail"><span>Почта</span></option>
                                <option value="phone"><span>Телефон</span></option>
                            </select>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                </div>
                <div class="form-dynamic__add-part">
                    <button class="button button-success" type="button" onclick="addFieldForm(this);">Добавить поле</button>
                </div>
            </fieldset>
            <div class="save-form">
                <button class="button button-success" onclick="sendData('/spa/form_settings', 'form-create-form'); return false;" type="submit">Сохранить</button>
                <!-- type="submit" -->
            </div>
        </form>
    </div>
</article>