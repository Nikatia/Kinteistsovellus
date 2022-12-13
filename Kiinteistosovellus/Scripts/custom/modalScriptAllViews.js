function submitModalPost(controller, actionResult, formId, modalId) {
    console.log(formId);
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
            console.log("Result:")
            console.log(result);
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

            updateDropdownList(controller, dropdownId);
            $(divForPartial).html("");
            $(divForPartial).html(result);

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

function isNumberKey(evt, priceFieldId) {
    var priceInputField = document.getElementById(priceFieldId);
    var decimalNumbersTotal = priceInputField.value.toString().split(',');
    var strLength = priceInputField.value.toString().length;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    tempStr = priceInputField.value.toString();
    if (evt.repeat) { evt.preventDefault(); }; //Estää saman näppäimen tuplaklikit
    //estää muut kuin numerot ja pilkun
    if (charCode > 31 && (charCode < 44 || charCode > 57) || charCode == 45 || charCode == 47 || charCode == 46) {
        evt.preventDefault();
        return false;
    }
    if (charCode == 44 && priceInputField.value.toString() == "") {
        evt.preventDefault();
        return false;
    }
    //numeroita ei saa olla enempää kuin 7 ja jos on pilkku, estää sen.
    if (priceInputField.value.toString().replace(',', '').length > 7 || (priceInputField.value.toString().includes(',') && charCode == 44)) {
        evt.preventDefault();
        return false;
    }
    if (decimalNumbersTotal[0].length == 5 && charCode != 44) {
        if (strLength > 5 && strLength < 8) {
            return true
        } else {
            evt.preventDefault();
            return false;
        }
    }
    return true;
};

var tempStr = "";
function validateKeyUp(evt, priceFieldId) {
    var input = document.getElementById(priceFieldId);
    var decimalNumbersTotal = input.value.toString().split(",");

    if (decimalNumbersTotal[0].length > 5) {
        input.value = "";
        input.value = tempStr;

    }
    if (decimalNumbersTotal[0].length > 5 && decimalNumbersTotal[1].length < 2) {
        tempStr = decimalNumbersTotal[0].substring(0, 5) + "," + decimalNumbersTotal[1].substring(0, 2);
        input.value = "";
        input.value = tempStr;
    }

    if (decimalNumbersTotal[1].length > 2) {
        input.value = "";
        tempStr = decimalNumbersTotal[0].substring(0, 5) + "," + decimalNumbersTotal[1].substring(0, 2);
        input.value = tempStr;
    }
};



//function isNumberKey(evt, priceFieldId) {
//    var priceInputField = document.getElementById(priceFieldId);
//    //console.log(priceFieldId);
//    //console.log(getCaretPosition(priceInputField));
//    var caretPosition = getCaretPosition(priceInputField);
//    var charCode = (evt.which) ? evt.which : evt.keyCode;

//    if (charCode > 31 && (charCode < 44 || charCode > 57) || charCode == 45 || charCode == 47 || charCode == 46) {
//        evt.preventDefault();
//        return false;
//    }
//    if (charCode == 44 && priceInputField.value.toString() == "") {
//        evt.preventDefault();
//        return false;
//    }
//    //console.log(priceInputField.value.toString().replace(',', '').length);
//    if (priceInputField.value.toString().replace(',', '').length >= 7 || (priceInputField.value.toString().includes(',') && charCode == 44)) {
//        evt.preventDefault();
//        return false;
//    }
//    var decimalNumbersTotal = priceInputField.value.toString().split(',');
//    if (decimalNumbersTotal.length > 1) {
//        switch (true) {
//            case (decimalNumbersTotal[1].length >= 2 && (caretPosition > decimalNumbersTotal[0].length)):
//                evt.preventDefault();
//                return false;

//            case (decimalNumbersTotal[0].length >= 5 && (caretPosition <= decimalNumbersTotal[0].length)):
//                evt.preventDefault();
//                return false;

//            default:
//                return true;
//        }
//    }
//    else {
//        if (priceInputField.value.toString().length >= 5 && charCode != 44) {
//            evt.preventDefault();
//            return false;
//        }
//    }
//    return true;
//};

//function getCaretPosition(ctrl) {
//    // IE < 9 Support
//    if (document.selection) {
//        ctrl.focus();
//        var range = document.selection.createRange();
//        var rangelen = range.text.length;
//        range.moveStart('character', -ctrl.value.length);
//        var start = range.text.length - rangelen;
//        return start;
//    } // IE >=9 and other browsers
//    else if (ctrl.selectionStart || ctrl.selectionStart == '0') {
//        return ctrl.selectionStart;
//    } else {
//        return 0;
//    }
//}