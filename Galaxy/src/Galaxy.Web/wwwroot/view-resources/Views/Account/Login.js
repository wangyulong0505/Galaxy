(function () {
    $(function () {
        //记住账号密码的初始化样式
        $('input').iCheck({
            checkboxClass: 'icheckbox_square-red',
            radioClass: 'iradio_square-red',
            increaseArea: '20%' // optional
        });
        $('#ReturnUrlHash').val(location.hash);

        var $loginForm = $('#LoginForm');
        /* bootstrapValidator插件
         * 使用说明：
         * 1、先引入必须的类库：jquery（js），Bootstrap（css和js），bootstrapValidator（css和js）
         * 2、页面需要有一个Form表单
         * 3、页面初始化的function里面使用$('form').bootstrapValidator({})，里面初始化对应的参数
         * 常用参数说明：
         * 1、feedbackIcons  这是错误显示的图标
         * 2、fields         这是定义错误的信息
         * 3、submitHandler  这个是表单提交触发的自定义函数
         */
        $loginForm.bootstrapValidator({
            message: '请输入用户名/密码',
            /* 这里没有添加feedbackIcons，不需要
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            */
            fields: {
                //这个是name属性的字段，注意大小写
                usernameOrEmailAddress: {
                    validators: {
                        notEmpty: {
                            message: '登录邮箱名或用户名不能为空'
                        }
                    }
                },
                Password: {
                    validators: {
                        notEmpty: {
                            message: '密码不能为空'
                        }
                    }
                }
            }
        });

        $('#LoginButton').click(function (e) {
            e.preventDefault();

            if (!$loginForm.data('bootstrapValidator').isValid()) {
                return;
            }
            /* 可以使用abp的js
            abp.ui.setBusy(
                $('#LoginArea'),
                abp.ajax({
                    contentType: 'application/x-www-form-urlencoded',
                    url: $loginForm.attr('action'),
                    data: $loginForm.serialize()
                })
            );
            */
            //可以使用自定义的ajax, 推荐使用abp的js
            /*
             * 如果使用type="submit"的按钮会执行两次ajax，
             * 解决方案1: 使用type="button"的按钮
             * 解决方案1: ajax最后添加return false
            */
            $.ajax({
                contentType: 'application/x-www-form-urlencoded',
                type: 'POST',
                url: $loginForm.attr('action'),
                data: $loginForm.serialize(),
                dataType: 'json',
                success: function (data, textStatus) {
                    console.log(data);
                    if (data.success) {
                        if (data.result == '0') {
                            location.href = '../Home/Index';
                        }
                        else if (data.result == '1') {
                            modals.error("账号不存在");
                        }
                        else if (data.result == '2') {
                            modals.error("密码错误");
                        }
                        else { }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    modals.error(errorThrown);
                }
            });
        });
        //账户名或邮箱输入框获取焦点
        $loginForm.find('input[type=text]:first-child').focus();
        //点击回车键触发事件
        document.onkeydown = function (e) {
            if (!e) {
                e = window.event;
            }
            if ((e.keyCode || e.which) == 13) {
                $('#LoginButton').focus();
                $('#LoginButton').click();
            }
        }
    });
})(jQuery);
