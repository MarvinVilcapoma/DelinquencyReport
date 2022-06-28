var PropertyRightList = [];
var ConstructionID = 0;
var RigthNumber;
var OwnerID;
var Area;
var Percentage;
var TerrainValue;
var TotalTerrainValue;
var TotalValue;
var RigthCodes;
var RigthCodesArray = [];
var ConstructionDetailList = [];
var OperationID = 0;

$(function () {
    //$(".select2dropdown").select2({ width: '100%' });
    PropertyRightList = $.parseJSON($('#OldAccountProperty_AccountPropertyConstructionDetailJson').val());

    var OwenerWithRigthCount = 0;

    $.grep(PropertyRightList, function (objItem, Objindex) {
        if (objItem.RigthNumber == $("#Property").val()) {
            OwenerWithRigthCount = OwenerWithRigthCount + 1;
        }
    });
    if (OwenerWithRigthCount == 0)
        PropertyRightList.push({ "RigthNumber": $("#Property").val(), "OwnerID": $("#OldAccountID").val() });

    $.each($(".Owner"), function (e) {
        if ($(this).attr("data-ownerid") > 0) {
            $(this).val($(this).attr("data-ownerid"));
        }
    });
});

function UserSuccessCallback(response) {
    if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        resolveRedirectURL(response);
    }
}

function resolveRedirectURL(response) {
    if (response.status == true) // Save OR Cancle
    {
        if ($("#workflowStatusID").val() != 43)
            window.location = ROOTPath + "/Collections/PropertyTransfer/EditSplit?ID=" + response.ID;
        else
            window.location = ROOTPath + "/Collections/PropertyTransfer/ViewSplit?ID=" + response.ID;
    }
    else if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        window.location = ROOTPath + "/Collections/PropertyTransfer";
    }
}

function ViewHistory() {
    DocumentWorkflowHistoryLogGetForSplit($("#TransferID").val(), function (logs) {
        if (logs.status) {
            $("#vertical-timeline").html(GetTimeline(logs.data));
            $("#dvAttachementModal").modal({ backdrop: "static", keyboard: false }, "show");
        }
    });
}


function DocumentWorkflowHistoryLogGetForSplit(transferID, callback) {
    $.ajax({
        url: ROOTPath + "/Collections/PropertyTransfer/DocumentWorkflowHistoryLogGetForSplit?transferID=" + $("#TransferID").val(),
        async: false,
        success: function (response) {
            callback(response);
        }
    });
}

function GetTimeline(data) {
    var tempString = "";
    if (data.length > 0) {
        tempString = GetLogs(data);
    }
    return tempString;
}

function GetLogs(userDetails) {
    var tempString = "";
    for (var j in userDetails) {
        tempString += "<div class=\"vertical-timeline-block\">";
        tempString += "<div class=\"vertical-timeline-icon gray-bg\">";
        tempString += "<i class=\"fa fa-briefcase\"></i>";
        tempString += "</div>";
        tempString += "<div class=\"vertical-timeline-content\">";
        tempString += "<p class=\"m-b-none m-t-xs m-l-sm\"><strong>" + userDetails[j].StatusName + "</strong></p>";
        tempString += "<p class=\"m-l-sm\">" + assignToText + " : " + userDetails[j].AssignToName + "</p>";
        tempString += "<p class=\"m-l-sm\">" + commentText + " : " + userDetails[j].Comments + "</p>";
        tempString += "<span class=\"vertical-date small text-muted m-l-sm\">" + userDetails[j].CreatedDateInString + "</span>";
        tempString += "</div>";
        tempString += "</div>";
    }
    return tempString;

}

function ValidateWorkFlowStatus($this) {
    if ($($this).val() == 44) {
        var confMessage = "";
        var textmessage = "";
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            SplitDelete($("#TransferID").val());
        });
    }
    else {
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: $($this).text(),
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed) {
                //AddWorkflowComment();
                Save();
            }
            else {
                return false;
            }
        });
    }

}

function AddWorkflowComment() {
    $.ajax({
        url: ROOTPath + "/PropertyTransfer/AddComment",
        data: { "WorkflowId": $("#workflowID").val(), "statusId": $("#workflowStatusID").val() },
        success: function (response) {
            if (response.status) {
                $('#divpopup').html('').html(response.RedirectUrl);
                $('#frmComments').validate();
                $('#divCommentModel').modal('show');
                $("#Comments").focus();
            }
        }
    });
}

$("#divpopup").on("click", "#btnAddComments", function () {
    if ($("#frmComments").valid()) {
        $("#AssignToID").val($("#ddlCommentAssignTo").val());
        $("#ReasonID").val($("#ddlCommentReasons").val());
        $("#Comments").val($("#txtComments").val());
        $('#divCommentModel').modal('hide');
        showAlert('info', WaitingProcessInfoMsg, 0);
        $("#form").submit();
    }
});

function Save() {
    var ConstructionRequire = false;
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    if ($("#form").valid()) {
        var ISOwnerRequired = false;

        $.grep(PropertyRightList, function (item, index) {
            PropertyRightList[index].ServiceIDs = "";
            $.each($(".Owner"), function (e) {
                var OwnerID = $(this).val();
                if ($(this).val() == "" || $(this).val() == 0) {
                    $(this).addClass("error");
                    ISOwnerRequired = true;
                }
                $(this).removeClass("error");

                var OwenerWithRigthCount = 0;

                $.grep(PropertyRightList, function (objItem, Objindex) {
                    if (objItem.OwnerID == OwnerID) {
                        OwenerWithRigthCount = OwenerWithRigthCount + 1;
                    }
                });

                if ($(this).val() == item.OwnerID && ((OwenerWithRigthCount > 1 && item.RigthNumber != $("#Property").val()) || (OwenerWithRigthCount == 1))) {
                    if (PropertyRightList[index].ServiceIDs == null || PropertyRightList[index].ServiceIDs == "") {
                        PropertyRightList[index].ServiceIDs = $(this).closest("tr").attr("ID");
                    }
                    else {
                        PropertyRightList[index].ServiceIDs += "," + $(this).closest("tr").attr("ID");
                    }

                }
            });
        });

        if (PropertyRightList.length < 2) {
            showAlert('error', PropertyRightvalidationmsg);
            return false;
        }
        else if (ISOwnerRequired && $(".Owner").length > 0) {
            showAlert('error', OwnerRequiredForService);
            return false;
        }
        else {
            $("#OldAccountProperty_AccountPropertyConstructionDetailJson").val(JSON.stringify(PropertyRightList));
        }
        AddWorkflowComment();
        // $("#form").submit();
    }
}

$(document).on("click", ".lnkViewServicePeriod", function () {
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/PropertyTransfer/GetServicePeriodDetail",
        data: { AccountServiceID: $(this).attr("data-accountserviceid") },
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {

                $("#divServicePeriodDetail").html(data.returnData);
                $("#ServicePeriodDetail").modal('show');
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

function ViewRightHistory(id,e) {
    
    $("#AccountPropertyRightHistory").attr("data-id", id);
    var fincaID = $(e).closest("tr").attr("data-code");
    $("#AccountPropertyRightHistory").attr("data-fincaid", fincaID);
    $.validator.unobtrusive.parse('#frmAccountPropertyRight');
    $('#frmAccountPropertyRight').validate();
    $("#txtPropertyNumber").val("");
    $("#CurrentPropertyNumber").val(fincaID);
    $("#txtRigthNumber").val("");
    $("#ddlCondo").val("").trigger('change');;
    $("#ddlCondoType").val("").trigger('change');;
    $("#txtPropertyNumber").focus();
    $("#AccountPropertyRightHistory").modal('show');
}
$("#btnUpdateRights").on("click", function () {
    if ($("#frmAccountPropertyRight").valid()) {

        var currentfincaID = $("#AccountPropertyRightHistory").attr("data-fincaid");
        var accountPropertyID = $("#AccountPropertyRightHistory").attr("data-id");
        var fincaID = "";
        fincaID = $("#txtPropertyNumber").val();
        if ($("#ddlCondoType").val() > 0) {
            fincaID = fincaID + "-" + $("#ddlCondoType option:selected").text()
        }
        if ($("#ddlCondo").val() > 0) {
            fincaID = fincaID + "-" + $("#ddlCondo option:selected").text()
        }
        fincaID = fincaID + "-" + $("#txtRigthNumber").val();
        console.log(fincaID)
        $.grep(PropertyRightList, function (item, index) {
            if (item.AccountPropertyID == accountPropertyID) {
                PropertyRightList[index].RigthNumber = fincaID;
                PropertyRightList[index].PropertyNumber = $("#txtPropertyNumber").val();                
                PropertyRightList[index].CondoID = $("#ddlCondo").val();
                PropertyRightList[index].CondoTypeID = $("#ddlCondoType").val();
                PropertyRightList[index].Condo = $("#ddlCondo option:selected").text()
                PropertyRightList[index].CondoType = $("#ddlCondoType option:selected").text()
                var $Span = $(".RightsTable tr[data-id='" + accountPropertyID + "'] td:eq(0) span")[0].outerHTML;
                $(".RightsTable tr[data-id='" + accountPropertyID + "'] td:eq(0)").html('').html($Span + fincaID)
                $(".RightsTable tr[data-id='" + accountPropertyID + "']").attr("data-code", fincaID);
            }
        });
        $("#AccountPropertyRightHistory").modal('hide');
    }
});

$("#tblAccountService").on("click", ".btnDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceID = $(this).attr("data-id");
    $("#DeleteConfirmModal #txtReason").val('');
    $("#DeleteConfirmModal").attr("data-accountserviceid", AccountServiceID);
    $("#DeleteConfirmModal").modal("show");

});
$(document).on("click", "#btnVoid", function () {
    var confMessage = "";
    var textmessage = "";      
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            SplitDelete($("#TransferID").val());
        });
   
});
function SplitDelete(TransferID) {
    $.ajax({
        type: "GET",
        url: ROOTPath + "/PropertyTransfer/DeleteSplit",
        data: { id: TransferID },
        success: function (data) {
            if (data.status == true) {                        
                window.location = ROOTPath + "/Collections/PropertyTransfer";
            }
            else
                showAlert('error', data.message);
        },
        error: function () { }
    });

}