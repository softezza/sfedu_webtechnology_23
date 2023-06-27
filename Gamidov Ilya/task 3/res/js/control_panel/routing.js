const navigate = async (pathname = window.location.pathname, element = document.getElementById("container"), state = {}) => {
    // TODO DELETE log
    console.log(`path: /spa/${stringOrEmpty(pathname.split('/').pop())}`);

    const response = await getPageData(`/spa/${stringOrEmpty(pathname.split('/').pop())}`);
    if(false == response) {
        console.log(`response is false`);
    }
    await Promise.all([
        loadJS(response.data.js),
        insertHtml(element, response.data.card)
    ]);
}

const route = async (pathname = window.location.pathname, element = document.getElementById("container"), state = {}) => {
    await navigate(pathname, element, state).then(() =>
        window.history.pushState(state, "", `${window.location.origin}${pathname}`)
    );
}

const getPageData = async (route) => {
    if (false == route) { return false; }

    try {
        const response = await fetch(route);
        const jsonResponse = await response.text();
        const responseData = jsonParseOrFalse(jsonResponse);
        return (responseData.isSuccess) ? responseData : false;
    } catch (error) {
        // TODO DELETE log
        console.log(`get data error: ${error.message}`);
        return false;
    }
}

const loadJS = async (route) => {
    // TODO logic
}

const insertHtml = async (element, content) => {
    if (false == content) { return false; }

    element.innerHTML = content;
}

window.onpopstate = async () => { await navigate(window.location.pathname); }

window.onload = async () => { await navigate(); }