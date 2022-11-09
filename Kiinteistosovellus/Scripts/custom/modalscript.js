function submitModalPost(controller, actionResult, formId, modalId) {
    var indexUrl = '/' + controller + '/Index';
    var submittedForm = new FormData(document.getElementById(formId));     //Luodaan formdata-tyyppinen muuttuja, joka kerää tiedot halutulta formilta
    var modalIdParent = parent.document.getElementById(modalId); //Etsitään parentista eli index-viewstä oikea kohta, johon modaali luodaan uudestaan

    $.ajax({
        url: "/" + controller + "/" + actionResult,
        cache: false,
        contentType: false,
        processData: false,
        method: 'POST', //post pitää spesifioida tässä, jotta saadaan post-tyyppinen toiminto controllerissa
        type: "POST",
        data: submittedForm, //aikaisemmin mainittu formdata viedään ajax-pyynnöllä controllerille
        success: function (result) {
            //console.log("Result:")
            //console.log(result);
            if (result == "") { //Jos lisäys onnistui, controller palauttaa nullin
                window.location.href = indexUrl; //Onnistumisen jälkeen käyttäjä ohjataan indexiin, jossa päivittyneet tiedot
            } else {//tallentaminen ei onnistunut, koska modelstate.isvalid ei ollut true
                $(modalIdParent).html(""); //tyhjennetään vanha modaali (en tiedä, onko pakollista)
                $(modalIdParent).replaceWith(result); //luodaan uusi modaali, jossa validate messaget
                $("#" + modalId).modal("show");//näytetään modaali
            }
        }
    });
};

function submitModalGet(controller, view, id) {

};