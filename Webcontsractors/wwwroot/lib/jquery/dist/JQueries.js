$(document).ready(function () {
    var optionText = '';
    var optionValue = '';
    var Prtname = "";
    var Projectname = "";
    var Amount = "";
    var Prtid = "";
    var Username = "";
    var Password = "";
    var Trndate = "";
    var Trnpro = "";
    var Trnnote = "";
    var Status = "";
    var Flag = false;
    $('#updateSelected').on('click', function () {
        var selectedIds = [];

        // لف على كل الصفوف في الـ DataTable
        DT.rows().every(function () {
            var row = this.node(); // Get the DOM element for the row
            var checkbox = $(row).find('.rowCheckbox:checked'); // ابحث عن الـ Checkbox المتفعل في الصف الحالي

            // لو الـ Checkbox متفعل، هات الـ value بتاعه (اللي هو الـ ID)
            if (checkbox.length > 0) {
                selectedIds.push(checkbox.val());
            }
        });

        // دلوقتي عندك Array فيه الـ IDs بتاعة الصفوف المحددة (selectedIds)
        //console.log("Selected IDs:", selectedIds);

        // أرسل الـ IDs دي للـ Controller باستخدام AJAX
        if (selectedIds.length > 0) {
            $.ajax({
                url: '/Transactions/UpdateMultiple', // استبدل بـ URL الـ Action بتاعك في الـ Controller
                type: 'POST',
                data: { ids: selectedIds }, // أرسل الـ Array كـ parameter
                success: function (response) {
                    // تعامل مع الرد من الـ Controller (مثلاً تحديث الجدول أو عرض رسالة)
                    //console.log("Response from Controller:", response);
                    // ممكن تعمل إعادة تحميل للـ DataTable هنا لو البيانات اتغيرت
                    //DT.ajax.reload(); // لو الـ DataTable بيجيب بياناته من AJAX
                    window.location.reload();
                },
                error: function (error) {
                    console.error("Error sending data to Controller:", error);
                }
            });
        } else {
            alert('من فضلك حدد صفوفًا أولًا.');
        }
    });
    $('#flexSwitchCheckChecked').change(function () {
        if ($("#amount").val() != "") {
            if (this.checked) {
                var amount = parseInt($("#amount").val()) / 1.15;
                var Gtax = $("#amount").val() - amount.toFixed(2);
                $("#amountvat").val(Gtax.toFixed(2));
            }
            else {
                $("#amountvat").val("0.00");
            }
        } else {
            $("#amountvat").val("0.00");
        }
    });
    $("body").on("click", "#Prtdelete", function () {
        Swal.fire({
            title: "هل تريد حذف هذا الشريك",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            if (!result.isConfirmed) {
                return;
            } else {
                var Row = $(this).closest('tr');
                var Prtid = $(this).closest('tr').children('td:eq(0)').text();
                var Prt = {
                    Id: Prtid,
                };
                $.ajax({
                    type: 'Post',
                    url: '/Partners/Prtdete',
                    data: Prt,
                    dataType: "JSON",
                    success: function (data) {
                        if (data== "Ok") {
                            $(Row).closest("tr").remove();
                        }
                    }
                });
            }
        });
    });
    $("body").on("click", "#Prodelete", function () {
        Swal.fire({
            title: "هل تريد حذف هذا المشروع",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            if (!result.isConfirmed) {
                return;
            } else {
                var Row = $(this).closest('tr');
                var Proid = $(this).closest('tr').children('td:eq(0)').text();
                var Pro = {
                    Id: Proid,
                };
                $.ajax({
                    type: 'Post',
                    url: '/Projects/Prodete',
                    data: Pro,
                    dataType: "JSON",
                    success: function (data) {
                        if (data == "Ok") {
                            $(Row).closest("tr").remove();
                        }
                    }
                });
            }
        });
    });
    $("body").on("click", "#Trndelete", function (e) {
        var Row = $(this).closest('tr');
        var Proid = $(this).closest('tr').children('td:eq(0)').text();
        var Trn = {
            Id: Proid,
        };
        Swal.fire({
            title: "هل تريد حذف هذا التعامل",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (!result.isConfirmed) {
                e.preventDefault();
                return;
            } else {
                $.ajax({
                    type: 'Get',
                    url: '/Transactions/Trndete',
                    data: Trn,
                    dataType: "JSON",
                    success: function (data) {
                        if (data == "Ok") {
                            //location.href = '/Transactions/Index';
                            $(Row).closest("tr").remove();
                        }
                    }
                });
            }
        });
    });
    $("body").on("click", "#Usrdelete", function (e) {
        var Row = $(this).closest('tr');
        var Id = $(this).closest('tr').children('td:eq(0)').text();
        var Usr = {
            Id: Id,
        };
        Swal.fire({
            title: "هل تريد حذف هذا المستخدم",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (!result.isConfirmed) {
                e.preventDefault();
            } else {
                $.ajax({
                    type: 'Post',
                    url: '/Users/Userdelete',
                    data: Usr,
                    dataType: "JSON",
                    success: function (data) {
                        if (data == "Ok") {
                            $(Row).closest("tr").remove();
                        } else {
                            alert("المستخدم لديه صلاحيات برجاء ايقاف صلاحياته وحذفه مره اخرى");
                        }
                        }
                    });
            }
        });
    });
    function Prtvalidation() {
        if (Prtname == "") {
            Flag = false;
            
            $("#errorprtname").css("display", "");
        } else {
            Flag = true;
            $("#errorprtname").css("display", "none");
        }
    }
    //
    $("#Prosave").on("click", function (e) {
        e.preventDefault();

        $("#STxt").val("Save");

        var Status = $('#Prostatus').is(':checked') ? "Open" : "Close";

        var Projectname = $("#proname").val();
        var Amount = $("#amount").val();
        var Amountvat = $("#amountvat").val();
        var Opningbalance = $("#Opningbalance").val();
        var Note = $("#Pronote").val();
        var Prtid = $("#Prtpro").find("option:selected").val();

        // ✅ Validation
        if (Projectname == "") {
            $("#errorproname").show();
            return;
        } else {
            $("#errorproname").hide();
        }

        if (Amount == "") {
            $("#erroramount").show();
            return;
        } else {
            $("#erroramount").hide();
        }

        if (Prtid == "0") {
            $("#errorPrtpro").show();
            return;
        } else {
            $("#errorPrtpro").hide();
        }

        // ✅ بعت البيانات يدويًا
        $.ajax({
            url: '/Projects/Prosave',   // <-- غيرها لاسم الكنترولر الصح
            type: 'POST',
            data: {
                Projectname: Projectname,
                Amount: Amount,
                Amountvat: Amountvat,
                Opningbalance: Opningbalance,
                Note: Note,
                Prtid: Prtid,
                Status: Status,
                STxt: $("#STxt").val()
            },
            success: function (res) {
                if (res.success) {
                    alert(res.message);
                    if (res.redirectUrl) {
                        window.location.href = res.redirectUrl;
                    }
                } else {
                    alert("خطأ: " + res.message);
                }
            },
            error: function () {
                alert("حصل خطأ في السيرفر");
            }
        });
    });

    $("#ProsaveAdd").on("click", function (e) {
        e.preventDefault(); // نمنع الإرسال العادي

        // تعبئة متغيرات الإدخال
        var Status = $('#Prostatus').is(':checked') ? "Open" : "Close";
        var Projectname = $("#proname").val();
        var Amount = $("#amount").val();
        var Amountvat = $("#amountvat").val();
        var Opningbalance = $("#Opningbalance").val();
        var Note = $("#Pronote").val();
        var Prtid = $("#Prtpro").find("option:selected").val();
        var Tdate = $("#prodate").val();

        // Validation يدوي
        if (Projectname === "" || Amount === "" || Prtid === "0") {
            if (Projectname === "") $("#errorproname").css("display", "");
            else $("#errorproname").css("display", "none");

            if (Amount === "") $("#erroramount").css("display", "");
            else $("#erroramount").css("display", "none");

            if (Prtid === "0") $("#errorPrtpro").css("display", "");
            else $("#errorPrtpro").css("display", "none");

            return;
        }

        // Ajax Call
        $.ajax({
            url: '/projects/Prosave', // عدّل اسم الكونترولر لو مش اسمه Projects
            method: 'POST',
            data: {
                STxt: "Saveadd", // نفس قيمة STxt في الأكشن
                Pro: {
                    Projectname: Projectname,
                    Amount: Amount,
                    Amountvat: Amountvat,
                    Opningbalance: Opningbalance,
                    Prtid: Prtid,
                    Tdate: Tdate,
                    Note: Note,
                    Status: Status
                }
            },
            success: function (response) {
                if (response === "Ok") {
                    // إعادة تعيين النموذج أو أي إجراء حسب المطلوب
                    alert("تم الحفظ والإضافة بنجاح!");
                    location.reload(); // أو افتح مودال جديد أو صفحة جديدة
                }
                else {
                    alert("خطأ: " + response.message);
                }
            },
            error: function (xhr) {
                alert("حدث خطأ أثناء الحفظ");
            }
        });
    });



    $("#TrnsaveAdd").on("click", function (e) {
        var Trn = {
            Id: $("#trnid").val(),
            Creditor: $("#trncred").val(),
            Debitor: $("#trndipt").val(),
            Tdate: $("#trndate").val(),
            Vatamount: $("#trntax").val(),
            Proid: $("#trnpro").val(),
            Detailes: $("#trndet").val(),
            Note: $("#trnnote").val(),
        };
        Trndate = Trn.Tdate;
        Trnpro = Trn.Proid;
        Trndetailes = Trn.Detailes;
        Trnnote = Trn.Note;
        Creditor = Trn.Creditor
        Debitor = Trn.Debitor
        if (Creditor == "0.00" && Debitor == "0.00") {
            Flag = false;
            e.preventDefault();
            if (Debitor == "0.00") {
                $("#errortrndipt").css("display", "");
            }
            if (Creditor == "0.00") {
                $("#errorcred").css("display", "");
            }
            return;
        }
        if (Creditor == "") {
            Flag = false;
            $("#errorcred").css("display", "");
            e.preventDefault();
            return;
        }
        if (Debitor == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrndipt").css("display", "");
            return;
        }
        else {
            Flag = true;
            if (Debitor == "0.00") {
                $("#errortrndipt").css("display", "none");
            }
            if (Creditor == "0.00") {
                $("#errorcred").css("display", "none");
            }
        }
        if (Trndetailes == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrnnote").css("display", "");
            return;
        } else {
            Flag = true;
            $("#errortrnnote").css("display", "none");
        }
        if (Trndate == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrndate").css("display", "");
            return;
        } else {
            Flag = true;
            $("#errortrndate").css("display", "none");
        }
        if (Trnpro == "0") {
            Flag = false;
            e.preventDefault();
            $("#errortrnpro").css("display", "");
            return;
        } else {
            Flag = true;
            $("#errortrnpro").css("display", "none");
        }
        if (Flag) {
            e.preventDefault();
            Swal.fire({
                title: "هل تريد حفظ هذا التعامل",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (!result.isConfirmed) {
                    e.preventDefault();
                    return;
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Transactions/Trnsave',
                        data: Trn,
                        dataType: "JSON",
                        success: function (data) {
                            if (data == "Ok")
                            {
                                alert("تم حفظ الحركه بنجاح");
                                $("#trnid").val("");
                                $("#trncred").val("0.00");
                                $("#trndipt").val("0.00");
                                $("#trntax").val("0.00");
                                $("#trndet").val("");
                                $("#trnnote").val("");
                                //window.location.reload();
                            }
                        }
                    });
                }
            });
        }
    });
    $("#Prtsave").on("click", function (e) {
        Partnername = $("#partname").val();
        if (Partnername == "") {
            e.preventDefault();
            Prtvalidation();
        }
    });
    $("#Prtedit").on("click", function (e) {
        Prtname = $("#partname").val();
        Prtvalidation();
        if (Flag) {
            var Prt = {
                Id: $("#partid").val(),
                Partnername: $("#partname").val(),
                Description: $("#floatingTextarea").val(),
                Amount: $("#payamount").val(),
                Percentage: $("#paypercentage").val(),
                txtpaypercentage: $("#txtpaypercentage").val(),
            };
            Swal.fire({
                title: "هل تريد تعديل هذا الشريك",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                if (!result.isConfirmed) {
                    e.preventDefault();
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Partners/Prtedit',
                        data: Prt,
                        dataType: "JSON",
                        success: function (data) {
                            if (data == "Ok") {
                                location.href = '/Partners/Index';
                            }
                        }
                    });
                }
            });
        }
    });

    $("#Proedit").on("click", function (e) {
        Projectname = $("#proname").val();
        Amount = $("#amount").val();
        Amountvat = $("#tax").val();
        Note = $("#Pronote").val();
        Prtid = $("#Prtpro").val();
        Projectname = $("#proname").val();
        Amount = $("#amount").val();
        Amountvat = $("#tax").val();
        Opningbalance = $("#Opningbalance").val();
        Note = $("#Pronote").val();
        Prtid = $("#Prtpro").find("option:selected").val();
        var Statuschaecked = $('#Prostatus').is(':checked');
        if (Statuschaecked) {
            Status = "Open";
        } else {
            Status = "Close";
        }
        //Status = 
        if (Projectname == "") {
            Flag = false;
            $("#errorproname").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#errorproname").css("display", "none");
        }
        if (Amount == "") {
            Flag = false;
            $("#erroramount").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#erroramount").css("display", "none");
        }
        if (Prtid == "0") {
            Flag = false;
            $("#errorPrtpro").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#errorPrtpro").css("display", "none");
        }
        if (Flag) {
            var Pro = {
                Id: $("#proid").val(),
                Projectname: $("#proname").val(),
                Amount: $("#amount").val(),
                Amountvat: $("#amountvat").val(),
                Opningbalance: $("#Opningbalance").val(),
                Note: $("#Pronote").val(),
                Prtid: $("#Prtid").find("option:selected").val(),
                Status
            };
            Swal.fire({
                title: "هل تريد تعديل هذا المشروع",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (!result.isConfirmed) {
                    e.preventDefault();
                    return;
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Projects/Proedit',
                        data: Pro,
                        dataType: "JSON",
                        success: function (data) {
                            if (data == "Ok") {
                                location.href = '/Projects/Index';
                            }
                        }
                    });
                }
            });
        }
    });

    $("#Trnsave").on("click", function (e) {
        var Trn = {
            Id: $("#trnid").val(),
            Creditor: $("#trncred").val(),
            Debitor: $("#trndipt").val(),
            Tdate: $("#trndate").val(),
            Vatamount: $("#trntax").val(),
            Proid: $("#trnpro").val(),
            Detailes: $("#trndet").val(),
            Note: $("#trnnote").val(),
        };
        Creditor = Trn.Creditor
        Debitor = Trn.Debitor
        Trndate = Trn.Tdate;
        Trnpro = Trn.Proid;
        Trndetailes = Trn.Detailes;
        Trnnote = Trn.Note;
        if (Creditor == "0.00" && Debitor == "0.00") {
            Flag = false;
            e.preventDefault();
            if (Debitor == "0.00") {
                $("#errortrndipt").css("display", "");
            }
            if (Creditor == "0.00") {
                $("#errorcred").css("display", "");
            }
            return;
        }
        if (Creditor == "") {
            Flag = false;
            $("#errorcred").css("display", "");
            e.preventDefault();
            return;
        }
        if (Debitor == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrndipt").css("display", "");
            return;
        }
        else {
            Flag = true;
            if (Debitor == "0.00") {
                $("#errortrndipt").css("display", "none");
            }
            if (Creditor == "0.00") {
                $("#errorcred").css("display", "none");
            }
        }
        if (Trndetailes == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrnnote").css("display", "");
            return;
        }
        else {
            Flag = true;
            $("#errortrnnote").css("display", "none");
        }
        if (Trndate == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrndate").css("display", "");
            return;
        }
        else {
            Flag = true;
            $("#errortrndate").css("display", "none");
        }
        if (Trnpro == "0") {
            Flag = false;
            e.preventDefault();
            $("#errortrnpro").css("display", "");
            return;
        }
        else {
            Flag = true;
            $("#errortrnpro").css("display", "none");
        }
        if (Flag) {
            e.preventDefault();
            Swal.fire({
                title: "هل تريد حفظ هذا التعامل",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (!result.isConfirmed) {
                    e.preventDefault();
                    return;
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Transactions/Trnsave',
                        data: Trn,
                        dataType: "JSON",
                        success: function (data) {
                            if (data == "Ok") {
                                location.href = '/Transactions/Index';
                            }
                        }
                    });
                }
            });
        }
    });
    $("#Trnedit").on("click", function (e) {
        var Trn = {
            Id: $("#trnid").val(),
            Creditor: $("#trncred").val(),
            Debitor: $("#trndipt").val(),
            Tdate: $("#trndate").val(),
            Vatamount: $("#trntax").val(),
            Proid: $("#trnpro").val(),
            Prtid: $("#Prtid").val(),
            Detailes: $("#trndet").val(),
            Note: $("#trnnote").val(),
        };
        Trndate = Trn.Tdate;
        Trnpro = Trn.Proid;
        Trndetailes = Trn.Detailes;
        Trnnote = Trn.Note;
        Creditor = Trn.Creditor
        Debitor = Trn.Debitor
        if (Creditor == "0.00" && Debitor == "0.00") {
            Flag = false;
            e.preventDefault();
            if (Debitor == "0.00") {
                $("#errortrndipt").css("display", "");
            }
            if (Creditor == "0.00") {
                $("#errorcred").css("display", "");
            }
            return;
        }
        if (Creditor == "") {
            Flag = false;
            $("#errorcred").css("display", "");
            e.preventDefault();
            return;
        }
        if (Debitor == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrndipt").css("display", "");
            return;
        }
        else {
            Flag = true;
            if (Debitor == "0.00") {
                $("#errortrndipt").css("display", "none");
            }
            if (Creditor == "0.00") {
                $("#errorcred").css("display", "none");
            }
        }
        if (Trndetailes == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrnnote").css("display", "");
            return;
        } else {
            Flag = true;
            $("#errortrnnote").css("display", "none");
        }
        if (Trndate == "") {
            Flag = false;
            e.preventDefault();
            $("#errortrndate").css("display", "");
            return;
        } else {
            Flag = true;
            $("#errortrndate").css("display", "none");
        }
        if (Trnpro == "0") {
            Flag = false;
            e.preventDefault();
            $("#errortrnpro").css("display", "");
            return;
        } else {
            Flag = true;
            $("#errortrnpro").css("display", "none");
        }
        if (Flag) {
            e.preventDefault();
            Swal.fire({
                title: "هل تريد تعديل هذا التعامل",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (!result.isConfirmed) {
                    e.preventDefault();
                    return;
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Transactions/Trnedit',
                        data: Trn,
                        dataType: "JSON",
                        success: function (data) {
                            if (data == "Ok") {
                                location.href = '/Transactions/Index';
                            }
                        }
                    });
                }
            });
        }
    });
    $("#Usrsave").on("click", function (e) {
        Username = $("#username").val();
        Password = $("#password").val();
        var Usr = {
            Id: $("#userid").val(),
            Username: $("#username").val(),
            Password: $("#password").val(),
        };
        if (Username == "") {
            Flag = false;
            $("#errorusrname").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#errorusrname").css("display", "none");
        }
        if (Password == "") {
            Flag = false;
            $("#errorpassword").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#errorpassword").css("display", "none");
        }
        if (Flag) {
            e.preventDefault();
            Swal.fire({
                title: "هل تريد حفظ هذا المستخدم",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (!result.isConfirmed) {
                    e.preventDefault();
                    return;
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Users/Signup',
                        data: Usr,
                        dataType: "JSON",
                        success: function (data) {
                            if (data == "Ok") {
                                location.href = '/Sign/Index';
                            }
                        }
                    });
                }
            });
        }
    });
    $("#Useredit").on("click", function (e) {
        Username = $("#username").val();
        Password = $("#password").val();
        var Usr = {
            Id: $("#userid").val(),
            Username: $("#username").val(),
            Password: $("#password").val(),
        };
        if (Username == "") {
            Flag = false;
            $("#errorusrname").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#errorusrname").css("display", "none");
        }
        if (Password == "") {
            Flag = false;
            $("#errorpassword").css("display", "");
            e.preventDefault();
            return;
        } else {
            Flag = true;
            $("#errorpassword").css("display", "none");
        }
        if (Flag) {
            e.preventDefault();
            Swal.fire({
                title: "هل تريد تعديل هذا المستخدم",
                showDenyButton: true,
                denyButtonText: 'لا',
                confirmButtonText: "نعم",
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (!result.isConfirmed) {
                    e.preventDefault();
                    return;
                } else {
                    $.ajax({
                        type: 'Post',
                        url: '/Sign/Useredit',
                        data: Usr,
                        dataType: "JSON",
                    });
                }
            });
        }
    });
    $("#Usrdelete").on("click", function (e) {
        var Usr = {
            Id: $("#userid").val(),
            Username: $("#username").val(),
            Password: $("#password").val(),
        };
        Swal.fire({
            title: "هل تريد حذف هذا المستخدم",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (!result.isConfirmed) {
                e.preventDefault();
            } else {
                $.ajax({
                    type: 'Post',
                    url: '/Usera/Userdelete',
                    data: Usr,
                    dataType: "JSON",
                    success: function () {
                        GetUserdata();
                        Swal.fire("تمت العمليه بنجاح", "نجاح");
                    }
                });
            }
        });
    });
    //========================//
    //Menu
    $("#OCNav").on("click", function () {
        var OCN = document.getElementById("side-menu").style.width;
        if (OCN == 0 || OCN == "0px") {
            document.getElementById("side-menu").style.width = "200px";
            document.getElementById("content-area").style.marginRight = "200px";
        } else {
            document.getElementById("side-menu").style.width = "0";
            document.getElementById("content-area").style.marginRight = "0";
        }
    });
    //End menu

    //Remove Class
    function Clearclasse() {
        $("#Homesection").removeClass("active");
        $("#Partnerssection").removeClass("active");
        $("#Projectssection").removeClass("active");
        $("#Transactionssection").removeClass("active");
        $("#Entriessection").removeClass("active");
        $("#Rolesection").removeClass("active");
        $("#Reportssection").removeClass("active");
    }
    //End Remove Class

    //Partners section
    //========================//
    $("#Partnerssection").on("click", function () {
        Clearclasse();
        $("#Partnerssection").addClass("active");
    });
    //========================//
    $('#trndipt').focus(function () {
        $("#trndipt").val("");
    });
    $('#trncred').focus(function () {
        $("#trncred").val("");
    });
    $('#trntaxc').change(function () {
        var cred = $("#trncred").val();
        var dept = $("#trndipt").val();
        var Amout = 0;
        var GT = 0;
        if (this.checked) {
            if (cred != "0.00") {
                Amount = parseFloat($("#trncred").val()).toFixed(2) / 1.15;
                GT = parseFloat(cred) - Amount.toFixed(2);
            }
            if (dept != "0.00") {
                Amount = parseFloat($("#trndipt").val()).toFixed(2) / 1.15;
                GT = parseFloat(dept) - Amount.toFixed(2);
            }
            var Gtax = GT.toFixed(2);
            $("#trntax").val(Gtax);
        } else {
            $("#trntax").val("0.00");
        }


        /*
        if ($("#trncred").val() != "0" && $("#trncred").val() != "") {
            if (this.checked) {
                var amount = parseFloat($("#trncred").val()).toFixed(2) / 1.15;
                var Gtax = $("#trncred").val() - amount.toFixed(2);
                var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
            } else {
                $("#trntax").val("");
            }
            $("#trntax").prop("disabled", false);
        }
        else if ($("#trndipt").val() != "0" && $("#trndipt").val() != "") {
            if (this.checked) {
                var amount = parseFloat($("#trndipt").val()).toFixed(2) / 1.15;
                var Gtax = $("#trndipt").val() - amount.toFixed(2);
                var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
            } else {
                $("#trntax").val("");
            }
            $("#trntax").prop("disabled", false);
        }
        else {
            $("#trntax").prop("disabled", true);
            $("#trncred").val("");
        }
        */
    });
    $('#amount').on('input', function (e) {
        if ($("#trntaxc").checked) {
            if ($("#trncred").val() != "0" && $("#trncred").val() != "") {
                if (this.checked) {
                    var amount = parseFloat($("#trncred").val()).toFixed(2) / 1.15;
                    var Gtax = $("#trncred").val() - amount.toFixed(2);
                    var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
                } else {
                    $("#trntax").val("");
                }
                $("#trntax").prop("disabled", false);
            }
            else if ($("#trndipt").val() != "0" && $("#trndipt").val() != "") {
                if (this.checked) {
                    var amount = parseFloat($("#trndipt").val()).toFixed(2) / 1.15;
                    var Gtax = $("#trndipt").val() - amount.toFixed(2);
                    var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
                } else {
                    $("#trntax").val("");
                }
                $("#trntax").prop("disabled", false);
            }
            else {
                $("#trntax").prop("disabled", true);
                $("#trncred").val("");
            }
        } else {
            $("#amountvat").val("");
        }
    })
    $('#trncred').on('input', function (e) {
        var Amount = $("#trncred").val();
        if (Amount != "0" || Amount != "") {
            if ($("#trntaxc").checked) {
                var amount = parseInt($("#trncred").val()) / 1.15;
                var Gtax = parseInt($("#trncred").val()) - amount.toFixed(2);
                var tax = parseInt($("#trntax").val(Gtax));
            } else {
                $("#trntax").val("0.00");
            }
        }
    });
    $('#trndipt').on('input', function (e) {
        var Amount = $("#trndipt").val();
        if (Amount != "0" || Amount != "") {
            if ($("#trndipt").val() != "" && $("#trntaxc").checked) {
                var amount = parseInt($("#trncred").val()) / 1.15;
                var Gtax = parseInt($("#trndipt").val()) - amount.toFixed(2);
                var tax = parseInt($("#trntax").val(Gtax));
            } else {
                $("#trntax").val("0.00");
            }
        }
    });
    $('#trncred').keyup(function () {
        $("#trndipt").val("0.00");
    })
    $('#trndipt').keyup(function () {
        $("#trncred").val("0.00");
    })
    $('#trncred').keyup(function () {
        if ($('#trntaxc').is(':checked')) {
            if ($("#trncred").val() != "0" && $("#trncred").val() != "") {
                var amount = parseFloat($("#trncred").val()).toFixed(2) / 1.15;
                var Gtax = $("#trncred").val() - amount.toFixed(2);
                var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
                $("#trntax").prop("disabled", false);
            }
            else if ($("#trndipt").val() != "0" && $("#trndipt").val() != "") {
                var amount = parseFloat($("#trndipt").val()).toFixed(2) / 1.15;
                var Gtax = $("#trndipt").val() - amount.toFixed(2);
                var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
            }
            else {
                $("#trncred").val("0.00");
            }
        } else {
            $("#amountvat").val("0.00");
        }
    })
    $('#trndipt').keyup(function () {
        if ($('#trntaxc').is(':checked')) {
            if ($("#trndipt").val() != "0" && $("#trndipt").val() != "") {
                var amount = parseFloat($("#trndipt").val()).toFixed(2) / 1.15;
                var Gtax = $("#trndipt").val() - amount.toFixed(2);
                var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
                $("#trntax").prop("disabled", false);
            }
            else if ($("#trncred").val() != "0" && $("#trncred").val() != "") {
                var amount = parseFloat($("#trncred").val()).toFixed(2) / 1.15;
                var Gtax = $("#trncred").val() - amount.toFixed(2);
                var tax = parseFloat($("#trntax").val(Gtax.toFixed(2)));
            }
            else {
                $("#trncred").val("0.00");
            }
        } else {
            $("#amountvat").val("0.00");
        }
    })
    $('#amount').keyup(function () {
        if ($('#flexSwitchCheckChecked').is(':checked')) {
            //amountvat
            var amount = parseFloat($("#amount").val()) / 1.15;
            var Gtax = $("#amount").val() - amount.toFixed(2);
            $("#amountvat").val(Gtax.toFixed(2));
        } else {
            $("#amountvat").val("0.00");
        }
    })


    //Transaction Excel 
    $("#EXC").on('click', function () {
        var DT = $('#Trntbl').DataTable();
        var transactionmodel = [];
        DT.rows().every(function () {
            var row = this.node(); // Get the DOM element for the row
            var id = parseInt($(row).find('td:eq(0)').text().trim());
            var creditor = parseFloat($(row).find('td:eq(1)').text()) || 0;
            var debitor = parseFloat($(row).find('td:eq(2)').text()) || 0;
            var vatamount = parseFloat($(row).find('td:eq(3)').text()) || 0;
            var projectname = $(row).find('td:eq(4)').text();
            var detailes = $(row).find('td:eq(5)').text();
            var Prtname = $(row).find('td:eq(7)').text();
            var tdate = $(row).find('td:eq(6)').text();
            var note = $(row).find('td:eq(8)').text();
            transactionmodel.push({
                'Id': id, 'Creditor': creditor, 'debitor': debitor, 'vatamount': vatamount,
                'projectname': projectname, 'detailes': detailes, 'Prtname': Prtname, 'tdate': tdate, 'Note': note
            });
        })
        // أرسل الـ IDs دي للـ Controller باستخدام AJAX
        if (transactionmodel.length > 0) {
            $.ajax({
                type: 'POST',
                url: '/Transactions/Exportdata',
                data: JSON.stringify(transactionmodel),
                contentType: 'application/json; charset=utf-8',
                //dataType: 'json',
                xhrFields: {
                    responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
                },
                success: function (blob, status, xhr) {
                    // إنشاء رابط تنزيل مؤقت
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;

                    // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
                    const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
                    a.download = 'TransactionsExport.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

                    document.body.appendChild(a);
                    a.click();

                    // تنظيف الـ URL المؤقت
                    window.URL.revokeObjectURL(url);
                    document.body.removeChild(a); // إزالة الرابط من DOM
                },
                error: function (xhr, status, error) {
                    console.error('خطأ في تصدير البيانات:', error);
                    // يمكنك هنا عرض رسالة خطأ للمستخدم
                }
            });
        } else {
            alert('من فضلك حدد صفوفًا أولًا.');
        }
    });
    //========================

    document.addEventListener("DOMContentLoaded", function () {
        var trncred = document.getElementById("trncred");
        var trndipt = document.getElementById("trndipt");

        if (trncred) {
            trncred.addEventListener("paste", handlePaste);
            trncred.addEventListener("input", handleInput);
        } else {
            alert("العنصر #trncred غير موجود في DOM");
        }

        if (trndipt) {
            trndipt.addEventListener("paste", handlePaste);
            trndipt.addEventListener("input", handleInput);
        } else {
            alert("العنصر #trndipt غير موجود في DOM");
        }
    });
    function handlePaste(event) {
        event.preventDefault();
        var pastedText = (event.clipboardData || window.clipboardData).getData("text");

        if (/^\d+(\.\d+)?$/.test(pastedText)) {
            event.target.value = pastedText;
        } else {
            alert("النص الملصوق غير صالح!");
        }
    }
    function handleInput(event) {
        event.target.value = event.target.value.replace(/[^0-9.]/g, "");
    }
    // نقرأ القيمة من الـ Hidden
    var val = parseFloat($("#txtpaypercentage").val());
    if (!isNaN(val) && val >= 0 && val <= 100) {
        $("#paypercentage").val(val).trigger("input");
        $("#percentageValue").text(val);
    } else {
        if (isNaN(val)) {
            val = 0;
            $("#paypercentage").val(val).trigger("input");
            $("#percentageValue").text(val);
        }
    }
    $('#Prostatus').change(function () {
        var PS = $('#Prostatus').is(':checked');
        if (PS) {
            $("#Profits").attr("disabled", true);
            $("#BtnPS").attr("disabled", true);
            $("#Profits").val("0");
        }
        else {
            $("#Profits").attr("disabled", false);
            $("#BtnPS").attr("disabled", false);
        }
    });
    $("#BtnPS").click(function (e) {
        e.preventDefault();
        var ProId = $("#proid").val();
        $.ajax({
            method: 'GET',
            url: '/Projects/CalcProfitJson',
            data: { id: ProId },
            beforeSend: function () {
                $("#Profits").val("جاري الحساب...");
            },
            success: function (data) {
                if (data != null) {
                    $("#Profits").val(data);
                } else {
                    $("#Profits").val(0.00);
                }
            },
            error: function () {
                alert("حدث خطأ أثناء الاتصال بالسيرفر");
            }
        });
    });
    $("#Profitsumprt").click(function () {
        Swal.fire({
            title: "هل تريد حساب المشروعات المغلقة لهذا الشريك",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            if (!result.isConfirmed) {
                return;
            } else {
                var Prt = {
                    Id: $("#partid").val(),
                };
                //var Receivepayment = [];
                $.ajax({
                    type: 'Get',
                    url: '/Partners/Calcprtprofits',
                    data: Prt,
                    dataType: "JSON",
                    success: function (data) {
                        if (data.message == 'No Data') {
                            Swal.fire("لا يوجد مشاريع مغلقه لهذا الشريك");
                            $("#payamount").val("0.00"); // أو أي نص توضيحي تحبه
                        } else {
                            //console.log(data); // للتأكيد فقط
                            $("#payamount").val(parseFloat(data.totalSum).toFixed(2));
                        }
                    }
                });
            }
        });
    });
    // لما تكتب في التكست، يعدّل السلايدر والقيمة المعروضة
    $("#txtpaypercentage").on("input", function () {
        var val = parseInt($(this).val());
        if (!isNaN(val) && val >= 0 && val <= 100) {
            $("#paypercentage").val(val).trigger("input");
        } else {
            if (isNaN(val)) {
                val = 0;
                $("#paypercentage").val(val).trigger("input");
            }
        }
    });
    // لما تحرّك السلايدر، ينعكس عالتكست وعلى الـ output
    $("#paypercentage").on("input", function () {
        var val = $(this).val();
        $("#txtpaypercentage").val(val);
        $("#percentageValue").text(val);
    });
    $("body").on("click", "#Profitspprt", function (e) {
        e.preventDefault();
        Swal.fire({
            title: "هل تريد حساب المشروعات المغلقة لهذا الشريك",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            if (!result.isConfirmed) {
                return;
            } else {
                var Row = $(this).closest('tr');
                var Prtid = $(this).closest('tr').children('td:eq(0)').text();
                var Prt = {
                    Id: Prtid,
                };
                //var Receivepayment = [];
                $.ajax({
                    type: 'Get',
                    url: '/Partners/CalcProfitJson',
                    data: Prt,
                    dataType: "JSON",
                    success: function (data) {
                        if (data == null) {
                            alert("لا يوجد مشاريع مغلقه لهذا الشريك");
                        } else if (data[0].message == "لا توجد بيانات متاحة") {
                            alert(data[0].message);
                        } else {
                            $.ajax({
                                type: "POST",
                                url: "/Partners/Receivepayment", // اسم الكنترولر والأكشن
                                data: JSON.stringify({ Receivepayment: data }), // إرسال البيانات بعد تحويلها إلى JSON
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    console.log("تم الإرسال بنجاح!", response);
                                },
                                error: function (xhr, status, error) {
                                    console.log("حدث خطأ:", error);
                                }
                            });
                        }
                    }
                });
            }
        });
    });
    $("body").on("click", "#pinfo", function () {
        $("#Addper").css("display", "none");
        $("#pedit").css("display", "");
        var Row = $(this).closest('tr');
        var Pid = $(this).closest('tr').children('td:eq(0)').text();
        var PName = $(this).closest('tr').children('td:eq(1)').text();
        var PDescription = $(this).closest('tr').children('td:eq(2)').text();
        $("#pid").val(Pid);
        $("#pName").val(PName);
        $("#pDescription").val(PDescription);
    });
    $("body").on("click", "#pedit", function (e) {
        e.preventDefault();
        Swal.fire({
            title: "هل تريد تعديل هذه الصلاحيه",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            if (!result.isConfirmed) {
                return;
            } else {
                var model = {
                    Id: $("#pid").val(),
                    Name: $("#pName").val(),
                    Description: $("#pDescription").val()
                };
                $.ajax({
                    type: 'Post',
                    url: '/Permessions/Edit',
                    data: model,
                    success: function (data) {
                        if (data == "Ok") {
                            $("#pedit").css("display", "none");
                            $("#Addper").css("display", "");
                            $("#pid").val("");
                            $("#pName").val("");
                            $("#pDescription").val("");
                            location.href = '/Permessions/Index';
                        }
                    }
                });
            }
        });
    });
    $("body").on("click", "#pdelete", function () {
        Swal.fire({
            title: "هل تريد حذف هذه الصلاحيه",
            showDenyButton: true,
            denyButtonText: 'لا',
            confirmButtonText: "نعم",
        }).then((result) => {
            if (!result.isConfirmed) {
                return;
            } else {
                var Row = $(this).closest('tr');
                var Pid = $(this).closest('tr').children('td:eq(0)').text();
                var Prt = {
                    Id: Pid,
                };
                $.ajax({
                    type: 'Post',
                    url: '/Permessions/Delete',
                    data: Prt,
                    dataType: "JSON",
                    success: function (data) {
                        if (data == "Ok") {
                            $(Row).closest("tr").remove();
                        }
                    }
                });
            }
        });
    });

    $('#Peredit').click(function () {
        // جمع كل معرفات الصلاحيات مع حالة التحديد
        var allPermissions = $('input[name="SelectedPermissionIds"]').map(function () {
            return {
                PermissionId: parseInt($(this).val()),
                IsSelected: $(this).is(':checked')
            };
        }).get();
        var SUId = parseInt($('#userid').val());
        var data = {
            SelectedUserId: SUId,  // لو داخل صفحة Razor بدون اقتباسات
            Permissions: allPermissions
        };

        $.ajax({
            type: 'POST',
            url: '/Sign/Assign',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                alert('تم تحديث الصلاحيات بنجاح');
                location.reload();
            },
            error: function () {
                alert('حدث خطأ أثناء التحديث');
            }
        });
    });
    $('#paycred').keyup(function () {
        $("#paydipt").val("0.00");
    })
    $('#paydipt').keyup(function () {
        $("#paycred").val("0.00");
    })
    $('#paydipt').focus(function () {
        $("#paydipt").val("");
    });
    $('#paycred').focus(function () {
        $("#paycred").val("");
    });
    $("#Saveandaddaccount").click(function (e) {
        e.preventDefault();
        var model = {
            Creditor: $("#paycred").val(),
            Debitor: $("#paydipt").val(),
            Date: $("#paydate").val(),
            Proid: $("#trnpro").val(),
            Prtid: $("#Prtid").val(),
            Note: $("#paynote").val(),
        };
        $.ajax({
            type: 'Post',
            url: '/Accountant/Saveadd',
            data: model,
            success: function (response) {
                if (response == "Ok") {
                    window.location.reload();
                }
            },

        });
        //$("#Prtid").val("0").selectpicker("refresh");

    });
});