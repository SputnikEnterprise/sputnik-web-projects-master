/*!
 Script for job finder app page.
*/

// Global variables:

// Needed for statistics:
var clientLatitude = 0;
var clientLongitude = 0;

// The page init event for the home page.
$("#home").live("pageinit", function () {

    // Set translation for 'please wait'-dialog.
    $.mobile.loadingMessage = clientSideTranslations.TEXT_PLEASE_WAIT;
    $.mobile.page.prototype.options.backBtnText = clientSideTranslations.TEXT_BACK_BUTTON

    // Retreive client geolocation, if he allows it.
    navigator.geolocation.getCurrentPosition(handle_geolocation_query);
    function handle_geolocation_query(position) {
        clientLatitude = position.coords.latitude;
        clientLongitude = position.coords.longitude;
    }

    // Register change handler for cantons select box.
    $("#cmbCantons").change(function () {
        var selectedCantonAbbreviation = $("#cmbCantons").val();
        var encryptedMandantGuid = $("#hidMDGuid").val();

        loadBranchesByCantonAbbreviation(encryptedMandantGuid, selectedCantonAbbreviation);
    });

    // Loads branches by canton.
    function loadBranchesByCantonAbbreviation(encryptedMandantGuid, cantonAbbreviation) {
        $.ajax({ type: "POST",
            url: applicationPath + "/Home/LoadBranchesByCantonAbbreviation",
            data: ({ encryptedMandantGuid: encryptedMandantGuid, cantonAbbreviation: cantonAbbreviation }),
            cache: false,
            dataType: "json",
            success: loadBranchesCallback
        });
    }

    // Callback from server for LoadBranchesByCantonAbbreviation ajax call.
    function loadBranchesCallback(data) {

        var $cmbBranches = $("#cmbBranches");

        // Remove existing options (except first one 'All'-entry).
        $cmbBranches.find('option').not(':first').remove();

        // Add new options.
        $cmbBranches.addItemsToSelectBox(data);

        // Select the first option ('Alle').
        $cmbBranches[0].selectedIndex = 0;

        // Refrehs the select menu.
        $cmbBranches.selectmenu("refresh");
    }

    // Register click handler for button search vacancy.
    $("#btnSearchVacancies").click(function () {

        var canton = $("#cmbCantons").val();
        var branch = $("#cmbBranches").val();
        var vacancyTitle = $("#vacancyTitle").val();
        var encryptedMandantGuid = $("#hidMDGuid").val();

        // Search the vacancies
        searchVacancies(encryptedMandantGuid, canton, branch, vacancyTitle);

        return false;
    });

    // Register click handler for button search candidate.
    $("#btnSearchCandidates").click(function () {

        var jobQualificationSearchValue = $("#cmbJobQualifications").val();
        var encryptedMandantGuid = $("#hidMDGuid").val();

        // Search the candidates
        searchCandidates(encryptedMandantGuid, jobQualificationSearchValue);

        return false;
    });

    // Searches vacancies by canton, branch and vacancy title.
    function searchVacancies(encryptedMandantGuid, cantonAbbreviation, branch, vacancyTitle) {
        $.mobile.showPageLoadingMsg(); // Start page loader
        $.ajax({ type: "POST",
            url: applicationPath + "/Home/SearchVacancies",
            data: ({ encryptedMandantGuid: encryptedMandantGuid, cantonAbbreviation: cantonAbbreviation, branch: branch, vacancyTitle: vacancyTitle, clientLatitude: clientLatitude, clientLongitude: clientLongitude }),
            cache: false,
            dataType: "json",
            success: searchCallback("vacancy")
        });
    }

    // Searches candidates a given job qualification search value.
    function searchCandidates(encryptedMandantGuid, jobQualificationSearchValue) {
        $.mobile.showPageLoadingMsg(); // Start page loader
        $.ajax({ type: "POST",
            url: applicationPath + "/Home/SearchCandidates",
            data: ({ encryptedMandantGuid: encryptedMandantGuid, jobQualificationSearchValue: jobQualificationSearchValue, clientLatitude: clientLatitude, clientLongitude: clientLongitude }),
            cache: false,
            dataType: "json",
            success: searchCallback("candidate")
        });
    }

    // Some variables that are used for various layout functions:
    var deviceHeight = $(document).height();
    var maxNofListItemsOnOnePage = 0;
    var actualPage = 0;
    var totNofPages = 0;

    // Callback from server for SearchVacancies or SearchCandidates ajax call.
    var searchCallback = function (searchedObject) {
        return function (data, textStatus) {

            $.mobile.changePage("#results", {
                transition: "flip",
                changeHash: false //do not add page location in url bar
            });

            // Clear items 
            var items = [];
            actualPage = 0;

            $.each(data, function (i, item) {
                items.push('<li><a href="#" data-' + searchedObject + 'Id=' + item.EncryptedID + '>' + item.Title + '</a></li>');
            });

            // Create new list for found vacancies.
            $ul = $("<ul></ul>");
            $ul.append(items.join(''));

            if (searchedObject == "vacancy") {
                // Register click hander for vacancies.
                $("li", $ul).click(function () {
                    var $valueAnchor = $("a[data-vacancyId]", $(this));
                    // Set the title on the details page.
                    $("#ResultTitle").text($valueAnchor.text());
                    var encryptedVacancyId = $valueAnchor.attr("data-vacancyId");
                    var encryptedMandantGuid = $("#hidMDGuid").val();
                    var encryptedLanguage = $("#hidLanguage").val();

                    // Load the vacancy details with an ajax call.
                    readVacancyDetails(encryptedMandantGuid, encryptedVacancyId, encryptedLanguage);
                });
            }
            else if (searchedObject == "candidate") {
                // Register click hander for vacancies.
                $("li", $ul).click(function () {
                    var $valueAnchor = $("a[data-candidateId]", $(this));
                    // Set the title on the details page.
                    $("#ResultTitle").text($valueAnchor.text());
                    var encryptedCandidateId = $valueAnchor.attr("data-candidateId");
                    var encryptedMandantGuid = $("#hidMDGuid").val();
                    var encryptedLanguage = $("#hidLanguage").val();

                    // Load the candidate details with an ajax call.
                    readCandidateDetails(encryptedMandantGuid, encryptedCandidateId, encryptedLanguage);
                });
            }


            $searchResults = $("#searchResults");
            $searchResults.empty();
            $searchResults.append($ul);

            // Create a list view out of the ul element
            $("ul", $searchResults).listview();

            var listItemHeight = $(".ui-listview li").height();
            var headerHeight = $("div.header").outerHeight(true);
            var footerHeight = $("div[data-role='footer']").outerHeight(true);
            var listItemsHeight = Math.floor(deviceHeight - headerHeight - footerHeight);

            var paginationNavbarHeight = $("#paginationNavBar").height();
            if (items.length * listItemHeight <= listItemsHeight) {
                // Do not show paginationNavBar
                $("#paginationNavBar").hide();
                paginationNavbarHeight = 0;
            }
            else {
                $("#paginationNavBar").show();
            }

            listItemsHeight = Math.floor(deviceHeight - headerHeight - paginationNavbarHeight - footerHeight);
            maxNofListItemsOnOnePage = Math.ceil(listItemsHeight / listItemHeight);
            totNofPages = Math.ceil(items.length / maxNofListItemsOnOnePage);

            showPage(0);

            $.mobile.hidePageLoadingMsg(); // Hide page loader
        }
    }

    // Loads the details of a vacancy.
    function readVacancyDetails(encryptedMandantGuid, encryptedVacancyId, encryptedLanguage) {
        $.mobile.showPageLoadingMsg();

        $.ajax({ type: "POST",
            url: applicationPath + "/Home/ReadVacancyDetails",
            data: ({ encryptedMandantGuid: encryptedMandantGuid, encryptedVacancyId: encryptedVacancyId, encryptedLanguage: encryptedLanguage }),
            cache: false,
            dataType: "json",
            success: readVacancyDetailsCallback
        });
    }

    // Callback from server for ReadVacancyDetails ajax call.
    function readVacancyDetailsCallback(data) {
        $.mobile.hidePageLoadingMsg();

        if (data == "") {
            // If data could not be loaded an empty string will be returned.
            return;
        }

        // Always do clear the contact picture.
        $("#resultContactPicture").empty();

        var $resultDetailsRegularValuesWrapper = $("#resultDetailsRegularValuesWrapper");

        // Empty regular details values from possible previous request.
        $resultDetailsRegularValuesWrapper.empty();

        var arrLength = data.RegularSearchResultDetailValues.length;

        // First fill the regular vacancy values.
        for (var i = 0; i < arrLength; i++) {

            var vacancyDetailValue = data.RegularSearchResultDetailValues[i];

            // Create a heading element for the value.
            var $headingElement = $("<strong>" + vacancyDetailValue.Title + ":&nbsp;</strong>");

            // Create the value element.
            var $valueElement = $("<span>" + vacancyDetailValue.Value + "</span><p></p>");

            // Add the heading and value to the DOM.
            $resultDetailsRegularValuesWrapper.append($headingElement);
            $resultDetailsRegularValuesWrapper.append($valueElement);

        }

        // Now fill in the contact data.

        // Show separator line
        $("#resultDetailsContactWrapper hr").show();

        $("#resultContactTitle").text(data.ContactTitle);

        // Telephone
        var telephone = data.ContactTelephone;

        if (telephone.Value == "") {
            $("#resultContactTelephone").hide();
        }
        else {
            $("#resultContactTelephone").show();
            $("#resultContactTelephoneTitle").text(telephone.Title);

            $contactTelephoneValue = $("#resultContactTelephoneValue");
            $contactTelephoneValue.attr("href", "tel:" + telephone.Value);
            $contactTelephoneValue.text(telephone.Value);

        }

        if (data.ContactPictureUrl != "") {
           var $image = $("<img src='" + data.ContactPictureUrl + "' width='40%' style='float: right;margin-right: 7%;'>").error(function () {
                // If an error occurs while loading the image does, e.g. 0 bytes, do not show it.
                $(this).remove();
            });

            $("#resultContactPicture").show()
            $("#resultContactPicture").append($image);
        }

        // Email
        var email = data.ContactEmail;

        if (email.Value == "") {
            $("#resultContactEmail").hide();
        }
        else {
            $("#resultContactEmail").show();
            $("#resultContactEmailTitle").text(email.Title);

            $contactEmailValue = $("#resultContactEmailValue");
            $contactEmailValue.attr("href", "mailTo:" + email.Value);
            $contactEmailValue.text(email.Value);
        }


        // Goto result detail page.
        $.mobile.changePage("#resultDetail", {
            // transition: "slide", // If not defined: use default slide transition.
            changeHash: false //do not add page location in url bar
        });
    }

    // Loads the details of a candidate.
    function readCandidateDetails(encryptedMandantGuid, encryptedCandidateId, encryptedLanguage) {
        $.mobile.showPageLoadingMsg();

        $.ajax({ type: "POST",
            url: applicationPath + "/Home/ReadCandidateDetails",
            data: ({ encryptedMandantGuid: encryptedMandantGuid, encryptedCandidateId: encryptedCandidateId, encryptedLanguage: encryptedLanguage }),
            cache: false,
            dataType: "json",
            success: readCandidateDetailsCallback
        });
    }

    // Callback from server for ReadCandidateDetails ajax call.
    function readCandidateDetailsCallback(data) {
        $.mobile.hidePageLoadingMsg();

        if (data == "") {
            // If data could not be loaded an empty string will be returned.
            return;
        }

        var $resultDetailsRegularValuesWrapper = $("#resultDetailsRegularValuesWrapper");

        // Empty regular details values from possible previous request.
        $resultDetailsRegularValuesWrapper.empty();

        var arrLength = data.RegularSearchResultDetailValues.length;

        // First fill the regular candidate values.
        for (var i = 0; i < arrLength; i++) {

            var candidateDetailValue = data.RegularSearchResultDetailValues[i];

            // Create a heading element for the value.
            var $headingElement = $("<strong>" + candidateDetailValue.Title + ":&nbsp;</strong>");

            // Create the value element.
            var $valueElement = $("<span>" + candidateDetailValue.Value + "</span><p></p>");

            // Add the heading and value to the DOM.
            $resultDetailsRegularValuesWrapper.append($headingElement);
            $resultDetailsRegularValuesWrapper.append($valueElement);
        }

        // Now fill in the contact data.

        // Show separator line
        $("#resultDetailsContactWrapper hr").show();

        $("#resultContactTitle").text(data.ContactTitle);

        // Telephone
        var telephone = data.ContactTelephone;

        if (telephone.Value == "") {
            $("#resultContactTelephone").hide();
        }
        else {
            $("#resultContactTelephone").show();
            $("#resultContactTelephoneTitle").text(telephone.Title);

            $contactTelephoneValue = $("#resultContactTelephoneValue");
            $contactTelephoneValue.attr("href", "tel:" + telephone.Value);
            $contactTelephoneValue.text(telephone.Value);

        }

        // Email
        var email = data.ContactEmail;

        if (email.Value == "") {
            $("#resultContactEmail").hide();
        }
        else {
            $("#resultContactEmail").show();
            $("#resultContactEmailTitle").text(email.Title);

            $contactEmailValue = $("#resultContactEmailValue");
            $contactEmailValue.attr("href", "mailTo:" + email.Value);
            $contactEmailValue.text(email.Value);
        }

        // Goto result detail page.
        $.mobile.changePage("#resultDetail", {
            // transition: "slide", // If not defined: use default slide transition.
            changeHash: false //do not add page location in url bar
        });

    }

    $("#btnBack").click(function () {
        actualPage = Math.max(0, actualPage - 1);
        showPage(actualPage);
    });

    $("#btnForward").click(function () {
        actualPage = Math.min(totNofPages - 1, actualPage + 1);
        showPage(actualPage);
    });

    function showPage(pageNumber) {
        if (pageNumber < 0 || pageNumber >= totNofPages) {
            // Do nothing: invalid range.
            return;
        }

        // Hide all list items entries  
        $(".ui-listview li").css("display", "none");


        var fromIndex = actualPage * maxNofListItemsOnOnePage;
        var toIndex = fromIndex + maxNofListItemsOnOnePage;

        $(".ui-listview li").slice(fromIndex, toIndex - 1).each(function (index, item) {
            $(item).css("display", "block");
        });

        $("#paginationNavBar .ui-btn-active").removeClass("ui-btn-active");
    }


    // Initalize the about page
    var infoUrl = $("#hidAboutInfoUrl").val();
    if (infoUrl) {
        // If infoUrl is given, hide the default info page and show the infoUrl in a frame.
        $aboutContent = $("#AboutContentWrapper");
        $aboutContent.css("padding", "0px");
        $aboutContent.css("margin", "0px");

        // Clear actual content with default info page.
        $aboutContent.html('');

        // Read the info url that should be shown.
        $div = $('<div id="aboutDiv" style="height: 0px;width: 100%; overflow: auto;border-width:0px"></div');
        $("#about").bind("pageshow", function () {

            var docHeight = $(document).height();
            var headerHeight = $("div.header").outerHeight(true);
            var footerHeight = $("div[data-role='footer']").outerHeight(true);
            var height = docHeight - headerHeight - footerHeight;

            // Set the height of the iframe wrapper
            $("#aboutDiv").height(height);
        });

        // Create an iframe to show the customers info url.        
        $iframe = $('<iframe frameBorder="0" style="height: 99%; width: 99%;" src=' + infoUrl + '/>');
        $div.append($iframe);
        $aboutContent.append($div);
    }
    else {
        // Do nothing, since the default info page is already showing.
    }
});


/* Adds as list of options to a html select box.
The selectOptions must be an array of objects with Text and Value properties.
*/
$.fn.addItemsToSelectBox = function (selectOptions) {
    return this.each(function () {
        var list = this;
        $.each(selectOptions, function (index, selectOption) {
            var option = new Option(selectOption.Text, selectOption.Value);
            list.add(option);
        });
    });
};


