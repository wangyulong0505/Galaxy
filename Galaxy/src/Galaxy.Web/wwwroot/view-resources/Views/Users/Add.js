(function () {
    $(function () {
        var $addForm = $('#user-form');
        $("#user-form").bootstrapValidator({
            message: '请输入有效值',
            /*
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
                            message: '请输入姓名'
                        }
                    }
                },
                sex: {
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
                loginName: {
                    validators: {
                        notEmpty: {
                            message: '请输入登录名'
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
        //初始化组织机构选择器
        /*
        $("button[data-flag='org']").org({
            idField: $("#deptId"),
            nameField: $("#deptName"),
            title: '选择部门',
            levels: 3
        })
        */
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

        //cancel
        $("[data-btn-type='cancel']").click(function () {
            window.location.href = appPath + 'Users/Index';
        });
        //upload
        $("[data-btn-type='upload']").click(function () {
            uploadAvatar();
        });
        //submit
        $("[data-btn-type='save']").click(function () {
            console.log($addForm.serialize());
            $.ajax({
                contentType: 'application/x-www-form-urlencoded',
                type: 'POST',
                url: $addForm.attr('action'),
                data: $addForm.serialize(),
                dataType: 'json',
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