$(window).on("load", function () {
    $('#AccountID').next().find('.select2-selection').focus();
});

$("#btnSave").click(function (event) {
    $("#hfOldAccountID").val($("#OldAccountID").val());
    $("#hfNewAccountID").val($("#NewAccountID").val());
    var accountServiceID = "";
    $('.chkPropertyTransfer:checked').each(function (index, item) {
        if (index == 0)
            accountServiceID = $(item).closest("td").data("id");
        else
            accountServiceID += "," + $(item).closest("td").data("id");
    });

    $("#AccountServiceIDs").val(accountServiceID);

});

$(document).on("click", ".lnkViewServicePeriod", function () {   
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/PropertyTransfer/GetServicePeriodDetail",
        data: { AccountServiceID: $(this).attr("data-accountserviceid")},
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

function ValidateWorkFlowStatus($this) {
    $("#hfOldAccountID").val($("#OldAccountID").val());
    $("#hfNewAccountID").val($("#NewAccountID").val());
    var accountServiceID = "";
    $('.chkPropertyTransfer:checked').each(function (index, item) {
        if (index == 0)
            accountServiceID = $(item).closest("td").data("id");
        else
            accountServiceID += "," + $(item).closest("td").data("id");
    });

    $("#AccountServiceIDs").val(accountServiceID);
    $('#DocumentTypeDetail').val($("#DocumentTypeID option:selected").text());
    $('#workflowStatusID').val($($this).val());

    
    if ($("#frmPropertyTransfer").valid()) {
        if ($("#TransferTypeID").val() == 2 && ($("#AccountPropertyID").val() == "" || $("#AccountPropertyID").val() == "0")) {
            showAlert('error', accountPropertyRequiredMessage);
            return false;
        }
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: $($this).text(),
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed)
                $("#frmPropertyTransfer").submit();
            else
                return false;
        });
    }
    else
        return false;
}


//function confirmPropertyTransfer() {

//    if ($("#frmPropertyTransfer").valid()) {
//        swal({
//            title: swalTitleText,
//            type: "warning",
//            showCancelButton: true,
//            cancelButtonText: cancelBtnText,
//            confirmButtonColor: "#DD6B55",
//            confirmButtonText: confirmBtnText,
//            closeOnConfirm: true
//        }, function (confirmed) {
//            if (confirmed)
//                $("#btnSave").prop("type", "submit").click().prop("type", "button");
//            else
//                return false;
//        });
//    }
//    else
//        return false;
//}

$("#AccountServiceList").on("click", "#chkAll", function () {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        $(".chkPropertyTransfer").prop("checked", true);
    }
    else {
        $(".chkPropertyTransfer").prop("checked", false);
    }
});

$(document).ready(function () {
    $('#TransferDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('#TransferDate').datepicker('update', new Date());
    initSelect2("AccountPropertyID");
    initSelect2("TransferTypeID");
    GetAccountForSelect('OldAccountID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
    GetAccountForSelect('NewAccountID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
});

$("#AccountServiceList").on("click", ".chkPropertyTransfer", function () {

    if (!$(this).is(":checked")) {
        if ($("#chkAll").is(":checked"))
            $("#chkAll").prop("checked", false);
    }

    if ($('.chkPropertyTransfer:checked').length == $('.chkPropertyTransfer').length) {
        $('#chkAll').prop('checked', true);
    }

});


$("#TransferTypeID").on("change", function () {
    $(".lblProperty").html(property);
    $(".divAccountProperty").removeClass("hide");
    if ($(this).val() == 2) {          
        $(".lblProperty").html(property + ' <span class="text-danger">&nbsp;*</span>');
    } else {
        $(".divAccountProperty").addClass("hide");       
    }
});

function initSelect2($selector) {
    $("#" + $selector).select2({ width: '100%' });
}

function isNotAssociatedServices() {
    var accountID = $("#OldAccountID").val();
    $.ajax({
        url: ROOTPath + "/PropertyTransfer/isNotAssociatedServicesByAccount",
        data: { "AccountID": accountID },
        success: function (response) {
            if (response.status == true) {
                if (response.isNotAssociatedServices == 1) {
                    $("#btnAction").hide();
                    $("#btnSaveWithToolTip").removeClass("hide");
                }
                else {
                    $("#btnAction").show();
                    $("#btnSaveWithToolTip").addClass("hide");
                }
            }
            else
                showAlert("error", response.message);
        }
    });
}

function loadAccountPropertyTax(source) {
    if ($("#OldAccountID").val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountPropertyRightByOwner",
            data: { ownerID: $('#OldAccountID').val(), serviceID: 0, fiscalYearID: 0, id: null,forAccountStatementReport:null,IsTransferByRight : true },
            success: function (data) {
                $("#AccountPropertyID").empty();
                if (data.length >= 1 ) {
                    $.each(data, function (index, optiondata) {
                        $("#AccountPropertyID").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                    });
                    $("#AccountPropertyID").select2({ width: '100%' });
                    $("#AccountPropertyID option:first").val("");
                }
                loadAccountService();
                isNotAssociatedServices();
            }
        });
    }
    else {
        $("#AccountPropertyID").empty();
    }

}

function loadAccountService() {
    var accountPropertyID = $("#AccountPropertyID").val();
    var transferTypeID = $("#TransferTypeID").val();
    $(".divNewPropertyRight").removeClass("hide");
    $(".divNewPropertyRight").addClass("hide");
    $("#PropertyNumber").val("");   
    $("#RigthNumber").val("");
    $("#CondoID").val("");
    $("#CondoTypeID").val("");
    if (accountPropertyID == undefined || accountPropertyID == "") {
        accountPropertyID = 0;
        $("#btnRightUpdate").prop("disabled", true);
        
    }
    else {
        $("#btnRightUpdate").prop("disabled", false);               
    }
    if (transferTypeID == undefined || transferTypeID == "") {
        transferTypeID = 0;
    }
    var accountId = $("#OldAccountID").val();
    if (accountId == undefined || accountId == "") {
        accountId = 0;
    }
    $.ajax({
        url: ROOTPath + "/PropertyTransfer/GetAccountService",
        data: { "accountId": accountId, "AccountPropertyID": accountPropertyID, "TransferTypeID": transferTypeID },
        success: function (response) {
            if (response.status == undefined) {
                $("#AccountServiceList").html(response);
                if (transferTypeID == 2) {
                    $("#chkAll").prop('checked', true);
                    $(".chkPropertyTransfer").prop("checked", true);
                    $(".chkPropertyTransfer").attr('disabled', 'disabled');
                    $("#chkAll").attr('disabled', 'disabled');
                }
            }
            else
                showAlert("error", response.message);
        }
    });
}


function onPropertyTransferBegin() {
    var retVal = true;
    if ($("#OldAccountID").val() == $("#NewAccountID").val()) {
        showAlert("warning", ValAccountCompare);
        retVal = false;
    }
    else if ($("#AccountServiceIDs").val() == "") {
        showAlert("warning", ValRequireService);
        retVal = false;
    }
    return retVal;

}
function PropertyTransferSuccessCallback(response) {
    if (!response.status) {
        var errmsg = response.message;
        if (response.message.indexOf('|') > -1) { errmsg = errmsg.replace(/\|/g, "<br />"); }
        showAlert("error", errmsg);
    } else {
        var url = (ROOTPath + "/Collections/PropertyTransfer/Index");
        //if (response.paymentID != undefined && response.paymentID != null && response.paymentID > 0) {
        //    url = (ROOTPath + "/Collections/Payment/View?ID=" + response.paymentID + "&Type=Service&TypeID=" + response.serviceTypeId);
        //}
        window.location.href = url;
    }
}

//Add Right
$(document).on('click', '#addRight', function (e) {
    var accountServiceModel = {};
    accountServiceModel.ID = $(this).attr("data-AccountServiceID");
    accountServiceModel.AccountID = $(this).attr("data-AccountID");
    accountServiceModel.Year = $(this).attr("data-Year");
    accountServiceModel.ServiceID = $(this).attr("data-ServiceID");
    accountServiceModel.RowVersion64 = $(this).attr("data-rowVersion64");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/AddRight",
        data: { 'model': accountServiceModel },
        success: function (data) {
            $("#dvRightModal").html(data);
            $.validator.unobtrusive.parse('#frmAddRight');
            $("#rightModal").modal('show');
            $(".select2dropdown").select2({ width: '100%' });
            $('#frmAddRight').validate();
            $("#ddlRight").focus();
        }, error: function () { }
    }).always(function () {
    });
});

function RightSuccessCallback(response) {
    $("#rightModal").modal('hide');

    if (response.status === false) {
        showAlert("error", response.message);
    }
    else {        
        loadAccountService();
        showAlert('success', response.message);
    }
}
//
function ViewHistory() {
    var id = $("#AccountPropertyID").val()
    var fincaID = $("#AccountPropertyID option:selected").text()
    $("#AccountPropertyRightHistory").attr("data-id", id);
    $.ajax({
        type: "GET",
        async: false,
        url: ROOTPath + "/AccountProperty/GetAccountPropertyRightHistory",
        data: { id: id },
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divAccountPropertyRightHistory").html(data.returnData);
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
        }, error: function () {
        }
    }).always(function () {
    });
}
$("#btnUpdateRights").on("click", function () {
    if ($("#frmAccountPropertyRight").valid()) {
        var AccountPropertyID = $("#AccountPropertyRightHistory").attr("data-id");

        var fincaID = "";
        fincaID = $("#txtPropertyNumber").val();
        $("#PropertyNumber").val($("#txtPropertyNumber").val());
        $("#RigthNumber").val($("#txtRigthNumber").val());
        
        if ($("#ddlCondoType").val() > 0) {
            fincaID = fincaID + "-" + $("#ddlCondoType option:selected").text()
            $("#CondoTypeID").val($("#ddlCondoType").val())
        }
        if ($("#ddlCondo").val() > 0) {
            fincaID = fincaID + "-" + $("#ddlCondo option:selected").text()
            $("#CondoID").val($("#ddlCondoType").val())
        }
        fincaID = fincaID + "-" + $("#RigthNumber").val()

        $(".NewPropertyRight").html(fincaID);        
        $(".divNewPropertyRight").removeClass("hide");
        $("#AccountPropertyRightHistory").modal('hide');
        //$.ajax({
        //    type: "GET",
        //    url: ROOTPath + '/Accounts/AccountProperty/AccountPropertyRightUpdate',
        //    data: { id: AccountPropertyID, PropertyNumber: $("#txtPropertyNumber").val(), RigthNumber: $("#RigthNumber").val(), CondoID: $("#ddlCondo").val() || null, CondoTypeID: $("#ddlCondoType").val() || null },
        //    success: function (data) {
        //        if (data.status == true) {
        //            window.location.reload();
        //        }
        //        else
        //            showAlert('error', data.message);
        //    },
        //    error: function () { }
        //});
    }
});
