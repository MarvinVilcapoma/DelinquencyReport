/// <reference path="chatclient.js" />
var isRedirectOrRefresh = false;
var isRedirectRequest = false;

$(document).on('click', '.chatcollapse', function (e) {
    var divid = this.id.split('_');
    $("#" + divid[1]).slideToggle("slow");
    e.preventDefault();
});

$(function () {

    showActiveChatDivs(false, false, false);

    if (performance.navigation.type == 1 || (document != undefined && document.referrer != undefined && document.referrer != '')) {
        isRedirectOrRefresh = true;
        $("#hdnVToConId").val('');

    } else {
        $("#hdnVToConId").val('');
        if (readLocaleStorage('numberOfActiveChatTabInMUN') == null || parseInt(readLocaleStorage('numberOfActiveChatTabInMUN')) == 0) {
            eraseLocaleStorage('connOfAgentAndVisitorInMUN');
        }
    }

    window.addEventListener("beforeunload", function (e) {
        var targetURL = document.activeElement.href;
        if (readLocaleStorage('numberOfActiveChatTabInMUN') != 'undefined' && !isNaN(readLocaleStorage('numberOfActiveChatTabInMUN')) && readLocaleStorage('numberOfActiveChatTabInMUN') != undefined && readLocaleStorage('numberOfActiveChatTabInMUN') != null && readLocaleStorage('numberOfActiveChatTabInMUN') != '') {
            isRedirectRequest = false;
            if (targetURL == undefined || window.location.href != targetURL) {
                if (targetURL != undefined && targetURL != null && (targetURL.indexOf('Login') != -1 || targetURL.indexOf('ServiceRequest') != -1 || targetURL.indexOf('LogOff') != -1)) {
                    isRedirectRequest = false;
                    closeMultipleTabChat();
                }
                setNumberOfTab(function (result) {
                    if (parseInt(readLocaleStorage('numberOfActiveChatTabInMUN')) <= 0) {
                        eraseLocaleStorage('numberOfActiveChatTabInMUN');
                        if (targetURL == undefined || targetURL == null || targetURL == '') {                            
                            $('#hdnChatUserId').val('');
                        }

                        if (HubClone != undefined && HubClone != null) {
                            HubClone.server.setUserDisconnected($("#hdnVConId").val(), isRedirectRequest);
                        }

                        if (targetURL != undefined && targetURL != null && targetURL.indexOf('ChatDashBoard') == -1) {
                            eraseLocaleStorage('chatAccepted');
                        }
                        UpdateUserStatus(false);
                    }
                });
            }
        }
    });

    if (readLocaleStorage('connOfAgentAndVisitorInMUN') != 'undefined' && readLocaleStorage('connOfAgentAndVisitorInMUN') != undefined && readLocaleStorage('connOfAgentAndVisitorInMUN') != "" && readLocaleStorage('connOfAgentAndVisitorInMUN') != null) {
        isRedirectRequest = true;
        $('.right-sidebar-toggle').click();
    }

});

function setNumberOfTab(callback) {
    var numberOfOpenTab = parseInt(readLocaleStorage('numberOfActiveChatTabInMUN'));
    if (numberOfOpenTab > 0) {
        numberOfOpenTab = parseInt(numberOfOpenTab) - 1;
        eraseLocaleStorage('numberOfActiveChatTabInMUN');
        createLocaleStorage('numberOfActiveChatTabInMUN', numberOfOpenTab);
    }
    callback();
}

$('.right-sidebar-toggle').click(function () {
    $('#right-sidebar').toggleClass('sidebar-open');
    chatShowLoading();
    if ($("#right-sidebar").hasClass("sidebar-open")) {

        if (detectIE() != '') {
            showIEMessage();
            $('#dvBOT').html('<div class="ibox-title ibox-heading iechatmessageclass"><h4>' + $('#hdnIEChatMessage').val() + '</h4></div>');
            chatHideLoading();
        }
        else {
            GetGlobJs(true, function (response) {
                if (response) {                    
                    if ($('#hdnVConId').val() != '') {
                        $('#txtMessage').removeAttr('disabled');
                    }
                    else {
                        $('#txtMessage').prop('disabled', true);
                    }
                }
            });
        }
    }
    else {
        isRedirectRequest = false;
        removeblinkclass();
        UpdateUserStatus(false);
        $('#hdnVToConId').val('');
        eraseLocaleStorage('connOfAgentAndVisitorInMUN');
        eraseLocaleStorage('VisitorUserID');
        eraseLocaleStorage("numberOfActiveChatTabInMUN");
        if (typeof HubClone != "undefined" && HubClone != undefined && HubClone !== null) {
            closeMultipleTabChat();
        }
        chatHideLoading();
    }
});

function ClientChatTabOpen() {
    if (HubClone != undefined && HubClone != null) {
        HubClone.server.allVisitorRefresh();
    }
}

function detectIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');

    var browsername = '';

    if (msie > 0) {
        // IE 10 or older => return version number
        browsername = 'IE ' + parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    var trident = ua.indexOf('Trident/'); if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        browsername = 'IE ' + parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    var edge = ua.indexOf('Edge/'); if (edge > 0) {
        // Edge (IE 12+) => return version number
        browsername = 'Edge ' + parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
    }

    // other browser
    return browsername;
}

function UpdateUserStatus(userstatus) {
    if (readLocaleStorage("VisitorUserID") != 'undefined' && readLocaleStorage("VisitorUserID") != undefined && readLocaleStorage("VisitorUserID") != null && readLocaleStorage("VisitorUserID") != '') {
        $.ajax({
            url: ROOTPath + '/ChatAndBot/Client/UpdateUserStatus',
            data: { id: readLocaleStorage("VisitorUserID"), isOnline: userstatus, CompanyID: $("#hdnChatUserCompanyID").val(), Language: $("#hdnLanguage").val() },
            type: "GET",
            async: false,
            dataType: "json",
            success: function (data) {
            }
        });
    }
}

function removeblinkclass() {
    $("#clientchatlogo").removeClass('blinkclientchat');
    $("#clientchatlogo").removeClass('blinkclienthatinfinite');
}

function ChatActiveTabNumberSet() {
    if (readLocaleStorage('numberOfActiveChatTabInMUN') != 'undefined' && !isNaN(readLocaleStorage('numberOfActiveChatTabInMUN')) && readLocaleStorage('numberOfActiveChatTabInMUN') != undefined && readLocaleStorage('numberOfActiveChatTabInMUN') != null && readLocaleStorage('numberOfActiveChatTabInMUN') != '' && parseInt(readLocaleStorage('numberOfActiveChatTabInMUN')) > 0) {
        var numberOfOpenTab = parseInt(readLocaleStorage('numberOfActiveChatTabInMUN'));
        numberOfOpenTab = parseInt(numberOfOpenTab) + 1;
        eraseLocaleStorage('numberOfActiveChatTabInMUN');
        createLocaleStorage('numberOfActiveChatTabInMUN', numberOfOpenTab);
    }
    else {
        eraseLocaleStorage('numberOfActiveChatTabInMUN');
        createLocaleStorage('numberOfActiveChatTabInMUN', 1);
    }
}

function closeMultipleTabChat() {
    if (HubClone != undefined && HubClone != null) {
        HubClone.server.setAllTabsOfUserAsDisconnected($("#hdnVConId").val());
        eraseLocaleStorage('numberOfActiveChatTabInMUN');
    }
}

function GetSignalR(callback) {
    $.ajax({
        url: ROOTPath + '/Scripts/jquery.signalR-2.2.1.js',
        dataType: "script",
        async: false,
        success: function (data) {
            callback(true);
        },
        error: function () {
            console.log('error');
            callback(false);
        }
    });
}

function GetHubs(callback) {
    GetSignalR(function (result) {
        if (result) {
            $.ajax({
                url: $('#hdnChatHubUrl').val() + '/hubs',
                dataType: "script",
                async: false,
                success: function (data1) {
                    chatHub = $.connection.chatHub;
                    $.connection.hub.url = $("#hdnChatHubUrl").val();
                    callback(true);
                },
                error: function () {
                    showActiveChatDivs(true, false, false);
                    chatHideLoading();
                    callback(false);
                }
            });
        }
    });
}

function GetGlobalJs(callback) {
    GetHubs(function (result) {
        if (result) {
            $.ajax({
                url: ROOTPath + '/Scripts/Culture/jquery.global.js',
                dataType: "script",
                async: false,
                success: function (data) {
                    callback(true);
                }
            });
        }
    });
}

function GetGlobJs(isLogedinUser, callback) {
    GetGlobalJs(function (result) {
        if (result) {
            $.ajax({
                url: ROOTPath + '/Scripts/Culture/jQuery.glob.min.js',
                dataType: "script",
                async: false,
                success: function (data) {
                    if (typeof startChat == 'undefined') {
                        $.ajax({
                            url: ROOTPath + '/Scripts/app/ChatClient.js',
                            dataType: "script",
                            async: false,
                            success: function (data2) {
                                CheckAgentIsOnline(null, function (response) {                                    
                                    callback(true);
                                });
                            }
                        });
                    }
                    else {
                        CheckAgentIsOnline(null, function (response) {                            
                            callback(true);
                        });
                    }
                }
            });
        }
    });
}

function showIEMessage() {
    showActiveChatDivs(false, false, true);
}

function showActiveChatDivs(isAgentOfflineUserOnline, isAgentOnlineUserOnline, isIEMessage) {
    if (isAgentOfflineUserOnline) {
        $("#divAgentOfflineUserOnline").show();
    }
    else {
        $("#divAgentOfflineUserOnline").hide();
    }
    if (isAgentOnlineUserOnline) {
        $("#divAgentOnlineUserOnline").show();
    }
    else {
        $("#divAgentOnlineUserOnline").hide();
    }
    if (isIEMessage) {
        $("#divIEMessage").show();
        $("#divIEMessage").html("<p>" + $('#hdnIEChatMessage').val() + "</p>");
    }
    else {
        $("#divIEMessage").hide();
    }
}

function createLocaleStorage(name, value) {
    localStorage.setItem(name, value);
}

function readLocaleStorage(name) {
    return localStorage.getItem(name);
}

function eraseLocaleStorage(name) {
    localStorage.removeItem(name);
}