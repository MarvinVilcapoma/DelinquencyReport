var emailList = [];
var emailcount = null;
var phoneList = [];
var phonecount = null;
var addresslist = [];
var addresscount = null;
var contactlist = [];
var contactcount = null;
var attachmentlist = [];
var attachmentcount = null;
var finalattachmentlist = [];
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
        if (response.parentAccountType > 0)
            window.location = ROOTPath + "/Account/View?AccountId=" + response.parentAccountId + "&accountType=" + response.parentAccountType + "&isBusiness=" + true;
        else
            window.location = ROOTPath + "/Account/List";
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

    emailList = $.parseJSON($('#EmailJson').val());
    phoneList = $.parseJSON($('#PhoneJson').val());
    addresslist = $.parseJSON($('#AddressJson').val());
    contactlist = $.parseJSON($('#ContactJson').val());
    attachmentlist = $.parseJSON($('#FileJson').val());

    //Get maximum sequence id and set next sequence for new record (Issue Raise due to duplicate Sequence ID) - CO-1112
    emailcount = emailList.length > 0 ? (Math.max.apply(Math, emailList.map(function (o) { return o.SequenceID; })) + 1) : 1;
    phonecount = phoneList.length > 0 ? (Math.max.apply(Math, phoneList.map(function (o) { return o.SequenceID; })) + 1) : 1;
    addresscount = addresslist.length > 0 ? (Math.max.apply(Math, addresslist.map(function (o) { return o.SequenceID; })) + 1) : 1;
    contactcount = contactlist.length > 0 ? (Math.max.apply(Math, contactlist.map(function (o) { return o.SequenceID; })) + 1) : 1;
    attachmentcount = attachmentlist.length > 0 ? (Math.max.apply(Math, attachmentlist.map(function (o) { return o.SequenceID; })) + 1) : 1;
    //

    $.each(attachmentlist, function (key, value) {
        if (attachmentlist[key].IsDeleted == 0)
            attachmentlist[key].IsDeleted = -1;
    });
    finalattachmentlist = JSON.parse(JSON.stringify(attachmentlist));

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

    $('.initialDate').datepicker('update', $('#InitialDate').val());

    $(document).on('click', '#btnAddEmail', function () {
        if ($('#frmEmail').valid()) {
            if ($('#btnAddEmail').attr('data-form-type') == 1) {
                var emailModel = {};
                emailModel.ID = 0;
                emailModel.SequenceID = emailcount++;
                emailModel.Email = $("#txtEmail").val();
                emailModel.EmailTypeID = $("#ddlEmailType").val();
                emailModel.EmailTypeTableID = 7;
                emailModel.Type = $("#ddlEmailType option:selected").text();
                emailModel.IsPrimary = false;
                emailModel.IsDeleted = false;
                emailModel.IsActive = true;
                emailList.push(emailModel);
            }
            if ($('#btnAddEmail').attr('data-form-type') == 2) {
                var result = $.grep(emailList, function (e) { return e.ID == $('#emailId').val(); });
                result[0].EmailTypeID = $("#ddlEmailType").val();
                result[0].Type = $("#ddlEmailType option:selected").text();
                result[0].Email = $("#txtEmail").val();
            }

            $.ajax({
                type: "POST",
                async: false,
                url: ROOTPath + "/Account/AddEmail",
                data: { 'emailList': emailList },
                success: function (data) {
                    $("#divEmail").html(data);
                    $('#btnAddEmail').attr('data-form-type', 1);
                    $("#emailmodel").modal('hide');
                }, error: function () { }
            }).always(function () {
            });
        }
        else
            $('#txtEmail').focus();
    });

    $(document).on('click', '#btnAddPhoneNumber', function () {
        if ($('#frmPhone').valid()) {
            if ($('#btnAddPhoneNumber').attr('data-form-type') == 1) {
                var phoneModel = {};
                phoneModel.ID = 0;
                phoneModel.SequenceID = phonecount++;
                phoneModel.PhoneNumber = $("#txtPhoneNumber").val();
                phoneModel.PhoneTypeID = $("#ddlPhoneType").val();
                phoneModel.PhoneType = $("#ddlPhoneType option:selected").text();
                phoneModel.PhoneTypeTableID = (($("#AccountTypeID").val() == 4) ? 5 : 38);
                phoneModel.IsWorkPrimary = false;
                phoneModel.IsMobilePrimary = false;
                phoneModel.IsActive = true;
                phoneModel.IsDeleted = false;
                phoneList.push(phoneModel);
            }
            if ($('#btnAddPhoneNumber').attr('data-form-type') == 2) {
                var result = $.grep(phoneList, function (e) { return e.ID == $('#phoneId').val(); });
                result[0].PhoneTypeID = $("#ddlPhoneType").val();
                result[0].PhoneType = $("#ddlPhoneType option:selected").text();
                result[0].PhoneNumber = $("#txtPhoneNumber").val();
            }

            $.ajax({
                type: "POST",
                async: false,
                url: ROOTPath + "/Account/AddPhone",
                data: { 'phoneList': phoneList },
                success: function (data) {
                    $("#divPhone").html(data);
                    $('#btnAddPhoneNumber').attr('data-form-type', 1);
                    $("#phonemodel").modal('hide');
                }, error: function () { }
            }).always(function () {
            });
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
            if ($('#btnAddAddress').attr('data-form-type') == 1) {
                $("#addressmodal").modal('hide');
            }
            if ($('#btnAddAddress').attr('data-form-type') == 2) {
                showAlert("error", deleteOrFillAddressDetailMsg);
            }
            return false;
        }
        //

        if ($('#frmAddress').valid()) {

            if ($("#chkIsAddressPrimary").prop('checked')) {
                $.each(addresslist, function (key, value) {
                    addresslist[key].IsPrimary = false;
                });
            }
            if ($('#btnAddAddress').attr('data-form-type') == 1) {
                var AddressModel = {};
                AddressModel.ID = 0;
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
                AddressModel.IsPrimary = $("#chkIsAddressPrimary").prop('checked');
                AddressModel.IsActive = true;
                addresslist.push(AddressModel);
            }
            if ($('#btnAddAddress').attr('data-form-type') == 2) {
                var result = $.grep(addresslist, function (e) { return e.ID == $('#addressId').val(); });
                result[0].AddressTypeID = $("#ddlAddressType").val() > 0 ? $("#ddlAddressType").val() : null;
                result[0].AddressTypeTableID = $("#ddlAddressType").val() > 0 ? 6 : null;
                result[0].AddressType = $("#ddlAddressType").val() > 0 ? $("#ddlAddressType option:selected").text() : null;
                result[0].CountryID = $("#ddlCountry").val() > 0 ? $("#ddlCountry").val() : null;
                result[0].CountryStateID = $("#ddlState").val() > 0 ? $("#ddlState").val() : null;
                result[0].CityID = $("#ddlCity").val() > 0 ? $("#ddlCity").val() : null;
                result[0].TownID = $("#ddlTown").val() > 0 ? $("#ddlTown").val() : null;
                result[0].AddressLine1 = $("#txtAddressLine1").val();
                result[0].AddressLine2 = $("#txtAddressLine2").val();
                result[0].ZipPostalCode = $("#txtZipCode").val();
                result[0].IsPrimary = $("#chkIsAddressPrimary").prop('checked');
            }

            $.ajax({
                type: "POST",
                async: false,
                url: ROOTPath + "/Account/AddAddress",
                data: { 'addressList': addresslist },
                success: function (data) {
                    $("#divAddress").html(data);
                    $('#btnAddAddress').attr('data-form-type', 1);
                    $("#addressmodal").modal('hide');
                }, error: function () { }
            }).always(function () {
            });
        }
        else
            $('#txtAddressLine1').focus();
    });

    $(document).on('click', '#btnAddContact', function () {
        // CO-1017
        if (
            $("#txtContactFirstName").val() == "" && $("#ddlContactPosition").val() == "" && $("#txtContactPrimaryEmail").val() == "" &&
            $("#txtContactOtherEmail").val() == "" && $("#txtContactPrimaryPhone").val() == "" && $("#txtContactOtherPhone").val() == ""
        ) {
            if ($('#btnAddContact').attr('data-form-type') == 1) {
                $("#contactmodel").modal('hide');
            }
            if ($('#btnAddContact').attr('data-form-type') == 2) {
                showAlert("error", deleteOrFillContactDetailMsg);
            }
            return false;
        }
        //

        if ($('#frmContacts').valid()) {

            if ($('#btnAddContact').attr('data-form-type') == 1) {
                var contactModel = {};
                contactModel.ID = 0;
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
            }
            if ($('#btnAddContact').attr('data-form-type') == 2) {
                var result = $.grep(contactlist, function (e) { return e.ID == $('#contactId').val(); });
                result[0].SalutationID = $("#ddlContactSalution").val() > 0 ? $("#ddlContactSalution").val() : null;
                result[0].SalutationTableID = $("#ddlContactSalution").val() > 0 ? 2 : null;
                result[0].FirstName = $("#txtContactFirstName").val();
                result[0].MiddleName = $("#txtContactMiddleName").val();
                result[0].LastName = $("#txtContactLastName").val();
                result[0].PositionID = $("#ddlContactPosition").val() > 0 ? $("#ddlContactPosition").val() : null;
                result[0].PositionTableID = $("#ddlContactPosition").val() > 0 ? 4 : null;
                result[0].PositionType = $("#ddlContactPosition").val() > 0 ? $("#ddlContactPosition option:selected").text() : null;
                result[0].PrimaryEmail = $("#txtContactPrimaryEmail").val();
                result[0].OtherEmail = $("#txtContactOtherEmail").val();
                result[0].PrimaryPhoneNumber = $("#txtContactPrimaryPhone").val();
                result[0].OtherPhoneNumber = $("#txtContactOtherPhone").val();
            }

            $.ajax({
                type: "POST",
                async: false,
                url: ROOTPath + "/Account/AddContact",
                data: { 'contactList': contactlist },
                success: function (data) {
                    $("#divContact").html(data);
                    $('#btnAddContact').attr('data-form-type', 1);
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

$("#btnUpdate").click(function (event) {
    if ($("#form").valid()) {

        if (addresslist.length > 0) {
            var result = $.grep(addresslist, function (e) { return e.IsPrimary == true });
            if (result.length == 0) addresslist[0].IsPrimary = true;
        }

        if (emailList.length > 0)
            UpdateSequenceID(emailList);
        $("#EmailJson").val(JSON.stringify(emailList));

        if (phoneList.length > 0)
            UpdateSequenceID(phoneList);
        $("#PhoneJson").val(JSON.stringify(phoneList));

        if (addresslist.length > 0)
            UpdateSequenceID(addresslist);
        $("#AddressJson").val(JSON.stringify(addresslist));

        if (contactlist.length > 0)
            UpdateSequenceID(contactlist);
        $("#ContactJson").val(JSON.stringify(contactlist));

        if (attachmentlist > 0)
            UpdateSequenceID(attachmentlist, 'file');
        attachmentlist = finalattachmentlist.filter(function (el) {
            return el.IsDeleted !== -1;
        });
        $("#FileJson").val(JSON.stringify(attachmentlist));
    }
});

function UpdateSequenceID(arr, data) {
    var count = 0;
    $.each(arr, function (key, value) {
        if (arr[key].ID == 0) {
            if (data == 'file')
                arr[key].ID = ++count;
            else
                arr[key].SequenceID = ++count;
        }
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
    ShowAccordionPanel('collapseemail');

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

$(document).on('click', '#btnEditEmail', function () {
    $('#btnAddEmail').attr('data-form-type', 2);
});

function EditEmail(emailId, type) {
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

                if (type == 'view') {
                    $("#emailtitle").html(viewTitle);
                    $("#frmEmail input").prop("disabled", true);
                    $('#frmEmail select').prop('disabled', true);
                    $("#btnAddEmail").hide();
                }
                else {
                    $("#emailtitle").html(editTitle);
                    $("#btnAddEmail").html(update);
                }

                $('#emailmodel').modal('show');
                var result = $.grep(emailList, function (e) { return e.ID == emailId });
                $("#emailId").val(emailId);
                $('#ddlEmailType').val(result[0].EmailTypeID).trigger('change');
                $("#txtEmail").val(result[0].Email);
            }
        }, error: function () {
        }
    }).always(function () {
    });
}

function DeleteEmail(data, sequenceId) {
    var newemaillist = emailList.filter(function (el) {
        return el.SequenceID !== sequenceId;
    });
    emailList = newemaillist;
    data.closest('tr').remove();
}

function ClearEmail() {
    $("#txtEmail").val('');
}

$('#collapseemail').on('shown.bs.collapse', function () {
    ShowAccordionPanel('collapseemail');
});

//phone
$('#phonemodel').on('shown.bs.modal', function () {
    $('#txtPhoneNumber').focus();
});

$('#addPhoneNumber').click(function (e) {
    ClearPhone();
    ShowAccordionPanel('collapsephone');

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

$(document).on('click', '#btnEditPhone', function () {
    $('#btnAddPhoneNumber').attr('data-form-type', 2);
});

function EditPhone(phoneId, type) {
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

                if (type == 'view') {
                    $("#phonetitle").html(viewTitle);
                    $("#frmPhone input").prop("disabled", true);
                    $('#frmPhone select').prop('disabled', true);
                    $("#btnAddPhoneNumber").hide();
                }
                else {
                    $("#phonetitle").html(editTitle);
                    $("#btnAddPhoneNumber").html(update);
                }

                $('#phonemodel').modal('show');
                var result = $.grep(phoneList, function (e) { return e.ID == phoneId });
                $("#phoneId").val(phoneId);
                $('#ddlPhoneType').val(result[0].PhoneTypeID).trigger('change');
                $("#txtPhoneNumber").val(result[0].PhoneNumber);
            }

        }, error: function () {
        }
    }).always(function () {
    });
}

function DeletePhone(data, sequenceId) {
    var newphonelist = phoneList.filter(function (el) {
        return el.SequenceID !== sequenceId;
    });
    phoneList = newphonelist;
    data.closest('tr').remove();
}

function ClearPhone() {
    $("#txtPhoneNumber").val('');
}

$('#collapsephone').on('shown.bs.collapse', function () {
    ShowAccordionPanel('collapsephone');
});

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

$(document).on('click', '#btnEditAddress', function () {
    $('#btnAddAddress').attr('data-form-type', 2);
});

function EditAddress(addressId, stateId, cityId, townId, type) {
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

                if (type == 'view') {
                    $("#addresstitle").html(viewTitle);
                    $("#frmAddress input").prop("disabled", true);
                    $('#frmAddress select').prop('disabled', true);
                    $("#btnAddAddress").hide();
                }
                else {
                    $("#addresstitle").html(editTitle);
                    $("#btnAddAddress").html(update);
                }

                var result = $.grep(addresslist, function (e) { return e.ID == addressId });

                $("#addressId").val(addressId);
                $('#ddlAddressType').val(result[0].AddressTypeID).trigger('change');
                $('#ddlCountry').val(result[0].CountryID).trigger('change');

                $("#txtAddressLine1").val(result[0].AddressLine1);
                $("#txtAddressLine2").val(result[0].AddressLine2);
                $("#txtZipCode").val(result[0].ZipPostalCode);

                if (result[0].IsPrimary == true)
                    $("#chkIsAddressPrimary").prop('checked', true);
                else
                    $("#chkIsAddressPrimary").prop('checked', false);
                $('#addressmodal').modal('show');
            }

        }
    }).then(function () {
        GetStatesByCountry(stateId, cityId, townId);
    });
}

function DeleteAddress(data, sequenceId, id) {
    if (id > 0) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Account/DeleteAddress",
            data: { 'ID': id, 'accountID': $("#AccountID").val() },
            success: function (response) {
                if (response.status == false) {
                    showAlert('error', response.message);
                }
                else {
                    var newaddresslist = addresslist.filter(function (el) { return el.SequenceID !== sequenceId; });
                    addresslist = newaddresslist;
                    data.closest('tr').remove();
                }
            }
        });
    }
    else {
        var newaddresslist = addresslist.filter(function (el) { return el.SequenceID !== sequenceId; });
        addresslist = newaddresslist;
        data.closest('tr').remove();
    }
}

function ClearAddress() {
    $("#txtCity").val('');
    $("#txtTown").val('');
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

$(document).on('click', '#btnEditContact', function () {
    $('#btnAddContact').attr('data-form-type', 2);
});

function EditContact(contactId, type) {
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

                if (type == 'view') {
                    $("#contacttitle").html(viewTitle);
                    $("#frmContacts input").prop("disabled", true);
                    $('#frmContacts select').prop('disabled', true);
                    $("#btnAddContact").hide();
                }
                else {
                    $("#contacttitle").html(editTitle);
                    $("#btnAddContact").html(update);
                }
                $('#contactmodel').modal('show');
                var result = $.grep(contactlist, function (e) { return e.ID == contactId });
                $("#contactId").val(contactId);
                $('#ddlContactSalution').val(result[0].SalutationID).trigger('change');

                $("#txtContactFirstName").val(result[0].FirstName);
                $("#txtContactMiddleName").val(result[0].MiddleName);
                $("#txtContactLastName").val(result[0].LastName);
                $('#ddlContactPosition').val(result[0].PositionID).trigger('change');

                $("#txtContactPrimaryEmail").val(result[0].PrimaryEmail);
                $("#txtContactOtherEmail").val(result[0].OtherEmail);
                $("#txtContactPrimaryPhone").val(result[0].PrimaryPhoneNumber);
                $("#txtContactOtherPhone").val(result[0].OtherPhoneNumber);
            }

        }, error: function () {
        }
    }).always(function () {
    });
}

function DeleteContact(data, sequenceId, id) {
    if (id > 0) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Account/DeleteContact",
            data: { 'ID': id, 'accountID': $("#AccountID").val() },
            success: function (response) {
                if (response.status == false) {
                    showAlert('error', response.message);
                }
                else {
                    var newcontactlist = contactlist.filter(function (el) { return el.SequenceID !== sequenceId; });
                    contactlist = newcontactlist;
                    data.closest('tr').remove();
                }
            }
        });
    }
    else {
        var newcontactlist = contactlist.filter(function (el) { return el.SequenceID !== sequenceId; });
        contactlist = newcontactlist;
        data.closest('tr').remove();
    }
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
            finalattachmentlist.push(attachmentModel);
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
    var result = $.grep(finalattachmentlist, function (e) { return e.FileID == fileid });
    result[0].IsDeleted = true;
    var newattachmentlist = $.grep(attachmentlist, function (element, index) {
        return element.FileID != fileid;
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

function GetStatesByCountry(stateid, cityid, townid) {
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
            async: false,
            url: ROOTPath + "/Account/GetStatesByCountry?countryId=" + $('#ddlCountry').val(),
            success: function (data) {
                $("#ddlState").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlState").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlState option:first").val("");

                if (stateid)
                    $('#ddlState').val(stateid).trigger('change');
                else
                    $('#ddlState').val(null).trigger('change');
            }
        }).then(function () {
            if (stateid)
                GetCitiesByCountryAndState(cityid, townid);
        });
    }
}

function GetCitiesByCountryAndState(cityid, townid) {
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
            async: false,
            url: ROOTPath + "/Account/GetCitiesByCountryAndState?countryId=" + $('#ddlCountry').val() + "&stateId=" + $('#ddlState').val(),
            success: function (data) {
                $("#ddlCity").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlCity").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlCity option:first").val("");

                if (cityid)
                    $('#ddlCity').val(cityid).trigger('change');
                else
                    $('#ddlCity').val(null).trigger('change');
            }
        }).then(function () {
            if (cityid)
                GetTownsByCountryStateAndCity(townid);
        });
    }
}

function GetTownsByCountryStateAndCity(townid) {
    if (!$('#ddlCity').val()) {
        $("#ddlTown").empty();
        $("#ddlTown").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $('#ddlTown').val(null).trigger('change');
    }
    else {
        $.ajax({
            async: false,
            url: ROOTPath + "/Account/GetTowndByCountryStateAndCity?countryId=" + $('#ddlCountry').val() + "&stateId=" + $('#ddlState').val() + "&cityId=" + $('#ddlCity').val(),
            success: function (data) {
                $("#ddlTown").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlTown").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlTown option:first").val("");

                if (townid) {
                    $('#ddlTown').val(townid).trigger('change');
                }
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