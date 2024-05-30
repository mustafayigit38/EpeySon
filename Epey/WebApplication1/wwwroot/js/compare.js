function Compareit(id) {
    var state = JSON.parse(localStorage.getItem("compareitems"));
    if (state === null) {
        state = [];
    }

    if (state.includes(id)) {
        state = state.filter(x => x !== id);
    } else {
        state.push(id);
    }

    localStorage.setItem("compareitems", JSON.stringify(state));
}
