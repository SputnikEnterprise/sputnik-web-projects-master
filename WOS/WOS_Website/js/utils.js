
function OnForwardDocumentComplete(result) {

   $text = result ? 'Email wurde erfolgreich versandt.' : "Email konnte nicht versandt werden!"
   $stylename = result ? 'green' : "red"

   $ref = $(".targetDocument").qtip(
                {
                    content: {
                        prerender: true,
                        text: $text 
                    },
                    position: {
                        corner: {
                            target: 'bottomMiddle', // Position the tooltip above the link
                            tooltip: 'topMiddle'
                        },
                        adjust: {
                            screen: true,
                            scroll: true,
                            resize: true
                        }
                    },
                    effect: {
                        type: 'fade'
                    },
                    show: {
                        when: 'mouseover',
                        solo: true // Only show one tooltip at a time
                    },
                    hide: 'unfocus',
                    style: {
                        tip: true, // Apply a speech bubble tip to the tooltip at the designated tooltip corner
                        border: {
                            width: 0,
                            radius: 4
                        },
                        name: $stylename // Use the default light style
                    }
                });
                $ref.qtip("show");

                // Start timeout to destroy the tool tip after 3.5 sec
                setTimeout(function () { $ref.qtip("destroy"); $(".targetDocument").removeClass("targetDocument") }, 3500);
}

function printWebPart(tagid, width, height) {
    if (tagid) {
        $("#" + tagid).removeAttr('height');
        $("#" + tagid).removeAttr('width');
        //build html for print page
        var html = "<HTML>\n<HEAD>\n" + $("head").html() + "\n</HEAD>\n<BODY>\n" + $("#" + tagid).html() + "\n</BODY>\n</HTML>";

        //open new window
        var printWP = window.open("", "printWebPart", "location=1,status=1,scrollbars=1, width=" + width + ",height=" + height + "");
        printWP.document.open();
        //insert content
        printWP.document.write(html);
        printWP.document.close();
        //open print dialog
        printWP.print();
    }
}