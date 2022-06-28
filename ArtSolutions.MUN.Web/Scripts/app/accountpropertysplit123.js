var PropertyRightList = [];
var ConstructionID = 0;
var RigthNumber;
var OwnerID;
var Area;
var Percentage;
var TerrainValue;
var TotalTerrainValue;
var ConstructionDetailList = [];

$(function () {
    $(".select2dropdown").select2({ width: '100%' });
    GetAccountForSelect('OldAccountID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
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
    $.ajax({
        url: ROOTPath + "/AccountProperty/GetAccountServiceForSplit",
        data: { "accountId": accountId, "AccountPropertyID": accountPropertyID, "TransferTypeID": 2 },
        success: function (response) {
            if (response.status == undefined) {
                $("#AccountServiceList").html(response);
                if (accountPropertyID > 0) {
                    $.ajax({
                        url: ROOTPath + "/AccountProperty/GetAccountPropertyDetail",
                        data: { ID: accountPropertyID },
                        success: function (response) {
                            if (response.status == false) {
                                showAlert('error', response.message);
                                e.stopPropagation();
                            }
                            else {
                                $('#AccountPropertyConstructionDetailJson').val(response.AccountPropertyConstructionDetailJson)
                                RigthNumber = response.RigthNumber;
                                OwnerID = response.OwnerID;
                                Area = response.Area;
                                Percentage = response.Percentage;
                                TerrainValue = response.TerrainValue;
                                TotalTerrainValue = response.TotalTerrainValue;                                
                                ConstructionDetailList = $.parseJSON($('#AccountPropertyConstructionDetailJson').val());                               
                                $(".RightsTable tbody").html('');                                
                                PropertyRightList = [];
                                $('#AccountPropertyConstructionDetailJson').val(); 
                            }
                        }
                    });
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
        resolveRedirectURL(response.actionType);
    }
}

function resolveRedirectURL(actionType) {
    if (actionType == 1 || actionType == 3) // Save OR Cancle
    {
        window.location = ROOTPath + "/AccountProperty/List";
    }
    else if (actionType == 2)// Save & Add New
    {
        window.location = ROOTPath + "/AccountProperty/Add"
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

function Save() {
    var ConstructionRequire = false;
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    if ($("#form").valid()) {
        $.grep(PropertyRightList, function (item, index) {
            var newAccountPropertyConstructionDetailJson = item.AccountPropertyConstructionDetailJson.filter(function (el) {
                return el.IsDeleted !== true;
            });
            if (newAccountPropertyConstructionDetailJson.length == 0) {
                ConstructionRequire = true;
            }
        });
        if (PropertyRightList.length == 0) {
            showAlert('error', PropertyRightvalidationmsg);
            return false;
        }
        else if (ConstructionRequire) {
            showAlert('error', Constructionvalidationmsg);
            return false;
        }
        //else if (CheckBalanceArea(RightsCode, mode) < 100) {
        //    showAlert('error', AreaErrMsg);
        //    return false;
        //}
        else {
            $("#AccountPropertyConstructionDetailJson").val(JSON.stringify(PropertyRightList));
        }
    }
}

//AddRights
$('.addRights').on('click', function () {
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    ClearRights();
    if ($("#form").valid()) {
        //if (CheckBalanceArea(RightsCode, mode) == 100) {
        //    showAlert('error', NoMoreAreaMsg);
        //    return;
        //}
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
                    $("#RigthNumber").val(RigthNumber);
                    $("#OwnerID").val(OwnerID).trigger('change');
                    $("#Area").val(Area);
                    $("#Percentage").val(Percentage);
                    $("#TerrainValue").val(TerrainValue);
                    $("#TotalTerrainValue").val(TotalTerrainValue);

                    $('#RigthNumber').focus();                    
                    $("#AddRightsmodal").attr('data-code', '');
                    $("#AddRightsmodal").attr('data-mode', 'new');
                    $("#OwnerID").select2({ width: '100%' });
                    $('#frmRights').validate();
                    $("#AddRightsmodal #btnAddRights").html(Add);
                    $("#AddRightsmodal #addresstitle").html(NewRight);
                    GetAccountForSelect('OwnerID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: ROOTPath + "/Account/GetAccountForSearch",
                        data: {
                            'accountID': OwnerID,
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
                    $("#AddRightsmodal").modal('show');
                }

            }, error: function () {
            }
        }).always(function () {
        });
    }

})

function ClearRights() {
    $("#AddRightsmodal input, #AddRightsmodal select").val('')
}

//$("#divAddRights").on("change", "#Percentage", function () {
//    if ($.isNumeric($("#Percentage").val()) && $.isNumeric($("#TotalArea").val())) {
//        $("#Area").val(($("#Percentage").val() * $("#TotalArea").val()) / 100)
//    }
//    else {
//        $("#Area").val('');
//    }
//})

//function CheckBalanceArea(RightsCode, mode) {
//    var _Percentage = 0;
//    if (RightsCode == "" && mode == "new") {
//        if (PropertyRightList.length > 0) {
//            $.grep(PropertyRightList, function (item, index) {
//                _Percentage = parseFloat(_Percentage) + parseFloat(item.Percentage)
//            });
//            return _Percentage;
//        }
//    }
//    else {
//        if (PropertyRightList.length > 0) {
//            $.grep(PropertyRightList, function (item, index) {
//                if (item.RigthNumber != RightsCode) {
//                    _Percentage = parseFloat(_Percentage) + parseFloat(item.Percentage)
//                }
//            });
//            return _Percentage;
//        }
//    }
//    return 0;
//}

//function IsNotExistsRightsCode(RightsCode, mode) {
//    var retval = true;
//    if (PropertyRightList.length > 0) {
//        if (RightsCode == "" && mode == "new") {
//            $.grep(PropertyRightList, function (item, index) {
//                if (item.RigthNumber.toLowerCase() == $("#RigthNumber").val().toLowerCase()) {
//                    showAlert('error', CodeErrMsg);
//                    retval = false;
//                }
//            });
//        } else {
//            $.grep(PropertyRightList, function (item, index) {
//                if (item.RigthNumber != RightsCode && item.RigthNumber.toLowerCase() == $("#RigthNumber").val().toLowerCase()) {
//                    showAlert('error', CodeErrMsg);
//                    retval = false;
//                }
//            });
//        }
//    }
//    return retval;
//}

$("#divAddRights").on("click", "#btnAddRights", function () {
    var RightsCode = $("#AddRightsmodal").attr('data-code');
    var mode = $("#AddRightsmodal").attr('data-mode');
    //if ($("#frmRights").valid() && IsNotExistsRightsCode(RightsCode, mode)) {
    if ($("#frmRights").valid()) {
        //var _Percentage = CheckBalanceArea(RightsCode, mode);
        //if (_Percentage == 100 || ((parseFloat(_Percentage) + parseFloat($("#Percentage").val())) > 100)) {
        //    showAlert('error', NoMoreAreaMsg);
        //    return;
        //}
        if (mode == "new") {
            var PropertyRightModel = {};
            PropertyRightModel.RigthNumber = $("#RigthNumber").val();
            PropertyRightModel.OwnerID = $("#OwnerID").val();
            PropertyRightModel.Percentage = $("#Percentage").val();
            PropertyRightModel.Area = $("#Area").val();
            PropertyRightModel.TerrainValue = $("#TerrainValue").val();
            PropertyRightModel.TotalTerrainValue = $("#TotalTerrainValue").val();
            PropertyRightModel.AccountPropertyConstructionDetailJson = ConstructionDetailList;

          
            var IsAvailableOwnerID = $.each($(".Owner:first option"), function () {
                return $(this).val() == PropertyRightModel.OwnerID;
            });

            if (!(IsAvailableOwnerID >= 0)) {
                $(".Owner").append('<option value="' + PropertyRightModel.OwnerID + '">' + $("#OwnerID option:selected").text().trim() +'</option>');
            }

            $(".footable-row-detail").remove();
            $(".footable-visible").removeClass("footable-first-column");
            $(".footable-visible").removeClass("footable-last-column");
            $(".footable-visible").removeClass("footable-visible");
            $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
            $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
            var $TrRow = '<tr data-code="' + PropertyRightModel.RigthNumber + '">\
                        <td class="table-description-field">'+ PropertyRightModel.RigthNumber + '</td >\
                        <td>'+ $("#OwnerID option:selected").text().trim() + '</td>\
                        <td class="text-right">'+ parseFloat(PropertyRightModel.Percentage).toFixed(__decimalPoints) + ' %</td>\
                        <td class="text-right">'+ parseFloat(PropertyRightModel.Area).toFixed(__decimalPoints) + '</td>\
                        <td class="text-right">'+ _currencySymbol + parseFloat(PropertyRightModel.TerrainValue).toFixed(__decimalPoints) + '</td>\
                        <td class="text-right">'+ _currencySymbol + parseFloat(PropertyRightModel.TotalTerrainValue).toFixed(__decimalPoints) + '</td>\
                        <td class="text-right">\
                            <a class="btn btn-white btn-sm btnAddContruction"><i class="fa fa-plus"></i></a>\
                            <a class="btn btn-white btn-sm btnEditRights"><i class="fa fa-pencil"></i></a>\
                            <a class="btn btn-white btn-sm btnDeleterights"><i class="fa fa-trash"></i></a>\
                        </td >\
        <td class="ConstructionList"> </td>\
                    </tr > ';
            $(".RightsTable tbody:first").append($TrRow);            
            $.grep(PropertyRightModel.AccountPropertyConstructionDetailJson, function (item, index) {
               
                var OperationID = 0;
                ConstructionID = ConstructionID + 1;                
                OperationID = ConstructionID;
                PropertyRightModel.AccountPropertyConstructionDetailJson[index]._ID = OperationID;


            var $Div = '<div class="row1 border-bottom">\
                            <table width ="100%">\
                                <tbody>\
                                    <tr data-code="'+ PropertyRightModel.RigthNumber + '">\
                                        <td class="col-lg-11">\
                                            <table class="table no-borders">\
                                                <tbody>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                                        <td class="text-left">'+ item.MaterialType + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionType + ' :</label></td>\
                                                        <td class="text-left">'+ item.ConstructionType + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionYear + ' :</label></td>\
                                                        <td class="text-left">'+ item.ConstructionYear + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Years + ' :</label></td>\
                                                        <td class="text-left">'+ item.TotalYears + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                                        <td class="text-left">'+ item.Status + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                                        <td class="text-left">'+ item.Floor + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                                        <td class="text-left">'+ item.Chambers + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                                        <td class="text-left">'+ item.InternalWalls + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                                        <td class="text-left">'+ item.ExternalWalls + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                                        <td class="text-left">'+ item.Structure + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                                        <td class="text-left">'+ item.Roof + ' </td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                                        <td class="text-left">'+ item.Ceiling + ' </td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                                        <td class="text-left">'+ item.Floor + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                                        <td class="text-left">'+ item.Bathroom + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' :</label></td>\
                                                        <td class="text-left">'+ parseFloat(item.BuildingArea).toFixed(__decimalPoints) + '</td>\
                                                        <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                        <td class="text-left">'+ _currencySymbol + parseFloat(item.UnitValue).toFixed(__decimalPoints) + '</td>\
                                                    </tr>\
                                                    <tr>\
                                                        <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
                                                        <td class="text-left">'+ _currencySymbol + parseFloat(item.TotalValue).toFixed(__decimalPoints) + '</td>\
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
                $(".ConstructionList", $(".RightsTable tr[data-code='" + PropertyRightModel.RigthNumber + "']")).append($Div);
            });

            $('.footable').footable();
            PropertyRightList.push(PropertyRightModel);
        }
        else {
            $.grep(PropertyRightList, function (item, index) {
                if (item.RigthNumber == RightsCode) {

                    var IsAvailableOwnerID = $.each($(".Owner:first option"), function () {
                        return $(this).val() == item.OwnerID;
                    });
                    

                    if ((IsAvailableOwnerID > 0 && item.OwnerID != $("#OwnerID").val())) {
                        $(".Owner[value='" + item.OwnerID + "']").remove();
                        $(".Owner").append('<option value="' + PropertyRightModel.OwnerID + '">' + $("#OwnerID option:selected").text().trim() + '</option>');
                    }

                    PropertyRightList[index].RigthNumber = $("#RigthNumber").val();
                    PropertyRightList[index].OwnerID = $("#OwnerID").val();
                    PropertyRightList[index].Area = $("#Area").val();
                    PropertyRightList[index].Percentage = $("#Percentage").val();
                    PropertyRightList[index].TerrainValue = $("#TerrainValue").val();
                    PropertyRightList[index].TotalTerrainValue = $("#TotalTerrainValue").val();
                }
            });
            var $Span = $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(0) span")[0].outerHTML;
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(0)").html('').html($Span + $("#RigthNumber").val())
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(1)").html($("#OwnerID option:selected").text().trim())
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(2)").html(parseFloat($("#Percentage").val()).toFixed(__decimalPoints) + ' %')
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(3)").html(parseFloat($("#Area").val()).toFixed(__decimalPoints))
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(4)").html(_currencySymbol + parseFloat($("#TerrainValue").val()).toFixed(__decimalPoints))
            $(".RightsTable tr[data-code='" + RightsCode + "'] td:eq(5)").html(_currencySymbol + parseFloat($("#TotalTerrainValue").val()).toFixed(__decimalPoints))
            $(".RightsTable tr[data-code='" + RightsCode + "']").attr("data-code", $("#RigthNumber").val());
            $('.footable').footable();
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
        PropertyConstructionModel.ConstructionTypeID = $("#ConstructionTypeID").val();
        PropertyConstructionModel.MaterialTypeID = $("#MaterialTypeID").val();
        PropertyConstructionModel.ConstructionYear = $("#ConstructionYear").val();
        PropertyConstructionModel.TotalYears = $("#TotalYears").val();
        PropertyConstructionModel.StatusID = $("#StatusID").val();
        PropertyConstructionModel.Floors = $("#Floors").val();
        PropertyConstructionModel.Chambers = $("#Chambers").val();
        PropertyConstructionModel.InternalWalls = $("#InternalWallsID").val();
        PropertyConstructionModel.ExternalWalls = $("#ExternalWallsID").val();
        PropertyConstructionModel.StructureID = $("#StructureID").val();
        PropertyConstructionModel.RoofID = $("#RoofID").val();
        PropertyConstructionModel.CeilingID = $("#CeilingID").val();
        PropertyConstructionModel.FloorID = $("#FloorID").val();
        PropertyConstructionModel.Bathroom = $("#Bathroom").val();
        PropertyConstructionModel.BuildingArea = $("#BuildingArea").val();
        PropertyConstructionModel.UnitValue = $("#UnitValue").val();
        PropertyConstructionModel.TotalValue = $("#TotalValue").val();
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
                if (item.RigthNumber == RightsCode) {
                    PropertyRightList[index].AccountPropertyConstructionDetailJson.push(PropertyConstructionModel)
                }
            });
        }

        $(".footable-row-detail").remove();
        $(".footable-visible").removeClass("footable-first-column");
        $(".footable-visible").removeClass("footable-last-column");
        $(".footable-visible").removeClass("footable-visible");
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
                                        <td class="text-left">'+ $("#ConstructionTypeID option:selected").text() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionYear + ' :</label></td>\
                                        <td class="text-left">'+ $("#ConstructionYear").val() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Years + ' :</label></td>\
                                        <td class="text-left">'+ $("#TotalYears").val() + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                        <td class="text-left">'+ $("#StatusID option:selected").text() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                        <td class="text-left">'+ $("#Floors").val() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                        <td class="text-left">'+ $("#Chambers").val() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                        <td class="text-left">'+ $("#InternalWallsID").val() + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                        <td class="text-left">'+ $("#ExternalWallsID").val() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                        <td class="text-left">'+ $("#StructureID option:selected").text() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                        <td class="text-left">'+ $("#RoofID option:selected").text() + ' </td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                        <td class="text-left">'+ $("#CeilingID option:selected").text() + ' </td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                        <td class="text-left">'+ $("#FloorID option:selected").text() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                        <td class="text-left">'+ $("#Bathroom").val() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' (<em>m</em><sup>2</sup>) :</label></td>\
                                        <td class="text-left">'+ parseFloat($("#BuildingArea").val()).toFixed(__decimalPoints) + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                        <td class="text-left">'+ _currencySymbol + parseFloat($("#UnitValue").val()).toFixed(__decimalPoints) + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
                                        <td class="text-left">'+ _currencySymbol + parseFloat($("#TotalValue").val()).toFixed(__decimalPoints) + '</td>\
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
                        data.InternalWalls = PropertyConstructionModel.InternalWalls;
                        data.ExternalWalls = PropertyConstructionModel.ExternalWalls;
                        data.StructureID = PropertyConstructionModel.StructureID;
                        data.RoofID = PropertyConstructionModel.RoofID;
                        data.CeilingID = PropertyConstructionModel.CeilingID;
                        data.FloorID = PropertyConstructionModel.FloorID;
                        data.Bathroom = PropertyConstructionModel.Bathroom;
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
        $('.footable').footable();
        $("#GeneralDescriptionConstructionmodal").modal('hide');
    }
});

$(".RightsTable").on("click", ".btnDeleteConstruction", function () {
    var Id = $(this).attr('data-id');

    $.grep(PropertyRightList, function (item, index) {
        item.AccountPropertyConstructionDetailJson = $.grep(item.AccountPropertyConstructionDetailJson, function (data, dataIndex) {
            return data._ID != Id
        })
    });
    $(this).closest('div').remove();
    $(".RightsTable a[data-id='" + Id + "']").closest("div").remove();
    $(".footable-row-detail").remove();
    $(".footable-visible").removeClass("footable-first-column");
    $(".footable-visible").removeClass("footable-last-column");
    $(".footable-visible").removeClass("footable-visible");
    $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
    $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
    $('.footable').footable();
});

$(".RightsTable").on("click", ".btnDeleterights", function () {
    var code = $(this).closest('tr').attr('data-code');

    $.grep(PropertyRightList, function (item, index) {
        if (item.RigthNumber == code) {
            $(".Owner[value='" + item.OwnerID + "']").remove();
        }
    });

    PropertyRightList = $.grep(PropertyRightList, function (item, index) {
        return item.RigthNumber != code;
    });

    $(this).closest('tr').remove();
    $(".footable-row-detail").remove();
    $(".footable-visible").removeClass("footable-first-column");
    $(".footable-visible").removeClass("footable-last-column");
    $(".footable-visible").removeClass("footable-visible");
    $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
    $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
    $('.footable').footable();
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
})

$('#btnAddGeoLocation').on('click', function () {
    if ($("#txtSearch").length)
        $("#txtSearch").focus();
    $("#lblLatitude").val($("#Latitude").val());
    $("#lblLongitude").val($("#Longitude").val());
    UpdateMap();
    $("#AddGeoLocationmodal").modal('show');


});

$("#btnAddLocation").on("click", function () {
    $("#Latitude").val($(".lblLatitude").html());
    $("#Longitude").val($(".lblLongitude").html());
    $("#AddGeoLocationmodal").modal('hide');
});

$("#btnClearLocation").on("click", function () {
    //$("#Latitude").val('');
    $(".lblLatitude").html('');
    //$("#Longitude").val('');
    $(".lblLongitude").html('');
    $("#pac-input").val('');
    UpdateMap();
});
$("#AddGeoLocationmodal #btnCancel").on("click", function () {

    $(".lblLatitude").html('');
    $(".lblLongitude").html('');
    $("#pac-input").val('');
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
                    if (item.RigthNumber = RigthCode) {
                        $.grep(item.AccountPropertyConstructionDetailJson, function (data, dataIndex) {
                            if (data._ID == Id) {
                                
                                $("#ConstructionTypeID").val(data.ConstructionTypeID).trigger('change');
                                $("#MaterialTypeID").val(data.MaterialTypeID).trigger('change');
                                $("#ConstructionYear").val(data.ConstructionYear);
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
                                $("#Bathroom").val(data.Bathroom);
                                $("#BuildingArea").val(data.BuildingArea).trigger('change');
                                $("#UnitValue").val(data.UnitValue);
                                $("#TotalValue").val(data.TotalValue);
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
    console.log(code)
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
                $("#AddRightsmodal").attr('data-code', code).modal('show');
                $("#AddRightsmodal").attr('data-mode', 'edit');
                $("#OwnerID").select2({ width: '100%' });
                $('#frmRights').validate();
                $.grep(PropertyRightList, function (item, index) {
                    if (item.RigthNumber == code) {
                        $("#RigthNumber").val(item.RigthNumber);
                        $("#OwnerID").val(item.OwnerID).trigger('change');
                        $("#Area").val(item.Area)
                        $("#Percentage").val(item.Percentage)
                        $("#TerrainValue").val(item.TerrainValue);
                        $("#TotalTerrainValue").val(item.TotalTerrainValue);
                    }
                });
                $('#RigthNumber').focus();
                $("#AddRightsmodal #btnAddRights").html(Update);
                $("#AddRightsmodal #addresstitle").html(EditRight);
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
    if ($("#UnitValue").val() == 0) {
        showAlert('error', UnitValueRequiredMsg);
        retval = false;
    }
    return retval;
}

function IsTotalValueGreaterThanZero() {
    var retval = true;
    if ($("#TotalValue").val() == 0) {
        showAlert('error', TotalValueRequiredMsg);
        retval = false;
    }
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