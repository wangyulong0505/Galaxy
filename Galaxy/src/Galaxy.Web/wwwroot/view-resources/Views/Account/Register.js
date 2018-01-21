(function () {
    $(function () {
        //初始化icheck
        $('input.icheck').iCheck({
            checkboxClass: 'icheckbox_square-red',
            radioClass: 'iradio_square-red',
            increaseArea: '20%' // optional
        });
        var $registerForm = $('#RegisterForm');
        $registerForm.bootstrapValidator({
            submitHandler: function (valiadtor, loginForm, submitButton) {
                if (!$("#agreement").is(":checked")) {
                    //modals.info("请先勾选同意遵循AdminEAP协议");
                    return;
                }
                valiadtor.defaultSubmit();
            },
            /* 这里没有添加feedbackIcons，不需要
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            */
            fields: {
                name: {
                    validators: {
                        notEmpty: {
                            message: '用户名不能为空'
                        },
                        stringLength: {
                            /*长度提示*/
                            min: 2,
                            max: 16,
                            message: '用户名长度必须在2到16之间'
                        },
                        threshold: 4,//只有4个字符以上才发送ajax请求
                        remote: {
                            //ajax验证。server result:{"valid",true or false} 向服务发送当前input name值，获得一个json数据。例表示正确：{"valid",true}； 表示错误：{"valid",false}
                            url: "/Account/CheckUnique",    //验证地址         
                            data: function (validator) {
                                return {
                                    loginName: $("#name").val(),
                                    userId: null
                                };
                            },
                            message: '该登录名已被使用，请使用其他登录名',       //提示消息
                            delay: 2000                                     //每输入一个字符，就发ajax请求，服务器压力还是太大，设置2秒发送一次ajax（默认输入一个字符，提交一次，服务器压力太大）
                        }
                    }
                },
                phone: {
                    validators: {
                        notEmpty: {
                            message: '手机号不能为空'
                        },
                        stringLength: {
                            /*长度提示*/
                            min: 2,
                            max: 16,
                            message: '用户登录名长度必须在2到16之间'
                        },
                        regexp: {//匹配规则
                            regexp: /^1\d{10}$/,
                            message: '手机号码不符合规范'
                        }
                    }
                },
                email: {
                    validators: {
                        notEmpty: {
                            message: '邮箱名不能为空'
                        },
                        stringLength: {
                            /*长度提示*/
                            min: 4,
                            max: 30,
                            message: '用户名长度必须在4到30之间'
                        },
                        regexp: {//匹配规则
                            regexp: /^[A-Za-zd]+([-_.][A-Za-zd]+)*@([A-Za-zd]+[-.])+[A-Za-zd]{2,5}$/,
                            message: '邮箱不符合规范'
                        }
                    }
                },
                password: {
                    validators: {
                        notEmpty: {
                            message: '密码不能为空'
                        },
                        stringLength: {
                            /*长度提示*/
                            min: 6,
                            max: 30,
                            message: '密码长度必须在6到30之间'
                        },
                        different: {//不能和用户名相同
                            field: 'name',//需要进行比较的input name值
                            message: '不能和用户名相同'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_\.]+$/,
                            message: '密码由数字字母下划线和.组成'
                        }
                    }
                },
                repassword: {
                    message: '密码无效',
                    validators: {
                        notEmpty: {
                            message: '密码不能为空'
                        },
                        stringLength: {
                            min: 6,
                            max: 30,
                            message: '密码长度必须在6到30之间'
                        },
                        identical: {//相同
                            field: 'password',
                            message: '两次密码不一致'
                        },
                        different: {//不能和用户名相同
                            field: 'name',
                            message: '不能和用户名相同'
                        },
                        regexp: {//匹配规则
                            regexp: /^[a-zA-Z0-9_\.]+$/,
                            message: '密码由数字字母下划线和.组成'
                        }
                    }
                }
            }
        });

        $('#RegisterButton').on('click', function () {
            if (!$registerForm.data('bootstrapValidator').isValid()) {
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
            $.ajax({
                contentType: 'application/x-www-form-urlencoded',
                type: 'POST',
                url: $registerForm.attr('action'),
                data: $registerForm.serialize(),
                dataType: 'json',
                success: function (data, textStatus) {
                    if (data) {
                        modals.info("注册成功");
                        window.location.href = appPath + 'Home/Index';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        });
        $registerForm.find('input[type=text]:first-child').focus();
    });
})(jQuery);