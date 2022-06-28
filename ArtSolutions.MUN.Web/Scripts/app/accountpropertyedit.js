﻿var PropertyConstructionList = [];
var ConstructionID = 0;

$(document).ready(function () {

    $('#DateOfMovement').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });

    if (GlobalConvertToDecimal($('#Percentage').val()) <= 0) {
        $('#Percentage').val(null);
    }
    if (GlobalConvertToDecimal($('#SlopePercentage').val()) <= 0) {
        $('#SlopePercentage').val(null);
    }
    if (GlobalConvertToDecimal($('#RegularFactor').val()) <= 0) {
        $('#RegularFactor').val(null);
    }
});

$(function () {
    $(".select2dropdown").select2({ width: '100%' });

    //Services DropDown
    if ($("#ServicesList1").val() !== "") {
        $('#Services1').val($("#ServicesList1").val().split(",")).trigger('change');
    }
    if ($("#ServicesList2").val() !== "") {
        $('#Services2').val($("#ServicesList2").val().split(",")).trigger('change');
    }
    //

    GetAccountForSelect('OwnerID', null, '4,5', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
    PropertyConstructionList = $.parseJSON($('#AccountPropertyConstructionDetailJson').val());
});

function UserSuccessCallback(response) {
    if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        window.location = ROOTPath + "/AccountProperty/List";
    }
}

function resolveRedirectURL(actionType) {
    if (actionType == 1 || actionType == 3) // Save OR Cancle
    {
        window.location = ROOTPath + "/AccountProperty/List";
    }
    else if (actionType == 2)// Save & Add New
    {
        window.location = ROOTPath + "/AccountProperty/Add";
    }
}

// save account property detail
$("#btnSave").click(function (event) {
    if (Save() == false)
        return false;
});

function Save() {
    var ConstructionRequire = false;
    var RightsCode = "";
    var mode = "new";

    if ($("#form").valid()) {

        if ($("#OwnerID").val() > 0) {

            var TotalCountructionDetail = 0;

            //CO-970
            $("#hfDateOfMovement").val($("#DateOfMovement").val());
            //

            //
            $.each(PropertyConstructionList, function (key, value) {
                if (PropertyConstructionList[key].Bathroom == "")
                    PropertyConstructionList[key].Bathroom = null;
                if (PropertyConstructionList[key].UsefulLife == "-")
                    PropertyConstructionList[key].UsefulLife = null;
            });

            if (isCopyMode == "True") { //Copy
                $("#AccountPropertyConstructionDetailJson").val(
                    JSON.stringify(
                        PropertyConstructionList.filter(function (el) {
                            return el.IsDeleted == false;
                        })
                    )
                );
            }
            else {
                $("#AccountPropertyConstructionDetailJson").val(JSON.stringify(PropertyConstructionList));
            }
            //

            $("#hfOwnerID").val($("#OwnerID").val());

            //
            var ListServices1;
            $.each($("#Services1").select2('data'), function (index, item) {
                if (index !== 0) {
                    ListServices1 = ListServices1 + "," + item.id;
                }
                else {
                    ListServices1 = item.id;
                }
            });
            $("#ServicesList1").val(ListServices1);

            var ListServices2;
            $.each($("#Services2").select2('data'), function (index, item) {
                if (index !== 0) {
                    ListServices2 = ListServices2 + "," + item.id;
                }
                else {
                    ListServices2 = item.id;
                }
            });
            $("#ServicesList2").val(ListServices2);
            //
        }
        else {
            showAlert('error', ownerRequiredValidationMsg);
        }

    }
}

$(".addConstruction").on("click", function () {
    $this = $(this);
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
                $("#GeneralDescriptionConstructionmodal").modal('show');
                $("#GeneralDescriptionConstructionmodal").attr('data-mode', 'new');
                $("#divGeneralDescription .select2dropdown").select2({ width: '100%' });
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

        var mode = $("#GeneralDescriptionConstructionmodal").attr('data-mode');
        var IndexID = $("#IndexID").val();
        var OperationID = 0;

        $(".footable-row-detail").remove();
        $(".footable-visible").removeClass("footable-first-column");
        $(".footable-visible").removeClass("footable-last-column");
        $(".footable-visible").removeClass("footable-visible");
        $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
        $(".RightsTable thead td:last,.RightsTable tbody td:last").show();

        var PropertyConstructionModel = new Object();
        var $Div = "";
        if (mode == "edit") {
            PropertyConstructionModel.ConstructionTypeID = ($("#ConstructionTypeID").val() == "" ? null : $("#ConstructionTypeID").val());
            PropertyConstructionModel.MaterialTypeID = $("#MaterialTypeID").val();
            PropertyConstructionModel.ConstructionYear = 0;
            PropertyConstructionModel.TotalYears = $("#TotalYears").val();
            PropertyConstructionModel.StatusID = ($("#StatusID").val() == "" ? null : $("#StatusID").val());
            PropertyConstructionModel.Floors = $("#Floors").val();
            PropertyConstructionModel.Chambers = $("#Chambers").val();
            PropertyConstructionModel.InternalWallsID = ($("#InternalWallsID").val() == "" ? null : $("#InternalWallsID").val());
            PropertyConstructionModel.ExternalWallsID = ($("#ExternalWallsID").val() == "" ? null : $("#ExternalWallsID").val());
            PropertyConstructionModel.StructureID = ($("#StructureID").val() == "" ? null : $("#StructureID").val());
            PropertyConstructionModel.RoofID = ($("#RoofID").val() == "" ? null : $("#RoofID").val());
            PropertyConstructionModel.CeilingID = ($("#CeilingID").val() == "" ? null : $("#CeilingID").val());
            PropertyConstructionModel.FloorID = ($("#FloorID").val() == "" ? null : $("#FloorID").val());
            PropertyConstructionModel.Bathroom = ($("#Bathroom").val() == "" ? "" : GlobalConvertToDecimal($("#Bathroom").val()));
            PropertyConstructionModel.UsefulLife = ($("#UsefulLife").val() == "" ? "-" : $("#UsefulLife").val());
            PropertyConstructionModel.BuildingArea = GlobalConvertToDecimal($("#BuildingArea").val());
            PropertyConstructionModel.UnitValue = GlobalConvertToDecimal($("#UnitValue").val());
            PropertyConstructionModel.TotalValue = GlobalConvertToDecimal($("#TotalValue").val());
            PropertyConstructionModel.IsActive = true;
            PropertyConstructionModel.IsDeleted = false;
            PropertyConstructionModel.ID = $("#frmGeneralDescription #ID").val() || 0;
            var Type = PropertyConstructionModel.ID > 0 ? "old" : "new";

            if (Type == "new")
                OperationID = $("#IndexID").val();
            else
                OperationID = $("#frmGeneralDescription #ID").val();

            var RemoveID = 0;
            if (Type == "new")
                RemoveID = $("#IndexID").val();
            else
                RemoveID = $("#frmGeneralDescription #ID").val();

            $Div = '<div class="row1 border-bottom">\
                        <table width ="100%">\
                            <tbody>\
                                <tr data-id="'+ RemoveID + '">\
                                    <td class="col-lg-10">\
                                        <table class="table no-borders">\
                                            <tbody>\
                                                <tr>\
                                                    <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                                    <td class="text-left">'+ $("#MaterialTypeID option:selected").text() + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ CurrentAge + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#TotalYears").val() > 0 ? $("#TotalYears").val() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#StatusID").val() > 0 ? $("#StatusID option:selected").text() : '-') + '</td>\
                                                    <td class="control-label text-right" > <label class="control-label">'+ UsefulLife + ' :</label></td >\
                                                    <td class="text-left">'+ ($("#UsefulLife").val() == "" ? "-" : $("#UsefulLife").val()) + '</td>\
                                                </tr>\
                                                <tr>\
                                                    <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#Floors").val() > 0 ? $("#Floors").val() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#Chambers").val() > 0 ? $("#Chambers").val() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#InternalWallsID").val() > 0 ? $("#InternalWallsID option:selected").text() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#ExternalWallsID").val() > 0 ? $("#ExternalWallsID option:selected").text() : '-') + '</td>\
                                                </tr>\
                                                <tr>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Structure + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#StructureID").val() > 0 ? $("#StructureID option:selected").text() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#RoofID").val() > 0 ? $("#RoofID option:selected").text() : '-') + ' </td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#CeilingID").val() > 0 ? $("#CeilingID option:selected").text() : '-') + ' </td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#FloorID").val() > 0 ? $("#FloorID option:selected").text() : '-') + '</td>\
                                                </tr>\
                                                <tr>\
                                                    <td class="control-label text-right" > <label class="control-label">'+ Bathroom + '  :</label></td >\
                                                    <td class="text-left">'+ (PropertyConstructionModel.Bathroom > 0 ? PropertyConstructionModel.Bathroom : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' :</label></td>\
                                                    <td class="text-left">'+ GlobalFormat(GlobalConvertToDecimal($("#BuildingArea").val())) + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                    <td class="text-left">'+ GlobalFormatWithText(GlobalConvertToDecimal($("#UnitValue").val())) + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
                                                    <td class="text-left">'+ GlobalFormatWithText(GlobalConvertToDecimal($("#TotalValue").val())) + '</td>\
                                                </tr>\
                                            </tbody>\
                                        </table>\
                                    </td>\
                                </tr>\
                            </tbody>\
                        </table >\
                      </div >';

            $.grep(PropertyConstructionList, function (data, dataIndex) {

                if ((Type == "new" && data._ID == IndexID) || (data.ID == PropertyConstructionModel.ID)) {
                    data.ConstructionTypeID = PropertyConstructionModel.ConstructionTypeID;
                    data.MaterialTypeID = PropertyConstructionModel.MaterialTypeID;
                    data.ConstructionYear = 0;
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
            });

            $(".RightsTable .ConstructionList tr[data-id='" + OperationID + "']").closest("div").replaceWith($Div);
            var $Span = $(".RightsTable tr[data-id='" + OperationID + "'] td:eq(0) span")[0].outerHTML;
            $(".RightsTable tr[data-id='" + OperationID + "'] td:eq(0)").html('').html($Span + $("#MaterialTypeID option:selected").text());
            $(".RightsTable tr[data-id='" + OperationID + "'] td:eq(1)").html('').html($Span + ($("#ConstructionTypeID").val() > 0 ? $("#ConstructionTypeID option:selected").text() : '-'));
            $(".RightsTable tr[data-id='" + OperationID + "'] td:eq(2)").html('').html($Span + GlobalFormat(PropertyConstructionModel.BuildingArea));
            $(".RightsTable tr[data-id='" + OperationID + "'] td:eq(3)").html('').html($Span + GlobalFormatWithText(GlobalConvertToDecimal($("#UnitValue").val())));
            $(".RightsTable tr[data-id='" + OperationID + "'] td:eq(4)").html('').html($Span + GlobalFormatWithText(GlobalConvertToDecimal($("#TotalValue").val())));
        }

        if (mode == "new") {
            PropertyConstructionModel.ConstructionTypeID = ($("#ConstructionTypeID").val() == "" ? null : $("#ConstructionTypeID").val());
            PropertyConstructionModel.MaterialTypeID = $("#MaterialTypeID").val();
            PropertyConstructionModel.ConstructionYear = 0;
            PropertyConstructionModel.TotalYears = $("#TotalYears").val();
            PropertyConstructionModel.StatusID = ($("#StatusID").val() == "" ? null : $("#StatusID").val());
            PropertyConstructionModel.Floors = $("#Floors").val();
            PropertyConstructionModel.Chambers = $("#Chambers").val();
            PropertyConstructionModel.InternalWallsID = ($("#InternalWallsID").val() == "" ? null : $("#InternalWallsID").val());
            PropertyConstructionModel.ExternalWallsID = ($("#ExternalWallsID").val() == "" ? null : $("#ExternalWallsID").val());
            PropertyConstructionModel.StructureID = ($("#StructureID").val() == "" ? null : $("#StructureID").val());
            PropertyConstructionModel.RoofID = ($("#RoofID").val() == "" ? null : $("#RoofID").val());
            PropertyConstructionModel.CeilingID = ($("#CeilingID").val() == "" ? null : $("#CeilingID").val());
            PropertyConstructionModel.FloorID = ($("#FloorID").val() == "" ? null : $("#FloorID").val());
            PropertyConstructionModel.Bathroom = ($("#Bathroom").val() == "" ? "" : GlobalConvertToDecimal($("#Bathroom").val()));
            PropertyConstructionModel.UsefulLife = ($("#UsefulLife").val() == "" ? "-" : $("#UsefulLife").val());
            PropertyConstructionModel.BuildingArea = GlobalConvertToDecimal($("#BuildingArea").val());
            PropertyConstructionModel.UnitValue = GlobalConvertToDecimal($("#UnitValue").val());
            PropertyConstructionModel.TotalValue = GlobalConvertToDecimal($("#TotalValue").val());
            PropertyConstructionModel.IsActive = true;
            PropertyConstructionModel.IsDeleted = false;
            PropertyConstructionModel.ID = $("#frmGeneralDescription #ID").val() || 0;
            ConstructionID = ConstructionID + 1;
            PropertyConstructionModel._ID = ConstructionID;
            OperationID = ConstructionID;
            var Type = PropertyConstructionModel.ID > 0 ? "old" : "new";
            PropertyConstructionList.push(PropertyConstructionModel);
            var IsAdded = true;
            if (IsAdded == true) {

                var $TrRow = '<tr data-id="' + OperationID + '">\
                                <td>'+ $("#MaterialTypeID option:selected").text() + '</td>\
                                <td>'+ ($("#ConstructionTypeID").val() > 0 ? $("#ConstructionTypeID option:selected").text() : '-') + '</td>\
                                <td class="text-right">'+ GlobalFormat(PropertyConstructionModel.BuildingArea) + '</td>\
                                <td class="text-right">'+ GlobalFormatWithText(GlobalConvertToDecimal($("#UnitValue").val())) + '</td>\
                                <td class="text-right">'+ GlobalFormatWithText(GlobalConvertToDecimal($("#TotalValue").val())) + '</td>\
                                <td class="text-right">\
                                    <a class="btn btn-white btn-sm btnCopyConstruction" data-type="'+ Type + '" data-id="' + OperationID + '"><i class="fa fa-copy"></i></a>\
                                    <a class="btn btn-white btn-sm btnEditConstruction" data-type="'+ Type + '" data-id="' + OperationID + '"><i class="fa fa-pencil"></i></a>\
                                    <a class="btn btn-white btn-sm btnDeleteConstruction" data-type="'+ Type + '" data-id="' + OperationID + '"><i class="fa fa-trash"></i></a>\
                                </td>\
                                <td class="ConstructionList"> </td>\
                            </tr > ';

                $(".RightsTable tbody:first").append($TrRow);

                $Div = '<div class="row1 border-bottom">\
                        <table width ="100%">\
                            <tbody>\
                                <tr data-id="'+ OperationID + '">\
                                    <td class="col-lg-10">\
                                        <table class="table no-borders">\
                                            <tbody>\
                                                <tr>\
                                                    <td class="control-label text-right"><label class="control-label">'+ _Type + ' :</label></td>\
                                                    <td class="text-left">'+ $("#MaterialTypeID option:selected").text() + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ CurrentAge + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#TotalYears").val() > 0 ? $("#TotalYears").val() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Status + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#StatusID").val() > 0 ? $("#StatusID option:selected").text() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ UsefulLife + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#UsefulLife").val() == "" ? "-" : $("#UsefulLife").val()) + '</td>\
                                                </tr>\
                                                <tr>\
                                                    <td class="control-label text-right"><label class="control-label">'+ NoFloors + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#Floors").val() > 0 ? $("#Floors").val() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ NoChambers + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#Chambers").val() > 0 ? $("#Chambers").val() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ InternalWalls + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#InternalWallsID").val() > 0 ? $("#InternalWallsID option:selected").text() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ ExternalWalls + ' :</label></td>\
                                                    <td class="text-left">'+ ($("#ExternalWallsID").val() > 0 ? $("#ExternalWallsID option:selected").text() : '-') + '</td>\
                                                </tr>\
                                                <tr>\
                                                    <td class="control-label text-right" > <label class="control-label">'+ Structure + '  :</label></td >\
                                                    <td class="text-left">'+ ($("#StructureID").val() > 0 ? $("#StructureID option:selected").text() : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Roof + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#RoofID").val() > 0 ? $("#RoofID option:selected").text() : '-') + ' </td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Ceiling + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#CeilingID").val() > 0 ? $("#CeilingID option:selected").text() : '-') + ' </td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Floors + '  :</label></td>\
                                                    <td class="text-left">'+ ($("#FloorID").val() > 0 ? $("#FloorID option:selected").text() : '-') + '</td>\
                                                </tr>\
                                                <tr>\
                                                    <td class="control-label text-right"><label class="control-label">'+ Bathroom + '  :</label></td>\
                                                    <td class="text-left">'+ (PropertyConstructionModel.Bathroom > 0 ? PropertyConstructionModel.Bathroom : '-') + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ BuildingArea + ' :</label></td>\
                                                    <td class="text-left">'+ GlobalFormat(PropertyConstructionModel.BuildingArea) + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ UnitValue + ' :</label></td>\
                                                    <td class="text-left">'+ GlobalFormatWithText(GlobalConvertToDecimal($("#UnitValue").val())) + '</td>\
                                                    <td class="control-label text-right"><label class="control-label">'+ TotalValue + '  :</label></td>\
                                                    <td class="text-left">'+ GlobalFormatWithText(GlobalConvertToDecimal($("#TotalValue").val())) + '</td>\
                                                </tr>\
                                            </tbody>\
                                        </table>\
                                    </td>\
                                </tr>\
                            </tbody>\
                        </table >\
                      </div >';

                $(".ConstructionList", $(".RightsTable tr[data-id='" + OperationID + "']")).append($Div);
            }
        }

        $('.footable').footable();
        $("#GeneralDescriptionConstructionmodal").modal('hide');
    }
    else
        return false;
});

$(".RightsTable").on("click", ".btnDeleteConstruction", function () {
    var Id = $(this).attr('data-id');
    var Type = $(this).attr('data-type');

    PropertyConstructionList = $.grep(PropertyConstructionList, function (data, dataIndex) {
        if (Type == "new") {
            return data._ID != Id;
        }
        else {
            if (data.ID == Id) {
                data.IsDeleted = true;
                return true;
            }
            return true;
        }
    });

    $(this).closest('div').closest('tr').remove();
    $(".RightsTable tr[data-id='" + Id + "']").remove();
    $(".footable-row-detail").remove();
    $(".footable-visible").removeClass("footable-first-column");
    $(".footable-visible").removeClass("footable-last-column");
    $(".footable-visible").removeClass("footable-visible");
    $(".RightsTable").removeClass("footable table table-stripped toggle-arrow-tiny tablet breakpoint no-paging footable-loaded").addClass("footable table table-stripped toggle-arrow-tiny")
    $(".RightsTable thead td:last,.RightsTable tbody td:last").show();
    $('.footable').footable();
});

$("#divGeneralDescription").on("change", "#BuildingArea,#UnitValue", function () {
    if ($("#BuildingArea").val() != "" && GlobalConvertToDecimal($("#BuildingArea").val()) != NaN && $("#UnitValue").val() != "" && GlobalConvertToDecimal($("#UnitValue").val()) != NaN) {
        $("#TotalValue").val(GlobalFormat(GlobalConvertToDecimal($("#BuildingArea").val()) * GlobalConvertToDecimal($("#UnitValue").val())))
    }
    else {
        $("#TotalValue").val('');
    }
});

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
    $(".lblLatitude").html('');
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
    var Type = $(this).attr('data-type');
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
                $("#GeneralDescriptionConstructionmodal").modal('show');
                $("#GeneralDescriptionConstructionmodal").attr('data-mode', 'edit');
                $("#divGeneralDescription .select2dropdown").select2({ width: '100%' });
                $('#frmGeneralDescription').validate();
                $('#MaterialTypeID').select2('open').select2('close');

                $.grep(PropertyConstructionList, function (data, dataIndex) {
                    if ((Type == "new" && data._ID == Id) || (data.ID == Id)) {
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
                        $("#Bathroom").val(data.Bathroom == "" ? "" : GlobalFormat(data.Bathroom));
                        $("#UsefulLife").val(data.UsefulLife == "-" ? "" : data.UsefulLife);
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
        }, error: function () {
        }
    }).always(function () {
    });
});

$(".RightsTable").on("click", ".btnCopyConstruction", function () {
    var Id = $(this).attr('data-id');
    var Type = $(this).attr('data-type');
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

                $('#frmGeneralDescription #addresstitle').html(CopyConstructionDescription);
                $("#btnAddConstruction").html(Copy);

                $.validator.unobtrusive.parse('#frmGeneralDescription');
                $("#GeneralDescriptionConstructionmodal").modal('show');
                $("#GeneralDescriptionConstructionmodal").attr('data-mode', 'new');
                $("#divGeneralDescription .select2dropdown").select2({ width: '100%' });
                $('#frmGeneralDescription').validate();
                $('#MaterialTypeID').select2('open').select2('close');

                $.grep(PropertyConstructionList, function (data, dataIndex) {
                    if ((Type == "new" && data._ID == Id) || (data.ID == Id)) {
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
                        $("#Bathroom").val(data.Bathroom == "" ? "" : GlobalFormat(data.Bathroom));
                        $("#UsefulLife").val(data.UsefulLife == "-" ? "" : data.UsefulLife);
                        $("#BuildingArea").val(GlobalFormat(data.BuildingArea)).trigger('change');
                        $("#UnitValue").val(GlobalFormat(data.UnitValue));
                        $("#TotalValue").val(GlobalFormat(data.TotalValue));
                        $("#IsDistributed").prop("checked", data.IsDistributed);
                        $("#IndexID").val(data._ID);
                    }
                });
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

$(document).on("click", "#btnViewHistory", function () {
    var $this = $(this);
    var timezone = /\((.*)\)/.exec(new Date().toString())[1];
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountProperty/GetFillingPropertyTaxHistory",
        data: { PropertyAccountID: $("#ID").val(), PropertyNumber: $("#PropertyNumber").val(), RightNumber: $("#RigthNumber").val(), TimeZone: timezone },
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {

                $("#divFillingPropertyTaxHistory").html(data.returnData);
                $("#TxtRightCode").val($("#RigthNumber").val());
                $("#FillingPropertyTaxHistory").modal('show');
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

$(document).on('change', '#ddlCountry', function () {
    GetStatesByCountry();
});

$(document).on('change', '#ddlState', function () {
    GetCitiesByCountryAndState();
});
$(document).on('change', '#ddlCity', function () {
    GetTownsByCountryStateAndCity();
});

function GetStatesByCountry() {
    if (!$('#ddlCountry').val()) {
        $("#ddlState").empty();
        $("#ddlCity").empty();
        $("#ddlTown").empty();
        $("#ddlState").append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
        $('#ddlState').val(null).trigger('change');
        $("#ddlCity").append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
        $('#ddlCity').val(null).trigger('change');
        $("#ddlTown").append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
        $('#ddlTown').val(null).trigger('change');
    }
    else {
        $.ajax({
            url: ROOTPath + "/Account/GetStatesByCountry?countryId=" + $('#ddlCountry').val(),
            success: function (data) {
                $("#ddlState").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlState").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlState option:first").val("");
                $('#ddlState').val($("#ddlState option:first").val()).trigger('change');
            }
        });
    }
}

function GetCitiesByCountryAndState() {
    if (!$('#ddlState').val()) {
        $("#ddlCity").empty();
        $("#ddlTown").empty();
        $("#ddlCity").append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
        $('#ddlCity').val(null).trigger('change');
        $("#ddlTown").append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
        $('#ddlTown').val(null).trigger('change');
    }
    else {
        $.ajax({
            url: ROOTPath + "/Account/GetCitiesByCountryAndState?countryId=" + $('#ddlCountry').val() + "&stateId=" + $('#ddlState').val(),
            success: function (data) {
                $("#ddlCity").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlCity").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlCity option:first").val("");
                $('#ddlCity').val($("#ddlCity option:first").val()).trigger('change');
            }
        });
    }
}

function GetTownsByCountryStateAndCity() {

    if (!$('#ddlCity').val()) {
        $("#ddlTown").empty();
        $("#ddlTown").append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
        $('#ddlTown').val(null).trigger('change');
    }
    else {
        $.ajax({
            url: ROOTPath + "/Account/GetTowndByCountryStateAndCity?countryId=" + $('#ddlCountry').val() + "&stateId=" + $('#ddlState').val() + "&cityId=" + $('#ddlCity').val(),
            success: function (data) {
                $("#ddlTown").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlTown").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlTown option:first").val("");
                $('#ddlTown').val($("#ddlTown option:first").val()).trigger('change');
            }
        });
    }
}

//CO-1166
$(document).on('change', '#Services1', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#Services1 option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#Services2', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#Services2 option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
//