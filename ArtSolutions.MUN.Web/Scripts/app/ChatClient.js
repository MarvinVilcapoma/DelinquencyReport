/*Hub methods*/

var HubClone;

function startChat() {
    // Declare a proxy to reference the hub.
    var chatHub = $.connection.chatHub;
    $.connection.hub.url = $("#hdnChatHubUrl").val();
    //$.connection.hub.url = 'http://localhost:64065/signalr';
    registerClientMethods(chatHub);
    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(chatHub);
        $.ajax({
            url: ROOTPath + '/ChatAndBot/Chat/AddVisitor',
            type: "GET",
            async: false,
            dataType: "json",
            success: function (data) {
                if ($('#hdnChatUserId').val() !== '') {

                    var objConnectInfo = {
                        UserName: data.Name,
                        UserID: data.ID,
                        Message: '',
                        IsBusy: false,
                        visitormodel: data,
                        IsAgent: false,
                        CompanyID: data.CompanyID,
                        isFirstTimeLoading: true,
                        Language: data.Language,
                        UserGUID: $('#hdnChatUserId').val(),
                        Email: data.Email,
                        VIPAddress: readLocaleStorage('VIPAddress'),
                        VCountry: readLocaleStorage('VCountry'),
                        VCity: readLocaleStorage('VCity'),
                        VRegion: readLocaleStorage('VRegion'),
                        isRedirectRequest: isRedirectRequest,
                        VisitorTimeZone: new Date().toString().match(/\((.*)\)/).pop()
                    };

                    chatHub.server.connect(objConnectInfo);
                }
                if ($('#hdnChatUserId').val() !== '' && readLocaleStorage('connOfAgentAndVisitorInMUN') !== undefined && readLocaleStorage('connOfAgentAndVisitorInMUN') !== null && readLocaleStorage('connOfAgentAndVisitorInMUN') !== 'null') {
                    var cookieValue = readLocaleStorage('connOfAgentAndVisitorInMUN');
                    var array = cookieValue.split('_');
                    if (array[0] !== "") {
                        $("#hdnVToConId").val(array[0]);
                    }
                    if ($("#hdnVToConId").val() !== '') {
                        $('#txtMessage').removeAttr('disabled');
                    }
                }
            }
        });
        chatHideLoading();
    });
    $(document).unbind('keypress');
    $("#txtMessage").unbind('keypress');

    $("#txtMessage").keypress(function (e) {
        if (e.which === 13) {
            sendMessageByVisitor();
        }
    });
    $("#btnSend").unbind('keypress');
    $("#btnSend").click(function () {
        sendMessageByVisitor();
    });

    HubClone = chatHub;
}

function sendMessageByVisitor() {
    if ($.trim($("#txtMessage").val()) !== '') {
        if ($('#hdnVConId').val() !== '') {
            chatHub.server.sendPrivateMessage($("#hdnVToConId").val(), $("#txtMessage").val(), false, $("#hdnLanguage").val(), readLocaleStorage('VIPAddress'), readLocaleStorage('VCountry'), readLocaleStorage('VRegion'), readLocaleStorage('VCity'), __culture, $('#hdnChatUserId').val());
        }
        setVisitorScroll();
    }
}

function registerClientMethods(chatHub) {
    // Calls when user successfully logged in
    chatHub.client.onConnected = function (id, UserName, Message, CompanyID, UserID) {
        $('#hdnVConId').val(id);
        createLocaleStorage("VisitorUserID", UserID);
        $('#hdnVUserName').html(UserName);
        GetChatHistory(UserID);
        if ($('#hdnChatUserId').val() !== '') {
            eraseLocaleStorage("connOfAgentAndVisitorInMUN");
            createLocaleStorage("connOfAgentAndVisitorInMUN", $("#hdnVToConId").val() + "_" + $('#hdnChatUserId').val());
        }
        if (Message !== "") {
            AddMessage(UserName, Message, 'right', false, '@Common.CurrentDateTime.ToString("dd MMM yyyy hh:mm tt")', 'Content/Images/a4.jpg');
        }
    };

    // On New User Connected
    chatHub.client.onNewUserConnected = function (id, name) {
        AddUser(chatHub, id, name);
    };

    // On User Disconnected
    chatHub.client.onUserDisconnected = function (id, userName) {
        $('#' + id).remove();
        var ctrId = 'private_' + id;
        $('#' + ctrId).remove();
        var disc = $('<div class="disconnect">"' + userName + '" logged off.</div>');
        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);
    };

    chatHub.client.messageReceived = function (userName, message) {
        AddMessage(userName, message);
    };

    chatHub.client.CallerPrivateMessage = function (fromUserId, UserName, Message) {
        AddMessage(UserName, Message, 'right', false, CurrentDate);
    };

    chatHub.client.sendPrivateMessage = function (ToUserId, UserName, Message, fromAgent, CurrentDate, dtCurrentDate, ProfilePath, ImagePath) {
        var imageurl;
        if (fromAgent !== false) {
            $("#hdnVToConId").val(ToUserId);
            imageurl = ProfilePath;
        }
        else {
            if ($("#hdnVToConId").val() === "") {
                $("#hdnVToConId").val(ToUserId);
            }
            if ($("#hdnChatUserProfileImage").val() === 0) {

                if (ROOTPath === '/') {
                    imageurl = '/' + $("#hdnNoImage").val();
                }
                else {
                    imageurl = ROOTPath + '/' + $("#hdnNoImage").val();
                }
            }
            else {
                imageurl = ImagePath + $("#hdnChatUserProfileImage").val();
            }
        }
        var positon = (fromAgent) ? 'left' : 'right';

        var culwiseDate = GetLocalDateTimeForChat(CurrentDate, dtCurrentDate);
        var culwisetime = fnGetCultureDateForChat(culwiseDate, true, false);
        AddMessage(UserName, Message, positon, fromAgent, culwisetime, imageurl);
    };

    chatHub.client.onAgentConnectedSetID = function (AgentConnectionID) {
        setconnOfAgentAndVisitorInMUN(AgentConnectionID);
        if ($('#hdnVToConId').val() === '') {
            SetStatusMessage($("#hdnAgentOnlineMessage").val(), false);
        }
        $("#hdnVToConId").val(AgentConnectionID);
        $('#txtMessage').removeAttr('disabled');
    }

    chatHub.client.OnTransferToAgentConnectionSet = function (AgentConnectionID, Name) {
        $("#hdnVToConId").val(AgentConnectionID);
        setconnOfAgentAndVisitorInMUN(AgentConnectionID);
        $("#chathistory").children("div:last").remove();
        SetStatusMessage($("#hdnTransferMessage").val() + Name + ".", false);
    };

    chatHub.client.ChatRefreshToVisitorSetAgentID = function (AgentConnectionID) {
        setconnOfAgentAndVisitorInMUN(AgentConnectionID);
        $('#txtMessage').removeAttr('disabled');
    };

    chatHub.client.onAgentDisConnectedMessage = function () {
        SetStatusMessage($("#hdnAgentOfflineMessage").val(), false);
        $("#hdnVToConId").val('');
        setconnOfAgentAndVisitorInMUN('');
        $('#txtMessage').prop('disabled', true);
    };

    chatHub.client.AddVisitorCookie = function (userGUID) {
        createLocaleStorage("connOfAgentAndVisitorInMUN", $("#hdnVToConId").val() + "_" + userGUID);
        $('#hdnChatUserId').val(userGUID);
    };

    chatHub.client.CloseCurrentTabChat = function () {
        if ($("#right-sidebar").hasClass("sidebar-open")) {
            $('#right-sidebar').toggleClass('sidebar-open');
            removeblinkclass();
            eraseLocaleStorage('currentUserActiveChatTab');
        }
    };

    chatHub.client.RefreshCurrentTabChat = function () {
        CheckAgentIsOnline(true, function (result) { });
    }

    chatHub.client.GetAgentID = function (VisitorID) {
        chatHub.server.setExistVisitorAgentID(VisitorID, $("#hdnVToConId").val());
    };

    chatHub.client.onExistingUserConnectedSetAgentID = function (AgentConnectionID) {
        $("#hdnVToConId").val(AgentConnectionID);
        setconnOfAgentAndVisitorInMUN(AgentConnectionID);
        if (isRedirectOrRefresh === false) {
            SetStatusMessage($("#hdnAgentOnlineMessage").val(), false);
        }
        $('#txtMessage').removeAttr('disabled');
    };

}

function registerEvents(chatHub) {
    $('.right-sidebar-toggle').click(function () {
        if (!$("#right-sidebar").hasClass("sidebar-open")) {
            eraseLocaleStorage("connOfAgentAndVisitorInMUN");
            eraseLocaleStorage("VisitorUserID");
            if ($("#clientLoginwindowTitle").hasClass('hide')) {
                chatHub.server.setUserDisconnected($("#hdnVConId").val(), false);
                eraseLocaleStorage("connOfAgentAndVisitorInMUN");
                eraseLocaleStorage("VisitorUserID");
            }
        }
        else {
            $('#hdnVToConId').val('');
        }
    });
}

function CheckAgentIsOnline(isRefreshChat, callback) {
    $(document).unbind('keypress');
    $.ajax({
        url: ROOTPath + '/ChatAndBot/Chat/GetOnlineAgent',
        type: "GET",
        async: false,
        dataType: "json",
        success: function (result) {
            if (result !== null && typeof result === "object" && result !== undefined) {
                ChatActiveTabNumberSet();

                if (result.agentList !== "" && result.agentList !== undefined && result.agentList.length > 0) {
                    showActiveChatDivs(false, true, false);
                    startChat();
                    setVisitorScroll();
                }
                else {
                    if (result.data !== "" && result.data !== undefined) {
                        $('#divChatTab').empty();
                        $('#divChatTab').html(result.data);

                        if ($("script[src*='unobtrusive']").length) {
                            $.validator.unobtrusive.parse('#frmChatServiceRequest');
                        }
                        
                        fnCheckAndCreateScripts();
                        $('#right-sidebar').toggleClass('sidebar-open');
                        if ($('#hdnChatUserId').val() !== '') {
                            $("#frmChatServiceRequest #RequesterUserID").val($('#hdnChatUserId').val());
                        }
                    }
                    
                    showActiveChatDivs(true, false, false);

                    if (isRefreshChat !== null) {
                        SetStatusMessage($("#hdnAgentOfflineMessage").val(), false);
                        $("#hdnVToConId").val('');
                        if ($('#hdnVToConId').val() === '') {
                            $('#txtMessage').prop('disabled', true);
                        }
                        chatHub.server.checkVisitorIsChatWithAgentOnAnotherLocation();
                    }
                }
            }
            callback(true);
        },
        error: function () {
            callback(false);
        }
    });
}

function SetStatusMessage(message, isWelcomeMessage) {
    var chatmsgclass = 'chat-messageOffline';
    if (isWelcomeMessage === true) {
        chatmsgclass += ' welcomeMessage';
    }
    $('#chathistory').append('<div>' + '<div class="text-center ' + chatmsgclass + '">' + message + '</div>' + '</div>');
    $('#txtMessage').val('');
    setVisitorScroll();
}

function AddMessage(userName, message, position, isactive, CurrentDate, imageurl) {
    $('#chathistory').append("<div class='sidebar-message'>" +
        "<div class='pull-left text-center'>" +
        "<img alt = 'image' class='img-circle message-avatar' src='" + imageurl + "'>" +
        "</div>" +
        "<div class='media-body'>" +
        "<strong>" + userName + "</strong><br><p class='headerchatMessage'>"
        + message +
        "</p><br>" +
        "<small class='text-muted'>" + CurrentDate + "</small>" +
        "</div>" +
        "</div>");
    $('#txtMessage').val('');

    if (isactive) {
        blinking();
    }
    else {
        removeblinkclass();
    }
    setVisitorScroll();
}

function GetChatHistory(ChatUserID) {
    $.ajax({
        url: ROOTPath + '/ChatAndBot/Chat/GetChatHistory',
        data: { ChatUserID: ChatUserID, IsVisitor: true },
        type: "GET",
        async: false,
        dataType: "json",
        success: function (data) {
            $('#chathistory').html('');
            var PrevDate = data.PreviousDate;
            var CurrentDate = data.CurrentDate;
            $('#chathistory').append(conversationClientHistory(data.Conversation));
            if (window.performance) {
                //console.info("window.performance work's fine on this browser");
            }
            if (performance.navigation.type === 1) {
                //console.info("This page is reloaded");
            } else {
                if (readLocaleStorage('numberOfActiveChatTabInMUN') !== 'undefined' && readLocaleStorage('numberOfActiveChatTabInMUN') !== undefined && !isNaN(readLocaleStorage('numberOfActiveChatTabInMUN')) && readLocaleStorage('numberOfActiveChatTabInMUN') !== undefined) {
                    //&& readLocaleStorage('currentUserActiveChatTab') != null && readLocaleStorage('currentUserActiveChatTab') != ''
                    if (readLocaleStorage('numberOfActiveChatTabInMUN') === 1) {
                        if ($(".welcomeMessage").length === 0) {
                            SetStatusMessage($('#hdnWelComeChatMessage').val(), true);
                        }
                        if ($('#hdnVToConId').val() === '') {
                            $('#txtMessage').prop('disabled', true);
                        }
                        chatHub.server.checkVisitorIsChatWithAgentOnAnotherLocation();
                    }
                    else {
                        if ($('#hdnVToConId').val() === '') {
                            $('#txtMessage').prop('disabled', true);
                        }
                    }
                }               
            }

            setVisitorScroll();
        }
    });
}

function GetTicketData() {
    /// <summary>Used to fill ticket dropdown based on department - cascading</summary>     

    $.ajax({
        type: "GET",
        url: ROOTPath + '/ChatAndBot/ServiceRequest/GetTicketTypeAndDeptUserIDByDepartmentID',
        data: {
            departmentID: $("#frmChatServiceRequest #DepartmentID").val()
        },
        success: function (response) {
            if (response !== null && typeof response === "object" && response !== undefined) {
                if (response.html !== null && response.html !== undefined) {
                    $("#TicketTypeID").html(response.html);                    
                    $('#TicketTypeID').val($("#TicketTypeID").val()).trigger('change'); //code change for select2 4.0.6 (CO-721)
                }
                if (response.data !== null && response.data !== undefined) {
                    $("#frmChatServiceRequest #AgentID").val(response.data);
                }
            }
        },
        error: function () { }
    });

}

function ServiceRequestInsertSuccess(response) {
    /// <summary>Used to display message after successfully inserting service request data</summary>  
    /// <param name="response" type="string">response which we get from server side after save service request data</param>
    if (response !== null && typeof response === "object" && response !== undefined) {
        callAlert(response);
        if ($("#clientLoginwindowBox").length === 1) {
            $("#DepartmentID-error").addClass('hide');
            $("#TicketTypeID-error").addClass('hide');
        }
        if (response.data !== "" && response.data !== undefined) {
            if ($("#spnTicketID").length && $("#divTicketMessage").length) {
                $("#spnTicketID").text(response.data);
                if ($("#divTicketMessage").hasClass('hidden'))
                    $("#divTicketMessage").removeClass('hidden');
                
                $('#DepartmentID').val(null).trigger('change'); //code change for select2 4.0.6 (CO-721)
                $('#TicketTypeID').val(null).trigger('change'); //code change for select2 4.0.6 (CO-721)


                $("#select2-DepartmentID-container").parent('span').removeClass('select2-validation-error');
                $("#select2-TicketTypeID-container").parent('span').removeClass('select2-validation-error');

                if ($("#frmChatServiceRequest").length)
                    setTimeout(function () { $("#frmChatServiceRequest")[0].reset(); }, 1000);

            }
        }
    }
}

function fnInitializeShareScreen() {
    // <summary>Used to initialize share screen</summary>  

    if (!$("script[src*='Surfly']").length) {

        (function (s, u, r, f, l, y) {
            s[f] = s[f] || { init: function () { s[f].q = arguments } };
            l = u.createElement(r); y = u.getElementsByTagName(r)[0]; l.async = 1;
            l.src = 'https://surfly.com/surfly.js'; y.parentNode.insertBefore(l, y);
        })
            (window, document, 'script', 'Surfly');
    }
    $.ajax({
        url: 'https://api.surfly.com/v2/sessions/?api_key=5a722cdfcab84f94ae467ca832f95123',
        type: 'POST',
        dataType: 'json',
        data: {
            url: (location.hostname === 'localhost' || location.hostname === '127.0.0.1' ?
                'http://art-rule-dev.azurewebsites.net/' :
                window.location.href
            )
        },
        success: function (data) {

            if (data !== null && data !== undefined && typeof data === "object") {
                var eventObj = $.Event('keypress');
                eventObj.which = 13;
                var strLink = '<a href="' + data.leader_link + '" target="_blank">' + data.viewer_link + '</a>';
                $("#txtMessage").val(strLink);
                $.event.trigger(eventObj);
            }
        }
    });

}

function blinking() {
    if ($(document.activeElement).filter(':focus').context.id === "#txtMessage" || $(document.activeElement).filter(':focus').context.id === "txtMessage") {
        $("#clientchatlogo").addClass('blinkclientchat');
    }
    else {
        $("#clientchatlogo").addClass('blinkclienthatinfinite');
    }
}

function removeblinkclass() {
    $("#clientchatlogo").removeClass('blinkclientchat');
    $("#clientchatlogo").removeClass('blinkclienthatinfinite');
}

function conversationClientHistory(Conversation) {
    var conversation = '';
    var ComBo = '';

    for (var i = 0; i < Conversation.length; i++) {

        var culwiseDate = GetLocalDateTimeForChat(Conversation[i].MessageTime, Conversation[i].strMessageTime);
        var culwisetime = fnGetCultureDateForChat(culwiseDate, true, false);
        var isNewCombo = false;

        if (ComBo === '' || ComBo !== Conversation[i].ComboID) {
            isNewCombo = true;
        }

        if (isNewCombo === true) {
            if (conversation !== '') {
                conversation += '</div>'; // end datewise conversation
            }
            if (Conversation[i].ComboID !== '' && Conversation[i].ComboID !== null) {
                conversation += '<div class="text-center chat-DateTitle">' +  // start date Title
                    '<a class="text-center ' + Conversation[i].CollapseClass + '" id="btn_' + Conversation[i].ComboID + '">' + Conversation[i].Title + '</a>' +
                    '</div>' + // start date Title
                    '<div ' + Conversation[i].HideClass + ' id="' + Conversation[i].ComboID + '">'; // start datewise conversation
                ComBo = Conversation[i].ComboID;
            }
            else {
                conversation += '<div class="text-center chat-DateTitle">' + Conversation[i].Title + '</div>';
            }
        }

        if (Conversation[i].Message !== '' && Conversation[i].Message !== null && Conversation[i].UserName !== '' && Conversation[i].UserName !== null) {
            conversation +=
                '<div class="sidebar-message">' +
                '<div class="pull-left text-center">' +
                '<img alt="image" class="img-circle message-avatar" src=' + Conversation[i].ImageSrc + '>' +
                '</div>' +
                '<div class="media-body">' +
                '<strong>' + Conversation[i].UserName + '</strong><br><p class="headerchatMessage">' + Conversation[i].Message + '</p><br>' +
                '<small class="text-muted">' + culwisetime + '</small>' +
                '</div>' +
                '</div>';
        }
    }
    if (conversation !== '') {
        conversation += '</div>'; // end datewise conversation
    }
    return conversation;
}

function setconnOfAgentAndVisitorInMUN(AgentConnectionID) {
    eraseLocaleStorage("connOfAgentAndVisitorInMUN");
    if ($('#hdnChatUserId').val() !== '') {
        createLocaleStorage("connOfAgentAndVisitorInMUN", AgentConnectionID + "_" + $('#hdnChatUserId').val());
    }
}

function setVisitorScroll() {
    var itemContainer = $("#scrollDiv");
    var scrollDown_int = $('#scrollDiv')[0].scrollHeight;
    itemContainer.slimScroll({
        height: ($(window).height() - 239) + 'px',
        scrollTo: scrollDown_int + 'px',
        start: 'bottom'
    });
}