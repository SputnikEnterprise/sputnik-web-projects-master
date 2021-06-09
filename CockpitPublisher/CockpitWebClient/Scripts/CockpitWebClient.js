/* Cockpit WebClient javascript functions */

// Setups method for table data control
function SetupTableDataControl($controlWrapperDiv, tableConfigurationId, dataWebServiceURL, gridRowInsertedJavaScriptHandler, isMobileBrowser, gridWidth, gridHeight) {

    // Find table caption element.
    var $tableCaption = $(".tableDataHeader .tableCaption", $controlWrapperDiv);

    // Register toggle hander for table caption element.
    $tableCaption.toggle(function () {

        // Show loading animation.
        ShowLoadingAnimation();


        // Get the table data container element.
        var $tableDataContainer = $(this).parent().next(".tableData");

        // Empty the table data container element (mabye there is existing table data)
        $tableDataContainer.empty();

        // The jqGrid needs a <table/> element to build its data grid. 
        // The table elements needs an unique id. 
        var tableDataContainerId = $tableDataContainer.attr("id")
        var jqGridTableId = tableDataContainerId + "_jqGridTableElement";

        // Create a table element used by jqGrid to build the final data table.
        var $newTableElement = $("<table id='" + jqGridTableId + "'></table>");

        // Place the newly created table elment in the table data container.
        $newTableElement.appendTo($tableDataContainer)

        // Get the selected MD guid value. 
        var selectedMdGuid = GetSelectedMDGuid();

        // Get the selectd KST value.
        var selectedKSTGuid = GetSelectedKST();

        // Load grid data with ajax.
        LoadGridData(selectedMdGuid,
                selectedKSTGuid,
                tableConfigurationId,
                dataWebServiceURL,
                gridRowInsertedJavaScriptHandler,
                $newTableElement,
                gridWidth,
                gridHeight,
                function () { HideLoadingAnimation(); $tableDataContainer.slideDown(); });

        return false;
    }, function () {
        // Get the table data container element.
        var $tableDataContainer = $(this).parent().next(".tableData");
        $tableDataContainer.slideUp();
    });

    // Find magnifier element.
    var $imgMagnifier = $(".tableDataHeader .imgMagnifier", $controlWrapperDiv);

    // Register click hander for magnifier element.
    $imgMagnifier.click(function () {

        // Show loading animation.
        ShowLoadingAnimation();

        // Find the global dialog wrapper element.
        // In this element dynamically created dialog content is placed before it gets presented.
        var $globalDialogWrapper = $("#globalDialogWrapper");
        $globalDialogWrapper.empty();

        // The dialog title
        var dialogTitle = "Detail: " + $tableCaption.text();

        // Create a div delement thats wrapps the dialog contents.
        var $dialogElement = $("<div title='" + dialogTitle + "'></div>")

        // Place the newly creaetd div in the global dialog wrapper element.
        $dialogElement.appendTo($globalDialogWrapper);

        // Get the table data container element.
        var $tableDataContainer = $(this).parent().next(".tableData");

        // The jqGrid needs a <table/> element to build its data grid. 
        // The table elements needs an unique id. 
        var tableDataContainerId = $tableDataContainer.attr("id")
        var jqGridTableId = tableDataContainerId + "_magnifiedTableElement";

        // Create a table element used by jqGrid to build the final data table.
        var $newTableElement = $("<table id='" + jqGridTableId + "'></table>");

        // Place the newly created table elment in the dialog element.
        $newTableElement.appendTo($dialogElement)

        var magnifiedGridWidth = 780;
        var magnifiedGridHeight = 530;
        var dialogWidth = 800;
        var dialogHeight = 600;

        if (isMobileBrowser == "true") {
            // Override dimensions for mobile browsers
            magnifiedGridWidth = 455;
            magnifiedGridHeight = 400
            dialogWidth = 480;
            dialogHeight = 480;
        }

        // Get the selected MD guid value. 
        var selectedMdGuid = GetSelectedMDGuid();

        // Get the selectd KST value.
        var selectedKSTGuid = GetSelectedKST();

        // Load grid data with ajax.
        LoadGridData(selectedMdGuid,
                selectedKSTGuid,
                tableConfigurationId,
                dataWebServiceURL,
                gridRowInsertedJavaScriptHandler,
                $newTableElement,
                magnifiedGridWidth,
                magnifiedGridHeight,
                function () {
                    HideLoadingAnimation();

                    // Show the dialog 
                    $dialogElement.dialog({
                        width: dialogWidth,
                        height: dialogHeight,
                        resizable: false,
                        modal: true,
                        close: function (ev, ui) { $(this).remove(); }
                    });
                });

        return false;
    });

    // Regiser hover event for magnifier image.
    // The magnifier image gets highlighted once the user hovers over the image. 
    $imgMagnifier.hover(function () {
        $(this).removeClass("imgMangifierStateNormal")
               .addClass("imgMangifierStateHighlight");
    }, function () {
        $(this).removeClass("imgMangifierStateHighlight")
               .addClass("imgMangifierStateNormal"); 
   });

}

// Loads grid data with ajax.
function LoadGridData(mdGuid, kst, tableId, serviceURL, gridRowInsertedJavaScriptHandler, grid, gridWidth, gridhHeight, completeHandler) {
    
    // Perform ajax call to get the table data.
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: serviceURL,
        // These are the parameters for the web server 
        data: '{mdGuid:"' + mdGuid + '",kst:"' + kst + '",tableId:"' + tableId + '"}',
        dataType: "json",
        success: function (result) {

            var data = {};

            // Complete handler function call wrapper.
            var completeHandlerCall = function () {
                // Check if the complete handler is defined
                if (typeof completeHandler == 'function') {
                    // Call the complete handler. 
                    completeHandler();
                }
            }

            try {
                // Parse the json data sent by the web service.
                // Some older browser may not support the JSON.parse method!
                data = JSON.parse(result.d)
            }
            catch (e) {
                // Call the complete handler.
                completeHandlerCall();
                return;
            }

            if (data.PagedListJson == undefined ||
                data.ColumnNamesJson == undefined ||
                data.ColumnModelsJson == undefined) {
                completeHandlerCall();
                return;
            }

            // The jqGrid paged list data (contains the grid row data).
            colD = data.PagedListJson;

            // The column names.
            colN = JSON.parse(data.ColumnNamesJson);

            // The column model.
            colM = JSON.parse(data.ColumnModelsJson);

            InsertFunctionReferenceForFormatters(colM);

            // Build the jqGrid with the use of the data sent by the web service.
            grid.jqGrid({
                jsonReader: { repeatitems: false },
                width: gridWidth - 2, // Ajust the width a little bit so it matches the header width.
                height: gridhHeight,
                datatype: 'jsonstring',
                mtype: 'POST',
                datastr: colD,
                colNames: colN,
                colModel: colM,
                afterInsertRow: function (rowid, aData) {

                    if (gridRowInsertedJavaScriptHandler.length != 0) {

                        // Get the function reference to the row inserted handler.
                        var functionReference = window[gridRowInsertedJavaScriptHandler];

                        if (typeof functionReference === 'function') {
                            functionReference(grid, rowid, aData);
                        }
                    }
                },
                shrinkToFit: false,
                autowidth: false,
                rowNum: -1,
                gridComplete: function () {
                    // Call the complete handler.
                    completeHandlerCall();
                }
            });
        }
    });
}

// Gets the selected MD guid value.
function GetSelectedMDGuid() {
    var selectedMDGuid = $("div#chooseIdentityWrapper #ddlMDs").val();

    return selectedMDGuid;
}

// Gets the selected KST guid value.
function GetSelectedKST() {
    var selectedKST = $("div#chooseIdentityWrapper #ddlKSTs").val();

    return selectedKST;
}

// Shows the loading animation gif image..
function ShowLoadingAnimation() {
    $("#imgLoadingAnimation").show();
}

// Hids the loading animation gif image.
function HideLoadingAnimation() {
    $("#imgLoadingAnimation").hide();
}

// Image column formatter for jqGrid.
function JqGridColumnImageFormatter(cellvalue, options, rowdata) {
    return '<img src="' + cellvalue + '" />';
}

// Insertes concrete formatter function references.
// ToDo: Make this function generic!!!
function InsertFunctionReferenceForFormatters(colM) {
    for (var i = 0; i < colM.length; i++) {
        var currentColumnModel = colM[i];

        if (currentColumnModel.hasOwnProperty("formatter")) {
            if (currentColumnModel["formatter"] == "JqGridColumnImageFormatter") {
                currentColumnModel["formatter"] = JqGridColumnImageFormatter
            }
        }
    }
}

/*  --Client side row inserted handlers-- */

// Row inserted handler for assignment of personnel table 3.
function RowInsertedHandler_AssignmentOfPersonnelTable3(grid, rowid, rowData) {
    if (rowData.AnzDiff < 0.0) {
        $("#" + rowid, grid).addClass('RowRedCss');
    }
}

// Row inserted handler for assignment of personnel table 5.
function RowInsertedHandler_AssignmentOfPersonnelTable5(grid, rowid, rowData) {
    if (rowData.AnzDiff < 0.0) {
        $("#" + rowid, grid).addClass('RowRedCss');
    }
}

// Row inserted handler for sales figures table 1.
function RowInsertedHandler_SalesFiguresTable1(grid, rowid, rowData) {

    var art = rowData.Art;
    var strOPArt = art.charAt(0).toUpperCase();
    var cssClass = '';

    if (strOPArt == "A") {
        cssClass = 'RowBlackCss';
    }
    else if(strOPArt == "F") {
        cssClass = 'RowBlueCss';
    }
    else if (strOPArt == "I") {
        cssClass = "RowVioletCss";
    }
    else if (strOPArt == "R") {
        cssClass = "RowRedCss";
    }

    if (cssClass.length != 0) {
        $("#" + rowid, grid).addClass(cssClass);
    }
}

// Row inserted handler for sales figures table 3.
function RowInsertedHandler_SalesFiguresTable3(grid, rowid, rowData) {

    var art = rowData.Art;
    var strOPArt = art.charAt(0).toUpperCase();
    var cssClass = '';

    if (strOPArt == "A") {
        cssClass = 'RowBlackCss';
    }
    else if (strOPArt == "F") {
        cssClass = 'RowBlueCss';
    }
    else if (strOPArt == "I") {
        cssClass = "RowVioletCss";
    }
    else if (strOPArt == "R" || strOPArt == "G") {
        cssClass = "RowRedCss";
    }

    if (cssClass.length != 0) {
        $("#" + rowid, grid).addClass(cssClass);
    }
}

// Row inserted handler for sales figures table 2.
function RowInsertedHandler_SalesFiguresTable2(grid, rowid, rowData) {
    if (rowData.RowCssClass.length != 0) {
        $("#" + rowid, grid).addClass(rowData.RowCssClass);
    }
}

// Row inserted handler for sales figures table 5.
function RowInsertedHandler_SalesFiguresTable5(grid, rowid, rowData) {
    if (rowData.RowCssClass.length != 0) {
        $("#" + rowid, grid).addClass(rowData.RowCssClass);
    }
}

// Row inserted handler for candidates data table 2.
function CandidatesDataTable2Formatter(grid, rowid, rowData) {
    if (rowData.HighlightRow == "true") {
        $("#" + rowid, grid).addClass("RowYellowBackgroundCss");
    }
}

// Row inserted handler for candidates data table 2.
function CandidatesDataTable4Formatter(grid, rowid, rowData) {
    if (rowData.HighlightRow == "true") {
         $("#" + rowid, grid).addClass("RowYellowBackgroundCss");
    }
}

