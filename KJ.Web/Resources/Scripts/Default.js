var KJ = {};

//初始化页面数组
KJ.InitPageArray = function (mid) {
    arrayPages = []; //清空数组
    arrayPages.length = 0;
    $.each(arrayAllPages, function (index, value) {
        var itemTemp = value.split("|");
        if (itemTemp[3] == mid) {
            var select = 0;
            if (itemTemp[2] == "True") {
                select = 1;
            }
            arrayPages.push(itemTemp[1] + "|" + itemTemp[4] + "|" + select + "|0");
        }
    });
};
//初始化的模板
KJ.template = function () {
    return "<iframe id=\"MainFrame\" name=\"MainFrame\"  scrolling=\"yes \"  frameborder=\"0\" src=\"[src]\"   style=\"width:100%;height:100%; padding:0px; margin:0px;display: block\"></iframe>";
};
/* 在tab中打开新页面 */
KJ.OpenTabPage = function (title, content, selected, closable) {
    var len = $('#centerTab').tabs("tabs").length;
    for (var i = 0; i < len; i++) {
        var tabTitle = $('#centerTab').tabs("tabs")[i].panel("options").title;
        if (title == tabTitle) {
            $('#centerTab').tabs("select", title);
            var parentDiv = $('#centerTab').tabs("tabs")[i].find("iframe").parent();
            parentDiv.empty();
            parentDiv.append(content);
            return;
        }
    }
    $('#centerTab').tabs('add', {
        title: title,
        content: content,
        closable: closable,
        scrollable: true,
        selected: selected,
        onBeforeOpen: function () {
            var iframe = $(this).find("iframe");
            if (iframe && !iframe.attr("src")) {
                iframe.attr("src", iframe.attr("href"));
            }
        }
    });
    var tab = $("#centerTab").tabs("getSelected");
}

/* 在tab中打开新页面,并延迟加载，当点击页签的时候，才加载 */
KJ.AddTab = function (title, url, closable) {
    title = decodeURI(title);
    var content = MF.template();
    content = content.replace("[src]", "");
    $('#centerTab').tabs('add', {
        title: title,
        content: content,
        closable: typeof (closable) == "undefine" ? true : closable,
        fit: true,
        selected: false,
        cacheurl: url,
        OnTabClick: function (val) {
            var iframe = $('#centerTab').tabs("getTab", title).find("iframe");
            if (iframe && iframe.attr("src") == "") {
                $('#centerTab').tabs("getTab", title).find("iframe").attr("src", url);
            }
        }
    });
};

//根据标题关闭tab
KJ.closeTab = function (title) {
    title = decodeURI(title);
    var iframe = $('#centerTab').tabs("getTab", title).find("iframe").get(0);
    if (iframe) {
        //iframe.contentWindow.document.write('');
        iframe.contentWindow.close();
        if (iframe.remove) {
            iframe.remove();
        }
    }

    $('#centerTab').tabs("close", title);
};

//关闭其它标签页，默认tab除外
KJ.closeOtherTabs = function (title) {
    var del = new Array();
    $.each($('#centerTab').tabs("tabs"), function (i, tab) {
        var tabTitle = tab.panel("options").title;
        if (tabTitle != title && KJ.GetPageItem(tabTitle) == "") {
            del.push(tabTitle);
        }
    });
    $.each(del, function (i, title) {
        KJ.closeTab(title);
    });
};
//获取子项
KJ.GetPageItem = function (title) {
    var result = "";
    $.each(arrayPages, function (index, value) {
        if (value.split("|")[0] == title) {
            result = value;
        }
    });
    return result;
};

var PW = {};
PW.Id = "mydialog";
PW.win = parent.PW;
PW.template = function () {
    return "<iframe id=\"MainFrame\" name=\"MainFrame\"  scrolling=\"yes \"  frameborder=\"0\" src=\"[src]\"   style=\"width:100%;height:100%; padding:0px; margin:0px;display: block\"></iframe>";
};
PW.getWindow = function (url, title, width, height, callBack) {
    if ($("#" + PW.Id).length == 0)
    {
        $("body").append("<div id='" + PW.Id + "'></div>");
    }
    var content = KJ.template();
    content = content.replace("[src]", "");
    $("#"+PW.Id).dialog({
        title: '添加客户',
        width: width,
        top: 100,
        height: height,
        autoOpen: false,
        cache: false,
        closed: true,
        modal: true,
        content: content,
        onBeforeOpen:function(){
            $("#" + PW.Id).find("iframe").attr("src", url);
        },
        onclose: function () {
            if(callBack!=undefined)
            {
                callBack();
            }
        }
    }).dialog('open');
};
PW.Close= function ()
{
    PW.win.CloseWindow();
}

PW.CloseWindow = function () {
    $("#" + PW.Id).dialog('close');
}
