var PropertyRightList = [];
var ConstructionID = 0;
var RigthNumber;
var OwnerID;
var Area;
var Percentage;
var TerrainValue;
var TotalTerrainValue;
var TotalValue;
var CurrentValueFromPropertyTax;
var RigthCodes;
var RigthCodesArray = [];
var ConstructionDetailList = [];
var OperationID = 0;
var PropertyNumber;
var FincaID;
var Condo;
var CondoType;
var CondoID;
var CondoTypeID;

$(function () {
    $(".select2dropdown").select2({ width: '100%' });
    GetAccountForSelect('OldAccountID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
    $('#SplitDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('#SplitDate').datepicker('update', new Date());
});

function loadAccountPropertyTax(source) {
    if ($("#OldAccountID").val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountPropertyRightByOwner",
            data: { ownerID: $('#OldAccountID').val(), serviceID: 0, fiscalYearID: 0 },
            success: function (data) {
                $("#AccountPropertyID").empty();
                if (data.length > 1) {
                    $.each(data, function (index, optiondata) {
                        $("#AccountPropertyID").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                    });
                    $("#AccountPropertyID").select2({ width: '100%' });
                    $("#AccountPropertyID option:first").val("");
                }
                $(".RightsTable tbody").html('');
                ConstructionDetailList = [];
                PropertyRightList = [];
                $('#AccountPropertyConstructionDetailJson').val();
            }
        });
    }
    else {
        $("#AccountPropertyID").empty();
    }

}
function isNotAssociatedServices() {
    var accountPropertyID = $("#AccountPropertyID").val();
    $.ajax({
        url: ROOTPath + "/PropertyTransfer/isNotAssociatedServices",
        data: { "commaSeparatedPropertyID": accountPropertyID },
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

function loadAccountService() {
    var accountPropertyID = $("#AccountPropertyID").val();
    var transferTypeID = $("#TransferTypeID").val();

    if (accountPropertyID == undefined || accountPropertyID == "") {
        accountPropertyID = 0;
    }

    var accountId = $("#OldAccountID").val();
    if (accountId == undefined || accountId == "") {
        accountId = 0;
    }
    if (accountPropertyID > 0) {
        $("#Property").val($("#AccountPropertyID").select2('data')[0].text);
        $("#btnRightUpdate").prop("disabled", false);
    }
    else {
        $("#btnRightUpdate").prop("disabled", true);
    }

    $.ajax({
        url: ROOTPath + "/PropertyTransfer/GetAccountServiceForSplit",
        data: { "accountId": accountId, "AccountPropertyID": accountPropertyID, "WorkflowStatusID": $("#workflowStatusID").val(), "TransferTypeID": 2 },
        success: function (response) {
            if (response.status == undefined) {
                $("#AccountServiceList").html(response);
                if (accountPropertyID > 0) {
                    $.ajax({
                        url: ROOTPath + "/PropertyTransfer/GetAccountPropertyDetail",
                        data: { ID: accountPropertyID },
                        async: false,
                        success: function (response) {
                            if (response.status == false) {
                                showAlert('error', response.message);
                                e.stopPropagation();
                            }
                            else {
                                isNotAssociatedServices();
                                $('#AccountPropertyConstructionDetailJson').val(response.AccountPropertyConstructionDetailJson)
                                RigthNumber = response.RigthNumber;
                                PropertyNumber = response.PropertyNumber;
                                FincaID = response.FincaID;
                                Condo = response.Condo;
                                CondoType = response.CondoType;
                                CondoID = response.CondoID;
                                CondoTypeID = response.CondoTypeID;
                                OwnerID = response.OwnerID;
                                Area = response.Area;
                                Percentage = response.Percentage;
                                TerrainValue = response.TerrainValue || 0;
                                TotalTerrainValue = response.TotalTerrainValue || 0;
                                TotalValue = response.TotalValue || 0;
                                CurrentValueFromPropertyTax = response.CurrentValueFromPropertyTax;
                                RigthCodes = response.RigthCodes;
                                RigthCodesArray = RigthCodes.split(',');
                                ConstructionDetailList = $.parseJSON($('#AccountPropertyConstructionDetailJson').val());
                                $(".RightsTable tbody").html('');
                                PropertyRightList = [];
                                $('#AccountPropertyConstructionDetailJson').val();
                                $(".Owner").append('<option value="' + $('#OldAccountID').val() + '">' + $("#OldAccountID option:selected").text().trim() + '</option>');

                                //var PropertyRightModel = {};
                                //PropertyRightModel.RigthNumber = RigthNumber;
                                //PropertyRightModel.OwnerID = OwnerID;
                                //PropertyRightModel.Percentage = Percentage;
                                //PropertyRightModel.Area = Area;
                                //PropertyRightModel.HdnArea = Area;
                                //PropertyRightModel.TerrainValue = TerrainValue;
                                //PropertyRightModel.TotalTerrainValue = TotalTerrainValue;
                                //PropertyRightModel.HdnTotalTerrainValue = TotalTerrainValue;
                                //PropertyRightModel.AccountPropertyConstructionDetailJson = jQuery.extend(true, [], ConstructionDetailList);
                                //PropertyRightList.push(PropertyRightModel);
                                //Old Account Property

                                $(".OldDivRightsTable .footable-row-detail").remove();
                                $(".OldDivRightsTable .footable-visible").removeClass("footable-first-column");
                                $(".OldDivRightsTable .footable-visible").removeClass("footable-last-column");
                                $(".OldDivRightsTable .footable-visible").removeClass("footable-visible");
                                $(".OldRightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
                                $(".Old thead td:last,.RightsTable tbody td:last").show();
                                var $TrRow = '<tr data-code="' + response.FincaID + '">\
                                                    <td class="table-description-field">'+ response.FincaID + '</td >\
                                                    <td>'+ response.Owner + '</td>\
                                                    <td class="text-right">'+ parseFloat(response.Percentage || 0).toFixed(__decimalPoints) + ' %</td>\
                                                    <td class="text-right">'+ parseFloat(response.Area || 0).toFixed(__decimalPoints) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(response.TerrainValue || 0)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(response.TotalTerrainValue || 0)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(TotalValue)) + '</td>\
                                                    <td class="text-right">\
                                                    </td >\
                                                    <td class="OldConstructionList"></td >\
                                                </tr > ';
                                $(".OldRightsTable tbody").html('');
                                $(".OldRightsTable tbody:first").append($TrRow);

                                var OldConstructionDetailList = $.parseJSON(response.AccountPropertyConstructionDetailJson);
                                $.map(OldConstructionDetailList, function (item, index) {

                                    $Div = '<div class="row1 border-bottom">\
                                                <table width ="100%">\
                                                    <tbody>\
                                                        <tr data-code="'+ response.FincaID + '">\
                                                            <td class="col-lg-11">\
                                                                <table class="table no-borders">\
                                                                    <tbody>\
                                                                        <tr>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                                                            <td class="text-left">'+ item.MaterialType + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ ConstructionType + ' :</label></td>\
                                                                            <td class="text-left"> '+ (item.ConstructionType == null ? '-' : item.ConstructionType) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ CurrentAge + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.TotalYears > 0 ? item.TotalYears : '-') + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.Status == null ? '-' : item.Status) + '</td>\
                                                                        </tr>\
                                                                        <tr>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ UsefulLife + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.UsefulLife == "" || item.UsefulLife == null ? "-" : item.UsefulLife) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.Floor > 0 ? item.Floor : '-') + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.Chambers > 0 ? item.Chambers : '-') + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.InternalWalls == null ? '-' : item.InternalWalls) + '</td>\
                                                                        </tr>\
                                                                        <tr>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                                                            <td class="text-left">'+ (item.ExternalWalls == null ? '-' : item.ExternalWalls) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                                                            <td class="text-left">'+ (item.Structure == null ? '-' : item.Structure) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                                                            <td class="text-left">'+ (item.Roof == null ? '-' : item.Roof) + ' </td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                                                            <td class="text-left">'+ (item.Ceiling == null ? '-' : item.Ceiling) + ' </td>\
                                                                        </tr>\
                                                                        <tr>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                                                            <td class="text-left">'+ (item.Floor == null ? '-' : item.Floor) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                                                            <td class="text-left">'+ (item.Bathroom > 0 ? item.Bathroom : "-") + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' :</label></td>\
                                                                            <td class="text-left">'+ parseFloat(item.BuildingArea).toFixed(__decimalPoints) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                                            <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.UnitValue)) + '</td>\
                                                                        </tr>\
                                                                        <tr>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ BuildingTotalValue + '  :</label></td>\
                                                                            <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.TotalValue)) + '</td>\
                                                                            <td class="control-label text-right" colspan="6"></td>\
                                                                        </tr>\
                                                                    </tbody>\
                                                                </table>\
                                                            </td>\
                                                            <td class="col-lg-1 text-right">\
                                                            </td>\
                                                        </tr>\
                                                </tbody>\
                                        </table >\
                                    </div >';

                                    $(".OldConstructionList", $(".OldRightsTable tr[data-code='" + response.FincaID + "']")).append($Div);

                                });

                                $('.OldDivRightsTable .footable').footable();

                            }
                        }
                    });
                    if ($("#IsNeedToDeleteFilling").val() == "True") {
                        showAlert('error', ValidSplitFillingError);
                    }

                }
                else {
                    $(".RightsTable tbody").html('');
                    PropertyRightList = [];
                    $(".OldRightsTable tbody").html('');
                    $("#btnAction").show();
                    $("#btnSaveWithToolTip").addClass("hide");
                }

            }
            else
                showAlert("error", response.message);
        }
    });
}

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
        window.location = ROOTPath + "/Collections/PropertyTransfer/EditSplit?ID=" + response.ID;
    }
    else if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        window.location = ROOTPath + "/Collections/PropertyTransfer";
    }
}

// save account property detail
$("#btnSave").click(function (event) {
    if (Save() == false)
        return false;
});

$("#btnSaveNew").click(function (event) {
    if (Save() == false)
        return false;
});

function ValidateWorkFlowStatus($this) {
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
            Save();
        }
        else {
            return false;
        }
    });

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
        var TotalPercentage = 0.0;
        var TotalConstructionPercentage = 0.0;
        var ValReqForConstructionArea = false;
        $.grep(PropertyRightList, function (item, index) {
            PropertyRightList[index].ServiceIDs = "";
            TotalPercentage = TotalPercentage + parseFloat(item.Percentage || 0);
            $.each($(".Owner"), function (e) {
                if ($(this).val() == "" || $(this).val() == 0) {
                    $(this).addClass("error");
                    ISOwnerRequired = true;
                }
                $(this).removeClass("error");
                if ($(this).val() == item.OwnerID) {
                    if (PropertyRightList[index].ServiceIDs == null || PropertyRightList[index].ServiceIDs == "") {
                        PropertyRightList[index].ServiceIDs = $(this).closest("tr").attr("ID");
                    }
                    else {
                        PropertyRightList[index].ServiceIDs += "," + $(this).closest("tr").attr("ID");
                    }

                }
            });
        });

        $.grep(ConstructionDetailList, function (item, index) {
            TotalConstructionPercentage = 0.0;
            $.grep(PropertyRightList, function (item1, index1) {
                $.grep(item1.AccountPropertyConstructionDetailJson, function (item2, index2) {
                    if (item.ID == item2.ID) {
                        TotalConstructionPercentage = TotalConstructionPercentage + parseFloat(item2.Percentage || 0);
                    }
                });
            });
            if (TotalConstructionPercentage > 100.05) {
                ValReqForConstructionArea = true;                
            }
        });

        if ($("#SplitTypeID").val() == 2) {
            var OriginalOwnerRequire = true;
            $.grep(PropertyRightList, function (item, index) {
                if (item.OwnerID == $("#OldAccountID").val()) {
                    OriginalOwnerRequire = false;
                }
            });
            if (OriginalOwnerRequire == true && ConstructionDetailList.length > 0) {
                showAlert('error', ValidSplitTyprError);
                return false;
            }
        }

        if ($("#IsNeedToDeleteFilling").val() == "True") {
            showAlert('error', ValidSplitFillingError);
            return false;
        }
        if (PropertyRightList.length < 2) {
            showAlert('error', PropertyRightvalidationmsg);
            return false;
        }
        else if (TotalPercentage > 100.05) {
            showAlert('error', validatePercentageMsg);
            return false;
        }
        else if (ValReqForConstructionArea) {
            showAlert('error', ValidConstructionAreaMsg);
            return false;
        }
        else if (ISOwnerRequired) {
            showAlert('error', OwnerRequiredForService);
            return false;
        }
        else if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == false && TotalValue == 0) {
            showAlert('error', ValidSplitCurrentValueError);
            return false;
        }
        else {
            $("#AccountPropertyConstructionDetailJson").val(JSON.stringify(PropertyRightList));
        }
        AddWorkflowComment();
    }
}

//AddRights
$('.addRights').on('click', function () {
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    ClearRights();
    if ($("#form").valid()) {

        $.ajax({
            type: "POST",
            async: false,
            url: ROOTPath + "/AccountProperty/AddRights",
            success: function (data) {
                if (data.status == false) {
                    showAlert('error', data.message);
                    e.stopPropagation();
                }
                else {
                    $("#divAddRights").html(data);
                    if ($("#SelectionTypeID").val() == "1") {
                        $("#Area").attr("readonly", "readonly");
                        $("#Percentage").removeAttr("readonly")
                    }
                    else {
                        $("#Percentage").attr("readonly", "readonly")
                        $("#Area").removeAttr("readonly")
                    }
                    $.validator.unobtrusive.parse('#frmRights');
                    $("#RigthNumber").val('');
                    //$("#OwnerID").val(OwnerID).trigger('change');
                    $("#Area").val('');
                    $("#HdnArea").val(Area.toFixed(__decimalPoints));
                    $("#Percentage").val('');
                    if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == false) {

                        $("#TerrainValue").val(TerrainValue.toFixed(__decimalPoints));
                        $("#HdnTotalTerrainValue").val(TotalTerrainValue.toFixed(__decimalPoints));
                    }
                    else {
                        var ZeroValue = 0;
                        $("#TerrainValue").val(ZeroValue.toFixed(__decimalPoints));
                        $("#HdnTotalTerrainValue").val(ZeroValue.toFixed(__decimalPoints));
                    }


                    $("#TotalTerrainValue").val('');
                    $("#TotalValue").val(TotalValue.toFixed(__decimalPoints));
                    $("#CurrentValueFromPropertyTax").val(CurrentValueFromPropertyTax.toFixed(__decimalPoints));
                    $("#NewCurrentValue").val('')
                    $("#BtnAddtype").val('newRight');
                    if ($("#SplitTypeID").val() == 1) {
                        $("#divConstructionArea").html('');
                        $("#divComplementaryArea").html('');
                        $.grep(jQuery.extend(true, [], ConstructionDetailList), function (data, Index) {

                            if (data.MaterialType == "Complementary") {
                                var DivComplementaryTr = '<tr data-id="' + data.ID + '">\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right Currencyinputdecimal ComplementaryArea" readonly="true" value="'+ data.BuildingArea.toFixed(__decimalPoints) + '"/>\
                                                            <input type="hidden" class="form-control text-right UnitValue" value="'+ data.UnitValue + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required ComplementaryValue inputpercentage"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required Currencyinputdecimal NewConstructionArea" />\
                                                    </td>\
                                                 </tr>';
                                $("#divComplementaryArea").append(DivComplementaryTr);
                            }
                            else {
                                var DivConstructionTr = '<tr data-id="' + data.ID + '">\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right Currencyinputdecimal ConstructionArea" readonly="true" value="'+ data.BuildingArea.toFixed(__decimalPoints) + '"/>\
                                                            <input type="hidden" class="form-control text-right UnitValue" value="'+ data.UnitValue + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required ContructionValue inputpercentage "/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required Currencyinputdecimal NewConstructionArea" />\
                                                    </td>\
                                                 </tr>';
                                $("#divConstructionArea").append(DivConstructionTr);
                            }
                        });

                        if ($("#divConstructionArea").html() == "") {
                            var NoData = '<tr>\
                                        <td class="text-center" colspan = "3">'+ nodatamsg + '\
                                        </td>\
                                     </tr>';
                            $("#divConstructionArea").append(NoData)
                        }
                        if ($("#divComplementaryArea").html() == "") {
                            var NoData = '<tr>\
                                        <td class="text-center" colspan = "3">'+ nodatamsg + '\
                                        </td>\
                                     </tr>';
                            $("#divComplementaryArea").append(NoData)
                        }
                    }
                    if ($("#SelectionTypeID").val() == "1") {
                        $(".NewConstructionArea").attr("readonly", "readonly");
                        $(".inputpercentage").removeAttr("readonly")
                    }
                    else {
                        $(".inputpercentage").attr("readonly", "readonly")
                        $(".NewConstructionArea").removeAttr("readonly")
                    }
                    $('#RigthNumber').focus();
                    $("#AddRightsmodal").attr('data-code', '');
                    $("#AddRightsmodal").attr('data-mode', 'new');
                    $("#OwnerID").select2({ width: '100%' });
                    $('#frmRights').validate();
                    $("#AddRightsmodal #btnAddRights").html(Add);
                    $("#AddRightsmodal #addresstitle").html(NewRight);
                    GetAccountForSelect('OwnerID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
                    //$.ajax({
                    //    type: "POST",
                    //    async: false,
                    //    url: ROOTPath + "/Account/GetAccountForSearch",
                    //    data: {
                    //        'accountID': OwnerID,
                    //        'isActive': true,
                    //        pageIndex: 1,
                    //        pageSize: 1
                    //    },
                    //    success: function (data) {
                    //        $('#OwnerID').append($('<option>', {
                    //            value: data.AccountList[0].id,
                    //            text: data.AccountList[0].text
                    //        }));
                    //        $("#OwnerID").val(data.AccountList[0].id).trigger('change');

                    //    }
                    //});
                    $("#AddRightsmodal").modal('show');
                }

            }, error: function () {
            }
        }).always(function () {
        });
    }

})

$('.addOriginalRights').on('click', function () {
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    ClearRights();
    if ($("#form").valid()) {
        $.ajax({
            type: "POST",
            async: false,
            url: ROOTPath + "/AccountProperty/AddRights",
            success: function (data) {
                if (data.status == false) {
                    showAlert('error', data.message);
                    e.stopPropagation();
                }
                else {
                    $("#divAddRights").html(data);
                    $.validator.unobtrusive.parse('#frmRights');
                    if ($("#SelectionTypeID").val() == "1") {
                        $("#Area").attr("readonly", "readonly");
                        $("#Percentage").removeAttr("readonly")
                    }
                    else {
                        $("#Percentage").attr("readonly", "readonly")
                        $("#Area").removeAttr("readonly")
                    }
                    $("#RigthNumber").val($("#AccountPropertyID option:selected").text().split('-').pop());
                    $("#RigthNumber").attr("readonly", "readonly")
                    //$("#OwnerID").val(OwnerID).trigger('change');
                    $("#Area").val('');
                    $("#HdnArea").val(Area.toFixed(__decimalPoints));
                    $("#Percentage").val('');
                    if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == false) {

                        $("#TerrainValue").val(TerrainValue.toFixed(__decimalPoints));
                        $("#HdnTotalTerrainValue").val(TotalTerrainValue.toFixed(__decimalPoints));
                    }
                    else {
                        var ZeroValue = 0;
                        $("#TerrainValue").val(ZeroValue.toFixed(__decimalPoints));
                        $("#HdnTotalTerrainValue").val(ZeroValue.toFixed(__decimalPoints));
                    }
                    $("#TotalTerrainValue").val('');
                    $("#TotalValue").val(TotalValue.toFixed(__decimalPoints));
                    $("#CurrentValueFromPropertyTax").val(CurrentValueFromPropertyTax.toFixed(__decimalPoints));
                    $("#BtnAddtype").val('originalRight');
                    if ($("#SplitTypeID").val() == 1) {
                        $("#divConstructionArea").html('');
                        $("#divComplementaryArea").html('');
                        $.grep(jQuery.extend(true, [], ConstructionDetailList), function (data, Index) {

                            if (data.MaterialType == "Complementary") {
                                var DivComplementaryTr = '<tr data-id="' + data.ID + '">\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right Currencyinputdecimal ComplementaryArea" readonly="true" value="'+ data.BuildingArea.toFixed(__decimalPoints) + '"/>\
                                                            <input type="hidden" class="form-control text-right UnitValue" value="'+ data.UnitValue + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required ComplementaryValue inputpercentage"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required Currencyinputdecimal NewConstructionArea"/>\
                                                    </td>\
                                                 </tr>';
                                $("#divComplementaryArea").append(DivComplementaryTr);
                            }
                            else {
                                var DivConstructionTr = '<tr data-id="' + data.ID + '">\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right Currencyinputdecimal ConstructionArea" readonly="true" value="'+ data.BuildingArea.toFixed(__decimalPoints) + '"/>\
                                                            <input type="hidden" class="form-control text-right UnitValue" value="'+ data.UnitValue + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required ContructionValue inputpercentage "/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required Currencyinputdecimal NewConstructionArea"/>\
                                                    </td>\
                                                 </tr>';
                                $("#divConstructionArea").append(DivConstructionTr);
                            }
                        });

                        if ($("#divConstructionArea").html() == "") {
                            var NoData = '<tr>\
                                        <td class="text-center" colspan = "3">'+ nodatamsg + '\
                                        </td>\
                                     </tr>';
                            $("#divConstructionArea").append(NoData)
                        }
                        if ($("#divComplementaryArea").html() == "") {
                            var NoData = '<tr>\
                                        <td class="text-center" colspan = "3">'+ nodatamsg + '\
                                        </td>\
                                     </tr>';
                            $("#divComplementaryArea").append(NoData)
                        }
                    }
                    if ($("#SelectionTypeID").val() == "1") {
                        $(".NewConstructionArea").attr("readonly", "readonly");
                        $(".inputpercentage").removeAttr("readonly")
                    }
                    else {
                        $(".inputpercentage").attr("readonly", "readonly")
                        $(".NewConstructionArea").removeAttr("readonly")
                    }
                    $('#Percentage').focus();
                    $("#AddRightsmodal").attr('data-code', '');
                    $("#AddRightsmodal").attr('data-mode', 'new');
                    $("#OwnerID").select2({ width: '100%' });
                    $('#frmRights').validate();
                    $("#AddRightsmodal #btnAddRights").html(Add);
                    $("#AddRightsmodal #addresstitle").html(NewRight);
                    //GetAccountForSelect('OwnerID', $('#OldAccountID').val(), '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: ROOTPath + "/Account/GetAccountForSearch",
                        data: {
                            'accountID': $('#OldAccountID').val(),
                            'isActive': true,
                            pageIndex: 1,
                            pageSize: 1
                        },
                        success: function (data) {
                            $('#OwnerID').append($('<option>', {
                                value: data.AccountList[0].id,
                                text: data.AccountList[0].text
                            }));
                            $("#OwnerID").val(data.AccountList[0].id).trigger('change');

                        }
                    });
                    $("#OwnerID").attr('disabled', true);
                    $("#AddRightsmodal").modal('show');
                }

            }, error: function () {
            }
        }).always(function () {
        });
    }

})

$("#divAddRights").on("change", "#Percentage", function () {
    if ($("#Percentage").prop('readonly')) {
        if ($.isNumeric($("#Area").val()) && $("#Percentage").val() > 0) {
            $("#TotalTerrainValue").val(($("#Area").val() * $("#TerrainValue").val()).toFixed(__decimalPoints))
        }
        else {
            $("#Area").val('');
            $("#TotalTerrainValue").val('');
        }
    }
    else {
        if ($.isNumeric($("#HdnArea").val()) && $("#Percentage").val() > 0) {
            $("#Area").val(($("#Percentage").val() * $("#HdnArea").val() / 100).toFixed(__decimalPoints))
            $("#TotalTerrainValue").val(($("#Area").val() * $("#TerrainValue").val()).toFixed(__decimalPoints))
        }
        else {
            $("#Area").val('');
            $("#TotalTerrainValue").val('');
        }
    }
    if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == true) {
        if ($("#Percentage").val() > 0 && TotalValue > 0) {
            $("#NewCurrentValue").val(($("#Percentage").val() * TotalValue / 100).toFixed(__decimalPoints))
            //GetTotalCurrentValue($(this));
        }
        else if ($("#Percentage").val() > 0) {
            var SumOfTotalValue = 0;
            $("#NewCurrentValue").val(ZeroVal.toFixed(__decimalPoints));
        }
        else {
            $("#NewCurrentValue").val('');
        }
    }
    else {
        if ($("#Percentage").val() > 0) {
            var SumOfTotalConstructionValue = 0.0;
            var SumOfTotalComplementaryValue = 0.0;
            var SumOfTotalValue = 0.0;
            if ($("#SplitTypeID").val() == 1 || ($("#SplitTypeID").val() == 2 && $("#OwnerID").val() == $("#OldAccountID").val())) {
                var Percenatge = $("#Percentage").val();
                if ($("#SplitTypeID").val() == 2 && $("#OwnerID").val() == $("#OldAccountID").val()) {
                    Percenatge = 100;
                    $.map(jQuery.extend(true, [], ConstructionDetailList), function (item, index) {
                        var BuildingArea = ((item.BuildingArea * Percenatge) / 100);
                        item.TotalValue = (item.BuildingArea * item.UnitValue);
                        if (item.MaterialTypeID == 1) {
                            SumOfTotalConstructionValue = SumOfTotalConstructionValue + (((item.BuildingArea * Percenatge) / 100) * item.UnitValue)
                        }
                        else {
                            SumOfTotalComplementaryValue = SumOfTotalComplementaryValue + (((item.BuildingArea * Percenatge) / 100) * item.UnitValue)
                        }
                    });
                    SumOfTotalValue = ($("#Area").val() * $("#TerrainValue").val()) + SumOfTotalConstructionValue + SumOfTotalComplementaryValue;
                    $("#NewCurrentValue").val(SumOfTotalValue.toFixed(__decimalPoints));
                }
                else {
                    GetTotalCurrentValue($(this))
                }

            }
            else {
                GetTotalCurrentValue($(this))
                //$("#NewCurrentValue").val(SumOfTotalValue.toFixed(__decimalPoints));
            }

        }
        else {
            $("#NewCurrentValue").val('');
        }
    }

});

$("#divAddRights").on("change", "#Area", function () {
    if (!$("#Area").prop('readonly')) {
        if ($("#Area").val() != "" && $("#HdnArea").val() != "" && parseFloat($("#Area").val()) > parseFloat($("#HdnArea").val())) {
            showAlert('error', ValidAreaMsg);
            $("#Area").val('')
        }
        $("#Percentage").val((($("#Area").val() / $("#HdnArea").val()) * 100).toFixed(__decimalPoints)).trigger('change');;
    }
});

function ClearRights() {
    $("#AddRightsmodal input, #AddRightsmodal select").val('')
}

function IsNotExistsRightsCode(RightsCode, mode) {
    var retval = true;
    //if ($.inArray($("#RigthNumber").val().toLowerCase(), RigthCodesArray) >= 0 && $("#OwnerID").val() != $('#OldAccountID').val()) {
    //    showAlert('error', CodeErrMsg);
    //    retval = false;
    //}
    if ($("#BtnAddtype").val() == 'newRight' && RightsCode == "") {
        if ($.inArray($("#RigthNumber").val().toLowerCase(), RigthCodesArray) >= 0) {
            showAlert('error', CodeErrMsg);
            retval = false;
        }
    }
    if (retval == true && PropertyRightList.length > 0) {
        if (mode == "new") {
            $.grep(PropertyRightList, function (item, index) {
                if (item.FincaID == RightsCode) {
                    showAlert('error', CodeErrMsg);
                    retval = false;
                }
            });
        } else {
            retval = true;
            $.grep(PropertyRightList, function (item, index) {
                //if ((item.RigthNumber != RightsCode && item.RigthNumber.toLowerCase() == $("#RigthNumber").val().toLowerCase()) || ($.inArray($("#RigthNumber").val().toLowerCase(), RigthCodesArray) >= 0 && $("#OwnerID").val() != $('#OldAccountID').val())) {
            /*if (retval == true && ((item.FincaID != RightsCode && item.RigthNumber.toLowerCase() == $("#RigthNumber").val().toLowerCase()) || ($.inArray($("#RigthNumber").val().toLowerCase(), RigthCodesArray) >= 0 && ($("#OwnerID").val() != $('#OldAccountID').val() || $("#RigthNumber").val().toLowerCase() != $("#AccountPropertyID option:selected").text().split('-').pop())))) {*/
                if (retval == true && ((item.FincaID != RightsCode && item.RigthNumber.toLowerCase() == $("#RigthNumber").val().toLowerCase()) || ($.inArray($("#RigthNumber").val().toLowerCase(), RigthCodesArray) >= 0 && $("#OwnerID").val() != $('#OldAccountID').val()))) {
                    showAlert('error', CodeErrMsg);
                    retval = false;
                }

            });
        }
    }
    return retval;
}

$("#divAddRights").on("click", "#btnAddRights", function () {
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    var OwenerWithRigthCount = 0;
    if (mode == "new") {
        $.grep(PropertyRightList, function (item, index) {
            if (item.OwnerID == $("#OwnerID").val()) {
                OwenerWithRigthCount = OwenerWithRigthCount + 1;
            }
        });
    }
    else {
        $.grep(PropertyRightList, function (item, index) {
            if (item.FincaID != RightsCode && item.OwnerID == $("#OwnerID").val()) {
                OwenerWithRigthCount = OwenerWithRigthCount + 1;
            }
        });
    }

    //if (OwenerWithRigthCount >= 1) {
    //    showAlert('error', OwnerAlreadyAdded);
    //}
    //else
        if ($("#frmRights").valid() && IsNotExistsRightsCode(RightsCode, mode)) {
        
        if (mode == "new") {
            var PropertyRightModel = {};
            PropertyRightModel.RigthNumber = $("#RigthNumber").val();
            PropertyRightModel.OwnerID = $("#OwnerID").val();
            PropertyRightModel.Percentage = $("#Percentage").val() == "" ? null : $("#Percentage").val();
            PropertyRightModel.Area = $("#Area").val();
            PropertyRightModel.HdnArea = $("#HdnArea").val();
            PropertyRightModel.TerrainValue = $("#TerrainValue").val();
            PropertyRightModel.TotalTerrainValue = $("#TotalTerrainValue").val();
            PropertyRightModel.HdnTotalTerrainValue = $("#HdnTotalTerrainValue").val();
            PropertyRightModel.CondoID = CondoID;
            PropertyRightModel.CondoTypeID = CondoTypeID;
            PropertyRightModel.Condo = Condo;
            PropertyRightModel.CondoType = CondoType;
            PropertyRightModel.PropertyNumber = PropertyNumber;
            PropertyRightModel.AccountPropertyConstructionDetailJson = jQuery.extend(true, [], ConstructionDetailList);

            var fincaID = "";
            fincaID = PropertyNumber;

            if (CondoTypeID > 0) {
                fincaID = fincaID + "-" + CondoType
            }
            if (CondoID > 0) {
                fincaID = fincaID + "-" + Condo
            }
            fincaID = fincaID + "-" + $("#RigthNumber").val();
            PropertyRightModel.FincaID = fincaID;

            if (IsNotExistsRightsCode(fincaID, 'new')) {
                var IsAvailableOwnerID = false;
                $.each($(".Owner:first option"), function () {
                    if ($(this).attr("value") == PropertyRightModel.OwnerID)
                        IsAvailableOwnerID = true;
                });

                if (IsAvailableOwnerID == false) {
                    $(".Owner").append('<option value="' + PropertyRightModel.OwnerID + '">' + $("#OwnerID option:selected").text().trim() + '</option>');
                }
                var strRigthEdit = '';
                //if (!$('#RigthNumber').prop('readonly')) {
                strRigthEdit = '<a class="btn btn-white btn-sm btnRightUpdate" onclick="ViewHistory(this)"><i class="fa fa-cog"></i></a>'
                //}

                if ($('#RigthNumber').prop('readonly')) {
                    PropertyRightModel.AccountPropertyID = $("#AccountPropertyID").val()
                }
                else {
                    PropertyRightModel.AccountPropertyID = 0;
                }

                $(".NewDivRightsTable .footable-row-detail").remove();
                $(".NewDivRightsTable .footable-visible").removeClass("footable-first-column");
                $(".NewDivRightsTable .footable-visible").removeClass("footable-last-column");
                $(".NewDivRightsTable .footable-visible").removeClass("footable-visible");
                $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
                $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
                var $TrRow = '<tr data-code="' + PropertyRightModel.FincaID + '">\
                        <td class="table-description-field">'+ PropertyRightModel.FincaID + '</td >\
                        <td>'+ $("#OwnerID option:selected").text().trim() + '</td>\
                        <td class="text-right">'+ parseFloat(PropertyRightModel.Percentage || 0).toFixed(__decimalPoints) + ' %</td>\
                        <td class="text-right">'+ parseFloat(PropertyRightModel.Area).toFixed(__decimalPoints) + '</td>\
                        <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(PropertyRightModel.TerrainValue || 0)) + '</td>\
                        <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(PropertyRightModel.TotalTerrainValue || 0)) + '</td>\
                        <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#NewCurrentValue").val() || 0)) + '</td>\
                        <td class="text-right">\
                            <a class="btn btn-white btn-sm btnAddContruction"><i class="fa fa-plus"></i></a>\
                            <a class="btn btn-white btn-sm btnEditRights"><i class="fa fa-pencil"></i></a>\
                            <a class="btn btn-white btn-sm btnDeleterights"><i class="fa fa-trash"></i></a>' + strRigthEdit + '\
                        </td >\
        <td class="ConstructionList"> </td>\
                    </tr > ';
                $(".RightsTable tbody:first").append($TrRow);

                if ($("#SplitTypeID").val() == 1 || ($("#SplitTypeID").val() == 2 && $("#OwnerID").val() == $("#OldAccountID").val())) {

                    $.map(PropertyRightModel.AccountPropertyConstructionDetailJson, function (item, index) {
                        ConstructionID = ConstructionID + 1;
                        OperationID = ConstructionID;
                        PropertyRightModel.AccountPropertyConstructionDetailJson[index]._ID = OperationID;
                        PropertyRightModel.AccountPropertyConstructionDetailJson[index].hdnBuildingArea = PropertyRightModel.AccountPropertyConstructionDetailJson[index].BuildingArea;

                        if ($("#SplitTypeID").val() == 1) {
                            //PropertyRightModel.AccountPropertyConstructionDetailJson[index].BuildingArea = ((PropertyRightModel.AccountPropertyConstructionDetailJson[index].BuildingArea * PropertyRightModel.Percentage) / 100);
                            PropertyRightModel.AccountPropertyConstructionDetailJson[index].BuildingArea = $(".NewConstructionArea", $('tr[data-id=' + item.ID + ']')).val() != "" ? $(".NewConstructionArea", $('tr[data-id=' + item.ID + ']')).val() : 0;
                            PropertyRightModel.AccountPropertyConstructionDetailJson[index].Percentage = $(".inputpercentage", $('tr[data-id=' + item.ID + ']')).val()

                        }
                        if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == false) {

                            PropertyRightModel.AccountPropertyConstructionDetailJson[index].TotalValue = (PropertyRightModel.AccountPropertyConstructionDetailJson[index].BuildingArea * PropertyRightModel.AccountPropertyConstructionDetailJson[index].UnitValue);
                        }
                        else {
                            PropertyRightModel.AccountPropertyConstructionDetailJson[index].UnitValue = 0;
                            PropertyRightModel.AccountPropertyConstructionDetailJson[index].TotalValue = 0;
                        }
                        if (($(".NewConstructionArea", $('tr[data-id=' + item.ID + ']')).val() > 0 && $("#SplitTypeID").val() == 1) || $("#SplitTypeID").val() == 2) {
                            var $Div = '<div class="row1 border-bottom">\
                            <table width ="100%">\
                                <tbody>\
                                    <tr data-code="'+ PropertyRightModel.FincaID + '">\
                                        <td class="col-lg-11">\
                                            <table class="table no-borders">\
                                                <tbody>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                                        <td class="text-left">'+ item.MaterialType + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionType + ' :</label></td>\
                                                        <td class="text-left"> '+ (item.ConstructionType == null ? '-' : item.ConstructionType) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ CurrentAge + ' :</label></td>\
                                                        <td class="text-left">'+ (item.TotalYears > 0 ? item.TotalYears : '-') + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                                        <td class="text-left">'+ (item.Status == null ? '-' : item.Status) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ UsefulLife + ' :</label></td>\
                                                        <td class="text-left">'+ (item.UsefulLife == "" || item.UsefulLife == null ? "-" : item.UsefulLife) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                                        <td class="text-left">'+ (item.Floor > 0 ? item.Floor : '-') + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                                        <td class="text-left">'+ (item.Chambers > 0 ? item.Chambers : '-') + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                                        <td class="text-left">'+ (item.InternalWalls == null ? '-' : item.InternalWalls) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                                        <td class="text-left">'+ (item.ExternalWalls == null ? '-' : item.ExternalWalls) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Structure == null ? '-' : item.Structure) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Roof == null ? '-' : item.Roof) + ' </td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Ceiling == null ? '-' : item.Ceiling) + ' </td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Floor == null ? '-' : item.Floor) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Bathroom > 0 ? item.Bathroom : "-") + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' :</label></td>\
                                                        <td class="text-left">'+ parseFloat(item.BuildingArea).toFixed(__decimalPoints) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.UnitValue)) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ BuildingTotalValue + '  :</label></td>\
                                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.TotalValue)) + '</td>\
                                                        <td class="control-label text-right" colspan="6"></td>\
                                                    </tr>\
                                                </tbody>\
                                            </table>\
                                        </td>\
                                        <td class="col-lg-1 text-right">\
                                            <a class="btn btn-white btn-sm btnEditConstruction"  data-id="' + OperationID + '"><i class="fa fa-pencil"></i></a>\
                                            <a class="btn btn-white btn-sm btnDeleteConstruction" data-id="' + OperationID + '"><i class="fa fa-trash"></i></a>\
                                        </td>\
                                    </tr>\
                                </tbody>\
                            </table >\
                        </div >';
                            $(".ConstructionList", $(".RightsTable tr[data-code='" + PropertyRightModel.FincaID + "']")).append($Div);
                        }
                    });
                }
                else {
                    PropertyRightModel.AccountPropertyConstructionDetailJson = [];
                }
                $('.NewDivRightsTable .footable').footable();
                PropertyRightList.push(PropertyRightModel);
            }
            else {
                return;
            }
        }
        else {
            var fincaID = "";
            fincaID = PropertyNumber;

            if (CondoTypeID > 0) {
                fincaID = fincaID + "-" + CondoType
            }
            if (CondoID > 0) {
                fincaID = fincaID + "-" + Condo
            }
            fincaID = fincaID + "-" + $("#RigthNumber").val();


            $.grep(PropertyRightList, function (item, index) {
                if (item.FincaID == RightsCode) {

                    var IsAvailableOwnerID = false;
                    $.each($(".Owner:first option"), function () {
                        if ($(this).attr("value") == $("#OwnerID").val())
                            IsAvailableOwnerID = true;
                    });

                    if (IsAvailableOwnerID == false && item.OwnerID != $("#OwnerID").val()) {

                        var PropertyOwnerID, OwenerWithRigthCount = 0;
                        PropertyOwnerID = item.OwnerID;

                        $.grep(PropertyRightList, function (item, index) {
                            if (item.OwnerID == PropertyOwnerID) {
                                OwenerWithRigthCount = OwenerWithRigthCount + 1;
                            }
                        });

                        if (OwenerWithRigthCount == 1) {
                            $(".Owner [value='" + item.OwnerID + "']").remove();
                        }

                        $(".Owner").append('<option value="' + $("#OwnerID").val() + '">' + $("#OwnerID option:selected").text().trim() + '</option>');
                    }

                    $(".NewDivRightsTable .footable-row-detail").remove();
                    $(".NewDivRightsTable .footable-visible").removeClass("footable-first-column");
                    $(".NewDivRightsTable .footable-visible").removeClass("footable-last-column");
                    $(".NewDivRightsTable .footable-visible").removeClass("footable-visible");
                    $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
                    $(".RightsTable thead td:last,.RightsTable tbody td:last").show();


                    //var fincaID = "";
                    //fincaID = PropertyRightList[index].PropertyNumber;

                    //if (CondoTypeID > 0) {
                    //    fincaID = fincaID + "-" + PropertyRightList[index].CondoType
                    //}
                    //if (CondoID > 0) {
                    //    fincaID = fincaID + "-" + PropertyRightList[index].Condo
                    //}
                    //fincaID = fincaID + "-" + $("#RigthNumber").val();
                    PropertyRightList[index].FincaID = fincaID;

                    PropertyRightList[index].RigthNumber = $("#RigthNumber").val();
                    PropertyRightList[index].OwnerID = $("#OwnerID").val();
                    PropertyRightList[index].Area = $("#Area").val();
                    PropertyRightList[index].Percentage = $("#Percentage").val() == "" ? null : $("#Percentage").val();
                    PropertyRightList[index].TerrainValue = $("#TerrainValue").val();
                    PropertyRightList[index].TotalTerrainValue = $("#TotalTerrainValue").val();

                    if ($("#SplitTypeID").val() == 1 || ($("#SplitTypeID").val() == 2 && $("#OwnerID").val() == $("#OldAccountID").val())) {
                        $.map(PropertyRightList[index].AccountPropertyConstructionDetailJson, function (item, itemindex) {
                            if ($("#SplitTypeID").val() == 1) {
                                //PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].BuildingArea = ((PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].hdnBuildingArea * PropertyRightList[index].Percentage) / 100);
                                PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].BuildingArea = $(".NewConstructionArea", $('tr[data-id=' + item.ID + ']')).val() != "" ? $(".NewConstructionArea", $('tr[data-id=' + item.ID + ']')).val() : 0;
                                PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].Percentage = $(".inputpercentage", $('tr[data-id=' + item.ID + ']')).val()
                            }
                            if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == false) {

                                PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].TotalValue = (PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].BuildingArea * PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].UnitValue);
                            }
                            else {
                                PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].UnitValue = 0;
                                PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex].TotalValue = 0;
                            }
                            if (($(".NewConstructionArea", $('tr[data-id=' + item.ID + ']')).val() > 0 && $("#SplitTypeID").val() == 1) || $("#SplitTypeID").val() == 2) {
                                var $Div = '<div class="row1 border-bottom">\
                            <table width ="100%">\
                                <tbody>\
                                    <tr data-code="'+ PropertyRightList[index].FincaID + '">\
                                        <td class="col-lg-11">\
                                            <table class="table no-borders">\
                                                <tbody>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                                        <td class="text-left">'+ item.MaterialType + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionType + ' :</label></td>\
                                                        <td class="text-left"> '+ (item.ConstructionType == null ? '-' : item.ConstructionType) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ CurrentAge + ' :</label></td>\
                                                        <td class="text-left">'+ (item.TotalYears > 0 ? item.TotalYears : '-') + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                                        <td class="text-left">'+ (item.Status == null ? '-' : item.Status) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ UsefulLife + ' :</label></td>\
                                                        <td class="text-left">'+ (item.UsefulLife == "" || item.UsefulLife == null ? "-" : item.UsefulLife) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                                        <td class="text-left">'+ (item.Floor > 0 ? item.Floor : '-') + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                                        <td class="text-left">'+ (item.Chambers > 0 ? item.Chambers : '-') + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                                        <td class="text-left">'+ (item.InternalWalls == null ? '-' : item.InternalWalls) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                                        <td class="text-left">'+ (item.ExternalWalls == null ? '-' : item.ExternalWalls) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Structure == null ? '-' : item.Structure) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Roof == null ? '-' : item.Roof) + ' </td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Ceiling == null ? '-' : item.Ceiling) + ' </td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Floor == null ? '-' : item.Floor) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                                        <td class="text-left">'+ (item.Bathroom > 0 ? item.Bathroom : "-") + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' :</label></td>\
                                                        <td class="text-left">'+ parseFloat(item.BuildingArea).toFixed(__decimalPoints) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.UnitValue)) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ BuildingTotalValue + '  :</label></td>\
                                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.TotalValue)) + '</td>\
                                                        <td class="control-label text-right" colspan="6"></td>\
                                                    </tr>\
                                                </tbody>\
                                            </table>\
                                        </td>\
                                        <td class="col-lg-1 text-right">\
                                            <a class="btn btn-white btn-sm btnEditConstruction"  data-id="' + PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex]._ID + '"><i class="fa fa-pencil"></i></a>\
                                            <a class="btn btn-white btn-sm btnDeleteConstruction" data-id="' + PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex]._ID + '"><i class="fa fa-trash"></i></a>\
                                        </td>\
                                    </tr>\
                                </tbody>\
                            </table >\
                        </div >';

                                if ($(".RightsTable a.btnEditConstruction[data-id='" + PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex]._ID + "']:last").closest("div").length > 0) {
                                    $(".RightsTable a.btnEditConstruction[data-id='" + PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex]._ID + "']:last").closest("div").replaceWith($Div);
                                }
                                else {
                                    $(".ConstructionList", $(".RightsTable tr[data-code='" + PropertyRightList[index].FincaID + "']")).append($Div);
                                }
                            }
                            else {
                                $(".RightsTable a.btnEditConstruction[data-id='" + PropertyRightList[index].AccountPropertyConstructionDetailJson[itemindex]._ID + "']:last").closest("div").remove();
                            }
                        });
                    }
                    else {
                        PropertyRightList[index].AccountPropertyConstructionDetailJson = [];
                    }
                }
            });

            var $Span = $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(0) span")[0].outerHTML;
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(0)").html('').html($Span + fincaID)
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(1)").html($("#OwnerID option:selected").text().trim())
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(2)").html(parseFloat($("#Percentage").val() || 0).toFixed(__decimalPoints) + ' %')
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(3)").html(parseFloat($("#Area").val()).toFixed(__decimalPoints))
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(4)").html(_currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#TerrainValue").val() || 0)))
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(5)").html(_currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#TotalTerrainValue").val() || 0)))
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(6)").html(_currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#NewCurrentValue").val() || 0)))
            $(".RightsTable tr[data-code='" + RightsCode + "']").attr("data-code", fincaID);
            $('.NewDivRightsTable .footable').footable();
        }
        $("#AddRightsmodal").modal('hide');
    }
});

$(".RightsTable").on("click", ".btnAddContruction", function () {
    $this = $(this)
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountProperty/NewGeneralDescriptionConstruction",
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divGeneralDescription").html(data);
                $.validator.unobtrusive.parse('#frmGeneralDescription');
                $("#GeneralDescriptionConstructionmodal").attr('data-code', $this.closest('tr').data('code')).modal('show');
                $("#GeneralDescriptionConstructionmodal").attr('data-mode', 'new');
                $("#divGeneralDescription .select2dropdown").select2({ width: '100%' });

                InitializeConstructionYear();

                $('#frmGeneralDescription').validate()
                $('#MaterialTypeID').select2('open').select2('close');
            }

        }, error: function () {
        }
    }).always(function () {
    })
});

$("#divGeneralDescription").on("click", "#btnAddConstruction", function () {
    if ($("#frmGeneralDescription").valid() && IsBuildingAreaGreaterThanZero() && IsUnitValueGreaterThanZero()
        && IsTotalValueGreaterThanZero() && CheckTotalValueLength()) {
        var PropertyConstructionModel = {};
        PropertyConstructionModel.ConstructionTypeID = ($("#ConstructionTypeID").val() == "" ? null : $("#ConstructionTypeID").val());
        PropertyConstructionModel.MaterialTypeID = $("#MaterialTypeID").val();
        PropertyConstructionModel.MaterialType = $("#MaterialTypeID option:selected").text();
        PropertyConstructionModel.ConstructionYear = 0;
        PropertyConstructionModel.TotalYears = $("#TotalYears").val();
        PropertyConstructionModel.StatusID = ($("#StatusID").val() == "" ? null : $("#StatusID").val());
        PropertyConstructionModel.Status = ($("#StatusID").val() > 0 ? $("#StatusID option:selected").text() : '-');
        PropertyConstructionModel.Floors = $("#Floors").val();
        PropertyConstructionModel.Chambers = $("#Chambers").val();
        PropertyConstructionModel.InternalWallsID = ($("#InternalWallsID").val() == "" ? null : $("#InternalWallsID").val());
        PropertyConstructionModel.InternalWalls = ($("#InternalWallsID").val() > 0 ? $("#InternalWallsID option:selected").text() : '-');
        PropertyConstructionModel.ExternalWallsID = ($("#ExternalWallsID").val() == "" ? null : $("#ExternalWallsID").val());
        PropertyConstructionModel.ExternalWalls = ($("#ExternalWallsID").val() > 0 ? $("#ExternalWallsID option:selected").text() : '-');
        PropertyConstructionModel.StructureID = ($("#StructureID").val() == "" ? null : $("#StructureID").val());
        PropertyConstructionModel.Structure = ($("#StructureID").val() > 0 ? $("#StructureID option:selected").text() : '-');
        PropertyConstructionModel.RoofID = ($("#RoofID").val() == "" ? null : $("#RoofID").val());
        PropertyConstructionModel.Roof = ($("#RoofID").val() > 0 ? $("#RoofID option:selected").text() : '-');
        PropertyConstructionModel.CeilingID = ($("#CeilingID").val() == "" ? null : $("#CeilingID").val());
        PropertyConstructionModel.Ceiling = ($("#CeilingID").val() > 0 ? $("#CeilingID option:selected").text() : '-');
        PropertyConstructionModel.FloorID = ($("#FloorID").val() == "" ? null : $("#FloorID").val());
        PropertyConstructionModel.Floor = ($("#FloorID").val() > 0 ? $("#FloorID option:selected").text() : '-');
        PropertyConstructionModel.Bathroom = ($("#Bathroom").val() == "" ? null : GlobalConvertToDecimal($("#Bathroom").val()));
        PropertyConstructionModel.UsefulLife = ($("#UsefulLife").val() == "" ? null : $("#UsefulLife").val());
        PropertyConstructionModel.BuildingArea = GlobalConvertToDecimal($("#BuildingArea").val());
        PropertyConstructionModel.UnitValue = GlobalConvertToDecimal($("#UnitValue").val());
        PropertyConstructionModel.TotalValue = GlobalConvertToDecimal($("#TotalValue").val());
        PropertyConstructionModel.IsActive = true;
        PropertyConstructionModel.IsDeleted = false;
        var RightsCode = $("#GeneralDescriptionConstructionmodal").attr('data-code');
        PropertyConstructionModel.ID = 0;
        var mode = $("#GeneralDescriptionConstructionmodal").attr('data-mode');
        var IndexID = $("#IndexID").val()
        var OperationID = 0;
        if (mode == "new") {
            ConstructionID = ConstructionID + 1;
            PropertyConstructionModel._ID = ConstructionID;
            OperationID = ConstructionID
        }
        else {
            OperationID = $("#IndexID").val()
        }
        if (mode == "new") {
            $.grep(PropertyRightList, function (item, index) {
                if (item.FincaID == RightsCode) {
                    PropertyRightList[index].AccountPropertyConstructionDetailJson.push(PropertyConstructionModel)
                }
            });
        }

        $(".NewDivRightsTable .footable-row-detail").remove();
        $(".NewDivRightsTable .footable-visible").removeClass("footable-first-column");
        $(".NewDivRightsTable .footable-visible").removeClass("footable-last-column");
        $(".NewDivRightsTable .footable-visible").removeClass("footable-visible");
        $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
        $(".RightsTable thead td:last,.RightsTable tbody td:last").show();

        var $Div = '<div class="row1 border-bottom">\
            <table width ="100%">\
                <tbody>\
                    <tr data-code="'+ RightsCode + '">\
                        <td class="col-lg-11">\
                            <table class="table no-borders">\
                                <tbody>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                        <td class="text-left">'+ $("#MaterialTypeID option:selected").text() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionType + ' :</label></td>\
                                        <td class="text-left">'+ ($("#ConstructionTypeID").val() == "" ? "-" : $("#ConstructionTypeID option:selected").text()) + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ CurrentAge + ' :</label></td>\
                                        <td class="text-left">'+ ($("#TotalYears").val() > 0 ? $("#TotalYears").val() : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                        <td class="text-left">'+ ($("#StatusID").val() > 0 ? $("#StatusID option:selected").text() : '-') + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ UsefulLife + ' :</label></td>\
                                        <td class="text-left">'+ ($("#UsefulLife").val() == "" ? "-" : $("#UsefulLife").val()) + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                        <td class="text-left">'+ ($("#Floors").val() > 0 ? $("#Floors").val() : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                        <td class="text-left">'+ ($("#Chambers").val() > 0 ? $("#Chambers").val() : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                        <td class="text-left">'+ ($("#InternalWallsID").val() > 0 ? $("#InternalWallsID option:selected").text() : '-') + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                        <td class="text-left">'+ ($("#ExternalWallsID").val() > 0 ? $("#ExternalWallsID option:selected").text() : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                        <td class="text-left">'+ ($("#StructureID").val() > 0 ? $("#StructureID option:selected").text() : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                        <td class="text-left">'+ ($("#RoofID").val() > 0 ? $("#RoofID option:selected").text() : '-') + ' </td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                        <td class="text-left">'+ ($("#CeilingID").val() > 0 ? $("#CeilingID option:selected").text() : '-') + ' </td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                        <td class="text-left">'+ ($("#FloorID").val() > 0 ? $("#FloorID option:selected").text() : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                        <td class="text-left">'+ (PropertyConstructionModel.Bathroom > 0 ? PropertyConstructionModel.Bathroom : '-') + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' (<em>m</em><sup>2</sup>) :</label></td>\
                                        <td class="text-left">'+ parseFloat($("#BuildingArea").val()).toFixed(__decimalPoints) + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#UnitValue").val())) + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ BuildingTotalValue + '  :</label></td>\
                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#TotalValue").val())) + '</td>\
                                        <td class="control-label text-right" colspan="6"></td>\
                                    </tr>\
                                </tbody>\
                            </table>\
                        </td>\
                        <td class="col-lg-1 text-right">\
                            <a class="btn btn-white btn-sm btnEditConstruction"  data-id="' + OperationID + '"><i class="fa fa-pencil"></i></a>\
                            <a class="btn btn-white btn-sm btnDeleteConstruction" data-id="' + OperationID + '"><i class="fa fa-trash"></i></a>\
                        </td>\
                    </tr>\
                </tbody>\
            </table >\
          </div >';

        if (mode == "edit") {
            $.grep(PropertyRightList, function (item, index) {
                $.grep(item.AccountPropertyConstructionDetailJson, function (data, dataIndex) {
                    if (data._ID == IndexID) {
                        data.ConstructionTypeID = PropertyConstructionModel.ConstructionTypeID;
                        data.MaterialTypeID = PropertyConstructionModel.MaterialTypeID;
                        data.ConstructionYear = PropertyConstructionModel.ConstructionYear;
                        data.TotalYears = PropertyConstructionModel.TotalYears;
                        data.StatusID = PropertyConstructionModel.StatusID;
                        data.Floors = PropertyConstructionModel.Floors;
                        data.Chambers = PropertyConstructionModel.Chambers;
                        data.InternalWallsID = PropertyConstructionModel.InternalWallsID;
                        data.ExternalWallsID = PropertyConstructionModel.ExternalWallsID;
                        data.StructureID = PropertyConstructionModel.StructureID;
                        data.RoofID = PropertyConstructionModel.RoofID;
                        data.CeilingID = PropertyConstructionModel.CeilingID;
                        data.FloorID = PropertyConstructionModel.FloorID;
                        data.Bathroom = PropertyConstructionModel.Bathroom;
                        data.UsefulLife = PropertyConstructionModel.UsefulLife;
                        data.BuildingArea = PropertyConstructionModel.BuildingArea;
                        data.UnitValue = PropertyConstructionModel.UnitValue;
                        data.TotalValue = PropertyConstructionModel.TotalValue;
                    }
                })
            });

            var RemoveID = $("#IndexID").val()
            $(".RightsTable a.btnEditConstruction[data-id='" + RemoveID + "']:last").closest("div").replaceWith($Div);
        } else {
            $(".ConstructionList", $(".RightsTable tr[data-code='" + RightsCode + "']")).append($Div);
        }
        $('.NewDivRightsTable .footable').footable();
        $("#GeneralDescriptionConstructionmodal").modal('hide');
    }
});

$(".RightsTable").on("click", ".btnDeleteConstruction", function () {
    var Id = $(this).attr('data-id');

    $.grep(PropertyRightList, function (item, index) {
        item.AccountPropertyConstructionDetailJson = $.grep(item.AccountPropertyConstructionDetailJson, function (data, dataIndex) {
            return data._ID != Id;
        });
    });
    $(this).closest('div').remove();
    $(".RightsTable a[data-id='" + Id + "']").closest("div").remove();
    $(".NewDivRightsTable .footable-row-detail").remove();
    $(".NewDivRightsTable .footable-visible").removeClass("footable-first-column");
    $(".NewDivRightsTable .footable-visible").removeClass("footable-last-column");
    $(".NewDivRightsTable .footable-visible").removeClass("footable-visible");
    $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
    $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
    $('.NewDivRightsTable .footable').footable();
});

$(".RightsTable").on("click", ".btnDeleterights", function () {
    var code = $(this).closest('tr').attr('data-code');

    var PropertyOwnerID, OwenerWithRigthCount = 0;
    $.grep(PropertyRightList, function (item, index) {
        if (item.FincaID == code) {
            PropertyOwnerID = item.OwnerID;
        }
    });

    $.grep(PropertyRightList, function (item, index) {
        if (item.OwnerID == PropertyOwnerID) {
            OwenerWithRigthCount = OwenerWithRigthCount + 1;
        }
    });

    if (OwenerWithRigthCount == 1) {
        $(".Owner [value='" + PropertyOwnerID + "']").remove();
    }

    PropertyRightList = $.grep(PropertyRightList, function (item, index) {
        return item.FincaID != code;
    });

    $(this).closest('tr').remove();
    $(".NewDivRightsTable .footable-row-detail").remove();
    $(".NewDivRightsTable .footable-visible").removeClass("footable-first-column");
    $(".NewDivRightsTable .footable-visible").removeClass("footable-last-column");
    $(".NewDivRightsTable .footable-visible").removeClass("footable-visible");
    $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
    $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
    $('.NewDivRightsTable .footable').footable();
});

$("#divGeneralDescription").on("change", "#BuildingArea,#UnitValue", function () {
    if ($.isNumeric($("#BuildingArea").val()) && $.isNumeric($("#UnitValue").val())) {
        $("#TotalValue").val(($("#BuildingArea").val() * $("#UnitValue").val()).toFixed(__decimalPoints))
    }
    else {
        $("#TotalValue").val('');
    }
});
$("#divGeneralDescription").on("change", "#ConstructionYear", function () {
    if ($("#ConstructionYear").val() != "" && $("#ConstructionYear").val().length == 4) {
        var CurrentYear = (new Date()).getFullYear()
        if (CurrentYear >= $("#ConstructionYear").val())
            $("#TotalYears").val(CurrentYear - $("#ConstructionYear").val())
        else
            $("#TotalYears").val('');
    }
    else {
        $("#TotalValue").val('');
    }
});



$(".RightsTable").on("click", ".btnEditConstruction", function () {
    var Id = $(this).attr('data-id');
    var $this = $(this);
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountProperty/NewGeneralDescriptionConstruction",
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divGeneralDescription").html(data);

                $('#frmGeneralDescription #addresstitle').html(EditConstructionDescription);
                $("#btnAddConstruction").html(Update);

                $.validator.unobtrusive.parse('#frmGeneralDescription');
                $("#GeneralDescriptionConstructionmodal").attr('data-code', $this.closest('tr').data('code')).modal('show');
                $("#GeneralDescriptionConstructionmodal").attr('data-mode', 'edit');
                $("#divGeneralDescription .select2dropdown").select2({ width: '100%' });

                InitializeConstructionYear();

                $('#frmGeneralDescription').validate();
                $('#MaterialTypeID').select2('open').select2('close');
                var RigthCode = $this.closest('tr').data('code');
                $.grep(PropertyRightList, function (item, index) {
                    if (item.FincaID == RigthCode) {
                        $.grep(item.AccountPropertyConstructionDetailJson, function (data, dataIndex) {
                            if (data._ID == Id) {

                                $("#ConstructionTypeID").val(data.ConstructionTypeID).trigger('change');
                                $("#MaterialTypeID").val(data.MaterialTypeID).trigger('change');
                                $("#TotalYears").val(data.TotalYears);
                                $("#StatusID").val(data.StatusID).trigger('change');
                                $("#Floors").val(data.Floors).trigger('change');
                                $("#Chambers").val(data.Chambers).trigger('change');
                                $("#InternalWallsID").val(data.InternalWallsID).trigger('change');
                                $("#ExternalWallsID").val(data.ExternalWallsID).trigger('change');
                                $("#StructureID").val(data.StructureID).trigger('change');
                                $("#RoofID").val(data.RoofID).trigger('change');
                                $("#CeilingID").val(data.CeilingID).trigger('change');
                                $("#FloorID").val(data.FloorID).trigger('change');
                                $("#Bathroom").val(data.Bathroom == "" || data.Bathroom == null ? "" : GlobalFormat(data.Bathroom));
                                $("#UsefulLife").val(data.UsefulLife == "-" || data.UsefulLife == null ? "" : data.UsefulLife);
                                $("#BuildingArea").val(data.BuildingArea).trigger('change');
                                $("#BuildingArea").attr('readonly', 'readonly');
                                $("#UnitValue").val(GlobalFormat(data.UnitValue));
                                $("#TotalValue").val(GlobalFormat(data.TotalValue));
                                $("#IsDistributed").prop("checked", data.IsDistributed);
                                $("#IsDistributed").attr("disabled", "disabled");
                                $("#frmGeneralDescription #ID").val(data.ID);
                                $("#IndexID").val(data._ID);
                            }
                        });
                    }
                });
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

$(".RightsTable").on("click", ".btnEditRights", function () {
    var code = $(this).closest('tr').attr('data-code');
    var $this = $(this);

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountProperty/AddRights",
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {

                $("#divAddRights").html(data);
                $.validator.unobtrusive.parse('#frmRights');
                $("#AddRightsmodal").attr('data-mode', 'edit');
                $("#OwnerID").select2({ width: '100%' });
                $('#frmRights').validate();

                $.grep(PropertyRightList, function (item, index) {
                    if (item.FincaID == code) {
                        $("#RigthNumber").val(item.RigthNumber);
                        $("#OwnerID").val(item.OwnerID).trigger('change');
                        $("#HdnArea").val(item.HdnArea);
                        $("#Area").val(item.Area)
                        $("#TotalValue").val(TotalValue.toFixed(__decimalPoints));
                        $("#CurrentValueFromPropertyTax").val(CurrentValueFromPropertyTax.toFixed(__decimalPoints));
                        $("#Percentage").val(item.Percentage)
                        $("#TerrainValue").val(item.TerrainValue);
                        $("#TotalTerrainValue").val(item.TotalTerrainValue);
                        //if (item.Percentage > 0 && TotalValue > 0) {
                        //    $("#NewCurrentValue").val(($("#Percentage").val() * TotalValue / 100).toFixed(__decimalPoints))
                        //}
                        //else if ($("#Percentage").val() > 0) {
                        //    var ZeroVal = 0;
                        //    $("#NewCurrentValue").val(ZeroVal.toFixed(__decimalPoints));
                        //}
                        //else {
                        //    $("#NewCurrentValue").val('');
                        //}

                        if ($("#SplitTypeID").val() == 1) {
                            $("#divConstructionArea").html('');
                            $("#divComplementaryArea").html('');
                            $.grep(item.AccountPropertyConstructionDetailJson, function (Itemdata, dataIndex) {

                                if (Itemdata.MaterialType == "Complementary") {
                                    var DivComplementaryTr = '<tr data-id="' + Itemdata.ID + '">\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right Currencyinputdecimal ComplementaryArea" readonly="true" value="'+ Itemdata.hdnBuildingArea + '"/>\
                                                            <input type="hidden" class="form-control text-right UnitValue" value="'+ Itemdata.UnitValue + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required ComplementaryValue inputpercentage" value="'+ Itemdata.Percentage + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required Currencyinputdecimal NewConstructionArea" value="'+ Itemdata.BuildingArea + '"/>\
                                                    </td>\
                                                 </tr>';
                                    $("#divComplementaryArea").append(DivComplementaryTr);
                                }
                                else {
                                    var DivConstructionTr = '<tr data-id="' + Itemdata.ID + '">\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right Currencyinputdecimal ConstructionArea" readonly="true" value="'+ Itemdata.hdnBuildingArea + '"/>\
                                                            <input type="hidden" class="form-control text-right UnitValue" value="'+ Itemdata.UnitValue + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required ContructionValue inputpercentage" value="'+ Itemdata.Percentage + '"/>\
                                                    </td>\
                                                    <td class="col-lg-4 text-right">\
                                                            <input type="text" class="form-control text-right required Currencyinputdecimal NewConstructionArea" value="'+ Itemdata.BuildingArea + '"/>\
                                                    </td>\
                                                 </tr>';
                                    $("#divConstructionArea").append(DivConstructionTr);
                                }
                            });

                            if ($("#divConstructionArea").html() == "") {
                                var NoData = '<tr>\
                                        <td class="text-center" colspan = "3">'+ nodatamsg + '\
                                        </td>\
                                     </tr>';
                                $("#divConstructionArea").append(NoData)
                            }
                            if ($("#divComplementaryArea").html() == "") {
                                var NoData = '<tr>\
                                        <td class="text-center" colspan = "3">'+ nodatamsg + '\
                                        </td>\
                                     </tr>';
                                $("#divComplementaryArea").append(NoData)
                            }
                        }

                        if ($("#SelectionTypeID").val() == "1") {
                            $(".NewConstructionArea,#Area").attr("readonly", "readonly");
                            $(".inputpercentage").removeAttr("readonly")
                        }
                        else {
                            $(".inputpercentage").attr("readonly", "readonly")
                            $(".NewConstructionArea,#Area").removeAttr("readonly")
                        }

                        if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == true) {
                            if ($("#Percentage").val() > 0 && TotalValue > 0) {
                                $("#NewCurrentValue").val(($("#Percentage").val() * TotalValue / 100).toFixed(__decimalPoints))
                                //GetTotalCurrentValue($(this))
                            }
                            else if ($("#Percentage").val() > 0) {
                                var SumOfTotalValue = 0;
                                $("#NewCurrentValue").val(ZeroVal.toFixed(__decimalPoints));
                            }
                            else {
                                $("#NewCurrentValue").val('');
                            }
                        }
                        else {
                            if ($("#Percentage").val() > 0) {
                                var SumOfTotalConstructionValue = 0.0;
                                var SumOfTotalComplementaryValue = 0.0;
                                var SumOfTotalValue = 0.0;
                                if ($("#SplitTypeID").val() == 1 || ($("#SplitTypeID").val() == 2 && $("#OwnerID").val() == $("#OldAccountID").val())) {
                                    var Percenatge = $("#Percentage").val();
                                    if ($("#SplitTypeID").val() == 2 && $("#OwnerID").val() == $("#OldAccountID").val()) {
                                        Percenatge = 100;
                                        $.map(jQuery.extend(true, [], ConstructionDetailList), function (item, index) {
                                            var BuildingArea = ((item.BuildingArea * Percenatge) / 100);
                                            item.TotalValue = (item.BuildingArea * item.UnitValue);
                                            if (item.MaterialTypeID == 1) {
                                                SumOfTotalConstructionValue = SumOfTotalConstructionValue + (((item.BuildingArea * Percenatge) / 100) * item.UnitValue)
                                            }
                                            else {
                                                SumOfTotalComplementaryValue = SumOfTotalComplementaryValue + (((item.BuildingArea * Percenatge) / 100) * item.UnitValue)
                                            }
                                        });
                                        SumOfTotalValue = ($("#Area").val() * $("#TerrainValue").val()) + SumOfTotalConstructionValue + SumOfTotalComplementaryValue;
                                        $("#NewCurrentValue").val(SumOfTotalValue.toFixed(__decimalPoints));
                                    }
                                    else {
                                        GetTotalCurrentValue($(this))
                                    }
                                }
                                else {
                                    GetTotalCurrentValue($(this))
                                    //$("#NewCurrentValue").val(SumOfTotalValue.toFixed(__decimalPoints));
                                }
                            }
                            else {
                                $("#NewCurrentValue").val('');
                            }
                        }
                        $("#HdnTotalTerrainValue").val(item.HdnTotalTerrainValue);



                        GetAccountForSelect('OwnerID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: ROOTPath + "/Account/GetAccountForSearch",
                            data: {
                                'accountID': item.OwnerID,
                                'isActive': true,
                                pageIndex: 1,
                                pageSize: 1
                            },
                            success: function (data) {
                                $('#OwnerID').append($('<option>', {
                                    value: data.AccountList[0].id,
                                    text: data.AccountList[0].text
                                }));
                                $("#OwnerID").val(data.AccountList[0].id).trigger('change');

                            }
                        });
                    }
                });
                $('#RigthNumber').focus();
                $("#AddRightsmodal #btnAddRights").html(Update);
                $("#AddRightsmodal #addresstitle").html(EditRight);

                $("#AddRightsmodal").attr('data-code', code).modal('show');

            }
        }, error: function () {
        }
    }).always(function () {
    });
});

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

function IsBuildingAreaGreaterThanZero() {
    var retval = true;
    if ($("#BuildingArea").val() == 0) {
        showAlert('error', BuildingAreaRequiredMsg);
        retval = false;
    }
    return retval;
}

function IsUnitValueGreaterThanZero() {
    var retval = true;
    return retval;
}

function IsTotalValueGreaterThanZero() {
    var retval = true;
    return retval;
}

function CheckTotalValueLength() {
    var retval = true;

    var totalValue = $("#TotalValue").val();
    if (totalValue.length > 16) {
        showAlert('error', TotalValueMaxLengthValidationMsg);
        retval = false;
    }
    return retval;
}


function InitializeConstructionYear() {
    $('#ConstructionYear').datepicker({
        minViewMode: 2,
        format: 'yyyy',
        autoclose: true,
        language: __culture,
        endDate: new Date()
    });
}

$(document).on("change", ".PropertyTaxOwner", function () {
    $(".PropertyTaxOwner").val($(this).val());
});

$(".divAccountProperty").on("change", "#MovementTypeID", function () {
    if (PropertyRightList.length > 0) {
        $(".RightsTable tbody").html('');
        PropertyRightList = [];
    }

});

$(".divSplitType").on("change", "#SplitTypeID", function () {
    if (PropertyRightList.length > 0) {
        $(".RightsTable tbody").html('');
        PropertyRightList = [];
    }

});
function ViewHistory(e) {
    // var id = $("#AccountPropertyID").val();
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
        var fincaID = "";
        fincaID = $("#txtPropertyNumber").val();
        if ($("#ddlCondoType").val() > 0) {
            fincaID = fincaID + "-" + $("#ddlCondoType option:selected").text()
        }
        if ($("#ddlCondo").val() > 0) {
            fincaID = fincaID + "-" + $("#ddlCondo option:selected").text()
        }
        fincaID = fincaID + "-" + $("#txtRigthNumber").val()
        if (IsNotExistsRightsCode(fincaID, 'new')) {
            $.grep(PropertyRightList, function (item, index) {
                if (item.FincaID == currentfincaID) {
                    PropertyRightList[index].FincaID = fincaID;
                    PropertyRightList[index].PropertyNumber = $("#txtPropertyNumber").val();
                    PropertyRightList[index].RigthNumber = $("#txtRigthNumber").val();
                    PropertyRightList[index].CondoID = $("#ddlCondo").val();
                    PropertyRightList[index].CondoTypeID = $("#ddlCondoType").val();
                    PropertyRightList[index].Condo = $("#ddlCondo option:selected").text()
                    PropertyRightList[index].CondoType = $("#ddlCondoType option:selected").text()

                    var $Span = $(".RightsTable tr[data-code='" + currentfincaID + "'] td:eq(0) span")[0].outerHTML;
                    $(".RightsTable tr[data-code='" + currentfincaID + "'] td:eq(0)").html('').html($Span + PropertyRightList[index].FincaID)
                    $(".RightsTable tr[data-code='" + currentfincaID + "']").attr("data-code", PropertyRightList[index].FincaID);
                }
            });
            $("#AccountPropertyRightHistory").modal('hide');
        }
    }
});

$("#divAddRights").on("change", ".ComplementaryValue", function () {
    if ($(this).prop('readonly')) {
        if (!$.isNumeric($(".NewConstructionArea", $(this).closest('tr')).val()) && $(this).val() > 0) {
            $(".NewConstructionArea", $(this).closest('tr')).val('')
        }
    }
    else {
        if ($.isNumeric($(".ComplementaryArea", $(this).closest('tr')).val()) && $(this).val() > 0) {
            $(".NewConstructionArea", $(this).closest('tr')).val(($(this).val() * $(".ComplementaryArea", $(this).closest('tr')).val() / 100).toFixed(__decimalPoints))
        }
        else {
            $(".NewConstructionArea", $(this).closest('tr')).val('');
        }
    }
    GetTotalCurrentValue($(this))
});

$("#divAddRights").on("change", ".ContructionValue", function () {
    if ($(this).prop('readonly')) {
        if (!$.isNumeric($(".NewConstructionArea", $(this).closest('tr')).val()) && $(this).val() > 0) {
            $(".NewConstructionArea", $(this).closest('tr')).val('')
        }
    }
    else {
        if ($.isNumeric($(".ConstructionArea", $(this).closest('tr')).val()) && $(this).val() > 0) {
            $(".NewConstructionArea", $(this).closest('tr')).val(($(this).val() * $(".ConstructionArea", $(this).closest('tr')).val() / 100).toFixed(__decimalPoints))
        }
        else {
            $(".NewConstructionArea", $(this).closest('tr')).val('');
        }
    }
    GetTotalCurrentValue($(this))
});

$("#divAddRights").on("change", ".NewConstructionArea", function () {
    if (!$(this).prop('readonly')) {
        if ($(this).val() != "" && $("input[type='text']:first", $(this).closest('tr')).val() != "" && parseFloat($(this).val()) > parseFloat($("input[type='text']:first", $(this).closest('tr')).val())) {
            showAlert('error', ValidConstructionAreaMsg);
            $(this).val('');
        }
        $(".inputpercentage", $(this).closest('tr')).val((($(this).val() / $("input[type='text']:first", $(this).closest('tr')).val()) * 100).toFixed(__decimalPoints)).trigger('change');
    }
    GetTotalCurrentValue($(this))
});


function GetTotalCurrentValue($this) {
    if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == true) {
        if ($("#Percentage").val() > 0 && TotalValue > 0) {
            $("#NewCurrentValue").val((($("#Percentage").val() * TotalValue / 100)).toFixed(__decimalPoints))
        }
        else if ($("#Percentage").val() > 0) {
            var SumOfTotalValue = 0;
            $("#NewCurrentValue").val(ZeroVal.toFixed(__decimalPoints));
        }
        else {
            $("#NewCurrentValue").val('');
        }
    }
    else {
        if ($("#Percentage").val() > 0) {
            var SumOfTotalConstructionValue = 0.0;

            var SumOfTotalValue = 0.0;

            $("#divConstructionArea tr").each(function (index, value) {

                if ($(".NewConstructionArea", $(this).closest('tr')).val() > 0) {
                    SumOfTotalConstructionValue = SumOfTotalConstructionValue + ($(".NewConstructionArea", $(this).closest('tr')).val() * $(".UnitValue", $(this).closest('tr')).val())
                }

            });
            $("#divComplementaryArea tr").each(function (index, value) {
                if ($(".NewConstructionArea", $(this).closest('tr')).val() > 0) {
                    SumOfTotalConstructionValue = SumOfTotalConstructionValue + ($(".NewConstructionArea", $(this).closest('tr')).val() * $(".UnitValue", $(this).closest('tr')).val())
                }

            });

            SumOfTotalValue = ($("#Area").val() * $("#TerrainValue").val()) + SumOfTotalConstructionValue;
            $("#NewCurrentValue").val(SumOfTotalValue.toFixed(__decimalPoints));
        }
    }

}