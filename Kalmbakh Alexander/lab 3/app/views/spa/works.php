<article class="card">
    <div class="card__header">
        <span class="card__title">
            Управление этапами/номинациями
        </span>
    </div>
    <div class="card__body">
        <form action="" method="post" class="form" id="form-create-form">
            <div class="form-group text">
                <label for="form-name" class="form-label">Конкурс:</label>
                <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                    <option value="select"><span>Выбор</span></option>
                    <option value="multy-select"><span>Студ-Весна</span></option>
                    <option value="single-select"><span>Студ-Осень</span></option>
                    <option value="text"><span>Хакатон</span></option>
                </select>
            </div>
            <fieldset class="form-dynamic">
                <legend class="form-dynamic__title">Этапы/Номинации:</legend>
                <div class="form-dynamic__body">
                    <div class="form-dynamic__record">
                        <div class="form-group text">
                            <label class="form-name" for="competition[0][max-score]-0">Макет поезда</label>
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Номинация</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Фото</span></option>
                                <option value="single-select"><span>Научно-техническое творчество</span></option>
                                <option value="text"><span>Прикладное исскуство</span></option>
                            </select>
                        </div>
                        <div class="form-group marks-group">
                            <div class="mark-form">
                                <label for="" class="label-mark">Новизна</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Сложность</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Исполнение</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Завершенность</label>
                                <input type="number" class="mark-input-number">
                            </div>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                    <div class="form-dynamic__record">
                        <div class="form-group text">
                            <label class="form-name" for="competition[0][max-score]-0">Сайт онлайн-деканат</label>
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Номинация</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Фото</span></option>
                                <option value="single-select"><span>Научно-техническое творчество</span></option>
                                <option value="text"><span>Прикладное исскуство</span></option>
                            </select>
                        </div>
                        <div class="form-group marks-group">
                            <div class="mark-form">
                                <label for="" class="label-mark">Новизна</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Сложность</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Исполнение</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Завершенность</label>
                                <input type="number" class="mark-input-number">
                            </div>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                    <div class="form-dynamic__record">
                        <div class="form-group text">
                            <label class="form-name" for="competition[0][max-score]-0">Автотестер</label>
                        </div>
                        <div class="form-group selector-short">
                            <label class="form-short-label" for="competition[0][max-score]-0">Номинация</label>
                            <select class="form-selector" name="competition[0][max-score]" id="competition[0][max-score]-0" value="1" min="1">
                                <option value="select"><span>Выбор</span></option>
                                <option value="multy-select"><span>Фото</span></option>
                                <option value="single-select"><span>Научно-техническое творчество</span></option>
                                <option value="text"><span>Прикладное исскуство</span></option>
                            </select>
                        </div>
                        <div class="form-group marks-group">
                            <div class="mark-form">
                                <label for="" class="label-mark">Новизна</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Сложность</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Исполнение</label>
                                <input type="number" class="mark-input-number">
                            </div>
                            <div class="mark-form">
                                <label for="" class="label-mark">Завершенность</label>
                                <input type="number" class="mark-input-number">
                            </div>
                        </div>
                        <div class="form-group button-row">
                            <button class="button button-delete" type="button">Удалить</button>
                        </div>
                    </div>
                </div>
            </fieldset>
            <div class="save-form">
                <button class="button button-success" onclick="sendData('/spa/form_settings', 'form-create-form'); return false;" type="submit">Сохранить</button>
                <!-- type="submit" -->
            </div>
        </form>
    </div>
</article>