$.extend({
    U1Ajax: function (url, postData, successHandle, Async, errorHandle, timeout) {
        if (url == null) {
            alert("Ajax 地址错误！");
        }

        var async = true;
        if (Async != null && Async != undefined) {
            async = Async;
        }
        if (successHandle == undefined || successHandle == null) {
            successHandle = "";
        }
        if (errorHandle == undefined || errorHandle == null) {
            errorHandle = "";
        }
        if (timeout == undefined || timeout == null) {
            timeout = "";
        }
        var key = url + async.toString() + successHandle.toString() + errorHandle.toString() + timeout.toString();
        if (postData != undefined && postData != null && postData != "") {
            $.each(postData, function (i, item) {
                if (i != undefined && i != null) {
                    key += i.toString();
                }
                if (item != undefined && item != null) {
                    key += item.toString();
                }
            });
        }

        //没有引用MD5 JS时不加密
        try {
            key = hex_md5(key); //md5 加密
        } catch (e) {
            key = encodeURIComponent(key);
        }

        var op = {
            type: "POST",
            url: url,
            data: postData,
            async: async,
            traditional: true,
            success: function (result) {
                if (result == null) {
                    return;
                }
                if (result.Tag == -999) {
                    alert(result.Message);
                    if (errorHandle)
                        errorHandle();
                    //parent.$("body").unmask();
                    return;
                } else if (result.Tag == -998) {
                    alert("提交的信息存在安全隐患！");
                    if (errorHandle)
                        errorHandle();
                    // parent.$("body").unmask();
                    return;
                }
                if (successHandle) {
                    successHandle(result);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (errorHandle)
                    errorHandle();
                var data = eval("(" + XMLHttpRequest.responseText + ")");
                if (data.Tag == -999) {
                    alert(errorThrown + "\n" + data.Message);
                } else {
                    alert(errorThrown + "\n" + XMLHttpRequest.responseText);
                }
                // parent.$("body").unmask();
            }, complete: function (XMLHttpRequest, T) {
            }
        }
        //op.timeout = 5 * 1000;
        if (timeout) {
            op.timeout = timeout;
        }
        $.ajax(op);
    }
});

$.fn.GetPostData = function (grid) {
    var data = {};
    if (grid != undefined) {
        data.sort = grid.datagrid('options').sortName;
        data.order = grid.datagrid('options').sortOrder;
    }

    $(this).find("[col]").each(function (i, value) {
        var field = $(value).attr("col");
        if (value.tagName == "INPUT") {
            if (value.type == "checkbox") {
                if ($(value).is(':checked')) {
                    if (data[field]) {
                        data[field] = data[field] + "," + "true";
                    } else {
                        data[field] = "true";
                    }
                }
            }
            else if (value.type == "radio") {
                if ($(value).attr("checked") == true) {
                    data[field] = $(value).val();
                }
            }
            else if ($(value).attr("type") == "combotree") {
                var val = $(value).combotree('getValue');
                if (val == null || val == 0)
                    val = "";
                data[field] = val;
            }
            else {
                data[field] = $(value).val();
            }
        }
        else if (value.tagName == "SELECT") {
            if ($(value).attr("type") == "combotree") {
                var values = $(value).combotree('getValues');
                if (values == null || values.count == 0)
                    values = "";
                data[field] = values.join(",");
            } else {
                data[field] = $(value).val();
            }
        }
        else if (value.tagName == "DIV") {
            data[field] = $(value).html();
        }
        else if (value.tagName == "IMG") {
            data[field] = $(value).attr("src");
        }
        else if (value.tagName == "SPAN") {
            data[field] = $(value).html();
        }
        else if (value.tagName == "TEXTAREA") {
            data[field] = $(value).val();
        }

    });

    return data;
}

function searchGrid(gridId, queryParams, url) {
    $(".datagrid-header-check").find(":checkbox").attr("checked", false);
    if (queryParams != null) {
        $('#' + gridId).datagrid('options').queryParams = queryParams;
    }
    if (url != null) {
        $('#' + gridId).datagrid('options').url = url;
    }
    grid_search_rload = true;//全局变量，判断是否刷新到第一页
    if (grid_search_rload)
        $("#" + gridId).datagrid('load');
    else
        $("#" + gridId).datagrid('reload');
};

function tFormatTime(val, row, rowIndex) {
    if (val == null) {
        return "";
    }
    return val.replace("T", " ");
};

function GetSelect() {
    var idList = [];
    var obj = $('#dg').datagrid('getSelections');
    var objLength = obj.length;
    for (var i = 0; i < objLength; i++) {
        idList.push(obj[i].Id);
    }
    return idList;
}