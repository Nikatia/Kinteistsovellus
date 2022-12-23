function submitModalPost(controller, actionResult, formId, modalId) {
    /*//console.log(formId);*/
    var indexUrl = '/' + controller + '/Index';
    var submittedForm = new FormData(document.getElementById(formId));     //Luodaan formdata-tyyppinen muuttuja, joka kerää tiedot halutulta formilta
    var modalIdParent = parent.document.getElementById(modalId); //Etsitään parentista eli index-viewstä oikea kohta, johon modaali luodaan uudestaan
    /*//console.log("menee tänne asti");*/
    $.ajax({
        url: "/" + controller + "/" + actionResult,
        cache: false,
        contentType: false,
        processData: false,
        method: 'POST', //post pitää spesifioida tässä, jotta saadaan post-tyyppinen toiminto controllerissa
        type: "POST",
        data: submittedForm, //aikaisemmin mainittu formdata viedään ajax-pyynnöllä controllerille
        success: function (result) {

            if (result == "") { //Jos lisäys onnistui, controller palauttaa nullin
                window.location.href = indexUrl; //Onnistumisen jälkeen käyttäjä ohjataan indexiin, jossa päivittyneet tiedot
            } if (result == "image") {
                indexUrl = '/' + "Images" + '/Upload';
                window.location.href = indexUrl;
            }
            else {//tallentaminen ei onnistunut, koska modelstate.isvalid ei ollut true
                $(modalIdParent).html(""); //tyhjennetään vanha modaali (en tiedä, onko pakollista)
                $(modalIdParent).replaceWith(result); //luodaan uusi modaali, jossa validate messaget
                $("#" + modalId).modal("show");//näytetään modaali
            }
        }
    });
};

function modalGet(controller, actionresult, actionresultId, iframeId, modalId) {
    var url = "/" + controller + "/" + actionresult;
    /*//console.log(actionresultId);*/
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
            ////console.log("Result:")
            ////console.log(result);

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
            $("#" + dropdownId).append('<option value=""></option>');
            $.each(result, function (i, item) {
                if (item.Value != 1000) {
                    $("#" + dropdownId).append('<option value="' + item.Value + '">' +
                        item.Text + '</option>');
                }
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
    if (decimalNumbersTotal.length == 1) {
        return;
    }
    if (decimalNumbersTotal[1].length > 2) {
        input.value = "";
        tempStr = decimalNumbersTotal[0].substring(0, 5) + "," + decimalNumbersTotal[1].substring(0, 2);
        input.value = tempStr;
    }
};


function filterTable(hidingFieldID, dropdownMenuButtonID, dateBeginInputID, dateEndInputID) {

    //etsi kaikki rivit taulukosta, jossa filtering-class
    var rows = $(".filtering"), //filtering class pitää olla kaikissa roweissa indexissä!!!
        rowsLength = rows.length;
    //console.log(rowsLength);

    var activateThis = $(".activate-this");
    var activeSelect = [];
    for (var i = 0; i < activateThis.length; i++) {
        if ($(activateThis[i]).hasClass("active")) {
            activeSelect.push(activateThis[i].innerText);
        }
    }

    var dateBeginInput = $("#" + dateBeginInputID).val();
    var dtDateBegin;

    var dateEndInput = $("#" + dateEndInputID).val();
    var dtDateEnd;

    //Haetaan span-elementit eli badget dropdownin sisäll'
    var spans = $(".spans");
    //Jos ei löydy mitään, tuodaan placeholder div takaisin, muutoin poistetaan se
    if (activeSelect.length == 0) {
        $("#" + hidingFieldID).removeClass("d-none");
    } else {
        $("#" + hidingFieldID).addClass("d-none");
    }

    //Poistaa kaikki span-elementit
    for (var i = 0; i < spans.length; i++) {
        spans[i].remove();
    }

    //Lisää kaikki aktiiviset listaelementit span-elementteinä diviin
    for (var i = 0; i < activeSelect.length; i++) {
        $("#" + dropdownMenuButtonID).append('<span class="badge rounded-pill bg-success spans">' + activeSelect[i] + '</span>');
    }

    //Käy kaikki rivit läpi
    for (var i = 0; i < rowsLength; ++i) {
        if (activeSelect.length == 0 && dateBeginInput == "" && dateEndInput == "") {//Jos mitään suodatusta ei ole, ohjelma poistaa ChosenType ja Chosen date kaikista, ja tuo kaikki näkyviin
            for (var k = 0; k < rowsLength; k++) {
                $(rows[k]).removeClass("d-none");
                $(rows[k]).removeClass("ChosenType");
                $(rows[k]).removeClass("ChosenDate");
            }
            return;
        } else {//Poistaa rivin näkyvistä ja poistaa chosentype ja chosendate classit
            $(rows[i]).addClass("d-none");
            $(rows[i]).removeClass("ChosenType");
            $(rows[i]).removeClass("ChosenDate");
        }

        var tds = rows[i].getElementsByTagName('td');

        //Alustetaan indeksissä taulukossa oleville päivämäärille muuttujat
        var tdsDateBegin;
        var tdsDateEnd;

        //switch-casella päätetään, millainen toteutus
        switch (true) {
            case (dateBeginInput != "" && dateEndInput == ""): //Suodatuksessa: alkupvm
                //console.log("VAin alkupvm");
                dtDateBegin = new Date(dateBeginInput);
                tdsDateEnd = new Date(createISO(tds[1].innerText));

                if (tdsDateEnd >= dtDateBegin) { //Jos Päättymispvm td:ssä on suurempi tai yhtäsuuri kuin suodatuksessa oleva alkupvm, annetaan class ChosenDate
                    //$(rows[i]).addClass("d-none");
                    $(rows[i]).addClass("ChosenDate");
                }
                break;

            case (dateEndInput != "" && dateBeginInput == ""): //Suodatuksessa: loppupvm
                //console.log("VAin loppupvm");
                dtDateEnd = new Date(dateEndInput)
                tdsDateBegin = new Date(createISO(tds[0].innerText));
                //console.log(dtDateEnd, tdsDateBegin, tds[0].innerText, createISO(tds[0].innerText));

                if (tdsDateBegin <= dtDateEnd) {//Jos alkupvm td:ssä on pienempi tai yhtäsuuri kuin suodatuksessa oleva loppupvm, annetaan class ChosenDate
                    //$(rows[i]).addClass("d-none");
                    //console.log("Rivi: " + rows[i] + " sisältää oikean esiintymän");
                    $(rows[i]).addClass("ChosenDate");
                }

                break;

            case (dateEndInput != "" && dateBeginInput != ""): //Suodatuksessa: alkupvm ja loppupvm
                //console.log("Kummatkin");
                dtDateBegin = new Date(dateBeginInput); //datetime-muuttuja alkupvm (suodatuksesta) vertailuun
                dtDateEnd = new Date(dateEndInput); //datetime-muuttuja loppupvm (suodatuksesta) vertailuun
                tdsDateEnd = new Date(createISO(tds[1].innerText));
                tdsDateBegin = new Date(createISO(tds[0].innerText));

                if (tdsDateBegin <= dtDateEnd && tdsDateEnd >= dtDateBegin) {
                    //$(rows[i]).addClass("d-none");
                    $(rows[i]).addClass("ChosenDate");
                }
                break;

            default:
                //console.log("Ei pitäisi mennä tänne");
                break;
        }

        for (var k = 0; k < activeSelect.length; k++) {//Käy läpi jokaisen tyyppikriteerin riville ja lisää chosenType
            if (tds[2].innerText.indexOf(activeSelect[k]) > -1) {
                $(rows[i]).addClass("ChosenType");
            }
        }
    }
    switch (true) {
        case ((dateEndInput != "" || dateBeginInput != "") && activeSelect.length > 0)://Alku / loppupvm on olemassa ja tyyppi on olemassa
            for (var i = 0; i < rowsLength; i++) {
                if ($(rows[i]).hasClass("ChosenType") && $(rows[i]).hasClass("ChosenDate")) {
                    $(rows[i]).removeClass("d-none");
                }
            }
            break;
        case ((dateEndInput != "" || dateBeginInput != "") && activeSelect.length == 0)://Vain päivämäärä on olemassa
            for (var i = 0; i < rowsLength; i++) {
                if ($(rows[i]).hasClass("ChosenDate")) {
                    $(rows[i]).removeClass("d-none");
                    //console.log(i);
                }
            }
            break;

        case ((dateEndInput == "" && dateBeginInput == "") && activeSelect.length > 0)://vain tyyppi on olemassa
            for (var i = 0; i < rowsLength; i++) {
                if ($(rows[i]).hasClass("ChosenType")) {
                    $(rows[i]).removeClass("d-none");
                }
            }
            break;
        default:
            //console.log("Ei pitäisi mennä tänne lopulliseen suodatukseen")
            break;
    }
    
}

function createISO(dateStringFiCulture) {
    var partsOfDate = dateStringFiCulture.split(".");
    var strIsoDate = partsOfDate[2].replace("\n", "").trim() + "-" + partsOfDate[1].replace("\n", "").trim() + "-" + partsOfDate[0].replace("\n", "").trim();
    return strIsoDate;
}

function sortTable(tableID, columnNumber, dateNumberOther, varThis) {
    var table, rows, switching, i, x, y, shouldSwitch, stop;
    table = document.getElementById(tableID);
    switching = true;
    //stop = 1;
    //console.log(varThis);
    var emptyRowArray = [];
    if ($(varThis).hasClass("desc")) {//Jos on laskeva, luodaan nouseva järjestys
        $(varThis).addClass("asc");
        $(varThis).removeClass("desc");
        while (switching) {
            //stop++;
            switching = false;
            rows = table.rows;
            for (i = 1; i < (rows.length-1); i++) {
                shouldSwitch = false;
                x = rows[i].getElementsByTagName("TD")[columnNumber];
                
                if (x.innerText == "") {
                    //console.log("Postaa rivin: " + $(rows[i]));
                    emptyRowArray.push(rows[i]);
                    $(rows[i]).remove();
                    i--;
                    continue;
                }
                y = rows[i + 1].getElementsByTagName("TD")[columnNumber];
                if (dateNumberOther == "DATE") {
                    ////console.log("Tänne meni päivämäärä");
                    if (createISO(x.innerText) < createISO(y.innerText)) {
                        shouldSwitch = true;
                        break;
                    }
                } else if (dateNumberOther == "NUMBER") {
                    if (y.innerText == "" && x.innerText != "") {
                        y = parseFloat("0");
                        ////console.log("funktioon menee y: " + x.innerText);
                        if (eurosToFloat(x.innerText) < y) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                    else if (y.innerText != "" && x.innerText == "") {
                        x = parseFloat("0");
                        ////console.log("funktioon menee x: " + y.innerText);
                        if (x < eurosToFloat(y.innerText)) {
                            shouldSwitch = true;
                            break;
                        }
                    }

                    else {
                        if (eurosToFloat(x.innerText) < eurosToFloat(y.innerText)) {
                            ////console.log("funktioon menee x ja y: " + x.innerText + " ja " + y.innerText);
                            shouldSwitch = true;
                            break;
                        }
                    }

                } else {
                    ////console.log("Tänne meni muu aakkosjärjestyksessä");
                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                        shouldSwitch = true;
                        break;
                    }
                }
            }
            if (shouldSwitch) {
                ////console.log("Pitäisi vaihtaa");
                rows[i].parentNode.insertBefore(rows[i+1], rows[i]);
                switching = true;
            }
            //if (stop == 4000) {
            //    return;
            //}
        }
        for (var i = 0; i < emptyRowArray.length; i++) {
            table.tBodies[0].insertBefore(emptyRowArray[i], table.tBodies[0].children[1]);
        }
    } else {
        $(varThis).addClass("desc");
        $(varThis).removeClass("asc");
        while (switching) {
            switching = false;
            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {
                shouldSwitch = false;
                x = rows[i].getElementsByTagName("TD")[columnNumber];
                y = rows[i + 1].getElementsByTagName("TD")[columnNumber];
                if (x.innerText == "") {
                    emptyRowArray.push(rows[i]);
                    $(rows[i]).remove();
                    i--;
                    continue;
                }
                if (dateNumberOther == "DATE") {
                    ////console.log("Tänne meni päivämäärä");
                    if (createISO(x.innerText) > createISO(y.innerText)) {
                        shouldSwitch = true;
                        break;
                    }
                } else if (dateNumberOther == "NUMBER") {
                    if (y.innerText == "" && x.innerText != "") {
                        y = parseFloat("0");
                        ////console.log("funktioon menee y: " + x.innerText);
                        if (eurosToFloat(x.innerText) > y) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                    else if (y.innerText != "" && x.innerText == "") {
                        x = parseFloat("0");
                        ////console.log("funktioon menee x: " + y.innerText);
                        if (x > eurosToFloat(y.innerText)) {
                            shouldSwitch = true;
                            break;
                        }
                    }

                    else {
                        if (eurosToFloat(x.innerText) > eurosToFloat(y.innerText)) {
                            ////console.log("funktioon menee x ja y: " + x.innerText + " ja " + y.innerText);
                            shouldSwitch = true;
                            break;
                        }
                    }

                } else {
                    ////console.log("Tänne meni muu aakkosjärjestyksessä");
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                        shouldSwitch = true;
                        break;
                    }
                }
            }
            if (shouldSwitch) {
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
            }
        }
        for (var i = 0; i < emptyRowArray.length; i++) {
            table.tBodies[0].appendChild(emptyRowArray[i]);
        }
    }

    
}

function eurosToFloat(euro) {
    var correctNumber = parseFloat(euro.toString().replace("€", "").replace(",", ".").replace(/\s/g, '').trim());
    ////console.log(parseFloat(correctNumber.toString().replace("€", "").replace(",", ".").replace(" ", "").trim()));
    return correctNumber;
}

function destroyDiv(modalId) {
    var div = parent.document.getElementById(modalId);
    div.parentNode.removeChild(div);
}

function createDiv(divName) {
    if (document.getElementById(divName) === null) {
        newIframe = document.createElement('iframe');
        newIframe.setAttribute("id", divName);
        newIframe.style.display = "none";
        document.body.appendChild(newIframe);
    }
}

function allHaveAttribute(elements, attrName, attrValue) {
    // Do all elements have given attribute?
    for (var i = 0; i < elements.length; i++) {
        if (!elements[i].hasAttribute(attrName)) return false;
    }

    //if given value...
    if (attrValue) {
        for (i = 0; i < elements.length; i++) {
            if (elements[i].getAttribute(attrName) !== attrValue) //is different than we want
                return false;
        }
        return true;
    } else { // if elements have the attribute we have given
        return true;
    }
}

function dismissOrRedirect(url) {
    var els = document.querySelectorAll('span.field-validation-error.text-danger');
    if (allHaveAttribute(els, 'innerText', '') == false) {
        $("#back").attr('href', url);
        $("#back").removeAttr('data-bs-dismiss');
        $("#back").removeAttr('onclick');
    }
}

function dismissOrRedirectWithPartials(url) {
    var els = document.querySelectorAll('span.field-validation-error.text-danger.realError');
    if (allHaveAttribute(els, 'innerText', '') == false) {
        $("#back").attr('href', url);
        $("#back").removeAttr('data-bs-dismiss');
        $("#back").removeAttr('onclick');
    }
}




//function filterTable(hidingFieldID, dropdownMenuButtonID) {

//    //etsi kaikki rivit taulukosta, jossa filtering-class
//    var rows = $(".filtering"), //filtering class pitää olla kaikissa roweissa indexissä!!!
//        rowsLength = rows.length;
//    //console.log(rowsLength);

//    var activateThis = $(".activate-this");
//    var activeSelect = [];
//    for (var i = 0; i < activateThis.length; i++) {
//        if ($(activateThis[i]).hasClass("active")) {
//            activeSelect.push(activateThis[i].innerText);
//        }
//    }
//    ////console.log(activeSelect);

//    //Haetaan span-elementit eli badget dropdownin sisäll'
//    var spans = $(".spans");
//    //Jos ei löydy mitään, tuodaan placeholder div takaisin, muutoin poistetaan se
//    if (activeSelect.length == 0) {
//        $("#" + hidingFieldID).removeClass("d-none");
//    } else {
//        $("#" + hidingFieldID).addClass("d-none");
//    }

//    //Poistaa kaikki span-elementit
//    for (var i = 0; i < spans.length; i++) {
//        spans[i].remove();
//    }

//    //Lisää kaikki aktiiviset listaelementit span-elementteinä diviin
//    for (var i = 0; i < activeSelect.length; i++) {
//        $("#" + dropdownMenuButtonID).append('<span class="badge rounded-pill bg-success spans">' + activeSelect[i] + '</span>');
//    }

//    //Käy kaikki rivit läpi
//    for (var i = 0; i < rowsLength; ++i) {

//        if (activeSelect.length == 0) {
//            for (var k = 0; k < rowsLength; k++) {
//                $(rows[k]).removeClass("d-none");
//            }
//            return;
//        } else {
//            $(rows[i]).addClass("d-none");
//        }

//        var tds = rows[i].getElementsByTagName('td'),
//            tdsLength = tds.length;

//        //console.log(tds);
//        for (var tdsCounter = 0; tdsCounter < tdsLength; ++tdsCounter) {
//            for (var k = 0; k < activeSelect.length; k++) {
//                if (tds[tdsCounter].innerText.indexOf(activeSelect[k]) > -1) {
//                    $(rows[i]).removeClass("d-none");
//                }
//            }
//        }
//    }
//}



//function isNumberKey(evt, priceFieldId) {
//    var priceInputField = document.getElementById(priceFieldId);
//    ////console.log(priceFieldId);
//    ////console.log(getCaretPosition(priceInputField));
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
//    ////console.log(priceInputField.value.toString().replace(',', '').length);
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

