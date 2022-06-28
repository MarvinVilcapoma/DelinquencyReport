var emailList = [];
var emailcount = 1;
var phoneList = [];
var phonecount = 1;
var addresslist = [];
var addresscount = 1;
var contactlist = [];
var contactcount = 1;
var attachmentlist = [];
var attachmentcount = 1;
var attachmentDropzone;

Dropzone.autoDiscover = false;
var Uploadedfilemaxsizemessage = uploadmaxsizemsg;

$(function () {
    $(".select2dropdown").not('#ddlParent').select2({ width: '100%' });
});

function UserSuccessCallback(response) {
    if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        resolveRedirectURL(response.actionType, response.parentAccountId, response.parentAccountType);
    }
}

function resolveRedirectURL(actionType, parentAccountId, parentAccountType) {
    if (actionType == 1 || actionType == 3) // Save OR Cancle
    {
        if (parentAccountType > 0)
            window.location = ROOTPath + "/Account/View?AccountId=" + parentAccountId + "&accountType=" + parentAccountType + "&isBusiness=" + true;
        else
            window.location = ROOTPath + "/Account/List";
    }
    else if (actionType == 2)// Save & Add New
    {
        if (parentAccountType > 0)
            window.location = ROOTPath + "/Account/Add?AccountType=" + $("#AccountTypeID").val() + "&accountId=" + parentAccountId + "&parentAccountType=" + parentAccountType + "&isBusiness=" + true;
        else
            window.location = ROOTPath + "/Account/Add?AccountType=" + $("#AccountTypeID").val();
    }
}

$('#ddlParent').on("select2:unselect", function (e) {
    $(this).empty();
    $(this).append("<option value='" + '' + "'>" + dropDownSelectMessage + "</option>");
}).trigger('change');

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

$('#ddlIDType').on('change', function () {
    var seldropDescription = $('#ddlIDType :selected').attr('data-Identifica');
    var selectedValue = $('#ddlIDType').val();
    $('#BanacioIdentifica').prop('readonly',true);
    if (selectedValue != '') {
        $('#BanacioIdentifica').val(seldropDescription);
        $('#GeneralIndividualModel_IDTypeID').val(selectedValue);
    } else {
        $('#BanacioIdentifica').val('');
        $('#GeneralIndividualModel_IDTypeID').val(0);
    }
});

$(document).ready(function () {
    GetAccountForSelect('ddlParent', null, '4', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);

    $("#collapsereginfo").addClass("panel-collapse collapse in");

    $('.initialDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });

    $('.initialDate').datepicker('update', new Date());

    $(document).on('click', '#btnAddEmail', function () {
        if ($('#frmEmail').valid()) {
            var emailModel = {};
            emailModel.SequenceID = emailcount++;
            emailModel.Email = $("#txtEmail").val();
            emailModel.EmailTypeID = $("#ddlEmailType").val();
            emailModel.EmailTypeTableID = 7;
            emailModel.IsPrimary = false;
            emailModel.IsDeleted = false;
            emailModel.IsActive = true;
            emailList.push(emailModel);
            $("#emailmodel").modal('hide');
        }
        else
            $('#txtEmail').focus();
    });

    $(document).on('click', '#btnAddPhoneNumber', function () {
        if ($('#frmPhone').valid()) {
            var phoneModel = {};
            phoneModel.SequenceID = phonecount++;
            phoneModel.PhoneNumber = $("#txtPhoneNumber").val();
            phoneModel.PhoneTypeID = $("#ddlPhoneType").val();
            phoneModel.PhoneTypeTableID = (($("#AccountTypeID").val() == 4) ? 5 : 38);
            phoneModel.IsWorkPrimary = false;
            phoneModel.IsMobilePrimary = false;
            phoneModel.IsActive = true;
            phoneModel.IsDeleted = false;
            phoneList.push(phoneModel);
            $("#phonemodel").modal('hide');
        }
        else
            $('#txtPhoneNumber').focus();
    });

    $(document).on('click', '#btnAddAddress', function () {

        // CO-1017
        if (
            $("#txtAddressLine1").val() == "" && $("#txtAddressLine2").val() == "" &&
            $("#ddlCountry").val() == "" && $("#ddlState").val() == "" &&
            $("#ddlCity").val() == "" && $("#ddlTown").val() == ""
        ) {
            $("#addressmodal").modal('hide');
            return false;
        }
        //

        if ($('#frmAddress').valid()) {
            var AddressModel = {};
            AddressModel.SequenceID = addresscount++;
            AddressModel.AddressTypeID = $("#ddlAddressType").val() > 0 ? $("#ddlAddressType").val() : null;
            AddressModel.AddressTypeTableID = $("#ddlAddressType").val() > 0 ? 6 : null;
            AddressModel.AddressType = $("#ddlAddressType").val() > 0 ? $("#ddlAddressType option:selected").text() : null;
            AddressModel.CountryID = $("#ddlCountry").val() > 0 ? $("#ddlCountry").val() : null;
            AddressModel.CountryStateID = $("#ddlState").val() > 0 ? $("#ddlState").val() : null;
            AddressModel.CityID = $("#ddlCity").val() > 0 ? $("#ddlCity").val() : null;
            AddressModel.TownID = $("#ddlTown").val() > 0 ? $("#ddlTown").val() : null;
            AddressModel.City = $("#ddlCity").val() > 0 ? $("#ddlCity option:selected").text() : null;
            AddressModel.Town = $("#ddlTown").val() > 0 ? $("#ddlTown option:selected").text() : null;
            AddressModel.AddressLine1 = $("#txtAddressLine1").val();
            AddressModel.AddressLine2 = $("#txtAddressLine2").val();
            AddressModel.ZipPostalCode = $("#txtZipCode").val();
            AddressModel.IsActive = true;

            if (addresslist.length == 0)
                AddressModel.IsPrimary = true;
            else {
                AddressModel.IsPrimary = $("#chkIsAddressPrimary").prop('checked');

                if (AddressModel.IsPrimary == true) {
                    $.each(addresslist, function (key, value) {
                        addresslist[key].IsPrimary = false;
                    });
                }
            }
            addresslist.push(AddressModel);

            $.ajax({
                type: "POST",
                async: false,
                url: ROOTPath + "/Account/AddAddress",
                data: { 'addressList': addresslist },
                success: function (data) {
                    $("#divAddress").html(data);
                    $("#addressmodal").modal('hide');
                }, error: function () { }
            }).always(function () {
            });
        }
        else
            $("#txtAddressLine1").focus();
    });

    $(document).on('click', '#btnAddContact', function () {

        // CO-1017
        if (
            $("#txtContactFirstName").val() == "" && $("#ddlContactPosition").val() == "" && $("#txtContactPrimaryEmail").val() == "" &&
            $("#txtContactOtherEmail").val() == "" && $("#txtContactPrimaryPhone").val() == "" && $("#txtContactOtherPhone").val() == ""
        ) {
            $("#contactmodel").modal('hide');
            return false;
        }
        //

        if ($('#frmContacts').valid()) {
            var contactModel = {};
            contactModel.SequenceID = contactcount++;
            contactModel.SalutationID = $("#ddlContactSalution").val() > 0 ? $("#ddlContactSalution").val() : null;
            contactModel.SalutationTableID = $("#ddlContactSalution").val() > 0 ? 2 : null;
            contactModel.FirstName = $("#txtContactFirstName").val();
            contactModel.MiddleName = $("#txtContactMiddleName").val();
            contactModel.LastName = $("#txtContactLastName").val();
            contactModel.PositionID = $("#ddlContactPosition").val() > 0 ? $("#ddlContactPosition").val() : null;
            contactModel.PositionTableID = $("#ddlContactPosition").val() > 0 ? 4 : null;
            contactModel.PositionType = $("#ddlContactPosition").val() > 0 ? $("#ddlContactPosition option:selected").text() : null;
            contactModel.PrimaryEmail = $("#txtContactPrimaryEmail").val();
            contactModel.OtherEmail = $("#txtContactOtherEmail").val();
            contactModel.PrimaryPhoneNumber = $("#txtContactPrimaryPhone").val();
            contactModel.OtherPhoneNumber = $("#txtContactOtherPhone").val();
            contactModel.IsActive = true;
            contactlist.push(contactModel);

            $.ajax({
                type: "POST",
                async: false,
                url: ROOTPath + "/Account/AddContact",
                data: { 'contactList': contactlist },
                success: function (data) {
                    $("#divContact").html(data);
                    $("#contactmodel").modal('hide');
                }, error: function () { }
            }).always(function () {
            });
        }
        else
            $('#txtContactFirstName').focus();
    });

    $('#attachmentDropzone').dropzone({
        url: ROOTPath + "/File/UploadFile",
        autoProcessQueue: true,
        addRemoveLinks: true,
        maxFiles: MaxFilesToUpload,
        maxFilesize: maxFileLength,
        acceptedFiles: allowedFileTypes,
        parallelUploads: 1,
        uploadMultiple: false,
        dictRemoveFile: removefile,
        dictDefaultMessage: dropzonemessage,
        init: function () {
            attachmentDropzone = this;
            this.on("addedfile", function (file) {
                if (this.files.length > this.options.maxFiles) {
                    showAlert("warning", MaxLimitMessage);
                    this.removeFile(file);
                    return false;
                }
                if (file.size > this.options.maxFilesize) {
                    showAlert("warning", Uploadedfilemaxsizemessage + file.name);
                    this.removeFile(file);
                    return false;
                }
                if (allowedFileTypes.indexOf(file.type) < 0) {
                    showAlert("warning", uploadfiletypemsg);
                    this.removeFile(file);
                    return false;
                }
            });
            this.on("removedfile", function (file) {
                RemoveAttachmentRow(file.fileID);
            });
        },
        success: function (file, response) {
            if (response.id <= 0) {
                showAlert("error", response.message);
                this.removeFile(file);
                return false;
            }
            else {
                file.fileID = response.id;
                AddAttachmentRow(response.id, file.name);
            }
        }
    });

    var accountType = getUrlVars()["accountType"];
    if (accountType != '') {
        if (accountType == '4') {
            $('#BanacioIdentifica').prop('readonly', true);
        }
    }
});

// save account detail
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
        var finalemaillist = JSON.parse(JSON.stringify(emailList));//Array.from(emailList);
        if ($("#txtPrimaryEmail").val() != "") {
            var emailModel = {};
            emailModel.SequenceID = emailcount++;
            emailModel.Email = $("#txtPrimaryEmail").val();
            emailModel.EmailTypeID = 1;
            emailModel.EmailTypeTableID = 7;
            emailModel.IsPrimary = true;
            emailModel.IsDeleted = false;
            emailModel.IsActive = true;
            finalemaillist.push(emailModel);
        }

        var finalphonelist = JSON.parse(JSON.stringify(phoneList));//Array.from(phoneList);
        if ($("#txtPrimaryWorkPhone").val() != "") {
            var phoneModel = {};
            phoneModel.SequenceID = phonecount++;
            phoneModel.PhoneNumber = $("#txtPrimaryWorkPhone").val();
            phoneModel.PhoneTypeID = 1;
            phoneModel.PhoneTypeTableID = (($("#AccountTypeID").val() == 4) ? 5 : 38);
            phoneModel.IsWorkPrimary = true;
            phoneModel.IsActive = true;
            phoneModel.IsDeleted = false;
            finalphonelist.push(phoneModel);
        }
        if ($("#txtPrimaryMobile").val() != "") {
            var phoneModel = {};
            phoneModel.SequenceID = phonecount++;
            phoneModel.PhoneNumber = $("#txtPrimaryMobile").val();
            phoneModel.PhoneTypeID = 2;
            phoneModel.PhoneTypeTableID = (($("#AccountTypeID").val() == 4) ? 5 : 38);
            phoneModel.IsMobilePrimary = true;
            phoneModel.IsActive = true;
            phoneModel.IsDeleted = false;
            finalphonelist.push(phoneModel);
        }
        if (finalemaillist.length > 0)
            UpdateSequenceID(finalemaillist);
        $("#EmailJson").val(JSON.stringify(finalemaillist));

        if (finalphonelist.length > 0)
            UpdateSequenceID(finalphonelist);
        $("#PhoneJson").val(JSON.stringify(finalphonelist));

        if (addresslist.length == 1)
            addresslist[0].IsPrimary = true;

        if (addresslist.length > 0)
            UpdateSequenceID(addresslist);
        $("#AddressJson").val(JSON.stringify(addresslist));

        if (contactlist.length > 0)
            UpdateSequenceID(contactlist);
        $("#ContactJson").val(JSON.stringify(contactlist));

        if (attachmentlist.length > 0)
            UpdateSequenceID(attachmentlist, 'file');
        $("#FileJson").val(JSON.stringify(attachmentlist));

        if ($("#ddlCurrency").select2('val') > 0) {
            GetCurrencyISOCode();
        }
    }
}

function UpdateSequenceID(arr, data) {
    var count = 0;
    $.each(arr, function (key, value) {
        if (data == 'file')
            arr[key].ID = ++count;
        else
            arr[key].SequenceID = ++count;
    });
}

function ShowAccordionPanel(id) {
    id = "#" + id;
    $('.collapsepanel:not(' + id + ')').collapse("hide");
    $(id).collapse("show");
}

$('.modal').on('hidden.bs.modal', function (e) {
    $('#txtFirstName').focus();
    $('#txtLegalName').focus();
});

//email 
$('#emailmodel').on('shown.bs.modal', function () {
    $('#txtEmail').focus();
});

$('#addEmail').click(function (e) {
    ClearEmail();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Account/NewEmail",
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divNewEmail").html(data);
                $(".select2dropdown").not('#ddlParent').select2({ width: '100%' });
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

function ClearEmail() {
    $("#txtEmail").val('');
}

//phone
$('#phonemodel').on('shown.bs.modal', function () {
    $('#txtPhoneNumber').focus();
});

$('#addPhoneNumber').click(function (e) {
    ClearPhone();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Account/NewPhone?AccountTypeID=" + $("#AccountTypeID").val(),
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divNewPhone").html(data);
                $(".select2dropdown").not('#ddlParent').select2({ width: '100%' });
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

function ClearPhone() {
    $("#txtPhoneNumber").val('');
}

//address
$('#addressmodal').on('shown.bs.modal', function () {
    $('#txtAddressLine1').focus();
});

$('#addAddress').click(function (e) {
    ClearAddress();
    ShowAccordionPanel('collapseaddress');

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Account/NewAddress",
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divNewAddress").html(data);
                $(".select2dropdown").not('#ddlParent').select2({ width: '100%' });
            }

        }, error: function () {
        }
    }).always(function () {
    });
});

function DeleteAddress(data, SequenceID) {
    var newaddresslist = addresslist.filter(function (el) {
        return el.SequenceID !== SequenceID;
    });
    addresslist = newaddresslist;
    data.closest('tr').remove();
}

function ClearAddress() {
    $("#txtAddressLine1").val('');
    $("#txtAddressLine2").val('');
    $("#txtZipCode").val('');
    $("#chkIsAddressPrimary").val('');
}

$('#collapseaddress').on('shown.bs.collapse', function () {
    ShowAccordionPanel('collapseaddress');
});

//contact
$('#contactmodel').on('shown.bs.modal', function () {
    $('#txtContactFirstName').focus();
});

$('#addContact').click(function (e) {
    ClearContact();
    ShowAccordionPanel('collapsecontact');

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Account/NewContact",
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divNewContact").html(data);
                $(".select2dropdown").not('#ddlParent').select2({ width: '100%' });
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

function DeleteContact(data, SequenceID) {
    var newcontactlist = contactlist.filter(function (el) {
        return el.SequenceID !== SequenceID;
    });
    contactlist = newcontactlist;
    data.closest('tr').remove();
}

function ClearContact() {
    $("#txtContactFirstName").val('');
    $("#txtContactMiddleName").val('');
    $("#txtContactLastName").val('');
    $("#txtContactPrimaryEmail").val('');
    $("#txtContactOtherEmail").val('');
    $("#txtContactPrimaryPhone").val('');
    $("#txtContactOtherPhone").val('');
}

$('#collapsecontact').on('shown.bs.collapse', function () {
    ShowAccordionPanel('collapsecontact');
});

//attachement
$('#attachementmodel').on('hidden.bs.modal', function () {
    $("#tblAttachmentRow").empty();
    $("#tblAttachmentHeadRow").hide();
    attachmentDropzone.removeAllFiles();
});

$('#addFile').click(function () {
    ClearFile();
    ShowAccordionPanel('collapseattachement');
});

$('#btnAddFile').click(function () {
    if ($('#frmAttachments').valid()) {
        $('#tblAttachmentRow tr').each(function () {
            attachmentModel = {};
            attachmentModel.ID = attachmentcount++;
            attachmentModel.FileID = $(this).find("#hdnFileID").val();
            attachmentModel.FileTypeID = $(this).find("#ddlFileType").val();
            attachmentModel.FileTypeTableID = 10;
            attachmentModel.FileType = $(this).find("#ddlFileType option:selected").text();
            attachmentModel.Name = $(this).find(".txtFileName").val();
            attachmentModel.Description = $(this).find("#lblFileNameWithExtension").html();
            attachmentModel.IsActive = true;
            attachmentModel.IsDeleted = false;
            attachmentlist.push(attachmentModel);
        });

        $.ajax({
            type: "POST",
            async: false,
            url: ROOTPath + "/Account/AddAttachment",
            data: { 'attachmentList': attachmentlist },
            success: function (data) {
                $("#divFile").html(data);
                $("#attachementmodel").modal('hide');
            }, error: function () {
            }
        }).always(function () {
        });
    }
});

function ClearFile() {
    $("#tblAttachmentRow").empty();
    $("#tblAttachmentHeadRow").hide();
    attachmentDropzone.removeAllFiles();
}

function DeleteAttachment(data, fileid) {
    var newattachmentlist = attachmentlist.filter(function (el) {
        return el.FileID !== fileid;
    });
    attachmentlist = newattachmentlist;
    data.closest('tr').remove();
}

var attachmentseq = 1;
function AddAttachmentRow(fileID, fileName) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        url: ROOTPath + '/Account/AddAttchmentRow',
        data: JSON.stringify({ 'attachmentID': fileID, 'attachmentName': fileName }),
        success: function (response) {
            $("#tblAttachmentHeadRow").show();
            $('#tblAttachmentRow').append(response);
            $('#tblAttachmentRow tr:last').find("#txtFileName").attr("id", "txtFileName" + attachmentseq);
            $('#tblAttachmentRow tr:last').find("#txtFileName" + attachmentseq).attr("name", "txtFileName" + attachmentseq);
            $('#tblAttachmentRow tr:last').find("#valFileName").attr("data-valmsg-for", "txtFileName" + attachmentseq);
            attachmentseq++;

            if ($("#tblAttachement > tbody > tr").length > 0)
                $("#btnAddFile").prop("disabled", false);
            $(".select2dropdown").not('#ddlParent').select2({ width: '100%' });
            $("#btnAddFile").focus();
        }
    });
    return false;
}

function RemoveAttachmentRow(fileID) {
    $('#tblAttachmentRow').find("tr[id='" + fileID + "']").remove();

    if ($("#tblAttachement > tbody > tr").length == 0)
        $("#btnAddFile").prop("disabled", true);
}

$('#collapseattachement').on('shown.bs.collapse', function () {
    ShowAccordionPanel('collapseattachement');
});

function IndividualDisplayName() {
    $('#txtDispalyName').val(($('#txtLastName').val() ? $('#txtLastName').val().trim() + " " : "") + ($('#txtSecondLastName').val() ? $('#txtSecondLastName').val() + " " : "") + ($('#txtFirstName').val() ? $('#txtFirstName').val().trim() + " " : "") + ($('#txtMiddleName').val() ? $('#txtMiddleName').val().trim() + " " : ""));
}

function BuisnessDisplayName() {
    $('#txtDispalyName').val($('#txtLegalName').val());
}

function GetCurrencyISOCode() {
    $('#CurrencyISOCode').val($("#ddlCurrency option:selected").text());
}

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
        $("#ddlState").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $('#ddlState').val(null).trigger('change');
        $("#ddlCity").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $('#ddlCity').val(null).trigger('change');
        $("#ddlTown").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
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
        $("#ddlCity").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $('#ddlCity').val(null).trigger('change');
        $("#ddlTown").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
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
        $("#ddlTown").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
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

function setOGPEDescription(source) {
    if ($(source).val() > 0) {
        var selectedNICS = $(source).select2('data');
        if (selectedNICS.length > 0)
            $('#txtARPEDescription').val(selectedNICS[0].text.substring(selectedNICS[0].id.length + 3, selectedNICS[0].text.length));
    }
}

function setCollectionTemplateLink() {
    if ($("#CollectionTemplateID").val() > 0)
        $("#lnktemplate").attr("href", ROOTPath + "/Services/CollectionTemplate/View/" + $("#CollectionTemplateID").val());
    else
        $("#lnktemplate").removeAttr("href");
}