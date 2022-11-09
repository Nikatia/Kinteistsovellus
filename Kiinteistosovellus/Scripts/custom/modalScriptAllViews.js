function submitModalPost(controller, actionResult, formId, modalId) {
    var indexUrl = '/' + controller + '/Index';
    var submittedForm = new FormData(document.getElementById(formId));     //Luodaan formdata-tyyppinen muuttuja, joka kerää tiedot halutulta formilta
    var modalIdParent = parent.document.getElementById(modalId); //Etsitään parentista eli index-viewstä oikea kohta, johon modaali luodaan uudestaan
    console.log("menee tänne asti");
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

function modalGet(controller, actionresult, actionresultId, iframeId, modalId) {
    var url = "/" + controller + "/" + actionresult;
    console.log(actionresultId);
    if (actionresultId != null) {
        url += "/?id=" + actionresultId;
    }

    var iframeForModal = $("#" + iframeId);

    $.ajax({
        url: url, //Haetaan haluttu controller-action
        cache: false,
        contentType: false,
        processData: false,
        method: 'get', //pitää spesifioida get, jos halutaan get-tyyppinen toiminto
        type: "get",
        success: function (result) {
            iframeForModal.replaceWith(result);
            $("#" + modalId).modal('show');
        }
    });
};

function partialViewGet(controller, actionresult, realFormDivId) {
    var url = "/" + controller + "/" + actionresult;
    var divForPartial = $("#" + realFormDivId);

    $.ajax({
        url: url, //Haetaan haluttu controller-action
        cache: false,
        contentType: false,
        processData: false,
        method: 'get', //pitää spesifioida get, jos halutaan get-tyyppinen toiminto
        type: "get",
        success: function (result) {
            $(divForPartial).html(""); //kaiken varalta tyhjennetään ensin partial view kohta
            $(divForPartial).html(result); //sitten se täytetään partialViewResultista saadulla partial viewillä
        }
    });
};

function partialViewSubmit(controller, actionresult, realFormDivId, formId, dropdownId) {
    var divForPartial = $("#" + realFormDivId);
    var submittedForm = new FormData(document.getElementById(formId));     //Luodaan formdata-tyyppinen muuttuja, joka kerää tiedot halutulta formilta
    $.ajax({
        url: "/" + controller + "/" + actionresult,
        cache: false,
        contentType: false,
        processData: false,
        method: 'POST', //post pitää spesifioida tässä, jotta saadaan post-tyyppinen toiminto controllerissa
        type: "POST",
        data: submittedForm, //aikaisemmin mainittu formdata viedään ajax-pyynnöllä controllerille
        success: function (result) {
            //console.log("Result:")
            //console.log(result);
            updateDropdownList(controller, "GetList", dropdownId);
            if (result == "") { //Jos lisäys onnistui, controller palauttaa nullin
            } else {//tallentaminen ei onnistunut, koska modelstate.isvalid ei ollut true
                $(divForPartial).html(""); //tyhjennetään vanha modaali (en tiedä, onko pakollista)
                $(divForPartial).html(result); //luodaan uusi modaali, jossa validate messaget
            }
        }
    });
};

function updateDropdownList(controller, dropdownId) {
    var url = "/" + controller + "/GetList";
    $.ajax({
        data: {},
        type: 'POST',
        cache: false,
        dataType: 'json',
        url: url,
        success: function (result) {
            $("#" + dropdownId).empty();
            //$("#OtherSpendingTypeID").append('<option value="">Select One</option>');
            $.each(result, function (i, item) {
                $("#" + dropdownId).append('<option value="' + item.Value + '">' +
                    item.Text + '</option>');
            });
        },
        error: function (ex) {
            alertify.alert('We face some technical difficulties. Hello World');
        }
    });
};