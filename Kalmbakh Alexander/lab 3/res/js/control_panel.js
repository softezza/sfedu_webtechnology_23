const selector =  document.getElementById("event-select");

const selectedEvent = () => {
    return {
        event_name: selector.options[selector.selectedIndex].text,
        event_name_translite: selector.options[selector.selectedIndex].value
    }
};

async function getSelector(){
    const response = await fetch("/spa/update_selector");
    const responseData = await response.json();
    return await responseData;
}

async function updateSelector(){
    const eventList = await getSelector();
    console.log(eventList)
    if(false == eventList){
        // TODO throw error
        return;
    }
    const lastSelectedEvent = selectedEvent().event_name_translite;

    selector.options.length = 0;

    eventList.data.forEach(eventItem => {
        selector.append(new Option(eventItem.event_name, eventItem.event_name_translite));
    });

    try{
        selector.querySelector(`option[value='${lastSelectedEvent}']`).selected = true;
    }catch(exception){
        alert(`exception: ${exception}`)
    }
}