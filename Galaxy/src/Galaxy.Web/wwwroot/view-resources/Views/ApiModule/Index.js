(function () {
    $(function () {
        /*弹出框效果
        $("[data-toggle='popover']").popover({
            html: true,
            content: function () {
                return $('#popover_content_wrapper').html();
            }
        });
        */
        $('button[data-event]').click(function () {
            var type = $(this).attr('data-event');
            switch (type) {
                case 'add':
                    //添加参数
                    addParams();
                    break;
                case 'send':
                    ajaxRequest();
                    break;
            }
        });
        $('.dropdown-menu li').on('click', function () {
            var text = $(this).find('a').text();
            var title = ' &nbsp;&nbsp;<span class="fa fa-caret-down" ></span >';
            $('button[data-toggle="dropdown"]').html(text + title);
        });
        //点击主Table行后面的删除图标，移除整行
        $(document).on("click", ".remove", function () {
            $(this).parent().remove();
        });

        //点击Header Table后面的添加参数图标，添加行
        $(document).on('click', '.addrow', function () {
            var html = '';
            var row = $('#bulkheader tbody').find('tr:last').attr('class') == 'odd' ? 'even' : 'odd';
            html += '<tr role="row" class="' + row + '">';
            html += '<td class="text-left"><input placeholder="New Key" data-name="key"  class="form-control" type="text" likeoption="true" /></td >';
            html += '<td class="text-left" name="value"><input placeholder="Value" data-name="value"  class="form-control" type="text" likeoption="true" /></td>';
            html += '<td class="text-left" name="description" colspan="2"><input placeholder="Description" data-name="description" class="form-control" type="text" likeoption="true" /></td>';
            html += '<td class="text-center removeheader" style="vertical-align:middle;"><i class="fa fa-remove"></i></td>';
            html += '</tr>';
            $('#bulkheader').find('tbody').append(html);
        });

        //点击Header Table行后面的删除图标，移除整行
        $(document).on("click", ".removeheader", function () {
            $(this).parent().remove();
        });

        //判断显示密码是否勾选
        $('#pwdprop').on('click', function () {
            if ($('#pwdprop').is(':checked')) {
                $('#password').prop('type', 'text');
            }
            else {
                $('#password').prop('type', 'password');
            }
        }); 
        $('#ntlmpwdprop').on('click', function () {
            if ($('#ntlmpwdprop').is(':checked')) {
                $('#ntlmpassword').prop('type', 'text');
            }
            else {
                $('#ntlmpassword').prop('type', 'password');
            }
        });
        //通过select控制table中tr的显示
        $('.authType').change(function () {
            var vs = this.value;
            console.log(this.value);
            $('#authtable tr').each(function () {
                $(this).hide();
            })
            $('#authtable tr').eq(vs).show();
        });

        //keyvalue富文本框改变触发
        /*
        $('#paramarea').on('input propertychange', function () {
            var text = $(this).val();
            var array = text.split("\n");//获取内容按\n分割成数组
            var html = '';
            for (var i = 0; i < array.length; i++) {
                //把第一行的文本替换成对应的字符
                if (i == 0) {
                    html = '?' + array[i].replace(':', '=');
                    $('#url').val(html);
                }
                else {
                    html = '&' + array[i].replace(':', '=');
                    $('#url').val(html);
                }
            }
            
        });
        */
    });

    //这个是设置textarea左边数字框的样式
    $(".textline").setTextareaCount({
        width: "30px",
        bgColor: "#EBEBEB",
        color: "#969696",
        display: "inline-block",
    });
    //切换输入参数的事件
    $("a[data-effect='change']").on('click', function () {
        $('#bulk').toggle();
        //清空bulk参数, 移除所有的tr再添加个新的tr
        $('#bulk tbody tr').remove();
        addParams();
        $('#keyvalue').toggle();
        //清空keyvalue textarea参数
        $('#paramarea').val('');
    });
    $("a[data-effect='changeheader']").on('click', function () {
        $('#bulkheader').toggle();
        $('#keyvalueheader').toggle();
    });
    /**
     * 点击Param添加新的tr并追加到table的tbody后面
     */
    function addParams() {
        var html = '';
        var row = $('#bulk tbody').find('tr:last').attr('class') == 'odd' ? 'even' : 'odd';
        html += '<tr role="row" class="' + row + '">';
        html += '<td class="text-left"><input placeholder="New Key" data-name="key" class="form-control" type="text" likeoption="true" /></td >';
        html += '<td class="text-left" name="value"><input data-name="value" placeholder="Value" class="form-control" type="text" likeoption="true" /></td>';
        html += '<td class="text-left" name="description" colspan="2"><input placeholder="Description" data-name="description" class="form-control" type="text" likeoption="true" /></td>';
        html += '<td class="text-center remove" style="vertical-align:middle;"><i class="fa fa-remove"></i></td>';
        html += '</tr>';
        $('#bulk').find('tbody').append(html);
    }
    
    /**
     * 点击Send，发送Api请求
     */
    function ajaxRequest() {
        //获取request Method
        var requestMethod = $.trim($('button[data-toggle="dropdown"]').text());
        //获取Url，有可能带参数
        var url = $('#url').val();
        //获取参数，这个有点复杂
        var data = '{';
        //先判断是bulk还是key-value
        if ($('#bulk').is(':visible')) {
            //说明bulk是显示的，获取Key和value的值
            var html = '';
            html += $('#url').val();
            var array = $('#bulk tbody tr');
            for (var i = 0; i < array.length; i++) {
                var key = array.eq(i).find('input[data-name="key"]').val();
                var value = array.eq(i).find('input[data-name="value"]').val();
                /*
                if (i == 0) {
                    html += '?' + key + '=' + value;
                }
                else {
                    html += '&' + key + '=' + value;
                }
                */
                data += '"' + key + '":"' + value + '"';
                data += ',';
            }
            data = data.substr(0, data.length - 1);
            data += '}';
        }
        else {
            //说明bulk是隐藏的，获取keyvalue的Json字符串值
            var text = $('#paramarea').val();
            var array = text.split("\n");//获取内容按\n分割成数组
            var html = '';
            for (var i = 0; i < array.length; i++) {
                console.log(array[i]);
                if (array[i].indexOf(':') > 0) {
                    var key = array[i].split(':')[0];
                    var value = array[i].substring(key.length + 1, array[i].length);
                    /*
                    if (i == 0) {
                        html += $('#url').val();
                        html += '?' + key + '=' + value;
                    }
                    else {
                        html += '&' + key + '=' + value;
                    }
                    */
                    data += '"' + key + '":"' + value + '"';
                    data += ',';
                }
            }
            data = data.substr(0, data.length - 1);
            data += '}';
        }
        $.ajax({
            url: url,
            type: requestMethod,
            data: data,
            success: function (data, textStatus) {
                //
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //
            }
        })
    }
})(jQuery);