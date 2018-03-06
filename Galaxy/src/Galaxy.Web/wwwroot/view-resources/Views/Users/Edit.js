(function () {
    $(function () {
        var $editForm = $('#user-form');
        $editForm.bootstrapValidator({
            message: '请输入有效值',
            fields: {
                name: {
                    validators: {
                        notEmpty: {
                            message: '请输入姓名'
                        }
                    }
                },
                gender: {
                    validators: {
                        notEmpty: {
                            message: '请选择性别'
                        }
                    }
                },
                birthday: {
                    validators: {
                        notEmpty: {
                            message: '请输入出生日期'
                        }
                    }
                },
                departmentName: {
                    validators: {
                        notEmpty: {
                            message: '请选择所在部门'
                        }
                    }
                },
                userName: {
                    validators: {
                        notEmpty: {
                            message: '请输入登录名'
                        }
                    }
                },
                gender: {
                    validators: {
                        notEmpty: {
                            message: '请选择性别'
                        }
                    }
                },
                email: {
                    validators: {
                        notEmpty: {
                            message: '请输入邮件',
                        },
                        emailAddress: {
                            message: '非法的邮件格式',
                        }

                    }
                }
            }
        });
        initDatePicker();
        initICheck();
        $("input[name='gender'][value='" + $('#gender').val() + "']").iCheck('check');
        //初始化组织机构选择器

        $("button[data-flag='org']").org({
            idField: $("#deptId"),
            nameField: $("#departmentName"),
            title: '选择部门',
            levels: 3
        })

        //回填id
        /*
        if (id != "0") {
            ajaxPost(basePath + "/user/get", { id: id }, function (data) {
                form.initFormData(data);
                $(".content-header h1 small").html("编辑用户【" + data.name + "】");
                //头像回填
                ajaxPost(basePath + "/user/getAvatar", { userId: id }, function (result) {
                    setAvatar(result.id, result.src, false);
                })
                //组织机构回填
                if (data.deptId) {
                    ajaxPost(basePath + "/org/show/" + data.deptId, null, function (ret) {
                        $("#deptName").val(ret.data);
                    })
                }
            })
        }
        */
        //showpwd
        $("[data-btn-type='showpwd']").click(function () {
            var input = $(this).parent().children().eq(0);
            if (input.attr('type') == 'password') {
                input.attr('type', 'text');
                $(this).find('i').removeClass().addClass('glyphicon glyphicon-eye-close');
            }
            else {
                input.attr('type', 'password');
                $(this).find('i').removeClass().addClass('glyphicon glyphicon-eye-open');
            }
        });
        //cancel
        $("[data-btn-type='cancel']").click(function () {
            window.location.href = appPath + 'Users/Index';
        });
        //upload
        $("[data-btn-type='upload']").click(function () {
            //uploadAvatar();
            return;
        });
        //submit
        $("[data-btn-type='save']").click(function () {
            $editForm.data('bootstrapValidator').validate();
            if (!$editForm.data('bootstrapValidator').isValid()) {
                return;
            }
            var param = {
                'Id': $('#id').val(),
                'Birthday': formatDate(new Date($('#birthday').val()), 'yyyy-mm-dd'),
                'CreateDate': formatDate(new Date(), 'yyyy-mm-dd'),
                'DepartmentId': $('#deptId').val(),
                'Telephone': $('#telphone').val(),
                'Email': $('#email').val(),
                'Gender': $('input[name="gender"]:checked').val(),
                'Name': $('#name').val(),
                'Phone': $('#phone').val(),
                'QQ': $('#qq').val(),
                'Status': '0',
                'UserName': $('#userName').val(),
                'Avatar': ''
            }
            console.log(JSON.stringify(param));
            modals.confirm("确认保存？", function () {
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    type: 'POST',
                    url: $editForm.attr('action'),
                    data: JSON.stringify(param),
                    dataType: 'JSON',
                    success: function (data, textStatus) {
                        console.log(data);
                        if (data.success) {
                            modals.correct("添加成功");
                            window.location.href = appPath + 'Users/Index';
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        modals.error(errorThrown);
                    }
                });
            })
        });
    });

    /**
     * 初始化日期选择器
     */
    function initDatePicker() {
        var form = $('#user-form');
        var datepickerElement = "[data-flag='datepicker']";
        if (form.find(datepickerElement).length > 0) {
            form.find(datepickerElement).datepicker({
                autoclose: true,
                format: "yyyy-mm-dd",
                language: 'zh-CN',
                clearBtn: true,
                todayHighlight: true
            }).on('change', function (e) {
                var field = $(this).attr('name');
                var validator = form.data('bootstrapValidator');
                if (validator && validator.options.fields[field])
                    validator.updateStatus(field, 'NOT_VALIDATED', null).validateField(field);
            }).parent().css("padding-left", "15px").css("padding-right", "15px");
        }
    }

    /**
     * 初始化ICheck
     */
    function initICheck() {
        var form = $('#user-form');
        if (form.find('[data-flag="icheck"]').length > 0) {
            form.find('[data-flag="icheck"]').each(function () {
                var cls = $(this).attr("class") ? $(this).attr("class") : "square-green";
                $(this).iCheck(
                    {
                        checkboxClass: 'icheckbox_' + cls,
                        radioClass: 'iradio_' + cls
                    }
                ).on('ifChanged', function (e) {
                    var field = $(this).attr('name');
                    var validator = form.data('bootstrapValidator');
                    if (validator && validator.options.fields[field])
                        validator.updateStatus(field, 'NOT_VALIDATED', null).validateField(field);
                });
            });
        }
    }

    /**
     * 上传头像
     */
    function uploadAvatar() {
        modals.openWin({
            winId: 'avatarWin',
            title: '上传头像',
            width: '700px',
            url: appPath + "Users/Avatar"
        });
    }

    /**
     * 没找到触发的入口
     */
    function resetForm() {
        form.clearForm();
        $("#user-form").data('bootstrapValidator').resetForm();
    }

    function setAvatar(avatar_id, avatar_url, isAdd) {
        $("#avatarImg").attr("src", appPath + avatar_url);
        //如果是新增 绑定用户
        if (isAdd) {
            $("#avatarId").val(avatar_id);
        } else {
            $("#avatarId").val(null);
        }
    }
})();