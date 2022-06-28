function linkRuleAndDetailList() {
    $.each(collectionRuleList, function (key, value) {
        collectionRuleList[key].IsNew = false;
        ruleSeq = collectionRuleList[key].SequenceID;
        _ruleID = collectionRuleList[key].ID;
        collectionRuleList[key].ServiceCollectionRuleDetailList = jQuery.grep(ruleDetailList, function (item) {
            return item.ServiceCollectionRuleID == collectionRuleList[key].ID;
        });
    });
    ruleSeq++;
    _ruleID++;
}

function enableRuleDetailInput(source) {
    $("#lidetail1").removeClass("hide");
    $("#tabdetail1").removeClass("hide");
    $("#lidetail3").addClass("hide");
    $("#tabdetail3").addClass("hide");
    if ($("#IsTestMode").val() == "False" && $("#hdnNew").val() == 'False') {
        $(".disable").attr("disabled", true);
        $('.RuleDetailInput a').bind('click', false);
    }
    else {
        var selected = $("#ddlCollectionRule").val();
        $(".RuleDetailInput input").attr("disabled", true);
        if (source == 'dropdown')
            $(".RuleDetailInput input").val('');
        $('.RuleDetailInput a').bind('click', false);
        $("#lidetail2").addClass("hide");
        $("#lidetail3").addClass("hide");

        $(".tabdetail input").addClass("inputdecimal");
        $(".tabdetail input").removeClass("inputnumber");

        $(".tabdetail #lblFromText").html(StartAmount);
        $(".tabdetail #lblToText").html(EndAmount);

        $(".singleValue").removeClass("input-validation-error");
        $(".singleValue").next(".error").hide();

        if (selected == 1) // Fee
        {
            $("#txtFixedAmount").attr("disabled", false);
        }
        else if (selected == 2) // Percentage
        {
            $("#txtPercent").attr("disabled", false);
        }
        else if (selected == 3) // Percentage of Percentage
        {
            $("#txtPercent").attr("disabled", false);
            $("#txtSecondPercent").attr("disabled", false);
        }
        else if (selected == 4) // Range Fee
        {
            $("#tabdetail1 input").attr("disabled", false);
            $('#dvRangeTab a:first').tab('show')

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            $("#tabdetail1 #lblValueHeaderText").html(flatamount);
            $("#lblTab1Text").html(rangeflat);
        }
        else if (selected == 5) // Range Percentage
        {
            $("#tabdetail1 input").attr("disabled", false);
            $('#dvRangeTab a:first').tab('show');

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            $("#tabdetail1 #lblValueHeaderText").html(percentage);
            $("#lblTab1Text").html(rangepercentage);
        }
        else if (selected == 6) // Range Fee and Percentage
        {
            $(".tabdetail input").attr("disabled", false);

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            //$('#dvRangeTab a:first').tab('show')

            $("#tabdetail2").removeClass("hide");
            $("#lidetail2").removeClass("hide");

            $("#tabdetail1 #lblValueHeaderText").html(flatamount);
            $("#tabdetail2 #lblValueHeaderText").html(percentage);
            $("#lblTab1Text").html(rangevariableflat);
            $("#lblTab2Text").html(rangevariablepercentage);
        }
        else if (selected == 7) // Day Range Fee
        {
            $("#tabdetail1 input").attr("disabled", false);

            $("#tabdetail1 .rangeamount").removeClass("inputdecimal");
            $("#tabdetail1 .rangeamount").addClass("inputnumber");

            $('#dvRangeTab a:first').tab('show');

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            $("#tabdetail1 #lblFromText").html(StartDay);
            $("#tabdetail1 #lblToText").html(EndDay);
            $("#tabdetail1 #lblValueHeaderText").html(flatamount);
            $("#lblTab1Text").html(rangeflat);
        }
        else if (selected == 8) // Day Range Percentage
        {
            $("#tabdetail1 input").attr("disabled", false);

            $("#tabdetail1 .rangeamount").removeClass("inputdecimal");
            $("#tabdetail1 .rangeamount").addClass("inputnumber");

            $('#dvRangeTab a:first').tab('show');

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            $("#tabdetail1 #lblFromText").html(StartDay);
            $("#tabdetail1 #lblToText").html(EndDay);
            $("#tabdetail1 #lblValueHeaderText").html(percentage);
            $("#lblTab1Text").html(rangepercentage);
        }
        else if (selected == 9) // Day Range Fee and Percentage
        {
            $("#tabdetail1 input").attr("disabled", false);
            $("#tabdetail2 input").attr("disabled", false);

            $(".tabdetail .rangeamount").removeClass("inputdecimal");
            $(".tabdetail .rangeamount").addClass("inputnumber");

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            $("#tabdetail2").removeClass("hide");
            $("#lidetail2").removeClass("hide");
            $("#lidetail1 a").click();

            $(".tabdetail #lblFromText").html(StartDay);
            $(".tabdetail #lblToText").html(EndDay);

            $("#tabdetail1 #lblValueHeaderText").html(flatamount);
            $("#tabdetail2 #lblValueHeaderText").html(percentage);
            $("#lblTab1Text").html(rangevariableflat);
            $("#lblTab2Text").html(rangevariablepercentage);
        }
        if (selected == 10) // Factor
        {
            $("#txtFactor").attr("disabled", false);
        }
        if (selected == 11) // Exceed Base and Fee
        {
            $("#tabdetail3 input").attr("disabled", false);

            $("#tabdetail3 .rangeamount").removeClass("inputdecimal");
            $("#tabdetail3 .rangeamount").addClass("inputnumber");

            $('#dvRangeTab a:last').tab('show');

            $('.RuleDetailInput a').unbind('click');
            $('.RuleDetailInput a').bind('click');

            $("#lidetail1").addClass("hide");
            $("#tabdetail1").addClass("hide");
            $("#tabdetail3").removeClass("hide");
            $("#lidetail3").removeClass("hide");
            $("#lidetail3 a").click();

            $("#tabdetail3 #lblFromText").html(StartingUnit);
            $("#tabdetail3 #lblToText").html(EndUnit);
            $("#tabdetail3 #lblValueHeaderText").html(BaseAmount);
            $("#tabdetail3 #lblFourthColumn").html(FactorValue);
            $("#lblTab3Text").html(ExceedBaseAndFee);
        }
    }
}

function enableFeeRule(isRuleDisable, isChanged) {
    if ($("#ddlCollectionField").val() == 1) {
        if (isChanged == true) {
            $("#ddlCollectionRule").val(1).trigger('change');
        }
        $("#ddlCollectionRule").attr("disabled", "disabled");
    }
    else if (isRuleDisable == true) {
        $("#ddlCollectionRule").attr("disabled", "disabled");
    }
    else {
        $("#ddlCollectionRule").removeAttr("disabled");
    }
}

$(document).on('click', '.addRule', function (e) {
    $.ajax({
        type: "GET",
        async: false,
        url: ROOTPath + "/Service/AddRule",
        success: function (data) {
            $("#dvrule").html(data);
            $(".select2dropdown").select2({ width: '100%' });
            $.validator.unobtrusive.parse('#frmSaveRule');
            mode = 'add';
            initializePopupDate('popupinputdate', __dateFormat);
            enableRuleDetailInput('');
            $("#rulemodal").modal("show");
            $("#txtRuleName").focus();
        }, error: function () { }
    }).always(function () {
    });
});

function editRule(ele, seqId) {

    var result = [];
    result = jQuery.grep(collectionRuleList, function (item) {
        return item.SequenceID == seqId && item.IsActive;
    });
    if (result.length > 0) {
        $.ajax({
            type: "POST",
            async: false,
            url: ROOTPath + "/Service/EditRule",
            data: { 'collectionRule': result[0] },
            success: function (data) {
                $("#dvrule").html(data);
                $(".select2dropdown").select2({ width: '100%' });
                $.validator.unobtrusive.parse('#frmSaveRule');
                mode = 'edit';
                initializePopupDate('popupinputdate', __dateFormat);

                //19-Feb-2020 -Change Reverted - Commented and move this code [Reason - control disable not working properly due to wrong code sequence issue]
                enableRuleDetailInput('');
                $("#rulemodal").modal("show");
                $("#txtRuleName").focus();

                if ((result[0].isAddedOrCopy && (result[0].IsNew || result[0].IsNew == "True")) || $(ele).attr("data-allowedit") == 1) {
                    if ($(ele).attr("data-allowedit") != 1) {
                        $("#dvrule #txtruleEffectiveFrom").attr('disabled', true);
                    }
                }
                else if (!result[0].IsNew || result[0].IsNew == "False") {
                    $("#dvrule .select2dropdown").select2({ width: '100%', disabled: true });
                    $("#dvrule input[type='text'], #dvrule input[type='radio'], #dvrule input[type='checkbox']").attr('disabled', true);
                    $("#dvrule #txtruleEffectiveTo").attr('disabled', false);
                    enableRuleDetailInput('')
                    $("#dvrule #txtRuleName").attr('disabled', false);
                }

                ////19-Feb-2020 - Moved - Change Reverted 
                //enableRuleDetailInput('');
                //$("#rulemodal").modal("show");
                //$("#txtRuleName").focus();
                ////

                //CO-628
                if (result[0].IsNew.toString().toLowerCase() == "true")
                    enableFeeRule(false);
                else
                    enableFeeRule(true);

            }, error: function () {
            }
        }).always(function () {
        });
    }
}

function copyRule(ele, ID, isViewMode) {
    var result = []
    result = jQuery.grep(collectionRuleList, function (item) {
        return item.ID == ID;
    });
    result[0].IsActive = 0;
    result[0].IsEffectiveApply = true;

    if (result.length > 0) {
        $.ajax({
            type: "POST",
            //async: false,
            url: ROOTPath + "/Service/CopyRule",
            data: { 'collectionRule': result[0], 'isViewMode': isViewMode },
            success: function (data) {
                $("#dvrule").html(data);
                $(".select2dropdown").select2({ width: '100%' });
                $.validator.unobtrusive.parse('#frmSaveRule');

                if (isViewMode) {
                    enableRuleDetailInput('');
                    $("#dvrule .select2dropdown").select2({ width: '100%', disabled: true });
                    $("#dvrule input[type='text'], #dvrule input[type='radio'], #dvrule input[type='checkbox']").attr('disabled', true);
                    $("#dvrule #btnAddRule").remove();
                    $("#dvrule #btnCancel").focus();
                    $("#CopyMode").val('view');
                }
                else {
                    mode = 'edit';
                    initializeCopyPopupDate('copypopupinputdate', __dateFormat);
                    initializePopupDate('popupinputdate', __dateFormat);
                    enableRuleDetailInput('');
                    $("#txtRuleName").focus();

                    ////19-Feb-2020 - Copy Rule - Enable Effective From and disable past date in Effective From,Disable Collection Type
                    //$("#dvrule #txtruleEffectiveFrom").attr('disabled', true);
                    $("#ddlCollectionType").attr("disabled", true);
                    ////

                    $("#CopyMode").val('copy');
                }
                $("#rulemodal").modal("show");

                //CO-628
                if (isViewMode)
                    enableFeeRule(true);
                else
                    enableFeeRule(false);

            }, error: function () {
            }
        }).always(function () {
        });
    }
}

function removeRule(ele, seqId) {
    var result = [];
    result = jQuery.grep(collectionRuleList, function (item) {
        return item.SequenceID == seqId;
    });
    if (result.length > 0) {
        if (result[0].IsNew == false) // existing record. so update IsDeleted Field
        {
            result[0].IsActive = false;

            // 08-july-2020
            result[0].IsDeleted = true; 

            $.each(result[0].ServiceCollectionRuleDetailList, function (key, value) {
                result[0].ServiceCollectionRuleDetailList[key].IsActive = false;
                result[0].ServiceCollectionRuleDetailList[key].IsDeleted = true;
            });
            //

            var TrInnerHtml = '<a id="btnView" class="btn btn-white btn-sm m-l-xs" title="' + ViewRule + '"  onclick="copyRule(this,' + result[0].SequenceID + ',true);"><i class="fa fa-folder"></i></a>';
            $(ele).closest("tr").find('td:lt(-1)').addClass('text-danger');
            $(ele).closest("td").html('').html(TrInnerHtml);
        }
        else {
            collectionRuleList = jQuery.grep(collectionRuleList, function (item) {
                return item.SequenceID != seqId;
            });
            $(ele).parents("tr").remove();
        }

    }
    else
        showAlert("error", DeleteFailedErrMsg);

}

$(document).on("click", "#addDetailRow", function (e) {
    var Url = "";
    if ($("#ddlCollectionRule").val() == 11) {
        Url = ROOTPath + "/Service/AddRuleDetailrowForExceed";
    }
    else {
        Url = ROOTPath + "/Service/AddRuleDetailrow";
    }
    $.ajax({
        url: Url,
        success: function (response) {
            var eleId = $("#dvRangeTab ul.nav-tabs li.active").attr("data-linkeddiv");
            $('#' + eleId + ' #tblDetailRow').append(response);
            $('#' + eleId + ' #tblDetailRow tr:last input').val('');
        }
    });
});

$(document).on("click", ".btnRemoveDetail", function (e) {
    $(e.target).parents("tr").remove();
});

$("#btnAddRule").click(function (event) {
    if (SaveRule() == false)
        return false;
});

$(document).on("click", "#rulemodal #btnCancel", function () {
    if ($("#hdnRuleSeqId").val() > 0) {
        var result = []
        result = jQuery.grep(collectionRuleList, function (item) {
            return item.SequenceID == $("#hdnRuleSeqId").val();
        });
        if ($("#CopyMode").val() == "copy") {
            result[0].IsActive = true;
            result[0].IsEffectiveApply = false;
        }
        else if ($("#CopyMode").val() == "view") {
            result[0].IsEffectiveApply = false;
            result[0].IsActive = false;
        }

    }
    $("#rulemodal").modal('hide');
})

function ValidateRuleDetailRangePercentage() {
    var ruleID = $("#ddlCollectionRule").val();

    if (ruleID == 5 || ruleID == 8 || ruleID == 6 || ruleID == 9) {
        var percentage = 0;

        if (ruleID == 5 || ruleID == 8) {
            $('#tabdetail1 td #txtValue').each(function () {
                percentage = Math.max(GlobalConvertToDecimal($(this).val() == "" ? "0" : $(this).val()), percentage);
            });
        }
        if (ruleID == 6 || ruleID == 9) {
            $('#tabdetail2 td #txtValue').each(function () {
                percentage = Math.max(GlobalConvertToDecimal($(this).val() == "" ? "0" : $(this).val()), percentage);
            });
        }

        if (percentage > 100)
            return false;
        else
            return true;
    }
    else
        return true;
}

function SaveRule(copyRuleID) {

    if (ValidateRuleDetailRangePercentage() == false) {
        showAlert("error", PercentageRangeValidationMsg);
        return false;
    }

    //var _effFrom = $("#txtruleEffectiveFrom").datepicker('getDate');
    //_effFrom.setDate(_effFrom.getDate());
    //_effFrom.setFullYear(_effFrom.getFullYear() + 1);
    //_effFrom.setDate(_effFrom.getDate() - 1);

    //if ($("#txtruleEffectiveTo").datepicker('getDate') < _effFrom) {
    //    showAlert("error", CompareEffectiveDatesValidationMsg);
    //    return false;
    //}
    if ($("#txtruleEffectiveTo").datepicker('getDate') <= $("#txtruleEffectiveFrom").datepicker('getDate')) {
        showAlert("error", EffectiveToShouldBeGreaterThanEffectiveFromMsg);
        return false;
    }
    if ($("#frmSaveRule").valid()) {
        setCollectionRule();
        if (setCollectionRuleDetail()) {
            if ((collectionRule.CollectionRuleID >= 4 && collectionRule.CollectionRuleID <= 9) || collectionRule.CollectionRuleID == 11) // show warning for range
            {
                if (maxRuleDetailRange < defaultToAmount) {
                    swal({
                        title: suremsg,
                        text: MaxCollectionRuleRangeValMsg,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: continueMessage,
                        cancelButtonText: cancel,
                        closeOnConfirm: true
                    }, function (status) {
                        if (status == true)
                            AddRule(copyRuleID);
                    });
                }
                else
                    AddRule(copyRuleID);
            }
            else
                AddRule(copyRuleID);
        }
        else
            return false;
    }
    else
        return false;
}

function AddRule(copyRuleID) {

    //var collectionTypeID = null;

    if (mode == 'add') {
        collectionRule.ServiceCollectionRuleDetailList = ruleDetailList;
        if ($("#hdnRuleSeqId").val() != "0")
            collectionRule.isAddedOrCopy = true;
        collectionRuleList.push(collectionRule);
    }
    // Set Valus to existing object in edit mode
    else if (mode == 'edit') {
        $.each(collectionRuleList, function (key, value) {
            if (collectionRuleList[key].ID == ruleId && collectionRuleList[key].IsActive) {

                //collectionTypeID = collectionRuleList[key].CollectionTypeID;

                if (collectionRuleList[key].isAddedOrCopy) {
                    collectionRule.isAddedOrCopy = true;
                }
                collectionRuleList[key] = collectionRule;
                collectionRuleList[key].ServiceCollectionRuleDetailList = ruleDetailList;
            }
        });
    }
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Service/AddRule",
        data: { 'collectionRuleList': collectionRuleList },
        success: function (data) {
            if (data.status) {
                $("#dvrule").html('');
                $("#rulemodal").modal('hide');
                $("#dvCollectionRuleList").html(data.returnData);
                if ($("#IsTestMode").val() == "False") {
                    $(".hideRule").addClass("hide");
                }
                //$.each(collectionRuleList, function (key, value) {
                //    if (collectionRuleList[key].IsEffectiveApply) {
                //        collectionRuleList[key].EffectiveTo = data.EffectiveTo;
                //    }
                //});

                if (copyRuleID > 0) {
                    $.each(collectionRuleList, function (key, value) {
                        if (collectionRuleList[key].ID == copyRuleID)
                            collectionRuleList[key].EffectiveTo = data.EffectiveTo;
                    });
                }
                else {
                    $.each(collectionRuleList, function (key, value) {
                        //if (collectionRuleList[key].IsEffectiveApply && collectionRuleList[key].CollectionTypeID == collectionTypeID) {
                        if (collectionRuleList[key].IsEffectiveApply && collectionRuleList[key].ID == ruleId) {    
                            collectionRuleList[key].EffectiveTo = data.EffectiveTo;
                        }
                    });
                }
            }
            else {
                showAlert("error", data.message);
            }
        }, error: function () {
        }
    }).always(function () {
    });
    return false;
}

function setCollectionRule() {
    collectionRule = {};
    ruleId = $("#hdnRuleId").val();
    if (ruleId > 0) {
        mode = 'edit';
    }
    else {
        ruleId = _ruleID;
        mode = 'add';
    }
    collectionRule.ID = ruleId;
    collectionRule.Name = $("#txtRuleName").val();
    collectionRule.CollectionTypeID = $("#ddlCollectionType").val();
    collectionRule.CollectionTypeName = $("#ddlCollectionType  option:selected").text();
    collectionRule.CollectionFieldID = $("#ddlCollectionField").val();
    collectionRule.CollectionFieldName = $("#ddlCollectionField option:selected").text();
    collectionRule.EffectiveFrom = $("#txtruleEffectiveFrom").val();
    collectionRule.EffectiveTo = $("#txtruleEffectiveTo").val();
    collectionRule.FrequencyID = $("#ddlRuleFrequency").val();
    collectionRule.CollectionFrequencyName = $("#ddlRuleFrequency option:selected").text();
    collectionRule.ApplyOnPaymentDueDate = $("input[name=ApplyOnPaymentDueDate]:checked").val() == "True" ? true : false;
    collectionRule.CollectionRuleID = $("#ddlCollectionRule").val();
    collectionRule.CollectionRuleName = $("#ddlCollectionRule option:selected").text();
    collectionRule.SequenceID = $("#hdnRuleSeqId").val() != 0 ? $("#hdnRuleSeqId").val() : ruleSeq;
    // Default Values    
    collectionRule.IsNew = $("#hdnNew").val();
    collectionRule.IsPublic = true;
    collectionRule.IsDeleted = false;
    collectionRule.IsActive = true;

    //18-Feb-2020
    var isValCopied;

    var item = $.grep(collectionRuleList, function (e) { return e.ID == ruleId });
    if (item.length > 0) {
        isValCopied = item[0].IsCopied == true ? true : false;
    }

    if (
        isValCopied != true && $("#Id").val() > 0 && mode == "edit" && $("#hdnRuleId").val() != "0"
    ) {
        collectionRule.IsChanged = true;
    }
    else {
        collectionRule.IsChanged = false;
    }

    if (
        isValCopied == true
        ||
        ($("#Id").val() > 0 && $("#CopyMode").val() == "copy")
    ) {
        collectionRule.IsCopied = true;
    }
    else {
        collectionRule.IsCopied = false;
    }
    //

    if ($("#hdnRuleSeqId").val() == '' || $("#hdnRuleSeqId").val() == 0 || $("#CopyMode").val() == "copy") {
        ruleSeq = ruleSeq + 1;
        _ruleID = _ruleID + 1;
    }
}

function setCollectionRuleDetail() {
    ruleDetailList = [];
    ruleDetail = {};
    var isvalid = true;
    ruledetailSeq = 1;
    maxRuleDetailRange = 0;

    if (collectionRule.CollectionRuleID == 1) {
        ruleDetail.ID = $("#hdnRuleDetailId").val() == '' ? 0 : $("#hdnRuleDetailId").val();
        ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
        ruleDetail.FromAmount = defaultFromAmount;
        ruleDetail.ToAmount = defaultToAmount;
        ruleDetail.Value = $("#txtFixedAmount").val();
        ruleDetail.SecondValue = null;
        ruleDetail.SequenceID = ruledetailSeq;
        ruleDetail.IsActive = true;
        ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID
        ruledetailSeq = ruledetailSeq + 1;
        if (ruleDetail.ID == 0)
            ruleDetail.IsNew = true;
        else
            ruleDetail.IsNew = false;
        ruleDetailList.push(ruleDetail);
    }
    else if (collectionRule.CollectionRuleID == 2) {
        ruleDetail.ID = $("#hdnRuleDetailId").val() == '' ? 0 : $("#hdnRuleDetailId").val();
        ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
        ruleDetail.FromAmount = defaultFromAmount;
        ruleDetail.ToAmount = defaultToAmount;
        ruleDetail.Value = $("#txtPercent").val();
        ruleDetail.SecondValue = null;
        ruleDetail.SequenceID = ruledetailSeq;
        ruleDetail.IsActive = true;
        ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
        ruledetailSeq = ruledetailSeq + 1;
        if (ruleDetail.ID == 0)
            ruleDetail.IsNew = true;
        else
            ruleDetail.IsNew = false;
        ruleDetailList.push(ruleDetail);
    }
    else if (collectionRule.CollectionRuleID == 3) {
        ruleDetail.ID = $("#hdnRuleDetailId").val() == '' ? 0 : $("#hdnRuleDetailId").val();
        ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
        ruleDetail.FromAmount = defaultFromAmount;
        ruleDetail.ToAmount = defaultToAmount;
        ruleDetail.Value = $("#txtPercent").val();
        ruleDetail.SecondValue = $("#txtSecondPercent").val();
        ruleDetail.SequenceID = ruledetailSeq;
        ruleDetail.IsActive = true;
        ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
        ruledetailSeq = ruledetailSeq + 1;
        if (ruleDetail.ID == 0)
            ruleDetail.IsNew = true;
        else
            ruleDetail.IsNew = false;
        ruleDetailList.push(ruleDetail);
    }
    else if (collectionRule.CollectionRuleID == 4 || collectionRule.CollectionRuleID == 7) {
        $('#tabdetail1 #tblDetailRow tr').each(function () {
            if (GlobalConvertToDecimal($(this).find("#txtValue").val() == "" ? "0" : $(this).find("#txtValue").val()) > 0) {
                ruleDetail = {};
                ruleDetail.ID = $(this).find("#hdnRuleDetailId").val() == '' ? 0 : $(this).find("#hdnRuleDetailId").val();
                ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
                if ((collectionRule.CollectionRuleID == 4 || collectionRule.CollectionRuleID == 7) && GlobalConvertToDecimal($(this).find("#txtFromAmount").val()) > maxRangeAmount)
                    ruleDetail.FromAmount = maxRangeAmount;
                else
                    ruleDetail.FromAmount = $(this).find("#txtFromAmount").val();
                if ((collectionRule.CollectionRuleID == 4 || collectionRule.CollectionRuleID == 7) && GlobalConvertToDecimal($(this).find("#txtToAmount").val()) > maxRangeAmount)
                    ruleDetail.ToAmount = maxRangeAmount;
                else
                    ruleDetail.ToAmount = $(this).find("#txtToAmount").val();
                ruleDetail.Value = $(this).find("#txtValue").val();
                ruleDetail.IsEditable = $(this).find("#IsEditable").is(":checked");
                ruleDetail.SequenceID = ruledetailSeq;
                ruleDetail.IsActive = true;
                ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
                ruleDetail.Tab = 1;
                if (ruleDetail.ID == 0)
                    ruleDetail.IsNew = true;
                else
                    ruleDetail.IsNew = false;
                if (!validateRange(rangeflat, 1)) {
                    isvalid = false;
                    return false;
                }
                ruledetailSeq = ruledetailSeq + 1;
                ruleDetailList.push(ruleDetail);
            }
        });
    }
    else if (collectionRule.CollectionRuleID == 5 || collectionRule.CollectionRuleID == 8) {
        $('#tabdetail1 #tblDetailRow tr').each(function () {
            if (GlobalConvertToDecimal($(this).find("#txtValue").val() == "" ? "0" : $(this).find("#txtValue").val()) > 0) {
                ruleDetail = {};
                ruleDetail.ID = $(this).find("#hdnRuleDetailId").val() == '' ? 0 : $(this).find("#hdnRuleDetailId").val();
                ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
                if ((collectionRule.CollectionRuleID == 5 || collectionRule.CollectionRuleID == 8) && GlobalConvertToDecimal($(this).find("#txtFromAmount").val()) > maxRangeAmount)
                    ruleDetail.FromAmount = maxRangeAmount;
                else
                    ruleDetail.FromAmount = $(this).find("#txtFromAmount").val();
                if ((collectionRule.CollectionRuleID == 5 || collectionRule.CollectionRuleID == 8) && GlobalConvertToDecimal($(this).find("#txtToAmount").val()) > maxRangeAmount)
                    ruleDetail.ToAmount = maxRangeAmount;
                else
                    ruleDetail.ToAmount = $(this).find("#txtToAmount").val();
                ruleDetail.Value = $(this).find("#txtValue").val();
                ruleDetail.IsEditable = $(this).find("#IsEditable").is(":checked");
                ruleDetail.SequenceID = ruledetailSeq;
                ruleDetail.IsActive = true;
                ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
                ruleDetail.Tab = 1;
                if (ruleDetail.ID == 0)
                    ruleDetail.IsNew = true;
                else
                    ruleDetail.IsNew = false;
                if (!validateRange(rangepercentage, 1)) {
                    isvalid = false;
                    return false;
                }
                ruledetailSeq = ruledetailSeq + 1;
                ruleDetailList.push(ruleDetail);
            }
        });
    }
    else if (collectionRule.CollectionRuleID == 6 || collectionRule.CollectionRuleID == 9) {

        $('#tabdetail1 #tblDetailRow tr').each(function () {
            if (GlobalConvertToDecimal($(this).find("#txtValue").val() == "" ? "0" : $(this).find("#txtValue").val()) > 0) {
                ruleDetail = {};
                ruleDetail.ID = $(this).find("#hdnRuleDetailId").val() == '' ? 0 : $(this).find("#hdnRuleDetailId").val();
                ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
                if ((collectionRule.CollectionRuleID == 6 || collectionRule.CollectionRuleID == 9) && GlobalConvertToDecimal($(this).find("#txtFromAmount").val()) > maxRangeAmount)
                    ruleDetail.FromAmount = maxRangeAmount;
                else
                    ruleDetail.FromAmount = $(this).find("#txtFromAmount").val();
                if ((collectionRule.CollectionRuleID == 6 || collectionRule.CollectionRuleID == 9) && GlobalConvertToDecimal($(this).find("#txtToAmount").val()) > maxRangeAmount)
                    ruleDetail.ToAmount = maxRangeAmount;
                else
                    ruleDetail.ToAmount = $(this).find("#txtToAmount").val();
                ruleDetail.Value = $(this).find("#txtValue").val();
                ruleDetail.IsEditable = $(this).find("#IsEditable").is(":checked");
                ruleDetail.SequenceID = ruledetailSeq;
                ruleDetail.IsActive = true;
                ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
                ruleDetail.Tab = 1;
                if (ruleDetail.ID == 0)
                    ruleDetail.IsNew = true;
                else
                    ruleDetail.IsNew = false;
                if (!validateRange(rangevariableflat, 1)) {
                    isvalid = false;
                    return false;
                }
                ruledetailSeq = ruledetailSeq + 1;
                ruleDetailList.push(ruleDetail);
            }
        });
        if (!isvalid)
            return false;

        $('#tabdetail2 #tblDetailRow tr').each(function () {
            if (GlobalConvertToDecimal($(this).find("#txtValue").val() == "" ? "0" : $(this).find("#txtValue").val()) > 0) {
                ruleDetail = {};
                ruleDetail.ID = $(this).find("#hdnRuleDetailId").val() == '' ? 0 : $(this).find("#hdnRuleDetailId").val();
                ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
                if (collectionRule.CollectionRuleID == 9 && GlobalConvertToDecimal($(this).find("#txtFromAmount").val()) > maxRangeAmount)
                    ruleDetail.FromAmount = maxRangeAmount;
                else
                    ruleDetail.FromAmount = $(this).find("#txtFromAmount").val();
                if (collectionRule.CollectionRuleID == 9 && GlobalConvertToDecimal($(this).find("#txtToAmount").val()) > maxRangeAmount)
                    ruleDetail.ToAmount = maxRangeAmount;
                else
                    ruleDetail.ToAmount = $(this).find("#txtToAmount").val();
                ruleDetail.SecondValue = $(this).find("#txtValue").val();
                ruleDetail.IsEditable = $(this).find("#IsEditable").is(":checked");
                ruleDetail.SequenceID = ruledetailSeq;
                ruleDetail.IsActive = true;
                ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
                ruleDetail.Tab = 2;
                if (ruleDetail.ID == 0)
                    ruleDetail.IsNew = true;
                else
                    ruleDetail.IsNew = false;
                if (!validateRange(rangevariablepercentage, 2)) {
                    isvalid = false;
                    return false;
                }
                ruledetailSeq = ruledetailSeq + 1;
                ruleDetailList.push(ruleDetail);
            }
        });
    }
    else if (collectionRule.CollectionRuleID == 10) {
        ruleDetail.ID = $("#hdnRuleDetailId").val() == '' ? 0 : $("#hdnRuleDetailId").val();
        ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
        ruleDetail.FromAmount = defaultFromAmount;
        ruleDetail.ToAmount = defaultToAmount;
        ruleDetail.Value = $("#txtFactor").val();
        ruleDetail.SecondValue = null;
        ruleDetail.SequenceID = ruledetailSeq;
        ruleDetail.IsActive = true;
        ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID
        ruledetailSeq = ruledetailSeq + 1;
        if (ruleDetail.ID == 0)
            ruleDetail.IsNew = true;
        else
            ruleDetail.IsNew = false;
        ruleDetailList.push(ruleDetail);
    }
    else if (collectionRule.CollectionRuleID == 11) {
        $('#tabdetail3 #tblDetailRow tr').each(function () {
            if (GlobalConvertToDecimal($(this).find("#txtValue").val() == "" ? "0" : $(this).find("#txtValue").val()) > 0
                ||
                GlobalConvertToDecimal($(this).find("#txtFactorValue").val() == "" ? "0" : $(this).find("#txtFactorValue").val()) > 0
            ) {
                ruleDetail = {};
                ruleDetail.ID = $(this).find("#hdnRuleDetailId").val() == '' ? 0 : $(this).find("#hdnRuleDetailId").val();
                ruleDetail.ServiceCollectionRuleID = collectionRule.ID;
                if (collectionRule.CollectionRuleID == 11 && GlobalConvertToDecimal($(this).find("#txtFromAmount").val()) > maxRangeAmount)
                    ruleDetail.FromAmount = maxRangeAmount;
                else
                    ruleDetail.FromAmount = $(this).find("#txtFromAmount").val();
                if (collectionRule.CollectionRuleID == 11 && GlobalConvertToDecimal($(this).find("#txtToAmount").val()) > maxRangeAmount)
                    ruleDetail.ToAmount = maxRangeAmount;
                else
                    ruleDetail.ToAmount = $(this).find("#txtToAmount").val();
                ruleDetail.Value = $(this).find("#txtValue").val();
                ruleDetail.SecondValue = $(this).find("#txtFactorValue").val();
                ruleDetail.SequenceID = ruledetailSeq;
                ruleDetail.IsActive = true;
                ruleDetail.CollectionRuleID = collectionRule.CollectionRuleID;
                ruleDetail.Tab = 1;
                if (ruleDetail.ID == 0)
                    ruleDetail.IsNew = true;
                else
                    ruleDetail.IsNew = false;
                if (!validateRange(rangevariableflat, 1)) {
                    isvalid = false;
                    return false;
                }
                ruledetailSeq = ruledetailSeq + 1;
                ruleDetailList.push(ruleDetail);
            }
        });
    }
    if (!isvalid)
        return false;

    if (ruleDetailList.length == 0) {
        showAlert("error", atleastOneRangeValidationMsg);
        return false;
    }

    return true;
}

function validateRange(errorTitle, tab) {
    var _fromAmount = GlobalConvertToDecimal(ruleDetail.FromAmount);
    var _toAmount = GlobalConvertToDecimal(ruleDetail.ToAmount);

    // Set MaxRange of Collection Rule
    if (_toAmount > maxRuleDetailRange)
        maxRuleDetailRange = _toAmount;

    // Compare Amount
    if (_fromAmount >= _toAmount) {
        showAlert("error", CompareAmountValidationMsg.format(_toAmount, _fromAmount));
        isvalid = false;
        return false;
    }
    // Compare Range    
    isvalid = compareRange(ruleDetailList, _fromAmount, _toAmount, tab);
    if (!isvalid) {
        showAlert("error", CompareRangeValidationMsg.format(errorTitle, GlobalFormat(_fromAmount), GlobalFormat(_toAmount)));
        return false;
    }
    return true;
}

function compareRange(arr, currentFromAmount, currentToAmount, tab) {

    var isconflict = false;
    var centerPoint = 0;

    $.each(arr, function (key, value) {

        var arrFromAmount = GlobalConvertToDecimal(arr[key].FromAmount);
        var arrToAmount = GlobalConvertToDecimal(arr[key].ToAmount);

        centerPoint = (arrFromAmount + arrToAmount) / 2;
        if (currentFromAmount >= arrFromAmount && currentFromAmount <= arrToAmount && arr[key].Tab == tab) {
            isconflict = true;
            return false;
        }
        else if (currentFromAmount <= arrFromAmount && currentToAmount >= arrToAmount && arr[key].Tab == tab) {
            isconflict = true;
            return false;
        }
    });
    if (isconflict)
        return false;

    return true;
}