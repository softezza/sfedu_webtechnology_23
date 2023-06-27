function stringOrNull(str) {
    return str ?? "";
}

function stringOrEmpty(str){
    return str || "";
}

function jsonParseOrFalse(json) {
    try {
        return JSON.parse(json);   
    } catch (error) {
        return false;
    }
}