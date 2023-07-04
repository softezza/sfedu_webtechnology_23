const requestPost = async (url = "", data = {}) => {
    // TODO DELETE log
    console.log(`requestResponse:\nurl: ${url}\ndata:${JSON.stringify(data)}`);
    const response = await fetch(url, {
        method: "POST",
        headers: {
            // 'Content-Type': 'multipart/form-data'
            // 'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
        },
        body: data
    });

    // TODO DELETE log
    const responseData = await response.text();
    const jsonResponse = JSON.stringify(responseData);
    console.log(`response: ${responseData.message}`);
    console.log(`jsonResponse: ${jsonResponse}`);
}

const sendData = async (url = "", formId) => {
    const formElement = document.getElementById(formId);
    const formData = new FormData(formElement);
    await requestPost(url, formData);
}

