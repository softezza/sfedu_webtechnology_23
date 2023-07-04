let competitionCounter = 0;

const addCompetiton = (elem) => {
    /*competiton pattern:
         {
             class: "",
             name: "",
             lable: "",
             min: ,
             defaultValue: 
         }, 
    */

    const arrayAttributes = [
        {
            class: "text-short",
            name: `competition[${competitionCounter}][name]`,
            lable: "Название конкурса"
        },
        {
            class: "number-short",
            name: `competition[${competitionCounter}][max-score]`,
            lable: "Максимум за критерий(число)",
            min: 1,
            defaultValue: 1
        }
    ];

    const formDynamic = elem.closest(".form-dynamic");
    const rootBlock = formDynamic.querySelector(".form-dynamic__body");
    addPart(rootBlock, arrayAttributes);
    ++competitionCounter;
}

const removePart = (elem) => {
    const rootBlock = elem.closest(".form-dynamic__part");
    rootBlock?.parentNode.removeChild(rootBlock);
}

const addPart = (rootBlock, arrayAttributes) => {
    // add form-group:
    const partBlock = document.createElement("div");
    partBlock.setAttribute("class", "form-dynamic__part");
    let newDiv;
    let newLabel;
    let newInput;
    arrayAttributes.forEach(attributes => {
        // create root div for part:
        newDiv = document.createElement("div");
        newDiv.setAttribute("class", `form-group ${attributes.class}`);

        // create label:
        newLabel = document.createElement("label");
        newLabel.setAttribute("class",
            "form-" + ((attributes.class == "text-short" || attributes.class == "number-short") ? "short-" : "") + "label");
        newLabel.setAttribute("for", `${attributes.name}-${competitionCounter}`);
        newLabel.textContent = attributes.lable;
        newDiv.appendChild(newLabel);

        // create input:
        newInput = document.createElement("input");
        newInput.setAttribute("class", "form-input");
        newInput.setAttribute("type", typeInputAttributeSwitch(attributes.class));
        newInput.setAttribute("name", attributes.name);
        newInput.setAttribute("id", `${attributes.name}-${competitionCounter}`);
        if (attributes.hasOwnProperty("defaultValue")) {
            newInput.setAttribute("value", attributes.defaultValue);
        }
        if (attributes.hasOwnProperty("min")) {
            newInput.setAttribute("min", attributes.min)
        }
        newDiv.appendChild(newInput);

        partBlock.appendChild(newDiv);
    });

    // add delete button row:
    newDiv = document.createElement("div");
    newDiv.setAttribute("class", "form-group button-row");
    const newButton = document.createElement("button");
    newButton.setAttribute("class", "button button-delete");
    newButton.setAttribute("type", "button");
    newButton.onclick = () => {removePart(newButton)};
    newButton.textContent = "Удалить";

    newDiv.appendChild(newButton);
    partBlock.appendChild(newDiv);

    rootBlock.appendChild(partBlock);
}

const typeInputAttributeSwitch = (rootClass) => {
    switch (rootClass) {
        case "text":
        case "text-short":
            return "text";
        case "number-short":
            return "number";
    }
}