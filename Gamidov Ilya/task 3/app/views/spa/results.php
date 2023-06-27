<article class="card">
    <div class="card__header">
        <span class="card__title">
            Создание события
        </span>
    </div>
    <div class="card__body">
        <form action="" method="post" class="form" id="form-create-event">
            <div class="form-group text">
                <label for="event-name" class="form-label">Название события:</label>
                <input type="text" id="event-name" name="event-name" class="form-input" placeholder="Название события">
            </div>
            <fieldset class="form-dynamic">
                <legend class="form-dynamic__title">Добавление конкурсов:</legend>
                <div class="form-dynamic__body">
                </div>
                <div class="form-dynamic__add-part">
                    <button class="button button-success" type="button" onclick="addCompetiton(this);">Добавить конкурс</button>
                </div>
            </fieldset>
            <div class="save-form">
                <button class="button button-success" onclick="sendData('/spa/create_event', 'form-create-event'); return false;" type="submit" >Сохранить</button> 
                <!-- type="submit" -->
            </div>
        </form>
    </div>
</article>