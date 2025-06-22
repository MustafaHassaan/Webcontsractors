$(document).ready(function () {
    $("#DPR").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#DDPR").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#DSR").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#DPuR").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#RBTP").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#RBACP").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#RBDept").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#RBprt").change(function () {
        if ($(this).is(":checked")) {
            var selectedText = $(this).next("label").text().trim(); // الحصول على النص
            $("#ORF").attr("data-bs-toggle", "modal"); // إعادة `data-bs-target`
            $("#staticBackdropLabel").text(selectedText);
        }
    });
    $("#ORF").click(function (e) {
        var DPR = $("#DPR").is(":checked");
        var DDPR = $("#DDPR").is(":checked");
        var DSR = $("#DSR").is(":checked");
        var DPuR = $("#DPuR").is(":checked");
        var RBTP = $("#RBTP").is(":checked");
        var RBACP = $("#RBACP").is(":checked");
        var RBDept = $("#RBDept").is(":checked");
        var RBprt = $("#RBprt").is(":checked");
        if (!DPR && !DDPR && !DSR && !DPuR && !RBTP && !RBDept && !RBprt) {
            $("#ORF").attr("data-bs-toggle", "");
            e.preventDefault();
            Swal.fire({
                icon: "error",
                title: "خطأ ...",
                text: "برجاء اختيار نوع التقرير",
            });
        }
    });
    $("#Showrep").click(function () {
            var DPR = $("#DPR").is(":checked");
            var DDPR = $("#DDPR").is(":checked");
            var DSR = $("#DSR").is(":checked");
            var DPuR = $("#DPuR").is(":checked");
            var RBTP = $("#RBTP").is(":checked");
            var RBACP = $("#RBACP").is(":checked");
            var RBDept = $("#RBDept").is(":checked");
            var RBprt = $("#RBprt").is(":checked");
            var fromDate = $("#fromDate").val();
            var toDate = $("#toDate").val();
            var projectIds = $("#trnpro").val(); // لجلب القيم المختارة
            var ownerId = $("#ownerId").val();
            // إغلاق المودال قبل تنفيذ الفلترة
            $("#staticBackdrop").modal("hide");
            if (DPR) {
                $.ajax({
                    url: "/PSReport/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "POST",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        // تحويل المستخدم إلى الصفحة الجديدة مع البيانات
                        window.location.href = "/PSReport/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else if (DDPR) {
                $.ajax({
                    url: "/PDReport/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "POST",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        window.location.href = "/PDReport/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else if (DSR) {
                $.ajax({
                    url: "/SalesReport/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "POST",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        window.location.href = "/SalesReport/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else if (DPuR) {
                $.ajax({
                    url: "/PurchaseReport/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "POST",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        window.location.href = "/PurchaseReport/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else if (RBTP) {
                $.ajax({
                    url: "/PTReport/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "POST",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        window.location.href = "/PTReport/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else if (RBDept) {
                $.ajax({
                    url: "/DTReport/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "POST",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        window.location.href = "/DTReport/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else if (RBprt) {
                $.ajax({
                    url: "/Prtacount/Index", // اسم الـ Controller والـ Action الخاص بالتقارير
                    type: "Post",
                    data: {
                        fromDate: fromDate,
                        toDate: toDate,
                        projectId: projectIds,
                        ownerId: ownerId
                    },
                    success: function (response) {
                        // تحديث المنطقة الخاصة بالتقرير داخل الصفحة بدون إعادة تحميل
                        //$("#content-area").html(response);
                        window.location.href = "/Prtacount/Index?fromDate=" + fromDate + "&toDate=" + toDate + "&projectId=" + projectIds + "&ownerId=" + ownerId;
                    },
                    error: function () {
                        console.log("حدث خطأ أثناء جلب التقرير، حاول مرة أخرى!");
                    }
                });
            }
            else {
                e.preventDefault();
                Swal.fire({
                    icon: "error",
                    title: "خطأ ...",
                    text: "برجاء اختيار نوع التقرير",
                });
            }
    });
});
