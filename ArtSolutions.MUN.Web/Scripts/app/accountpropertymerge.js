var PropertyRightList = [];
var ConstructionID = 0;
var OperationID = 0;


$(function () {
    $(".select2dropdown").select2({ width: '100%' });
    GetAccountForSelect('OwnerID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
    GetAccountPropertyRightForSelect('PropertyID', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, true);
    $('#PropertyID').on("select2:select", function (e) {
        //var value = e.params.data;  Using {id,text format}        
        loadAccountServiceAndRights(e.params.data.id);
    });

    $('#PropertyID').on("select2:unselect", function (e) {

        $(".RightsTable tbody tr[data-code='" + e.params.data.text + "']").remove();
        $("#AccountServiceList tr[id='" + e.params.data.id + "']").remove();
        $(".footable-row-detail").remove();
        $(".footable-visible").removeClass("footable-first-column");
        $(".footable-visible").removeClass("footable-last-column");
        $(".footable-visible").removeClass("footable-visible");
        $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
        $('.footable').footable();
        $.grep(PropertyRightList, function (item, index) {
            if (item.FincaID == e.params.data.text) {
                //$("#TerrainValue").val(parseFloat($("#TerrainValue").val() || 0) - parseFloat(item.TerrainValue || 0));
                //$("#Area").val((parseFloat($("#Area").val() || 0) - parseFloat(item.Area || 0)).toFixed(__decimalPoints));
                ////$("#TotalTerrainValue").val(parseFloat($("#TotalTerrainValue").val() || 0) - parseFloat(item.TotalTerrainValue || 0));
                //$("#TotalTerrainValue").val((parseFloat($("#Area").val() || 0) * parseFloat($("#TerrainValue").val() || 0)).toFixed(__decimalPoints))
                //$("#Percentage").val((parseFloat($("#Percentage").val() || 0) - parseFloat(item.Percentage || 0)).toFixed(__decimalPoints));

                $("#TerrainValue").val(GlobalFormatWithTextAndThousandSeparator(parseFloat(Globalize.parseNumber($("#TerrainValue").val() || "0")) - parseFloat(item.TerrainValue || "0")));
                $("#Area").val(GlobalFormatWithTextAndThousandSeparator((parseFloat(Globalize.parseNumber($("#Area").val() || "0")) - parseFloat(item.Area || 0))));
                //$("#TotalTerrainValue").val(parseFloat($("#TotalTerrainValue").val() || 0) - parseFloat(item.TotalTerrainValue || 0));
                $("#TotalTerrainValue").val(GlobalFormatWithTextAndThousandSeparator((parseFloat(Globalize.parseNumber($("#Area").val() || "0")) * parseFloat(Globalize.parseNumber($("#TerrainValue").val() || "0")))))
                $("#Percentage").val(GlobalFormatWithTextAndThousandSeparator((parseFloat(Globalize.parseNumber($("#Percentage").val() || "0")) - parseFloat(item.Percentage || "0"))));
            }
        });
        PropertyRightList = $.grep(PropertyRightList, function (item, index) {
            return item.FincaID != e.params.data.text;
        });

        //CO-1475 - 11-Feb-2020
        if ($("#PropertyID").val() == null || $("#PropertyID").val() == "" || $("#PropertyID").val() == undefined) {
            $("#btnSaveWithDropDown").show();
            $("#btnSaveWithToolTip").addClass("hide");
        }
        else {
            $.ajax({
                url: ROOTPath + "/PropertyTransfer/isNotAssociatedServices",
                data: { "commaSeparatedPropertyID": $("#PropertyID").val().join(",") },
                success: function (response) {
                    if (response.status == true) {
                        if (response.isNotAssociatedServices == 1) {
                            $("#btnSaveWithDropDown").hide();
                            $("#btnSaveWithToolTip").removeClass("hide");
                        }
                        else {
                            $("#btnSaveWithDropDown").show();
                            $("#btnSaveWithToolTip").addClass("hide");
                        }
                    }
                    else
                        showAlert("error", response.message);
                }
            });
        }
        //

    });
    $('#MergetDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('#MergetDate').datepicker('update', new Date());

});

function loadAccountServiceAndRights(accountPropertyID) {
    $.ajax({
        url: ROOTPath + "/PropertyTransfer/GetAccountServiceForMerge",
        data: { "AccountPropertyID": accountPropertyID, "TransferTypeID": 2, "commaSeparatedPropertyID": $("#PropertyID").val().join(",") },
        success: function (response) {
            if (response.status == true) {

                $("#AccountServiceList").append(response.returnData);

                if (response.isNotAssociatedServices == 1) {
                    $("#btnSaveWithDropDown").hide();
                    $("#btnSaveWithToolTip").removeClass("hide");
                }
                else {
                    $("#btnSaveWithDropDown").show();
                    $("#btnSaveWithToolTip").addClass("hide");
                }

                if (accountPropertyID > 0) {
                    $.ajax({
                        url: ROOTPath + "/PropertyTransfer/GetAccountPropertyDetail",
                        data: { ID: accountPropertyID },
                        success: function (response) {
                            if (response.status == false) {
                                showAlert('error', response.message);
                                e.stopPropagation();
                            }
                            else {
                                var PropertyRightModel = {};
                                PropertyRightModel.FincaID = response.FincaID;
                                PropertyRightModel.OwnerID = response.OwnerID;
                                PropertyRightModel.Percentage = response.Percentage;
                                PropertyRightModel.Area = response.Area;
                                PropertyRightModel.TerrainValue = response.TerrainValue;
                                PropertyRightModel.TotalTerrainValue = response.TotalTerrainValue;
                                PropertyRightModel.TotalValue = response.TotalValue;

                                //$("#TerrainValue").val(parseFloat(response.TerrainValue || 0) + parseFloat($("#TerrainValue").val() || 0))
                                //$("#Area").val((parseFloat($("#Area").val() || 0) + parseFloat(response.Area || 0)).toFixed(__decimalPoints));
                                ////$("#TotalTerrainValue").val(parseFloat(response.TotalTerrainValue || 0) + parseFloat($("#TotalTerrainValue").val() || 0))
                                //$("#TotalTerrainValue").val((parseFloat($("#Area").val() || 0) * parseFloat($("#TerrainValue").val() || 0)).toFixed(__decimalPoints))
                                //$("#Percentage").val((parseFloat($("#Percentage").val() || 0) + parseFloat(response.Percentage || 0)).toFixed(__decimalPoints));
                                $("#TerrainValue").val(GlobalFormatWithTextAndThousandSeparator(parseFloat(response.TerrainValue || "0") + parseFloat(Globalize.parseNumber($("#TerrainValue").val() || "0"))))
                                $("#Area").val(GlobalFormatWithTextAndThousandSeparator((parseFloat(Globalize.parseNumber($("#Area").val() ||"0")) + parseFloat(response.Area || "0"))));
                                //$("#TotalTerrainValue").val(parseFloat(response.TotalTerrainValue || 0) + parseFloat($("#TotalTerrainValue").val() || 0))
                                $("#TotalTerrainValue").val(GlobalFormatWithTextAndThousandSeparator((parseFloat(Globalize.parseNumber($("#Area").val() || "0")) * parseFloat(Globalize.parseNumber($("#TerrainValue").val() || "0")))))
                                $("#Percentage").val(GlobalFormatWithTextAndThousandSeparator((parseFloat(Globalize.parseNumber($("#Percentage").val() || "0")) + parseFloat(response.Percentage || "0"))));
                                
                                $(".footable-row-detail").remove();
                                $(".footable-visible").removeClass("footable-first-column");
                                $(".footable-visible").removeClass("footable-last-column");
                                $(".footable-visible").removeClass("footable-visible");
                                $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
                                $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
                                var $TrRow = '<tr data-code="' + response.FincaID + '">\
                                                    <td class="table-description-field">'+ response.FincaID + '</td >\
                                                    <td>'+ response.Owner + '</td>\
                                                    <td class="text-right">'+ GlobalFormatWithTextAndThousandSeparator(parseFloat(response.Percentage || 0)) + ' %</td>\
                                                    <td class="text-right">'+ GlobalFormatWithTextAndThousandSeparator(parseFloat(response.Area || 0)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(response.TerrainValue || 0)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(response.TotalTerrainValue || 0)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(response.TotalValue || 0)) + '</td>\
                                                    <td class="text-right">\
                                                        <a class="btn btn-white btn-sm btnAddContruction"><i class="fa fa-plus"></i></a>\
                                                    </td >\
                                                    <td class="ConstructionList"></td >\
                                                </tr > ';

                                $(".RightsTable tbody:first").append($TrRow);

                                var ConstructionDetailList = $.parseJSON(response.AccountPropertyConstructionDetailJson);
                                $.map(ConstructionDetailList, function (item, index) {
                                    ConstructionID = ConstructionID + 1;
                                    OperationID = ConstructionID;
                                    ConstructionDetailList[index]._ID = OperationID;
                                    ConstructionDetailList[index].hdnUnitValue = item.UnitValue;

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
                                                                            <td class="text-left">'+ GlobalFormatWithTextAndThousandSeparator(parseFloat(item.BuildingArea)) + '</td>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                                            <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.UnitValue)) + '</td>\
                                                                        </tr>\
                                                                        <tr>\
                                                                            <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
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

                                    $(".ConstructionList", $(".RightsTable tr[data-code='" + response.FincaID + "']")).append($Div);

                                });

                                PropertyRightModel.AccountPropertyConstructionDetailJson = jQuery.extend(true, [], ConstructionDetailList);

                                $('.footable').footable();
                                PropertyRightList.push(PropertyRightModel);
                                if ($("#MovementTypeID").val() > 0) {
                                    $("#MovementTypeID").val($("#MovementTypeID").val()).trigger('change');
                                }
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
        resolveRedirectURL(response);
    }
}

function resolveRedirectURL(response) {
    if (response.actionType == 1) // Save 
    {
        window.location = ROOTPath + "/Collections/PropertyTransfer/ViewMerge?ID=" + response.ID;
    }
    else if (response.actionType == 2)// Save & Add New
    {
        window.location = ROOTPath + "/PropertyTransfer/NewMerge";
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

    if ($("#form").valid()) {
        if ($("#PropertyID").val().length < 2) {
            showAlert('error', PropertyRightvalidationmsg);
            return false;
        }
        $("#AccountServiceIDs").val("");
        var AccountServiceIDs = "";
        $(".collectionItem:checked").each(function (index) {
            if (index !== 0) {
                AccountServiceIDs = AccountServiceIDs + "," + $(this).attr("data-id");
            }
            else {
                AccountServiceIDs = $(this).attr("data-id");
            }
            $("#AccountServiceIDs").val(AccountServiceIDs);
        });
        if ($(".collectionItem:checked").length <= 0) {
            showAlert('error', AccountServicevalidationmsg);
            return false;
        }
        $("#PropertyIDs").val($("#PropertyID").val().join(","));
        $("#AccountPropertyConstructionDetailJson").val(JSON.stringify(PropertyRightList));
    }
}

$(document).on("click", ".chkAll", function (e) {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        $(".collectionItem").prop("checked", true);
    }
    else {
        $(".collectionItem").prop("checked", false);
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

                $('#frmGeneralDescription').validate();
                $('#MaterialTypeID').select2('open').select2('close');
            }

        }, error: function () {
        }
    }).always(function () {
    });
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
        var FincaID = $("#GeneralDescriptionConstructionmodal").attr('data-code');
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
                if (item.FincaID == FincaID) {
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
                    <tr data-code="'+ FincaID + '">\
                        <td class="col-lg-11">\
                            <table class="table no-borders">\
                                <tbody>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                        <td class="text-left">'+ $("#MaterialTypeID option:selected").text() + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ ConstructionType + ' :</label></td>\
                                        <td class="text-left">'+ $("#ConstructionTypeID option:selected").text() + '</td>\
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
                                        <td class="text-left">'+ GlobalFormatWithTextAndThousandSeparator(parseFloat($("#BuildingArea").val())) + '</td>\
                                        <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#UnitValue").val())) + '</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
                                        <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat($("#TotalValue").val())) + '</td>\
                                        <td class="control-label text-right" colspan="6"></td>\
                                    </tr>\
                                </tbody>\
                            </table>\
                        </td>\
                        <td class="col-lg-1 text-right">\
                            <a class="btn btn-white btn-sm btnEditConstruction" data-type="new"  data-id="' + OperationID + '"><i class="fa fa-pencil"></i></a>\
                            <a class="btn btn-white btn-sm btnDeleteConstruction" data-type="new" data-id="' + OperationID + '"><i class="fa fa-trash"></i></a>\
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
            $(".ConstructionList", $(".RightsTable tr[data-code='" + FincaID + "']")).append($Div);
        }
        $('.footable').footable();
        $("#GeneralDescriptionConstructionmodal").modal('hide');
        if ($("#MovementTypeID").val() > 0) {
            $("#MovementTypeID").val($("#MovementTypeID").val()).trigger('change');
        }
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
    if ($("#MovementTypeID").val() > 0) {
        $("#MovementTypeID").val($("#MovementTypeID").val()).trigger('change');
    }
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
                var FincaID = $this.closest('tr').data('code');
                $.grep(PropertyRightList, function (item, index) {
                    if (item.FincaID == FincaID) {
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
                                $("#BuildingArea").val(GlobalFormat(data.BuildingArea)).trigger('change');
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

function InitializeConstructionYear() {
    $('#ConstructionYear').datepicker({
        minViewMode: 2,
        format: 'yyyy',
        autoclose: true,
        language: __culture,
        endDate: new Date()
    });
}

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

$(".divAccountProperty").on("change", "#MovementTypeID", function () {

    var TerrainValue = 0;
    var TerrainArea = 0;
    var TotalTerrainValue = 0;
    var TotalValues = 0;
    if (PropertyRightList.length > 0) {
        $(".NewRightsTable tbody").html('');
        $(".footable-row-detail").remove();
        $(".footable-visible").removeClass("footable-first-column");
        $(".footable-visible").removeClass("footable-last-column");
        $(".footable-visible").removeClass("footable-visible");
        $(".NewRightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
        $(".NewRightsTable thead td:last,.NewRightsTable tbody td:last").show();
        $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
        $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
        $.map(PropertyRightList, function (data, index) {
            TerrainValue = TerrainValue + (data.TerrainValue || 0);
            TerrainArea = TerrainArea + (data.Area || 0);
            //TotalTerrainValue = TotalTerrainValue + (data.TotalTerrainValue || 0);
            TotalTerrainValue = TerrainArea * TerrainValue;
            TotalValues = TotalValues + (data.TotalValue || 0);
            $.map(data.AccountPropertyConstructionDetailJson, function (item, dataIndex) {

                if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == false) {

                    PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].TotalValue = (PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].BuildingArea * PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].hdnUnitValue);
                    PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].UnitValue = PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].hdnUnitValue;
                }
                else {
                    PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].UnitValue = 0;
                    PropertyRightList[index].AccountPropertyConstructionDetailJson[dataIndex].TotalValue = 0;
                }

                var $TrRow = '<tr data-code="' + item.ID + '">\
                                                    <td class="table-description-field">'+ item.MaterialType + '</td >\
                                                    <td>'+ (item.ConstructionType == null ? '-' : item.ConstructionType) + '</td>\
                                                    <td class="text-right">'+ GlobalFormatWithTextAndThousandSeparator(parseFloat(item.BuildingArea)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.UnitValue)) + '</td>\
                                                    <td class="text-right">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.TotalValue)) + '</td>\
                                                    <td class="ConstructionList"></td >\
                                                </tr > ';

                $(".NewRightsTable tbody:first").append($TrRow);
                $Div = '<div class="row1 border-bottom">\
                                                    <table width ="100%">\
                                                        <tbody>\
                                                            <tr data-code="'+ item.FincaID + '">\
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
                                                                                <td class="text-left">'+ GlobalFormatWithTextAndThousandSeparator(parseFloat(item.BuildingArea)) + '</td>\
                                                                                <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                                                <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.UnitValue)) + '</td>\
                                                                            </tr>\
                                                                            <tr>\
                                                                                <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
                                                                                <td class="text-left">'+ _currencySymbol + GlobalFormatWithTextAndThousandSeparator(parseFloat(item.TotalValue)) + '</td>\
                                                                                <td class="control-label text-right" colspan="6"></td>\
                                                                            </tr>\
                                                                        </tbody>\
                                                                    </table>\
                                                                </td>\
                                                            </tr>\
                                                    </tbody>\
                                            </table >\
                                        </div >';

                $(".ConstructionList", $(".NewRightsTable tr[data-code='" + item.ID + "']")).append($Div);
            });

        });

        $('.footable').footable();
    }
    else {
        $(".NewRightsTable tbody").html('');
    }

    if (($("#MovementTypeID").val() == 3 || $("#MovementTypeID").val() == 4 || $("#MovementTypeID").val() == 5 || $("#MovementTypeID").val() == 6 || $("#MovementTypeID").val() == 13) == true) {

        var ZeroValue = 0;
        $("#TerrainValue").val(GlobalFormatWithTextAndThousandSeparator(parseFloat(ZeroValue)))
        $("#TotalTerrainValue").val(GlobalFormatWithTextAndThousandSeparator(parseFloat(ZeroValue)))
        if (TotalValues >= 0) {
            //$("#NewCurrentValue").val(TotalValues.toFixed(__decimalPoints))

            $("#NewCurrentValue").val(GlobalFormatWithTextAndThousandSeparator(TotalValues)); // CO-1626

        }
        else {
            $("#NewCurrentValue").val('');
        }
    }
    else {
        $("#TerrainValue").val(GlobalFormatWithTextAndThousandSeparator(parseFloat(TerrainValue)))
        $("#TotalTerrainValue").val(GlobalFormatWithTextAndThousandSeparator(parseFloat(TotalTerrainValue)))
        //if ($("#Percentage").val() > 0) {
        var SumOfTotalConstructionValue = 0.0;
        var SumOfTotalComplementaryValue = 0.0;
        var SumOfTotalValue = 0.0;
        $.map(PropertyRightList, function (data, index) {
            $.map(data.AccountPropertyConstructionDetailJson, function (item, dataIndex) {
                if (item.MaterialTypeID == 1) {
                    SumOfTotalConstructionValue = SumOfTotalConstructionValue + item.TotalValue
                }
                else {
                    SumOfTotalComplementaryValue = SumOfTotalComplementaryValue + item.TotalValue
                }
            });
        });
        SumOfTotalValue = (Globalize.parseNumber($("#Area").val()) * Globalize.parseNumber($("#TerrainValue").val())) + SumOfTotalConstructionValue + SumOfTotalComplementaryValue;

        //$("#NewCurrentValue").val(SumOfTotalValue.toFixed(__decimalPoints));
        $("#NewCurrentValue").val(GlobalFormatWithTextAndThousandSeparator(SumOfTotalValue)); // CO-1626

        //}
        //else {
        //    $("#NewCurrentValue").val('');
        //}
    }

});