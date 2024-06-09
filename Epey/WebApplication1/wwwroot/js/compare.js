function Compareit(id) {

    fetch('/Phone/AddToCompare?id=' + id, {
        method: 'POST',
    }).then(response => response.status)
        .then(status => {

            if (status == 201) {
                location.reload();
            }

        })       

   
}

function ClearCompare() {

    fetch('/Phone/CompareItemsClear', {
        method: 'POST',
    }).then(response => response.status)
        .then(status => {

            if (status == 202) {
                location.reload();
            }

        })


}
