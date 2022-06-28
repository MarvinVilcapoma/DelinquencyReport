var slider1 = null, slider2 = null, oldthemepath = null, SamplesList = null, editcontrolpath = null, selthemecolor = null, cssheight = null; window.theme = "saffron", minScrollerHeight = 550;
window.themecolor = ""; window.themestyle = ""; window.themevarient = "";

$(function () {
    var sbModel = new ViewModel(), isloadLeft = false;
    this.PathLoc = null, this.themeonchange=false;
    window.theme = "saffron";
    //debugger
    if ((readCookie('themeName') != null) && (readCookie('themeName') != ""))
        window.theme = readCookie('themeName');
    if ((readCookie('themeColor') != null) && (readCookie('themeColor') != ""))
        window.themecolor = readCookie('themeColor');
    $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + window.themecolor);
    if ((readCookie('themeStyle') != null) && (readCookie('themeStyle') != ""))
        window.themestyle = readCookie('themeStyle');
    if ((readCookie('themeVarient') != null) && (readCookie('themeVarient') != ""))
        window.themevarient = readCookie('themeVarient');
    if (window.themevarient == 'dark') $("body").attr("class", window.themevarient + "theme"); else $("body").attr("class", "");
    $(function () {
        var control = "", categories = [], currentSample, theme = "";
        var urlread = window.location.toString().toLowerCase(), cur_sample = "";

        for (i = 0; i < window.SampleControls.length; i++) {
            if (urlread.indexOf("/" + window.SampleControls[i].name.toLowerCase() + "/") >= 0) {
                control = window.SampleControls[i].name.toLowerCase();
                cur_sample = urlread.substr(urlread.lastIndexOf("/") + 1, urlread.length);
                if (cur_sample == control)
                    cur_sample = window.SamplesList.filter(function (e) { if (e.id == control) return e })[0].action
                break;
            }
        }

        categories.push(cur_sample);
        isloadLeft = true;
        currentSample = categories.pop();
        control !== "" ? loadSamplePage(control, currentSample, categories): showTooltipbox();        
        updateSBTheme();
        updateTheme(window.theme);
        //replaceTheme();
    });
    $.views.converters("ensureURL", function (val) {
        if (val != null) return val.toLowerCase(); else return val;
            });
    $.views.converters("Upperstring", function (val) {
        return val.toUpperCase();
    });
	$.views.converters("RemoveWhiteSpace", function(val) {
	  return val.replace(/ /g, "") ;
	});
    var updateSBTheme = function () {
        if (window.theme.indexOf("lime") != -1) {
            $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + "lime");
            $("#themebutton").ejButton('option', 'prefixIcon', 'e-icons ' + "lime");
        }
        else if (window.theme.indexOf("saffron") != -1) {
            $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + "saffron");
            $("#themebutton").ejButton('option', 'prefixIcon', 'e-icons ' + "saffron");
        }
    };
    var loadSamplePage = function (control, currentSample, categories) {
        if (control == null) {
            $("#samplesDiv").css("display", "none");
            $(".samples").css("display", "none");
            self.loadSBMainPage(null);
            return;
        }

        var sample = findSample(control, currentSample, categories), name = sample, parentId = null;

        while (!ej.isNullOrUndefined(name) && name.samples) {
            if (name.samples) {
                parentId = parseInt(name.id) ? name.id : null;
                name = name.samples[0];
                if( !ej.isNullOrUndefined(name) && name.hasOwnProperty('action'))
                     currentSample = name.action;
            } else
                break;
        }


        if ($("#sbtooltipbox").data("ejDialog"))
            $("#sbtooltipbox").ejDialog("close");

        if (isloadLeft || Number(name.id) <= 1)
            self.loadLeftTab(control, currentSample);
        var sampleurl = "/" + control + "/" + currentSample;
        if(!ej.isNullOrUndefined(name))
            self.loadSamplePage(sampleurl, control, currentSample, parentId, name.id);
    };

    var findSample = function (control, currentSample, categories) {
        var currentcontrollist = null, subcontrollist = null;
        $(SamplesList).each(function () {
            if ($(this)[0].controller.toString().toLowerCase() == control.toLowerCase()) {
                currentcontrollist = $(this);
            }

        });

        $($(currentcontrollist)[0].samples).each(function () {
            if ($(this)[0].samples) {
                for (var i = 0; i < $(this)[0].samples.length; i++) {
                    if ($(this)[0].samples[i].action.toLowerCase() == currentSample.toLowerCase()) {
                        categories.push($(this)[0].name);
                        break;
                    }
                }
            }

        });

        var sample = ej.Query().using(ej.DataManager(SamplesList)).where("controller", "==", control, true), current = sample, res;

        for (var k = 0; k < categories.length; k++) {
            current.hierarchy((current = ej.Query("samples").where("name", "==", categories[k], true)));
        }

        if (currentSample)
            current.hierarchy(ej.Query("samples").where("action", "==", currentSample, true));

        res = sample.executeLocal();

        if (res.length) res = res[0];
        return res;
    };
    $("#Res-prodnav").on("click", function (e) {
        if ($(window).width() < 900 && !($(this).hasClass("selectIt")) && $(".product-naviation").hasClass("hideIt"))
            $(this).addClass("selectIt");
        else if (!$(".product-naviation").hasClass("hideIt"))
            $(".product-naviation").removeClass("hideIt");
    });
    $("#scrollcontainer").on("click", ".secondlevelload, .anchorclass", function (e) {
        var hashvalue = $(e.currentTarget).attr('hashbang');
        var categorieslist = hashvalue.slice(1, hashvalue.lastIndexOf("/"));
        eraseCookie("catalog");
        createCookie('catalog', categorieslist, 1);
        var viewportWidth = $(window).width();
        if (viewportWidth < 981) {
            $('.content-container-fluid .row > .navigation').addClass('collapsePanel');
            $('.accordion-panel').removeClass("expandPanel");
        }
    });

   
    $("#themebutton").ejButton({
        size: "normal",
        width: "60px",
        height: "55px",
        cssClass: "metroblue",
        click: "themebtnClick",
        contentType: "imageonly",
        prefixIcon: "e-uiLight e-icon-handup"
    });
    $("#buybutton").ejButton({
        size: "normal",
        width: "66px",
        height: "55px",
        cssClass: "metroblue",
        contentType: "imageonly",
        prefixIcon: "e-uiLight e-icon-handup",
		click:"downlinkclick"
    });
    $(document).delegate(".product-naviation div", "click", function (e) {
        viewdemo({text:e.target.innerHTML});
    });
    $("#sbtooltipbox").ejDialog({ height: "86px", width: "176px", enableResize: false, showOnInit: false, showHeader: false, cssClass: "metroblue", allowKeyboardNavigation :false });
    $("#themeDialog").ejDialog({ height: "160px", enableResize: false, showOnInit: false, showHeader: false, cssClass: "metroblue" });
    $("#metrotext").ejRadioButton({ value: "flat", size: "medium", name: "themestyle", cssClass: "metroblue", change: 'themeonchange' });
    $("#gradienttext").ejRadioButton({ cssClass: "metroblue", size: "medium", name: "themestyle", change: 'themeonchange' });
    $("#lighttext").ejRadioButton({ size: "medium", name: "themevarient", cssClass: "metroblue", change: 'themeonchange' });
    $("#darktext").ejRadioButton({ cssClass: "metroblue", name: "themevarient", size: "medium", change: 'themeonchange' });
    $("#bootstrapcheck").ejCheckBox({ size: "small", change:"bootstraponselect" });
    themeButtonSelect();
    var metroradio = $("#JobSearch").data("ejMenu");
    var firstlevelsamples = [];

    var isContentPageLoaded = null;
    var index = 0;

    function ViewModel() {
        this.Controls = SamplesList;
        this.controlname = "";
        this.controlName = null;
        this.sampleName = null;

        self.editSubItem = function (id, back) {
            var divid = id;
            removeSelectedItemcss();
            $("#subsamplesDiv").html('');
            $("#" + divid).css("margin-top", "0px");
            $("#subsamplesDiv").hide().css("left", "250px");
            $("#samplesDiv").css("left", "0px").show();
            $("#" + divid).show().css("visibility", "visible");
            $("#" + divid + "_back").css("display", "block");
            $("#" + divid + "_back").addClass("dashboarddiv");
            $("#" + divid).children(".subsamples").find("a >div").removeClass("dashboardhover");
            $("#samplesDiv").find("#" + divid).css("left", "-250px");
            $("#samplesDiv").find("#" + divid).children(".subsamples").show();
            $("#samplesDiv").find("#" + divid).animate({ left: '0px' }, 200);
            var samplename = null, controlname = null;
       
        };

        self.loadLeftTab = function (divid) {
            divid = divid + "_nav";
            if ($("#" + divid).prev().length > 0) {
                $("#" + divid).css("margin-top", "0px");
            }
            $("#accordion2").css("left", "-250px");
            $("#accordion2").prev().css("display", "none");
            $("#accordion2").css("display", "none");
            $("#subsamplesDiv").css("display", "none");
            $(".samples").hide();
            $("#samplesDiv").children("." + divid).css("display", "block");
            $("#samplesDiv").children("." + divid).children(".subsamples").show();
            $("#samplesDiv").children("." + divid).children(".subsamples").find("#subControls").hide();
            $("#" + divid + "_back").css("display", "block");
            $("#" + divid + "_header").css("display", "block");
            if ($("#currentheader").length > 0)
                $("#currentheader").remove();
            var element = $("#" + divid + "_back").clone();
            $(element).appendTo("#dashboardheader>a").attr("id", "currentheader");
            $("#" + divid + "_back").css("display", "none");
            if ($("#dashboardheader .current_control").length > 0)
                $("#dashboardheader .current_control").remove();
            $($("#" + divid + "_header").clone()).insertAfter("#dashboardheader>a").addClass("current_control").attr("id", "current_control");
            $("#" + divid + "_header").hide();
            $("#sbdashboard").hide();
            $("#samplesDiv").css("display", "block");
            $("#samplesDiv").css("margin-top", "0px");
            $("#samplesDiv").animate({ left: '0px' }, 0);
            $('html, body').animate({
                scrollTop: 0
            }, 0);
            $("#sbdashboard").hide();
            $(".accordion-panel").height($(".accordion-panel")[0].scrollHeight);
            refreshScroller();
        };


        self.findSample = function (ctrlname, samplename, subchild, currentsampleid) {
            var query = ej.Query().using(ej.DataManager(SamplesList))
                .where("id", "==", ctrlname, true), curr = query, res;

            if (subchild)
                query.hierarchy(
                    curr = ej.Query("samples")
                          .where("id", "==", subchild, true));

            curr.hierarchy(
                ej.Query("samples")
                    .where("id", "==", currentsampleid, true)
            );

            res = query.executeLocal();

            if (res.length) res = res[0];
            return res;

        },
        self.loadSamplePage = function (url, ctrlname, samplename, subchild, currentsampleid) {
            currentSamplepage = url;
            window.currentSamplepage = url;
            var sample = self.findSample(ctrlname, samplename, subchild, currentsampleid), sampleTitle = "",samplePath="";
            if ($("#auto").data("ejAutocomplete"))
                $("#auto").ejAutocomplete("clearText");
            $(".sampleheadingtext").empty();
            
            while (sample) {
                if (sampleTitle)
                    sampleTitle += " / ";

                sampleTitle += sample.name;

                sample = sample.samples && sample.samples[0];
            }

            var names = sampleTitle.split("/"), _samplename = names.pop();
            var _halfTitle = names.join("/") + "/ ";
            var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
            var serverconfig = links[0].href.substr(0, links[0].href.indexOf("Content") - 1);
            var sampletitleobj = ej.buildTag("div.samplename sbsamplename");
            ej.buildTag("span", _halfTitle).appendTo(sampletitleobj);
            ej.buildTag("span.sampletitle", _samplename).appendTo(sampletitleobj);
            var navigation = ej.buildTag("span.navigation-btn");
            var paclk = prevnext(ctrlname, samplename, 'prev');
            var ancprev = ej.buildTag("a.navprev", "", {}, { href:serverconfig+ paclk }).appendTo(navigation);
            var naclk = prevnext(ctrlname, samplename, 'next');
            var ancnext = ej.buildTag("a.navprev", "", {}, { href: serverconfig+naclk }).appendTo(navigation);
            var prevState = ej.buildTag("span.prev", "Prev").appendTo(ancprev);
            var nextstate = ej.buildTag("span.next", "Next ").appendTo(ancnext);
            navigation.appendTo(sampletitleobj);
            $(".sampleheadingtext:first").html(sampletitleobj);
            editcontrolpath = "/" + samplename;
            document.title = "Essential Studio for ASP.NET MVC : " + sampleTitle.replace(/\//g, "-") + " Demo";
            index = 0;
            
            $(".prev").ejButton({
                size: "mini",
                cssClass: "metroblue",
                contentType: "imageonly",
                prefixIcon: "e-rarrowleft-2x"
            });
            $(".next").ejButton({
                size: "mini",
                cssClass: "metroblue",
                contentType: "imageonly",
                prefixIcon: "e-rarrowright-2x"
            });

            var curr = $("#samplesDiv").children("." + ctrlname + "_nav").find("div[action='" + samplename + "']");
            if (curr) {
                curr.children('span.anchor').addClass("itemselected");
                curr.addClass("highlighttextbg");
                curr.addClass("selecteddashboard");
                }
            if (curr.length == 1) {
                var scrollercontrol = $("#scrollcontainer").ejScroller("instance");
                scrollercontrol.model.scrollTop = (curr.offset().top - $("#scrollcontainer>div:first-child").offset().top);
                scrollercontrol.refresh();
            }
          
        };


        self.loadSBMainPage = function (divid) {
            $(".samplesection").hide();
            if (divid != null) {
                $("#" + divid).css("visibility", "hidden");
                $("#" + divid + "_back").css("display", "none");
            }
            self.loadSBPage(divid);
            $(".accordion-panel").height($(".accordion-panel")[0].scrollHeight);
        };
        self.loadSBPage = function (divid) {
            $("#accordion2").prev().css("display", "block");
            $("#accordion2>a>.dashboardhover").removeClass("dashboardhover");
            $(".sourcecodetab").hide();
            if (divid != null)
                $("#" + divid).hide();
            $("#accordion2").show();
            $("#samplesDiv").css("left", "250px");
            $("#accordion2").animate({ left: 0 }, 200, function () {
                //self.setVisibility("cols-iframe", "productpage");
            });
            $(".accordion-panel").height($(".content-container-fluid").height() - ($(".htmljssamplebrowser>.header").height() + 5));
            $('html, body').animate({
                scrollTop: 0
            }, 0);
            $(".accordion-panel").height($(".accordion-panel")[0].scruollHeight);
        };

        self.setVisibility = function (oldpage, newpage) {
            $("." + newpage).show();
            $("." + oldpage).hide();
        };
        this.getCssClass = function (item) {
            var value = Number(item.childcount);

            if (value >= 1) {
                return 'arrow';
            }
            return;
        };
        this.getCssVisibility = function (item) {
            var value = Number(item.id);

            if (value != 1) {
                return 'hideParent';
            }
            return 'dashboard';
        };
        self.removeSelectedItemcss = function (ctrlname, controlid) {
            var obj = $(".itemselected");
            obj.each(function () {
                $(obj).removeClass('itemselected');
                $(obj).parent().removeClass('highlighttextbg');
            });
            $(".selecteddashboard").removeClass("selecteddashboard");
        };
    }
    var Mainlist = [];

    $(GroupingList).each(function () {
        Mainlist[this] = [];
    });
    
    var samplesdetails = SamplesList;
    $(samplesdetails).each(function (index1, sampleslist) {
        var smpls = {}, secsamples = [];
        smpls["id"] = sampleslist.id;
        smpls["name"] = sampleslist.name;
        smpls["controller"] = sampleslist.controller;
        smpls["action"] = sampleslist.action;
        smpls["childcount"] = sampleslist.childcount;
        smpls["type"] = sampleslist.type;
        $(sampleslist.samples).each(function (ind, subsampleslist) {
            var subsmpls = {};
            subsmpls["id"] = subsampleslist.id;
            subsmpls["name"] = subsampleslist.name;
            subsmpls["controller"] = subsampleslist.controller;
            subsmpls["action"] = subsampleslist.action;
            subsmpls["childcount"] = subsampleslist.childcount;
            subsmpls["type"] = subsampleslist.type;
            if (subsampleslist.childcount == 1)
                subsmpls["arrowclass"] = "arrow";
            else
                subsmpls["arrowclass"] = "";
            subsmpls["samples"] = subsampleslist.samples;
            secsamples.push(subsmpls);
        });
        smpls["samples"] = secsamples;
        var temp = this;
        $(GroupingList).each(function () {
            if (temp["Group"] == this.toString()) {
                Mainlist[this.toString()].push(smpls);
                return false;
            }
        });
    });
	$("#accordion2").html($("#accordionGroupTmpl").render(GroupingList));	
    $(GroupingList).each(function () {
        var content = $("#accordionTmpl").render(Mainlist[this]);
        if ($.trim(content) != ""){
            $(".SB-group." + this.toString().replace(/ /g, "")).html(content);
            $(Mainlist[this.toString()]).each(function () {
                firstlevelsamples.push(this);
            });
        }
        else
            $(".SB-group." + this.toString().replace(/ /g, "")).hide().prev().hide();
    });

    $("#samplesDiv").html($("#accordionTmplchild").render(firstlevelsamples));
    $("#scrollcontainer").ejScroller({ buttonSize: 0, scrollerSize: 9});
	$("#scrollcontainer .e-vhandle").addClass("e-box");
    $("#scrollcontainer .SB-groupIt").click(function () {
        if ($(this).hasClass("downArrow")) {
            $(this).next().hide(300,'linear', function () { refreshScroller() });
            $(this).removeClass("downArrow");
            $(this).addClass("rightArrow");
        }
        else {
            $(this).next().show(300,'linear', function () { refreshScroller() });
            $(this).removeClass("rightArrow");
            $(this).addClass("downArrow");
        }
    });
	RefreshPoductNav();
    refreshScroller();
    refreshContentWindow();
    function refreshScroller() {
        //scroller resize refresh
        $(".accordion-panel.cols-fixed-sidebar").attr("style", "height:" + (($(window).height() - $(".sbheader").outerHeight())) + "px ;");
        var controlheight = $("#scrollcontainer").ejScroller("instance");
        var Minheight = ($(window).height()) - ($(".header").outerHeight() + $("#dashboardheader").outerHeight() + $(".search").outerHeight());
        controlheight.model.height = Minheight;
        controlheight.refresh();
    }
    function refreshContentWindow() {
        //resizing for content fluid width 
        $(".scrollit").attr("style", "height:" + (($(window).height() - $(".sbheader").outerHeight())) + "px;");
    }
    $('div.firstlevelback').bind('click', function (evt, args) {
        $("#subsamplesDiv").hide().css("left", "250px");
    });
	function RefreshPoductNav()
	{
	$(".header").removeClass("Mobile").removeClass("Desktop");
	if($(window).width()>899)
		$(".header").addClass("Desktop");
	else
		$(".header").addClass("Mobile");
	}	
    $(".dashboard").mouseover(function () {
        if (!$(".vHandle").is(":visible")) {           
                if (window.themevarient.indexOf("dark")!= -1 && !$(this).hasClass("dark-highlighttextbg"))
                    $(this).addClass("dark-dashboardhover");
                else
                    $(this).addClass("dashboardhover");           
        }
    });
    $(".dashboard").click(function (e) {
        $(".anchor").removeClass("itemselected");
        $(this).find(".anchor").addClass("itemselected");
        $(".dark-dashboard").removeClass("dark-highlighttextbg");
        $(".dashboard").removeClass("highlighttextbg");
        $(this).find(".itemselected").parent().addClass("highlighttextbg");
        $("#sbdashboard").addClass("highlighttextbg");
        if ($(e.target).hasClass("analytics_nav") || $(e.target).parents('.analytics_nav').length > 0) viewdemo({ text: "analytics" });
    });

    $(".dashboard").mouseout(function () {
        if ($(this).hasClass("dashboardhover"))
            $(this).removeClass("dashboardhover");
        else if ($(this).hasClass("dark-dashboardhover"))
            $(this).removeClass("dark-dashboardhover");
    });

    //autocomplete selection list
    var collection = SamplesList;
    var subdata = [], i, j, k;
    for (i = 0; i < collection.length; i++) {
        if (collection[i].samples) {
            for (j = 0; j < collection[i].samples.length; j++) {
                var properties = {}, len;
                var controlName = collection[i].controller;
                if(!collection[i].samples[j].samples)
                {
                properties["id"] = controlName.toLowerCase() + "/" + collection[i].samples[j].action.toLowerCase();
                properties["control"] = collection[i].name;
                properties["text"] = collection[i].samples[j].name;
                properties["data"] = collection[i].name + " " + collection[i].samples[j].name;
                subdata.push(properties);
                }
                if (collection[i].samples[j].childcount != "0") {
                    if (/msie 8.0/.test(navigator.userAgent.toLowerCase()))
                        len = collection[i].samples[j].samples.length - 1;
                    else
                        len = collection[i].samples[j].samples.length;
                    if (collection[i].samples[j].name == "Selection")
                        var s = true;
                    for (k = 0; k < len; k++) {
                        var subprops = {};
                        var subcontrolName = collection[i].samples[j].name;
                        subprops["id"] = controlName.toLowerCase() + "/" + collection[i].samples[j].samples[k].action.toLowerCase();
                        subprops["control"] = collection[i].name;
                        subprops["text"] = collection[i].samples[j].name+"/"+collection[i].samples[j].samples[k].name;
                        subprops["data"] = collection[i].name + " " + collection[i].samples[j].name + "/" + collection[i].samples[j].samples[k].name;
                        subdata.push(subprops);
                    }
                }
            }
        }
    }
    $('#auto').ejAutocomplete({
        dataSource: subdata,
        fields: { key: "id", text: "data" },
        popupHeight: "180px"
    });
    //
    var autocompletScroller = $("#auto").data("ejAutocomplete").scrollerObj;
    autocompletScroller.model.buttonSize = 0;
    autocompletScroller.model.scrollerSize = 8;
    $('.navigation').bind('click', function () {
        var move, proxy = this;
        if ($(this).hasClass("expandPanel")) {
            $('.navigation').removeClass("expandPanel").addClass("collapsePanel"), move = -$('.accordion-panel').outerWidth();
            $('.control-panel.cols-content-fluid').addClass('center-flow');
            $('content-container-fluid > row > .navigation').addClass("expandPanel");
        }
        else {
            $(proxy).removeClass("expandPanel");
            $('.accordion-panel').removeClass('accordionHide').addClass("expandPanel");
            $('.control-panel.cols-content-fluid').removeClass('accordionHide').removeClass('center-flow');
            $('.accordion-panel > .navigation').addClass("expandPanel");
            $('.navigation').removeClass("collapsePanel"), move = 0;
            if (parseInt($('.accordion-panel').css('left')) >= 0) {
                $('.accordion-panel').css('left', '-' + $('.accordion-panel').outerWidth() + 'px');
            }
            $('.accordion-panel .navigation').addClass("expandPanel")
        }
        $('.accordion-panel').animate({
            "left": move + "px",
        }, 500, function () {
            if ($('.navigation').hasClass("collapsePanel")) {
                $('.accordion-panel').addClass('accordionHide');
                $('.navigation').show();

            }
            if ($(proxy).hasClass("expandPanel")) {
                $('.accordion-panel').addClass("expandPanel");
            }
        });
        refreshScroller();
    });
    $(window).bind("resize", function () {
        var viewportWidth = $(window).width();
        if (viewportWidth < 981) {
            $('.content-container-fluid .row > .navigation').addClass('collapsePanel');
            $('.accordion-panel').removeClass("expandPanel");
        }
		RefreshPoductNav();
        refreshScroller();
        refreshContentWindow();
    });
   

    $(document).click(function (evt) {
        if ($("#sbtooltipbox").ejDialog("isOpened"))
            $("#sbtooltipbox").ejDialog("close");
        var ele = false;
        var chromebrowser = /chrome/.test(navigator.userAgent.toLowerCase());
        var safaribrowser = /safari/.test(navigator.userAgent.toLowerCase());
        if (chromebrowser || safaribrowser)
            if(evt.target && evt.target.parentElement != null && evt.target.parentElement.parentElement != null && evt.target.parentElement.parentElement.id != null && evt.target.parentElement.parentElement.id == "themebutton")
            
            if (evt.target && evt.target.parentElement!=null && evt.target.parentElement.parentElement!=null && evt.target.parentElement.parentElement.id != null && evt.target.parentElement.parentElement.id == "themebutton")
                ele = true;
        if (!ele && $(evt.target).parents("#themeDialog_wrapper").length <= 0 && evt.target.id != "themebutton") {
            if ($("#themeDialog").ejDialog("isOpened"))
                $("#themeDialog").ejDialog("close");
        }
        if ($(window).width() < 900 && $("#Res-prodnav").hasClass("selectIt")) {
            $(".product-naviation").removeClass("hideIt");
            $("#Res-prodnav").removeClass("selectIt");
            $(".product-naviation").attr("style", "margin-left:" + ($(window).width() - 124) + "px;");
        }
        else
            $(".product-naviation").addClass("hideIt");
    });
   
    $(".circlebaseouter").mouseover(function (evt) {
        if ($(".circlebaseouter").parent().hasClass("e-disable")) return false;
        $(".hoverselcolor").removeClass("hoverselcolor");
        $(this).addClass("hoverselcolor");
    });
    $(".circlebaseouter").mouseout(function (evt) {
        if ($(".circlebaseouter").parent().hasClass("e-disable")) return false;
        $(".hoverselcolor").removeClass("hoverselcolor");
    });
    $(".circlebaseouter .azure,.circlebaseouter .lime,.circlebaseouter .saffron").click(function (evt) {
        if ($(".circlebaseouter").parent().hasClass("e-disable")) return false;
        $(".colorsel").removeClass("colorsel");
        $(this).parent().addClass("colorsel");
        var selcolor = $(this).parent().attr("id");
        window.themecolor = selcolor.replace("theme", "");
        updateTheme();
    });
    window.setTimeout(function () {
        var waitingPopup = $(".e-waitingpopup.e-js"), timeout;
        if (waitingPopup.length > 0) {
            $(".scrollit").scroll(function () {
			var waitingObj = $(".e-waitingpopup.e-js").data("ejWaitingPopup");
			if (waitingObj && waitingObj.model.showOnInit) {
                    if (timeout) clearTimeout(timeout);
                    timeout = setTimeout(function () { waitingObj.refresh(); }, 20);
                }
            });
        }
    }, 10);
      
});


function preparetheme() {
    var themename = "";
    if (window.theme.indexOf('dark') != -1 && window.themevarient != 'light')
        window.themevarient = "dark";
    if (window.theme.indexOf('gradient') != -1 && window.themestyle != 'flat')
        window.themestyle = "gradient";
    if (window.themevarient == "dark") {
        if (window.themestyle == "gradient")
            themename = window.themestyle + window.themecolor + window.themevarient;
        else
            themename = (window.themecolor != "") ? window.themecolor + window.themevarient : window.themestyle + window.themevarient;

    }
    else {
        if (window.themestyle == "gradient")
            themename = window.themestyle + window.themecolor;
        else
            themename = (window.themecolor != "") ? window.themecolor : window.themestyle;
    }
    if (window.theme.indexOf( "bootstrap")>=0)
        themename = window.theme;
    window.theme = themename;
}

function updateTheme(selcolor) {
    preparetheme();
    replacebg(window.themevarient == "dark");
    replaceTheme();
}
var themes = {
    "flat": "/Content/ej/default-theme/ej.theme.min.css",
    "flatdark": "/Content/ej/flat-azure-dark/ej.theme.min.css",
    "azure": "/Content/ej/default-theme/ej.theme.min.css",
    "azuredark": "/Content/ej/flat-azure-dark/ej.theme.min.css",
    "lime": "/Content/ej/flat-lime/ej.theme.min.css",
    "limedark": "/Content/ej/flat-lime-dark/ej.theme.min.css",
    "saffron": "/Content/ej/flat-saffron/ej.theme.min.css",
    "saffrondark": "/Content/ej/flat-saffron-dark/ej.theme.min.css",
    "gradient": "/Content/ej/gradient-azure/ej.theme.min.css",
    "gradientdark": "/Content/ej/gradient-azure-dark/ej.theme.min.css",
    "gradientazure": "/Content/ej/gradient-azure/ej.theme.min.css",
    "gradientazuredark": "/Content/ej/gradient-azure-dark/ej.theme.min.css",
    "gradientlime": "/Content/ej/gradient-lime/ej.theme.min.css",
    "gradientlimedark": "/Content/ej/gradient-lime-dark/ej.theme.min.css",
    "gradientsaffron": "/Content/ej/gradient-saffron/ej.theme.min.css",
    "gradientsaffrondark": "/Content/ej/gradient-saffron-dark/ej.theme.min.css",
    "bootstrap": "/Content/ej/bootstrap-theme/ej.theme.min.css"
};
var core = {
    "bootstrap": "/Content/ej/ej.widgets.core.bootstrap.min.css"
};
function refreshdata() {
    var Mainlist = [];
    var mulsamples = [];

    $(GroupingList).each(function () {
        Mainlist[this] = [];
    });

    $(SamplesList).each(function (index1, sampleslist) {
        var temp = this;
        $(GroupingList).each(function () {
            if (temp["Group"] == this.toString()) {
                Mainlist[this.toString()].push(temp);
                return false;
            }
        });

    });
    $(GroupingList).each(function () {
        $(Mainlist[this]).each(function () {
            mulsamples.push(this);
        });
    });
    return mulsamples;
}
function replaceTheme() {
    if ((window.theme.indexOf("lime") != -1) || (window.theme.indexOf('lime') != -1 && window.themecolor != 'azure' && window.themecolor != "saffron"))
        window.themecolor = 'lime';
    else if ((window.theme.indexOf('saffron') != -1) || (window.theme.indexOf('saffron') != -1 && window.themecolor != 'azure' && window.themecolor != 'lime'))
        window.themecolor = 'saffron';
    else
        window.themecolor = "azure";
    var selcolor = (window.themecolor == "") ? "saffron" : window.themecolor;
    $("#themebutton").ejButton('option', 'prefixIcon', 'e-icons ' + selcolor);
    $("#themebutton .e-uiLight").addClass(selcolor);
    $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + selcolor);
    var urlread = window.location.toString();
    var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
    for (var i = 0; i < links.length; i++) {
        if (links[i].href.indexOf("ej.theme.min.css") != -1) {
            var cssref = links[i].href,serverconfig = cssref.substr(0, cssref.indexOf("Content")-1),
			fileref = $('<link rel="stylesheet" type="text/css" href="' + serverconfig + themes[theme] + '" />');
            if (window.theme == "bootstrap") {
                var corefile = $('<link rel="stylesheet" type="text/css" href="' + serverconfig + core[theme] + '" />');
                $(corefile).insertBefore(links[i]);
                if ($(".e-prop").length) $(".e-prop").addClass("e-bootstrap");
            }
            else {
                if (links[i-1].href.indexOf("ej.widgets.core.bootstrap.min.css") != -1)
                    $(links[i - 1]).remove();
                if ($(".e-prop").length && $(".e-prop").hasClass("e-bootstrap")) $(".e-prop").removeClass("e-bootstrap");
            }
            $(fileref).insertAfter(links[i]);
            $(links[i]).remove();
            window.setTimeout(function () {
                if ($(".cols-sample-area").find(".e-rte.e-js").length) $(".cols-sample-area").find(".e-rte.e-js").data("ejRTE")._setIframeHeight();
                if ($(".cols-sample-area").find(".e-fileexplorer.e-js").length) $(".cols-sample-area").find(".e-fileexplorer.e-js").data("ejFileExplorer").adjustSize();
            }, 500);
            break;
        }
    }
    //gauge chart and others
    if (urlread.toLowerCase().indexOf("gauge") >= 0) {
        loadGaugeTheme();
    }
    else if ((urlread.indexOf("chart") >= 0) || (urlread.indexOf("Client") >= 0) || (urlread.indexOf("rangenavigator") >= 0)) {
        loadTheme(undefined);
    }
    else if (urlread.indexOf("graph") >= 0) {
        loadBulletTheme();
    }
    else if (urlread.indexOf("Grid/Adaptive") >= 0 || urlread.indexOf("grid/adaptive") >= 0) {
        loadGridFrameTheme();
    }
    else if (urlread.indexOf("schedule/adaptive") >= 0) {
        loadScheduleFrameTheme();
    }
    resetCookies();
}
function replacebg(enable) {
    if (enable)
        $("body").addClass("darktheme");
    else
        $("body").removeClass("darktheme");
}

function themebtnClick(args) {
    showThemeDialog();
}
function showTooltipbox() {
    //var $btnelement = $("#themebutton");
    //var pos = $btnelement.offset();
    //var left = $("#sbtooltipbox_wrapper").width() + pos.left;
    //if (left > $(".samplecontainerparent").width())
    //    left = (pos.left + ($btnelement.width() / 2)) - $("#sbtooltipbox_wrapper").width();
    //else
    //    left = pos.left;
    //$("#sbtooltipbox").ejDialog("option", "position", { X: left + 10, Y: $(".header").height() + 7 });
    //$("#sbtooltipbox").ejDialog("open");
}
function showThemeDialog() {
    var $btnelement = $("#themebutton");
    var pos = $btnelement.offset();
    var left = $("#themeDialog_wrapper").width() + pos.left;
    if (left > $(".samplecontainerparent").width())
        left = (pos.left + $btnelement.width()) - $("#themeDialog_wrapper").width();
    else
        left = pos.left;
    $("#themeDialog").ejDialog("option", "position", { X: left, Y: $(".header").height() + 7 });
    if ($("#themeDialog").ejDialog("isOpened") != true)
        $("#themeDialog").ejDialog("open");
    else
        $("#themeDialog").ejDialog("close");
   
}
function themeonchange(args) {
    if ($("." + args.model.name).hasClass("e-disable")) return false;
    switch (args.model.name) {
        case "themestyle":
            window.themestyle = args.model.value;
            break;
        case "themevarient":
            window.themevarient = args.model.value;
            break;
    }
    updateTheme(window.theme);
        //radioButtonSelect();
}
function bootstraponselect(args) {
    if (args.isChecked) {
        enableBootstrap(true);
        window.theme = "bootstrap";
        window.themecolor = "azure";
        window.themestyle = "flat";
        window.themevarient = "light";
        resetCookies();        
        updateTheme(window.theme);
    }
    else {
        enableBootstrap(false);
        window.theme = "saffron";
        window.themecolor = "saffron";
        window.themestyle = "flat";
        window.themevarient = "light";
        resetCookies();
        themeButtonSelect();
        updateTheme(window.theme);
    }
    
    
}
function enableBootstrap(enable) {
    if (enable) {
        $(".themecolors").addClass("e-disable");
        $(".themevarient").addClass("e-disable");
        $(".themestyle").addClass("e-disable");
    }
    else {
        $(".themecolors").removeClass("e-disable");
        $(".themevarient").removeClass("e-disable");
        $(".themestyle").removeClass("e-disable");
    }
    $("#lighttext").ejRadioButton("option", "enabled", !enable);
    $("#darktext").ejRadioButton("option", "enabled", !enable);
    $("#metrotext").ejRadioButton("option", "enabled", !enable);
    $("#gradienttext").ejRadioButton("option", "enabled", !enable);


}
function radioButtonSelect() {
    $('#ejmetrotext,#ejgradienttext,#ejlighttext,#ejdarktext').children('span').children('span').removeClass('e-rad-active');
    if (window.themestyle.indexOf("gradient") != -1) {
        $('#ejgradienttext').attr({'aria-checked':true,'tabindex':'0'}).children('span').children('span').addClass('e-rad-active');
        window.themestyle = 'gradient';
    }
    else {
        $('#ejmetrotext').attr({'aria-checked':true,'tabindex':'0'}).children('span').children('span').addClass('e-rad-active');
        window.themestyle = 'flat';
    }
    if (window.themevarient.indexOf("dark") != -1) {
        $('#ejdarktext').attr({'aria-checked':true,'tabindex':'0'}).children('span').children('span').addClass('e-rad-active');
        window.themevarient = 'dark';
    }
    else  {
        $('#ejlighttext').attr({'aria-checked':true,'tabindex':'0'}).children('span').children('span').addClass('e-rad-active');
        window.themevarient = 'light';
    }
    if (window.theme == "bootstrap") {
        $("#bootstrapcheck").ejCheckBox("option", "checked", true);
        enableBootstrap(true);
    }

}
function themeButtonSelect() {
    //
    if ((readCookie('themeName') != null) && (readCookie('themeName') != ""))
        window.theme = readCookie('themeName');
    if ((readCookie('themeColor') != null) && (readCookie('themeColor') != ""))
        window.themecolor = readCookie('themeColor');
    if ((readCookie('themeStyle') != null) && (readCookie('themeStyle') != ""))
        window.themestyle = readCookie('themeStyle');
    if ((readCookie('themeVarient') != null) && (readCookie('themeVarient') != ""))
        window.themevarient = readCookie('themeVarient');
    //
    $('#themesaffron,#themeazure,#themelime').removeClass('colorsel');
    if ((window.theme.indexOf("lime") != -1) || (window.theme.indexOf('lime') != -1)) {
        $('#themelime').addClass('colorsel');
        window.themecolor = 'lime';
    }
    else if ((window.theme.indexOf("saffron") != -1) || (window.theme.indexOf('saffron') != -1)) {
        $('#themesaffron').addClass('colorsel');
        window.themecolor = 'saffron';
    }
    else {
        $('#themeazure').addClass('colorsel');
        window.themecolor = 'azure';
    }
    radioButtonSelect()
}

//next previous location
function prevnext(name, cursample, navloc) {
    var cursampitem, prevsampitem, nextsampitem, cursamplist, prevsamplist, nextsamplist, chx, tempsample, cursampquery, anchref, newlist = refreshdata();
    for (i = 0; i < newlist.length; i++) {
        chx = newlist[i].id;
        if (chx.toLowerCase() == name.toLowerCase()) {
            cursampitem = newlist[i];
            prevsampitem = newlist[i - 1];
            nextsampitem = newlist[i + 1];
            break;
        }
    }
    cursamplist = retrivesamplkist(cursampitem);
    prevsamplist = retrivesamplkist(prevsampitem);
    nextsamplist = retrivesamplkist(nextsampitem);
    if (navloc == "next") {
        for (i = 0; i < cursamplist.length; i++) {
            if (cursamplist[i].action == cursample) {
                if (i == cursamplist.length - 1) {
                    if (nextsamplist.length == 0) {
                        anchref = "/Introduction/Index";
                    }
                    else {
                        anchref = "/" + nextsamplist[0].controller + "/" + nextsamplist[0].action;
                    }
                }
                else if (i < cursamplist.length - 1) {
                    anchref = "/" + cursamplist[i + 1].controller + "/" + cursamplist[i + 1].action;
                }
                break;
            }
        }
    }
    else if (navloc == "prev") {
        for (i = 0; i < cursamplist.length; i++) {
            if (cursamplist[i].action == cursample) {
                if (i == 0) {
                    if (prevsamplist.length == 0) {
                        anchref = "/Introduction/Index";
                    }
                    else {
                        anchref = "/" + prevsamplist[prevsamplist.length - 1].controller + "/" + prevsamplist[prevsamplist.length - 1].action;
                    }
                }
                else if (i > 0 && i < cursamplist.length) {
                    anchref = "/" + cursamplist[i - 1].controller + "/" + cursamplist[i - 1].action;
                }
                break;
            }
        }
    }
    return anchref.toLowerCase();
}
function retrivesamplkist(cursampitem) {
    var cursamplist = [];
    if (cursampitem && !ej.isNullOrUndefined(cursampitem.samples)) {
        for (i = 0; i < cursampitem.samples.length; i++) {
            if (cursampitem.samples[i].childcount == "0") {
                cursamplist.push(cursampitem.samples[i]);
            }
            else if (cursampitem.samples[i].samples.length != 0) {
                for (j = 0; j < cursampitem.samples[i].samples.length; j++) {
                    if (cursampitem.samples[i].samples[j].childcount == "0")
                        cursamplist.push(cursampitem.samples[i].samples[j]);
                }
            }
        }
    }
    return cursamplist;
}
//cookies
function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (3 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = escape(name) + "=" + escape(value) + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = escape(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return unescape(c.substring(nameEQ.length, c.length));
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}




/* Launching Other Products*/
function viewdemo(args) {
    var text = 'Handler/StartDevServer.ashx', product = $.trim(args.text).toLowerCase(), prot = window.location.protocol, hostname = window.location.host;
	var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
    var serverconfig = links[0].href.substr(0, links[0].href.indexOf("Content") - 1);
    if (typeof (iisPrefixLink) != "undefined" && iisPrefixLink != null) {
        if (product == "mobile") window.location = "http://localhost:"+window.location.port+"/sfejmobilemvcsamplebrowser"; 
        else if (product == "analytics") window.open("http://localhost:"+window.location.port+"/sfejmvcanalyticssamplebrowser");
        else return false;        
    }
    else if (product == "mobile" || product == "analytics") {
        $.ajax({
            url: serverconfig + "/" + text + "?product=" + product,
            success: function (data) {
                if (product == "mobile") window.location = data;
                if (product == "analytics") window.open(data);
            }
        });
    }
}
//autocomplete
function onSelectSearchItem(args) {
    var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
    var serverconfig = links[0].href.substr(0, links[0].href.indexOf("Content") - 1);
    var url = serverconfig+ "/" + args.key;
    window.location = url.toString().toLowerCase();
}

function resetCookies() {
    eraseCookie("themeName");
    createCookie('themeName', window.theme, 1);
    eraseCookie("themeColor");
    createCookie('themeColor', window.themecolor, 1);
    eraseCookie("themeStyle");
    createCookie('themeStyle', window.themestyle, 1);
    eraseCookie("themeVarient");
    createCookie('themeVarient', window.themevarient, 1);
}

//Downlink function 
function downlinkclick(e){
	window.open("http://www.syncfusion.com/downloads/aspnetmvc","_blank");
}
